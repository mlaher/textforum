





namespace textforum.domain.interfaces
{
    public interface IPostService
    {
        Task<domain.models.Post> CreatePost(domain.models.Post post, string correlationId);
        Task<domain.models.Post> GetPost(long postId, string correlationId);
        Task<List<domain.models.Post>?> GetPosts(DateTimeOffset? startDate, DateTimeOffset? endDate, string correlationId, int? pageNumber = 1, int? pageSize = 10);
    }
}