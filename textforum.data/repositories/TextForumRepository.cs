using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using textforum.data.contexts;
using textforum.domain.interfaces;
using textforum.domain.models;

namespace textforum.data.repositories
{
    public class TextForumRepository<T> : ITextForumRepository<T> where T : class
    {
        private readonly TextForumDatabaseContext _textForumDatabaseContext;
        private readonly DbSet<T> _dbSet;

        public TextForumRepository(TextForumDatabaseContext textForumDatabaseContext)
        {
            _textForumDatabaseContext = textForumDatabaseContext;
            _dbSet = textForumDatabaseContext.Set<T>();
        }

        public async Task<List<T>> ListAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> orderBy, int pageNumber = 1, int pageSize = 10, bool orderByDirectionDescending = false)
        {
            IQueryable<T> query = _dbSet;

            if (!orderByDirectionDescending)
            {
                query = _dbSet.OrderBy(orderBy);
            }
            else
            {
                query = _dbSet.OrderByDescending(orderBy);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var items = await query
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

            return items;
        }

        public async Task<T?> GetAsync(params object[] keys)
        {
            return await _dbSet.FindAsync(keys);
        }

        public async Task<T?> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);

            await _textForumDatabaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _textForumDatabaseContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _textForumDatabaseContext.SaveChangesAsync();
        }

        public async Task<int> GetCountAsync(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }
    }
}
