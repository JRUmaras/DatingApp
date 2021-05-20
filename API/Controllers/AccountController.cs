using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Interfaces.Repositories;
using AutoMapper;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AccountController(ITokenService tokenService, IUserRepository userRepository, IMapper mapper)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserRegistrationDto registrationDto)
        {
            if (await UsernameExistsInDb(registrationDto.Username)) return BadRequest("Username already exists.");

            var newUser = _mapper.Map<AppUser>(registrationDto);

            // TODO: move this to a "hashing service"
            using var hmac = new HMACSHA512();

            newUser.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registrationDto.Password));
            newUser.PasswordSalt = hmac.Key;

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

        private UserDto CreateUserDto(AppUser user)
        {
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Token = _tokenService.CreateToken(user);

            return userDto;
        }
    }
}
