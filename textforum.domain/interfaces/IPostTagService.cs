using textforum.domain.models;

namespace textforum.domain.interfaces
{
    public interface IPostTagService
    {
        Task<List<PostTag>> GetPostTags(long postId, int? pageNumber = 1, int? pageSize = 10);
        Task<PostTag> AddPostTag(PostTag postTag);
    }
}