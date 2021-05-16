using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Errors.Data.Repositories;
using API.Extensions;
using API.Interfaces;
using API.Interfaces.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var users = await _userRepository.GetMemberDtosAsync();

            return Ok(users);
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<MemberDto>> GetUser(int id)
        {
            var user = await _userRepository.GetMemberDtoByIdAsync(id);

            return user;
        }

        [HttpGet("{username}", Name = "GetUserByUsername")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await _userRepository.GetMemberDtoByUsernameAsync(username);

            return user;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var saveSuccess = await _userRepository.UpdateUserByUsernameAsync(User.GetUsername(), memberUpdateDto);

            if (saveSuccess) return NoContent();

            return BadRequest("Failed to save the user.");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var username = User.GetUsername();

            var uploadedPhoto = await _userRepository.AddPhotoAsync(username, file);

            var uploadedPhotoDto = _mapper.Map<PhotoDto>(uploadedPhoto);

            return CreatedAtRoute("GetUserByUsername", new { username }, uploadedPhotoDto);
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var username = User.GetUsername();

            var success = await _userRepository.SetMainPhotoAsync(username, photoId);

            return success ? NoContent() : BadRequest("Unexpected error encountered while setting the main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var username = User.GetUsername();

            var success = await _userRepository.DeletePhotoAsync(username, photoId);

            return success ? NoContent() : BadRequest();
        }
    }
}
