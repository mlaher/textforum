





namespace textforum.domain.interfaces
{
    public interface IPostService
    {
        Task<domain.models.Post> CreatePost(domain.models.Post post);
        Task<domain.models.Post> GetPost(long postId);
        Task<List<domain.models.Post>?> GetPosts(DateTimeOffset? startDate, DateTimeOffset? endDate, int? pageNumber = 1, int? pageSize = 10);
    }
}