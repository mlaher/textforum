using textforum.domain.models;

namespace textforum.domain.interfaces
{
    public interface IPostCommentService
    {
        Task<PostComment> CreateComment(PostComment postComment);
        Task<List<PostComment>> GetPostComments(long postId, int? pageNumber = 1, int? pageSize = 10, bool? latestFirst = true);
    }
}