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
        #region Including
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];
        protected void AddIncludeExpression(Expression<Func<TEntity, object>> includeExpression)
        {
            IncludeExpressions.Add(includeExpression);
        }

        #endregion

        #region Filteration
        public Expression<Func<TEntity, bool>>? Criteria { get; }
        #endregion

        #region Ordering
        public Expression<Func<TEntity, object>> OrderBy { private set; get; }

        public Expression<Func<TEntity, object>> OrderByDesc { private set; get; } 
        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDescExpression)
        {
            OrderByDesc = orderByDescExpression;
        }
        #endregion

        #region Pagination
        public int Skip { private set; get; }

        public int Take { private set; get; }

        public bool IsPagingEnabled { private set; get; }

        protected void ApplyPaging(int pageSize, int pageIndex)
        {
            IsPagingEnabled = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }
        #endregion


    }
}
