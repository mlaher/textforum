using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using textforum.domain.interfaces;
using textforum.domain.models;
using textforum.logic.filters;
using textforum.logic.services;

namespace textforum.api.Controllers
{
    [UserAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("GetPosts")]
        public async Task<ActionResult<List<Post>>> GetPosts([FromHeader(Name = "X-App-Token")] string appToken,
            [FromHeader(Name = "X-Forwarded-For")] string ip,
            [FromHeader(Name = "X-Machine-Name")] string machineName,
            [FromHeader(Name = "X-User-Token")] string userToken, 
            int? pageNumber,
            int? pageSize,
            DateTimeOffset startDate,  
            DateTimeOffset endDate)
        {
            return Ok(await _postService.GetPosts(startDate, endDate, pageNumber, pageSize));
        }

        [HttpPost("CreatePost")]
        public async Task<ActionResult<Post>> CreatePost([FromHeader(Name = "X-App-Token")] string appToken,
            [FromHeader(Name = "X-Forwarded-For")] string ip,
            [FromHeader(Name = "X-Machine-Name")] string machineName,
            [FromHeader(Name = "X-User-Token")] string userToken, 
            [FromBody] Post post)
        {
            var result = await _postService.CreatePost(post);

            return Ok(result);
        }
    }
}
