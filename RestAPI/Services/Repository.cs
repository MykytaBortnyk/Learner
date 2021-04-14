using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestAPI.Data;
using RestAPI.Interfaces;
using RestAPI.Models;

namespace RestAPI.Services
{
    public class Repository<T> : IRepository<T>, IDisposable where T : class
    {
        private readonly AppDbContext _appContext;

        public Repository(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public void Create(T item)
        {
            _appContext.Set<T>().Add(item);
        }

        public void Delete(T item)
        {
            _appContext.Set<T>().Remove(item);
        }

        public IEnumerable<T> Get() => _appContext.Set<T>().ToList();

        public async Task<IEnumerable<T>> GetAsync() => await _appContext.Set<T>().ToListAsync();

        public T Get(Guid id) => _appContext.Set<T>().Find(id);

        public async Task<T> GetAsync(Guid id) => await _appContext.Set<T>().FindAsync(id);

        public async Task UpdateAsync(T item)
        {
            _appContext.Set<T>().Update(item);

            await _appContext.SaveChangesAsync();
        }

        public void Save() => _appContext.SaveChanges();
        public async Task SaveAsync() => await _appContext.SaveChangesAsync();

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _appContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

//namespace ContosoUniversity.DAL
//{
//    public class GenericRepository<T> where T : class
//    {
//        internal AppDbContext _context;
//        internal DbSet<T> dbSet;

//        public GenericRepository(AppDbContext context)
//        {
//            this._context = context;
//            this.dbSet = context.Set<T>();
//        }

//        public virtual IEnumerable<T> Get(
//            Expression<Func<T, bool>> filter = null,
//            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
//            string includeProperties = "")
//        {
//            IQueryable<T> query = dbSet;

//            if (filter != null)
//            {
//                query = query.Where(filter);
//            }

//            foreach (var includeProperty in includeProperties.Split
//                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
//            {
//                query = query.Include(includeProperty);
//            }

//            if (orderBy != null)
//            {
//                return orderBy(query).ToList();
//            }
//            else
//            {
//                return query.ToList();
//            }
//        }

//        public virtual T GetByID(object id)
//        {
//            return dbSet.Find(id);
//        }

//        public virtual void Insert(T entity)
//        {
//            dbSet.Add(entity);
//        }

//        public virtual void Delete(object id)
//        {
//            T entityToDelete = dbSet.Find(id);
//            Delete(entityToDelete);
//        }

//        public virtual void Delete(T entityToDelete)
//        {
//            if (_context.Entry(entityToDelete).State == EntityState.Detached)
//            {
//                dbSet.Attach(entityToDelete);
//            }
//            dbSet.Remove(entityToDelete);
//        }

//        public virtual void Update(T entityToUpdate)
//        {
//            dbSet.Attach(entityToUpdate);
//            _context.Entry(entityToUpdate).State = EntityState.Modified;
//        }
//    }
//}