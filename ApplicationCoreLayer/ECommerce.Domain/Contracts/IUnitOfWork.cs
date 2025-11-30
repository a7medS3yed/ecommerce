using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Contracts
{
    public interface IUnitOfWork
    {
        IGenaricRepository<TModel, TKey> GenaricRepository<TModel, TKey>()
           where TModel : BaseEntity<TKey>;

        Task<int> SaveChangesAsync();

    }
}
