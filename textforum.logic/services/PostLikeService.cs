using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using textforum.domain.interfaces;
using textforum.domain.models;

namespace textforum.logic.services
{
    public class PostLikeService
    {
        ITextForumRepository<data.classes.PostLike> _postLikeRepository;

        public PostLikeService(ITextForumRepository<data.classes.PostLike> postLikeRepository)
        {
            _postLikeRepository = postLikeRepository;
        }

        public async Task ToggleLike(long postId, long userId)
        {
            var existingLike = await _postLikeRepository.GetAsync(postId, userId);

            if (existingLike != null) {
                await _postLikeRepository.DeleteAsync(existingLike);
            }
            else
            {
                await _postLikeRepository.AddAsync(new data.classes.PostLike()
                {
                    PostId = postId,
                    UserId = userId,
                    Timestamp = DateTimeOffset.Now
                });
            }
        }

        public async Task<List<PostLike>> GetPostLikes(long postId, int? pageNumber = 1, int? pageSize = 10)
        {
            if (pageSize == null)
                pageSize = 10;

            if (pageNumber == null)
                pageNumber = 1;

            var results = await _postLikeRepository.ListAsync(a => a.PostId == postId, o => o.Timestamp, pageNumber.Value, pageSize.Value, true);

            return [.. results.Select(s => mapDataPostLikeToModelPostLike(s))];
        }

        public async Task<int> GetPostLikesCount(long postId)
        {
            return await _postLikeRepository.GetCountAsync(a => a.PostId == postId);
        }

        private PostLike mapDataPostLikeToModelPostLike(data.classes.PostLike postLike)
        {
            if (postLike == null)
                return null;

            return new PostLike()
            {
                PostId = postLike.PostId,
                UserId = postLike.UserId,
                Timestamp = postLike.Timestamp
            };
        }
    }
}
