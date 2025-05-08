using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using textforum.logic.services;

namespace textforum.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostCommentsController : ControllerBase
    {
        private readonly PostCommentService _postCommentService;

        public PostCommentsController(PostCommentService postCommentService)
        {
            _postCommentService = postCommentService;
        }
    }
}
