using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface ICompleteDml : IQuerySyntax
    {
        /// <summary>
        /// Executes a DML query in the database.
        /// </summary>
        /// <returns>Number of the rows affected by the query.</returns>
        int Execute();

        /// <summary>
        /// asynchronously executes a DML query in the database.
        /// </summary>
        /// <returns>Number of the rows affected by the query.</returns>
        Task<int> ExecuteAsync();
    }
}
