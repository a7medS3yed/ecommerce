using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Contracts
{
    public interface IGenaricRepository<TModel, TKey> where TModel : BaseEntity<TKey>
    {
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel?> GetByIdAsync(TKey id);
        Task AddAsync(TModel model);
        void UpdateAsync(TModel model);
        void DeleteAsync(TModel model);
    }
}
