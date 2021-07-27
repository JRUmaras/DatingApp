using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // TODO: Allow users to undo a like
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;

        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _userRepository = userRepository;
            _likesRepository = likesRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var liker = await _likesRepository.GetUserWithLikes(User.GetUserId());
            var likee = await _userRepository.GetUserByUsernameAsync(username);

            var erroneousActionResult = await ValidateLikeAsync(liker, likee);
            if (erroneousActionResult is not null) return erroneousActionResult;

            var userLike = new UserLike
            {
                LikerId = liker.Id,
                LikeeId = likee.Id
            };

            liker.LikedUsers.Add(userLike);

            // TODO: figure out a better way to save changes, as we now have two repos, each of them could potentially save the changes
            if (!await _userRepository.SaveAllAsync()) return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesSettings likesSettings)
        {
            likesSettings.UserId = User.GetUserId();

            var paginatedLikeDtos = await _likesRepository.GetUserLikes(likesSettings);
            
            Response.AddPaginationHeader(paginatedLikeDtos.PageNumber, paginatedLikeDtos.PageSize, paginatedLikeDtos.TotalCount, paginatedLikeDtos.TotalPages);
            
            return Ok(paginatedLikeDtos);
        }

        private async Task<ActionResult> ValidateLikeAsync(AppUser liker, AppUser likee)
        {
            if (likee is null) return NotFound();
            if (likee.Id == liker.Id) return BadRequest("We know you like yourself and we like you too, but you cannot explicitly like yourself here...");

            var userLikeInDb = await _likesRepository.GetUserLike(liker.Id, likee.Id);
            if (userLikeInDb is not null) return BadRequest("You've already liked this member.");

            return null;
        }
    }
}
