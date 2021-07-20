using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces.Repositories;
using API.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public LikesRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserLike> GetUserLike(int likerId, int likeeId)
        {
            return await _context.Likes
                .FirstOrDefaultAsync(like => like.LikeeId == likeeId && like.LikerId == likerId);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
                .Include(user => user.LikedUsers)
                .FirstOrDefaultAsync(user => user.Id == userId);
        }

        public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
        {
            IQueryable<AppUser> users;
            var likes = _context.Likes.AsQueryable();

            switch (predicate)
            {
                case StringService.LikeRelationship.Likes:
                    likes = likes.Where(like => like.LikerId == userId);
                    users = likes.Select(like => like.Likee);
                    break;

                case StringService.LikeRelationship.LikedBy:
                    likes = likes.Where(like => like.LikeeId == userId);
                    users = likes.Select(like => like.Liker);
                    break;

                default:
                    throw new Exception($"Unknown type of like: {predicate}");
            }

            return await users
                .OrderBy(user => user.UserName)
                .ProjectTo<LikeDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
