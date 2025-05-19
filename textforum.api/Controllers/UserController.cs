using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using textforum.logic.helpers;
using textforum.domain.interfaces;
using textforum.domain.models;
using textforum.logic.filters;
using textforum.domain.exceptions;

namespace textforum.api.Controllers
{
    [AppAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

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
            try
            {
                var result = await _userService.Register(user, HttpContext.GetCorrelationId());

                return result;
            }
            catch (UserException ux)
            {
                _logger.LogError("Error Detail: {techError}", ux.GetTechnicalErrorDetails());
                return BadRequest(ux.GetFriendlyErrorDetails());
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new FriendlyError() { HasError=true, Message="An unknown exception has occurred. Please report.", 
                CorrelationId = HttpContext.GetCorrelationId() });
            }
            
        }
    }
}
