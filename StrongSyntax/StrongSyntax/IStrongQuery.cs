using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IStrongQuery<TEntity> : IQuerySyntax
        where TEntity : class
    {
        IStrongQuery<TEntity> Select(Expression<Func<TEntity, object[]>> selector);
        IStrongQuery<TEntity> From();
        IStrongQuery<TEntity> LeftJoin<TJoin>(Expression<Func<TEntity, TJoin, bool>> condition);
        IStrongQuery<TEntity> GroupBy(Expression<Func<TEntity, object[]>> groupings);
    }
}
