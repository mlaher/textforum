
using System.Linq.Expressions;

namespace textforum.domain.interfaces
{
    public interface ITextForumRepository<T> where T : class
    {
        Task<List<T>> ListAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> orderBy, string correlationId, int pageNumber = 1, int pageSize = 10, bool orderByDirectionDescending = false);
        Task<T?> AddAsync(T entity, string correlationId);
        Task<T?> GetAsync(string correlationId, params object[] keys);
        Task UpdateAsync(T entity, string correlationId);

        Task DeleteAsync(T entity, string correlationId);

        Task<int> GetCountAsync(Expression<Func<T, bool>> filter, string correlationId);
    }
}