using textforum.domain.models;

namespace textforum.domain.interfaces
{
    public interface IPostCommentService
    {
        Task<PostComment> CreateComment(PostComment postComment, string correlationId);
        Task<List<PostComment>> GetPostComments(long postId, string correlationId, int? pageNumber = 1, int? pageSize = 10, bool? latestFirst = true);
    }
}