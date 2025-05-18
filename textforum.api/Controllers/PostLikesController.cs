﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using textforum.logic.helpers;
using textforum.domain.interfaces;
using textforum.domain.models;
using textforum.logic.services;

namespace textforum.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostLikesController : ControllerBase
    {
        private readonly IPostLikeService _postLikeService;
        public PostLikesController(IPostLikeService postLikeService)
        {
            _postLikeService = postLikeService;
        }

        [HttpGet("GetPostLikes")]
        public async Task<ActionResult<List<PostLike>>> GetPostsLikes([FromHeader(Name = "X-App-Token")] string appToken,
            [FromHeader(Name = "X-Forwarded-For")] string ip,
            [FromHeader(Name = "X-Machine-Name")] string machineName,
            [FromHeader(Name = "X-User-Token")] string userToken,
            int? pageNumber,
            int? pageSize,
            long postId)
        {
            return Ok(await _postLikeService.GetPostLikes(postId, HttpContext.GetCorrelationId(), pageNumber, pageSize));
        }

        [HttpGet("GetPostLikeCount")]
        public async Task<ActionResult<List<int>>> GetPostsLikeCount([FromHeader(Name = "X-App-Token")] string appToken,
            [FromHeader(Name = "X-Forwarded-For")] string ip,
            [FromHeader(Name = "X-Machine-Name")] string machineName,
            [FromHeader(Name = "X-User-Token")] string userToken,
            long postId)
        {
            return Ok(await _postLikeService.GetPostLikesCount(postId, HttpContext.GetCorrelationId()));
        }

        [HttpPost("ToggleUserLike")]
        public async Task<ActionResult<List<int>>> ToggleUserLike([FromHeader(Name = "X-App-Token")] string appToken,
            [FromHeader(Name = "X-Forwarded-For")] string ip,
            [FromHeader(Name = "X-Machine-Name")] string machineName,
            [FromHeader(Name = "X-User-Token")] string userToken,
            PostLike like)
        {
            like.UserId = HttpContext.GetUserId(like.UserId);

            await _postLikeService.ToggleLike(like.PostId, like.UserId, HttpContext.GetCorrelationId());
            return Ok();
        }

    }
}
