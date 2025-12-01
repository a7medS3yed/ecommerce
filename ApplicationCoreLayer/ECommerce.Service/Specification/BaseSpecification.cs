using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;

namespace ECommerce.Service.Specification
{
    public abstract class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
    {
        protected BaseSpecification(Expression<Func<TEntity, bool>>? criteria)
        {
            Criteria = criteria;
        }
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];

        public Expression<Func<TEntity, bool>>? Criteria { get; }

        public Expression<Func<TEntity, object>> OrderBy { private set; get; }

        public Expression<Func<TEntity, object>> OrderByDesc { private set; get; }

        protected void AddIncludeExpression(Expression<Func<TEntity, object>> includeExpression)
        {
            IncludeExpressions.Add(includeExpression);
        }

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDescExpression)
        {
            OrderByDesc = orderByDescExpression;
        }
    }
}
