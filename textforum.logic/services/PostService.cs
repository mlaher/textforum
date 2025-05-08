using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using textforum.domain.interfaces;
using textforum.domain.models;

namespace textforum.logic.services
{
    public class PostService : IPostService
    {
        ITextForumRepository<data.classes.Post> _postRepository;

        public PostService(ITextForumRepository<data.classes.Post> postRepository)
        {
            _postRepository = postRepository;
        }

        /// <summary>
        /// Oversimplified post listing, it will just list a list of posts based on date range, which will not work practically in real life, it needs to be tailored in real life
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<List<Post>?> GetPosts(DateTimeOffset? startDate, DateTimeOffset? endDate, int? pageNumber = 1, int? pageSize = 10)
        {
            if (startDate == null)
                startDate = DateTimeOffset.Now.AddDays(-7);

            if (endDate == null)
                endDate = DateTimeOffset.Now;

            if (pageSize == null)
                pageSize = 10;

            if (pageNumber == null)
                pageNumber = 1;

            var results = await _postRepository.ListAsync(x => x.Timestamp >= startDate.Value && x.Timestamp <= endDate.Value, o => o.Timestamp, pageNumber.Value, pageSize.Value, true);

            return [.. results.Select(s => mapDataPostToModelPost(s))];
        }

        public async Task<Post> GetPost(long postId)
        {
            var result = await _postRepository.GetAsync(postId);

            return mapDataPostToModelPost(result);
        }

        public async Task<Post> CreatePost(Post post)
        {
            var result = await _postRepository.AddAsync(new data.classes.Post()
            {
                Content = post.Content,
                Timestamp = DateTimeOffset.Now,
                UserId = post.UserId
            });

            return mapDataPostToModelPost(result);
        }

        private Post mapDataPostToModelPost(data.classes.Post post)
        {
            if (post == null)
                return null;

            return new Post()
            {
                UserId = post.UserId,
                Content = post.Content,
                PostId = post.PostId,
                Timestamp = post.Timestamp
            };
        }
    }
}
