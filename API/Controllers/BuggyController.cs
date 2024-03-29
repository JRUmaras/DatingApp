﻿using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Used for testing error handling (Middleware on server side, interceptors on client side)
    /// </summary>
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "A secret.";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var nullUser = _context.Users.Find(-1);

            if (nullUser is null) return NotFound();

            return Ok(nullUser);
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var nullUser = _context.Users.Find(-1);

            var thingToReturn = nullUser.ToString();

            return thingToReturn;
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was a bad request.");
        }
    }
}
