using textforum.domain.models;

namespace textforum.domain.interfaces
{
    public interface IPostLikeService
    {
        Task<List<PostLike>> GetPostLikes(long postId, string correlationId, int? pageNumber = 1, int? pageSize = 10);
        Task<int> GetPostLikesCount(long postId, string correlationId);
        Task ToggleLike(long postId, long userId, string correlationId);
    }
}