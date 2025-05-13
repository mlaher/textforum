using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using textforum.api.Helpers;
using textforum.domain.interfaces;
using textforum.domain.models;
using textforum.logic.filters;
using textforum.logic.services;

namespace textforum.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostTagsController : ControllerBase
    {
        private readonly IPostTagService _postTagService;

        public PostTagsController(IPostTagService postTagService)
        {
            _postTagService = postTagService;
        }

        [UserAuth]
        [HttpGet("GetPostTags")]
        public async Task<ActionResult<List<PostTag>>> GetPostTags([FromHeader(Name = "X-App-Token")] string appToken,
            [FromHeader(Name = "X-Forwarded-For")] string ip,
            [FromHeader(Name = "X-Machine-Name")] string machineName,
            [FromHeader(Name = "X-User-Token")] string userToken,
            int? pageNumber,
            int? pageSize,
            long postId)
        {
            return Ok(await _postTagService.GetPostTags(postId, pageNumber, pageSize));
        }

        [ModeratorAuth]
        [HttpPost("AddPostTag")]
        public async Task<ActionResult<PostTag>> AddPostTag([FromHeader(Name = "X-App-Token")] string appToken,
            [FromHeader(Name = "X-Forwarded-For")] string ip,
            [FromHeader(Name = "X-Machine-Name")] string machineName,
            [FromHeader(Name = "X-User-Token")] string userToken,
            PostTag postTag)
        {
            postTag.UserId = HttpHelper.getUserId(this.HttpContext, postTag.UserId);

            var result = await _postTagService.AddPostTag(postTag);

            return Ok(result);
        }
    }
}
