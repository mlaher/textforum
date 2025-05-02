using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using textforum.data.contexts;

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
    }
}
