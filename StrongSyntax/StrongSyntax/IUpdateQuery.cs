using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IUpdateQuery
    {
        /// <summary>
        /// Appends an update clause to the query.
        /// </summary>
        /// <param name="tableName">Table name to update.</param>
        /// <returns></returns>
        IUpdateClause Update(string tableName);
    }
}
