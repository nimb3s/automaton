using Nimb3s.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Core.Repositories.Sql.InMemory
{
    public class InMemoryRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        private readonly List<TEntity> store;

        public InMemoryRepository()
        {
            store = new List<TEntity>();
        }

        public Task DeleteAsync(TEntity entity)
        {
            store.RemoveAll(i => i.Id.Equals(entity.Id));

            return Task.CompletedTask;
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return Task.FromResult(store.AsEnumerable());
        }

        public Task<TEntity> GetAsync(TKey Id)
        {
            var entity = store.FirstOrDefault(i => i.Id.Equals(Id));

            return Task.FromResult(entity);
        }

        public Task UpsertAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
