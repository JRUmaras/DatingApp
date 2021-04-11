using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            // Initial approach - download all and map in memory
            //var users = await _userRepository.GetUsersAsync();
            //var memberDtos = _mapper.Map<IEnumerable<MemberDto>>(users);

            // Slightly better approach - download only what is needed and map in memory
            var users = await _userRepository.GetMemberDtosAsync();

            return Ok(users);
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<MemberDto>> GetUser(int id)
        {
            // Initial approach - download all fields and map in memory
            // var user = await _userRepository.GetUserByIdAsync(id);
            //return _mapper.Map<MemberDto>(user);

            // Slightly better approach - download only what is needed and map in memory
            var user = await _userRepository.GetMemberDtoByIdAsync(id);

            return user;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            // Initial approach - download all fields and map in memory
            //var user = await _userRepository.GetUserByNameAsync(username);
            //return _mapper.Map<MemberDto>(user);

            // Slightly better approach - download only what is needed and map in memory
            var user = await _userRepository.GetMemberDtoNameAsync(username);

            return user;
        }
    }
}
