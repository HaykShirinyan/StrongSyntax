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
        /// <summary>
        /// Reads data from the database.
        /// </summary>
        /// <returns></returns>
        IList<TEntity> Read();

        /// <summary>
        /// Asynchronously data from the database.
        /// </summary>
        /// <returns></returns>
        Task<IList<TEntity>> ReadAsync();

        /// <summary>
        /// Projects each returned datum to a new object.
        /// </summary>
        /// <typeparam name="TDestination">Destination type of the object to project the data to.</typeparam>
        /// <param name="projection">Delegate that will do the mapping of database returned records to the new object.</param>
        /// <returns></returns>
        IList<TDestination> Project<TDestination>(Func<TEntity, TDestination> projection) where TDestination : class;

        /// <summary>
        /// Asynchronously projects each returned datum to a new object.
        /// </summary>
        /// <typeparam name="TDestination">Destination type of the object to project the data to.</typeparam>
        /// <param name="projection">Delegate that will do the mapping of database returned records to the new object.</param>
        /// <returns></returns>
        Task<IList<TDestination>> ProjectAsync<TDestination>(Func<TEntity, TDestination> projection) where TDestination : class;
    }
}
