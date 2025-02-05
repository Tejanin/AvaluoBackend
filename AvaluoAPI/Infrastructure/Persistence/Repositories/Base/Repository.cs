using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Persistence.Repositories.Base
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        public Repository(DbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            return entities;
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }


        public async Task<IEnumerable<TEntity>?> FindAllAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>().Where(expression).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

       

        public async Task<IEnumerable<TEntity>> GetAllIncluding<TEntity>(Expression<Func<TEntity, bool>>? expression,
        params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            if (expression != null)
            {
                query = query.Where(expression);
            }

            return await query.ToListAsync();
        }

        public async Task Update(TEntity entity)
        {
             _context.Set<TEntity>().Update(entity);

        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(expression);
        }

        public IEnumerable<TEntity> Paginate(IEnumerable<TEntity> entities, int page, decimal recordPerPage)
        {
            if (page == 0) return entities;

            var records = entities
                .Skip((page - 1) * ((int)recordPerPage))
                .Take((int)recordPerPage)
                .ToList();

            return records;

        }

        public IEnumerable<TEntity> Filter(IEnumerable<TEntity> entities, List<Func<TEntity, bool>> filters)
        {
            if (filters == null || !filters.Any())
            {
                return entities.ToList();
            }

            var filteredEntities = entities;

            foreach (var filter in filters)
            {
                filteredEntities = filteredEntities.Where(filter).ToList();
            }

            return filteredEntities;
        }

        public async Task<TProperty> FindIncluding<TProperty>(Expression<Func<TProperty, bool>>? expression, params Expression<Func<TProperty, object>>[] includeProperties) where TProperty : class
        {
            IQueryable<TProperty> query = _context.Set<TProperty>();

            if (expression != null)
            {
                query = query.Where(expression);
            }
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }



            return await query.FirstOrDefaultAsync();
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }


    }
}
