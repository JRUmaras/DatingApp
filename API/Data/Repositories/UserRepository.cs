using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<bool> SetMainPhoto(string username, int newMainPhotoId)
        {
            var user = await GetUserByUsernameAsync(username);

            var wasSuccess = SetNewMainPhoto(user.Photos, newMainPhotoId);

            return wasSuccess && await SaveAllAsync();
        }

        /// <summary>
        /// Update the main photo of a set of Photos.
        /// Assumptions:
        /// - Each Photo.Id is unique in the set.
        /// - There is at most one main photo in the set.
        /// </summary>
        /// <param name="userPhotos">Set of photos.</param>
        /// <param name="newMainPhotoId">Photo.Id of the new main photo.</param>
        /// <returns></returns>
        private static bool SetNewMainPhoto(IEnumerable<Photo> userPhotos, int newMainPhotoId)
        {
            Photo oldMainPhoto = null;
            Photo newMainPhoto = null;

            foreach (var photo in userPhotos)
            {
                if (photo.IsMain)
                {
                    oldMainPhoto = photo;
                    oldMainPhoto.IsMain = false;

                    if (newMainPhoto is not null) return true;
                }

                // ReSharper disable once InvertIf
                if (photo.Id == newMainPhotoId)
                {
                    newMainPhoto = photo;
                    newMainPhoto.IsMain = true;

                    if (oldMainPhoto is not null) return true;
                }
            }

            // In case there was no main photo in the set to begin with
            if (newMainPhoto is not null) return true;

            // If we got down to here, the update failed
            // Reset old photo as main if possible
            if (oldMainPhoto is not null) oldMainPhoto.IsMain = true;

            return false;
        }

    }
}
