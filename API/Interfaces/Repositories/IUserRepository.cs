using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces.Repositories
{
    public interface IUserRepository
    {
        void Update(AppUser user);

        Task<bool> SaveAllAsync();

        Task<IEnumerable<AppUser>> GetUsersAsync();

        Task<AppUser> GetUserByIdAsync(int id);

        Task<AppUser> GetUserByNameAsync(string username);

        Task<MemberDto> GetMemberDtoNameAsync(string username);

        Task<MemberDto> GetMemberDtoByIdAsync(int id);

        Task<IEnumerable<MemberDto>> GetMemberDtosAsync();
    }
}
