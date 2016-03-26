using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IInsertQuery : IQuerySyntax
    {
        /// <summary>
        /// Appends an insert clause to the query.
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        IInsertClause Insert(params string[] columns);
    }
}
