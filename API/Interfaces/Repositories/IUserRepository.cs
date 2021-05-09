using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Http;

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

        // TODO: Move this stuff to PhotosCollection class, see other TODOs
        #region Photos stuff
        Task<Photo> AddPhotoAsync(string username, IFormFile photo);

        Task<bool> SetMainPhotoAsync(string username, int newMainPhotoId);

        Task<bool> DeletePhotoAsync(string username, int photoId);
        #endregion
    }
}
