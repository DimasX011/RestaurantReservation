using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.InMemoryDb
{
    public class InMemoryRepository<T> : IInMemoryRepository<T> where T : class
    {
        private readonly ConcurrentBag<T> _repo = new();

        public void AddOrUpdate(T entity)
        {
            _repo.Add(entity);
            Task.Run(() => DeleteOp(entity));
        }

        public IEnumerable<T> Get()
        {
            return _repo;
        }

        public async Task DeleteOp(T entity)
        {
            await Task.Delay(30000);
            if (_repo.TryPeek(out entity) == true)
            {
                _repo.TryTake(out entity);
            }
        }
    }
}
