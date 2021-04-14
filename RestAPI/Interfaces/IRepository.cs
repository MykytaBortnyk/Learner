using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestAPI.Models;

namespace RestAPI.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> Get();
        Task<IEnumerable<T>> GetAsync();
        T Get(Guid id);
        Task<T> GetAsync(Guid id);
        void Create(T item);
        Task UpdateAsync(T item);
        void Delete(T item);
    }
}