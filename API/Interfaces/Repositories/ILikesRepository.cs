using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces.Repositories
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int likerId, int likeeId);

        Task<AppUser> GetUserWithLikes(int userId);

        Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId);
    }
}
