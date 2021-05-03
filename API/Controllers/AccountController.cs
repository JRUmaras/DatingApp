using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public AccountController(DataContext context, ITokenService tokenService, IUserRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserRegistrationDto registrationDto)
        {
            if (await UsernameExistsInDb(registrationDto.Username)) return BadRequest("Username already exists.");

            using var hmac = new HMACSHA512();

            var newUser = new AppUser
            {
                UserName = registrationDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registrationDto.Password)),
                PasswordSalt = hmac.Key
            };

            _userRepository.AddUser(newUser);
            await _userRepository.SaveAllAsync();

            return CreateUserDto(newUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserLoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);

            if (user is null) return Unauthorized("Invalid username.");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            if (!computedHash.SequenceEqual(user.PasswordHash)) return Unauthorized("Invalid password.");

            return CreateUserDto(user);
        }

        private async Task<bool> UsernameExistsInDb(string username) => await _userRepository.GetUserByUsernameAsync(username.ToLower()) is not null;

        private UserDto CreateUserDto(AppUser user) => new()
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user),
            PhotoUrl = user.Photos?.FirstOrDefault(photo => photo.IsMain)?.Url ?? ""
        };
    }
}
