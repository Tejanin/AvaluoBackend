﻿using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AvaluoAPI.Presentation.ViewModels;

namespace Avaluo.Infrastructure.Persistence.Repositories.Base
{
    public interface IRepository<TEntity> where TEntity : class
    {
        // GET
        Task<TEntity?> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAllIncluding<TEntity>(
            Expression<Func<TEntity, bool>>? expression,
            params Expression<Func<TEntity, 
            object>>[] includeProperties) 
                where TEntity : class;


        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> FindIncluding<TEntity>(
            Expression<Func<TEntity, bool>>? expression,
            params Expression<Func<TEntity, 
            object>>[] includeProperties) 
                where TEntity : class;

        IEnumerable<TEntity> Filter(IEnumerable<TEntity> entities, List<Func<TEntity, bool>> filters);
        IEnumerable<TEntity> Paginate(IEnumerable<TEntity> entities, int page, decimal recordPerPage);

        Task<List<TEntity>?> FindAllAsync(Expression<Func<TEntity, bool>> expression);

        // POST
        Task AddAsync(TEntity entity);
        void Add(TEntity entity);
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

        // PUT
        Task Update(TEntity entity);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);

        // DELETE 
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);

        // Metodos queryable
        Task<PaginatedResult<TEntity>> PaginateWithQuery(IQueryable<TEntity> query, int? page, int? recordsPerPage);
        IQueryable<TEntity> AsQueryable();
        IQueryable<TEntity> FilterQuery(IQueryable<TEntity> query, List<Expression<Func<TEntity, bool>>> filters);
        IQueryable<TEntity> FindAllQuery(Expression<Func<TEntity, bool>> expression);
    }
}
