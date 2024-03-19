using Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly AppDbContext _context = null;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task Insert(T obj)
        {
            await _context.Set<T>().AddAsync(obj);
        }

        public async Task Delete(object id)
        {
            T existing = await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(existing);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }


        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        //Options:
        //public void Update(T entity)
        //{
        //    var entry = dbContext.Entry(entity);
        //    if (entry == null || entry.State == EntityState.Detached)
        //    {
        //        targetDbSet.Attach(entity);
        //    }
        //    entry.State = EntityState.Modified;
        //}
        public async Task<T> Update(T obj, object key)
        {
            if (obj == null)
                return null;

            T existing = await _context.Set<T>().FindAsync(key);
            if (existing != null)
            {
                 _context.Entry(existing).CurrentValues.SetValues(obj);
                await _context.SaveChangesAsync();
            }

            return existing;
        }

        public IQueryable<T> List()
        {
            return _context.Set<T>();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = List();

            foreach (var property in includeProperties)
            {
                queryable = queryable.Include<T, object>(property);
            }

            return queryable;
        }
    }
}
