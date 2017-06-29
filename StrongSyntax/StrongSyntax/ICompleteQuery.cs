using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface ICompleteQuery : IQuerySyntax
    {
        /// <summary>
        /// Prepares a data reader that can actually execute the generated query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the POCO object to populate when reading data.</typeparam>
        /// <returns></returns>
        IQueryReader<TEntity> PrepareReader<TEntity>() where TEntity : class, new();

        /// <summary>
        /// Prepares a data reader that can actually execute the generated query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the POCO object to populate when reading data.</typeparam>
        /// <param name="entityName">The name of the TEntity argument.</param>
        /// <returns></returns>
        IQueryReader<TEntity> PrepareReader<TEntity>(string entityName) where TEntity : class, new();

        /// <summary>
        /// Returns the SQL query.
        /// </summary>
        /// <returns></returns>
        string ToString();

        /// <summary>
        /// Return the SQL query with an alias.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        string ToString(string alias);
    }
}
