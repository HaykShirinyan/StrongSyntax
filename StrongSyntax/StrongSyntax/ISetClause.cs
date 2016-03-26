using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface ISetClause : ICompleteDml
    {
        /// <summary>
        /// Appends a where clause to the query.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        ICompleteDml Where(string filter, params object[] values);
    }
}
