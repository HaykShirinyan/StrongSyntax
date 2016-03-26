using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IDeleteQuery
    {
        /// <summary>
        /// Appends a delete clause to the query.
        /// </summary>
        /// <returns></returns>
        IDeleteClause Delete();
    }
}
