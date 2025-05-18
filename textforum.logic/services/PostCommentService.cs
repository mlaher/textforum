using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using textforum.domain.interfaces;
using textforum.domain.models;

namespace textforum.logic.services
{
    public class PostCommentService : IPostCommentService
    {
        ITextForumRepository<data.classes.PostComment> _postCommentRepository;

        public PostCommentService(ITextForumRepository<data.classes.PostComment> postCommentRepository)
        {
            _postCommentRepository = postCommentRepository;
        }

        public async Task<List<PostComment>> GetPostComments(long postId, string correlationId, int? pageNumber = 1, int? pageSize = 10, bool? latestFirst = true)
        {
            if (pageSize == null)
                pageSize = 10;

            if (pageNumber == null)
                pageNumber = 1;

            if (latestFirst == null)
                latestFirst = true;

            var result = await _postCommentRepository.ListAsync(x => x.PostId == postId, o => o.Timestamp, correlationId, pageNumber.Value, pageSize.Value, latestFirst.Value);

            return [.. result.Select(s => mapDataPostCommentToModelPostComment(s))];
        }

        public async Task<PostComment> CreateComment(PostComment postComment, string correlationId)
        {
            if(string.IsNullOrWhiteSpace(postComment.Content))
            {
                throw new InvalidOperationException("Content cannot be blank");
            }

            var result = await _postCommentRepository.AddAsync(new data.classes.PostComment()
            {
                Content = postComment.Content,
                PostId = postComment.PostId,
                Timestamp = DateTimeOffset.Now,
                UserId = postComment.UserId
            }, correlationId);

            return mapDataPostCommentToModelPostComment(result);
        }

        private PostComment mapDataPostCommentToModelPostComment(data.classes.PostComment postComment)
        {
            if (postComment == null)
                return null;

            return new PostComment()
            {
                PostId = postComment.PostId,
                Content = postComment.Content,
                PostCommentId = postComment.PostId,
                Timestamp = postComment.Timestamp,
                UserId = postComment.UserId
            };
        }
    }
}
