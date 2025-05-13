using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using textforum.domain.interfaces;
using textforum.domain.models;

namespace textforum.logic.services
{
    public class PostTagService : IPostTagService
    {
        ITextForumRepository<data.classes.PostTag> _postTagRepository;

        public PostTagService(ITextForumRepository<data.classes.PostTag> postTagRepository)
        {
            _postTagRepository = postTagRepository;
        }

        public async Task<List<PostTag>> GetPostTags(long postId, int? pageNumber = 1, int? pageSize = 10)
        {
            if (pageSize == null)
                pageSize = 10;

            if (pageNumber == null)
                pageNumber = 1;

            var result = await _postTagRepository.ListAsync(x => x.PostId == postId, o => o.Timestamp, pageNumber.Value, pageSize.Value, true);

            return [.. result.Select(s => mapDataPostTagToModelPostTag(s))];
        }

        public async Task<PostTag> AddPostTag(PostTag postTag)
        {
            var existingTag = await _postTagRepository.GetAsync(postTag.Tag, postTag.PostId);

            if(existingTag == null)
            {
                var result = await _postTagRepository.AddAsync(new data.classes.PostTag
                {
                    PostId = postTag.PostId,
                    Tag = postTag.Tag,
                    Description = postTag.Description,
                    Timestamp = DateTimeOffset.Now,
                    UserId = postTag.UserId
                });

                return mapDataPostTagToModelPostTag(result);
            }

            return mapDataPostTagToModelPostTag(existingTag);
        }

        private PostTag mapDataPostTagToModelPostTag(data.classes.PostTag postTag)
        {
            if (postTag == null)
                return null;

            return new PostTag()
            {
                PostId = postTag.PostId,
                Description = postTag.Description,
                Tag = postTag.Tag,
                Timestamp = postTag.Timestamp,
                UserId = postTag.UserId
            };
        }


    }
}
