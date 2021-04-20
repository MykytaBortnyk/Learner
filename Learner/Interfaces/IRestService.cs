using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Learner
{
    public interface IRestService<T> where T : class
    {
        public Task<List<T>> GetAsync();
        public Task<T> GetByIdAsync(Guid id);
        public Task PostAsync(T item);
        public Task Put(T item);
        public Task DeleteAsync(Guid id);
    }
}