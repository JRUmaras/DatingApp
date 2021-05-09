using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Errors.Data.Repositories;
using API.Extensions;
using API.Interfaces;
using API.Interfaces.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UserRepository(DataContext context, IMapper mapper, IPhotoService photoService)
        {
            _context = context;
            _mapper = mapper;
            _photoService = photoService;
        }

        public void AddUser(AppUser user)
        {
            _context.Users.Add(user);
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(user => user.Photos)
                .SingleOrDefaultAsync(user => user.UserName == username);
        }

        public async Task<IEnumerable<MemberDto>> GetMemberDtosAsync()
        {
            return await _context.Users
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<MemberDto> GetMemberDtoByIdAsync(int id)
        {
            return await _context.Users
                .Where(user => user.Id == id)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<MemberDto> GetMemberDtoByUsernameAsync(string username)
        {
            return await _context.Users
                .Where(user => user.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> UpdateUserByUsernameAsync(string username, MemberUpdateDto memberUpdateDto)
        {
            var appUser = await GetUserByUsernameAsync(username);

            _mapper.Map(memberUpdateDto, appUser);

            Update(appUser);

            return await SaveAllAsync();
        }

        /// <summary>
        /// Uploads the specified photo into the users gallery.
        /// </summary>
        /// <param name="username">User's username.</param>
        /// <param name="file">The photo file.</param>
        /// <returns>The uploaded photo as a <see cref="Photo"/>.</returns>
        /// <exception cref="PhotoUploadFailedException">Throws when the upload fails.</exception>
        public async Task<Photo> AddPhotoAsync(string username, IFormFile file)
        {
            var imageUploadResult = await _photoService.AddPhotoAsync(file);

            if (imageUploadResult.Error is not null) throw PhotoUploadFailedException.CloudUploadFailedException(imageUploadResult.Error);

            var user = await GetUserByUsernameAsync(username);

            var photo = new Photo
            {
                Url = imageUploadResult.SecureUrl.AbsoluteUri,
                IsMain = !user.Photos.Any(),
                PublicId = imageUploadResult.PublicId
            };

            user.Photos.Add(photo);
            if (await SaveAllAsync()) return photo;

            throw PhotoUploadFailedException.UnknownIssueException();
        }

        public async Task<bool> SetMainPhotoAsync(string username, int newMainPhotoId)
        {
            var user = await GetUserByUsernameAsync(username);

            var wasSuccess = user.Photos.SetNewMainPhoto(newMainPhotoId);

            return wasSuccess && await SaveAllAsync();
        }

        public async Task<bool> DeletePhotoAsync(string username, int newMainPhotoId)
        {
            throw new System.NotImplementedException();
        }
    }
}
