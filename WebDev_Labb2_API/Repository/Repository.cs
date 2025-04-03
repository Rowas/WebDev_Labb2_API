using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using WebDev_Labb2_API.Model;

namespace WebDev_Labb2_API.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DBContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DBContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(new ObjectId(id));
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> ExistsAsync(string id)
        {
            return await _dbSet.FindAsync(new ObjectId(id)) != null;
        }
    }
}
