using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistance.Repositories
{
    internal class GenaricRepository<TModel, TKey>(StoreDbContext dbContext) : IGenaricRepository<TModel, TKey>
        where TModel : BaseEntity<TKey>
    {
        public async Task<IEnumerable<TModel>> GetAllAsync() => await dbContext.Set<TModel>().ToListAsync();
        public async Task<TModel?> GetByIdAsync(TKey id) => await dbContext.Set<TModel>().FindAsync(id);
        public async Task AddAsync(TModel model) => await dbContext.Set<TModel>().AddAsync(model);
        public void DeleteAsync(TModel model) => dbContext.Set<TModel>().Remove(model);
        public void UpdateAsync(TModel model) => dbContext.Set<TModel>().Update(model);
        
    }
}
