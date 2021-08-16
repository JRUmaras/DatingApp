using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces.Repositories
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int likerId, int likeeId);

        Task<AppUser> GetUserWithLikes(int userId);

        Task<PagedList<LikeDto>> GetUserLikes(LikesSettings likesSettings);
    }
}
