using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IQueryReader<TEntity>
        where TEntity : class, new()
    {
        ICollection<TEntity> Read();
        ICollection<TDestination> Project<TDestination>(Func<TEntity, TDestination> projection) where TDestination : class;
    }
}
