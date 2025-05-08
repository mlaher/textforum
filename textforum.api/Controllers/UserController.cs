using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using textforum.logic.services;

namespace textforum.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
    }
}
