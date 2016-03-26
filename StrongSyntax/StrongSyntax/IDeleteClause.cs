using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IDeleteClause
    {
        /// <summary>
        /// Appends a from clause to the query.
        /// </summary>
        /// <param name="tableName">Table name to add with from clause.</param>
        /// <returns></returns>
        IFromDml From(string tableName);
    }
}
