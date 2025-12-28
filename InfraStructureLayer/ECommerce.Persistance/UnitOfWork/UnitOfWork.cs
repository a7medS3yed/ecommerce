using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Persistance.Data;

namespace ECommerce.Persistance.UnitOfWork
{
    public class UnitOfWork(StoreDbContext dbContext) : IUnitOfWork
    {
        private readonly ConcurrentDictionary<string, object> _repositories = new();
        public IGenaricRepository<TModel, TKey> GenaricRepository<TModel, TKey>() where TModel : BaseEntity<TKey>
        {
           return (IGenaricRepository<TModel, TKey>)_repositories.GetOrAdd(typeof(TModel).Name,
                _ => new Repositories.GenaricRepository<TModel, TKey>(dbContext));
        }

        public async Task<int> SaveChangesAsync() => await dbContext.SaveChangesAsync();
    }
}
