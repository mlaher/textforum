﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using textforum.logic.helpers;
using textforum.domain.interfaces;
using textforum.domain.models;
using textforum.logic.filters;

namespace textforum.api.Controllers
{
    [AppAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("RegisterUser")]
        public async Task<ActionResult<User?>> RegisterUser([FromHeader(Name = "X-App-Token")] string appToken,
            [FromHeader(Name = "X-Forwarded-For")] string ip,
            [FromHeader(Name = "X-Machine-Name")] string machineName,
            [FromBody] User user)
        {
            var result = await _userService.Register(user, HttpContext.GetCorrelationId());

            return result;
        }
    }
}
