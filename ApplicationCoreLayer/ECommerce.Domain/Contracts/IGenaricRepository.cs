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
        Task<IEnumerable<TModel>> GetAllAsync(ISpecification<TModel, TKey> specification);
        Task<TModel?> GetByIdAsync(TKey id);
        Task<TModel?> GetByIdAsync(ISpecification<TModel, TKey> specification);
        Task AddAsync(TModel model);
        void UpdateAsync(TModel model);
        void DeleteAsync(TModel model);
        Task<int> CountAsync(ISpecification<TModel, TKey> specification);
    }
}
