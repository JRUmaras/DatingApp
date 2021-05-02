using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Interfaces.Repositories;
using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IPhotoService photoService, IMapper mapper)
        {
            _userRepository = userRepository;
            _photoService = photoService;
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

        [HttpGet("{username}")]
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
        // TODO: refactor the upload of photo, put the logic into the user repository or dedicated photo repo
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var imageUploadResult = await _photoService.AddPhotoAsync(file);

            if (imageUploadResult.Error is not null) return BadRequest(imageUploadResult.Error.Message);

            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = new Photo
            {
                Url = imageUploadResult.Url.AbsoluteUri,
                IsMain = !user.Photos.Any(),
                PublicId = imageUploadResult.PublicId
            };

            user.Photos.Add(photo);
            if (await _userRepository.SaveAllAsync()) return _mapper.Map<PhotoDto>(photo);

            return BadRequest("Unexpected photo upload error.");
        }
    }
}
