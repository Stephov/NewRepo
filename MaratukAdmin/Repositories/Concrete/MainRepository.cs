﻿using MaratukAdmin.Business.Models.Common;
using MaratukAdmin.Entities;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class MainRepository<T> : IMainRepository<T> where T : BaseDbEntity, new()
    {
        protected readonly MaratukDbContext _context;
        private readonly DbSet<T> _entities;

        public MainRepository(MaratukDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public virtual async Task<List<T>> GetAllAsync(params string[] includes)
        {
            try
            {
                var query = ProcessQuery(_entities, includes);
                return await query.ToListAsync();
            }catch(Exception e)
            {
                string s = e.Message;
            }
            return await _entities.ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            try
            {
                _entities.Add(entity);
                await _context.SaveChangesAsync();
            }catch(Exception e)
            {
                string s = e.Message;
            }
            

            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            if (entity != null)
            {
                _entities.Remove(entity);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            try
            {
                _entities.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return entity;
            }catch(Exception e) {
                string s = e.Message;

            }

            _entities.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return entity;

        }

        public virtual async Task<T> GetAsync(int id, params string[] includes)
        {
            var query = ProcessQuery(_entities.AsQueryable<T>(), includes);
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<T> GetAsNoTrackingAsync(int id, params string[] includes)
        {
            var query = ProcessQuery(_entities.AsNoTracking(), includes);
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        private IQueryable<T> ProcessQuery(IQueryable<T> query, params string[] includes)
        {
            foreach (string include in includes)
            {
                query = _entities.Include(include);
            }

            return query;
        }

        //public async Task ResetTableIdentityAsync(T entity)
        //{
        //    //var tableName = _dbContext.Model.FindEntityType(typeof(entity)).GetTableName();
        //    var tableName = entity.GetType().Name;

        //    await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('" + tableName + "', RESEED, 0)");

        //}
    }
}
