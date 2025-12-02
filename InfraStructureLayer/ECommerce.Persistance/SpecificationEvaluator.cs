using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistance
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(
          IQueryable<TEntity> entryPoint,
          ISpecification<TEntity, TKey> specification)
          where TEntity : BaseEntity<TKey>
        {
            var query = entryPoint;

            if (specification is not null)
            {
                if (specification.Criteria is not null)
                {
                    query = query.Where(specification.Criteria);
                }
                if (specification.IncludeExpressions is not null && specification.IncludeExpressions.Any())
                {
                    query = specification.IncludeExpressions
                        .Aggregate(query, (current, includeExpression) => current.Include(includeExpression));
                }
                if (specification.OrderBy is not null)
                {
                    query = query.OrderBy(specification.OrderBy);
                }
                if (specification.OrderByDesc is not null)
                {
                    query = query.OrderByDescending(specification.OrderByDesc);
                }
                if (specification.IsPagingEnabled)
                {
                    query = query.Skip(specification.Skip).Take(specification.Take);
                }
            }

            return query;
        }

    }
}