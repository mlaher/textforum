using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using textforum.logic.services;

namespace textforum.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostLikesController : ControllerBase
    {
        private readonly PostLikeService _postLikeService;
        public PostLikesController(PostLikeService postLikeService)
        {
            _postLikeService = postLikeService;
        }
    }
}
