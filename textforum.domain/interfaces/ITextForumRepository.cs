
using System.Linq.Expressions;

namespace textforum.domain.interfaces
{
    public interface ITextForumRepository<T> where T : class
    {
        Task<List<T>> ListAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> orderBy, int pageNumber = 1, int pageSize = 10, bool orderByDirectionDescending = false);
        Task<T?> AddAsync(T entity);
        Task<T?> GetAsync(params object[] keys);
        Task UpdateAsync(T entity);
    }
}