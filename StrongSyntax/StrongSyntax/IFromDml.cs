using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IFromDml : ICompleteDml
    {
        /// <summary>
        /// Appends a where clause to the query.
        /// </summary>
        /// <param name="filter">Filter to use to create the where clause</param>
        /// <param name="values">Values to use to parametrize the query to avoid SQL injection problems and enable execution plan caching.</param>
        /// <returns></returns>
        ICompleteDml Where(string filter, params object[] values);
    }
}
