using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using textforum.logic.helpers;
using textforum.domain.interfaces;
using textforum.domain.models;
using textforum.logic.filters;
using textforum.logic.services;

namespace textforum.api.Controllers
{
    [UserAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class PostCommentsController : ControllerBase
    {
        private readonly IPostCommentService _postCommentService;

        public PostCommentsController(IPostCommentService postCommentService)
        {
            _postCommentService = postCommentService;
        }

        [HttpGet("GetPostComments")]
        public async Task<ActionResult<List<PostComment>>> GetPostComments([FromHeader(Name = "X-App-Token")] string appToken,
            [FromHeader(Name = "X-Forwarded-For")] string ip,
            [FromHeader(Name = "X-Machine-Name")] string machineName,
            [FromHeader(Name = "X-User-Token")] string userToken,
            int? pageNumber,
            int? pageSize,
            long postId)
        {
            return Ok(await _postCommentService.GetPostComments(postId, HttpContext.GetCorrelationId(), pageNumber, pageSize));
        }

        [HttpPost("AddComment")]
        public async Task<ActionResult<PostComment>> AddComment([FromHeader(Name = "X-App-Token")] string appToken,
            [FromHeader(Name = "X-Forwarded-For")] string ip,
            [FromHeader(Name = "X-Machine-Name")] string machineName,
            [FromHeader(Name = "X-User-Token")] string userToken,
            PostComment comment)
        {
            comment.UserId = HttpContext.GetUserId(comment.UserId);

            var result = await _postCommentService.CreateComment(comment, HttpContext.GetCorrelationId());

            return Ok(result);
        }
    }
}
