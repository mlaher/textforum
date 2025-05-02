
namespace textforum.data.repositories
{
    public interface ITextForumRepository<T> where T : class
    {
        Task<T?> AddAsync(T entity);
        Task<T?> GetAsync(params object[] keys);
        Task UpdateAsync(T entity);
    }
}