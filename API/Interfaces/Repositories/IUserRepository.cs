using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces.Repositories
{
    public interface IUserRepository
    {
        void AddUser(AppUser user);
        
        void Update(AppUser user);

        Task<bool> SaveAllAsync();

        Task<AppUser> GetUserByUsernameAsync(string username);

        Task<MemberDto> GetMemberDtoByUsernameAsync(string username);

        Task<MemberDto> GetMemberDtoByIdAsync(int id);

        Task<IEnumerable<MemberDto>> GetMemberDtosAsync();

        Task<bool> UpdateUserByUsernameAsync(string username, MemberUpdateDto memberUpdateDto);

        Task<bool> SetMainPhoto(string username, int newMainPhotoId);
    }
}
