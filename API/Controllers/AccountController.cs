using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;

        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserTokenDto>> Register(UserRegistrationDto registrationDto)
        {
            if (await UsernameExistsInDb(registrationDto.Username)) return BadRequest("Username already exists.");

            using var hmac = new HMACSHA512();

            var newUser = new AppUser
            {
                UserName = registrationDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registrationDto.Password)),
                PasswordSalt = hmac.Key
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return CreateUserTokenDto(newUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserTokenDto>> Login(UserLoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(usr => usr.UserName == loginDto.Username);

            if (user is null) return Unauthorized("Invalid username.");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            if (!computedHash.SequenceEqual(user.PasswordHash)) return Unauthorized("Invalid password.");

            return CreateUserTokenDto(user);
        }

        private async Task<bool> UsernameExistsInDb(string username) => await _context.Users.AnyAsync(user => user.UserName == username.ToLower());

        private UserTokenDto CreateUserTokenDto(AppUser user) => new()
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }
}
