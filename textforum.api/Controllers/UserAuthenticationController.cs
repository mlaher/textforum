using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using textforum.domain.interfaces;
using textforum.domain.models;
using textforum.logic.filters;

namespace textforum.api.Controllers
{
    [AppAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IUserAuthenticationService _userAuthenticationService;

        public UserAuthenticationController(IUserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
        }

        [HttpPost("GetUserToken")]
        public async Task<ActionResult<string>> GetUserToken(
            [FromHeader(Name = "X-App-Token")] string appToken, 
            [FromHeader(Name = "X-Forwarded-For")] string ip, 
            [FromHeader(Name = "X-Machine-Name")] string machineName,
            [FromBody] Login credentials)
        {
            var result = await _userAuthenticationService.AuthenticateUser(credentials.Email, credentials.Password);

            if (!result.isValid)
                return Unauthorized();

            return Ok(result.token);
        }

        [HttpPost("VerifyTokenValidity")]
        public async Task<bool> VerifyTokenValidity([FromHeader(Name = "X-App-Token")] string appToken,
            [FromHeader(Name = "X-Forwarded-For")] string ip,
            [FromHeader(Name = "X-Machine-Name")] string machineName,
            [FromBody] string userToken)
        {
            var result = await _userAuthenticationService.GetClaims(userToken);

            return result.isValid;
        }

        [HttpPost("GetClaims")]
        public async Task<ActionResult<IDictionary<string, string>>> GetClaims([FromHeader(Name = "X-App-Token")] string appToken,
            [FromHeader(Name = "X-Forwarded-For")] string ip,
            [FromHeader(Name = "X-Machine-Name")] string machineName,
            [FromBody] string userToken)
        {
            var result = await _userAuthenticationService.GetClaims(userToken);

            if (!result.isValid)
                return Unauthorized();

            return Ok(result.claims);
        }
    }
}
