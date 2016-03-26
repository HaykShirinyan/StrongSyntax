using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IInsertClause : IQuerySyntax
    {
        /// <summary>
        /// Appends into part to the insert statement.
        /// </summary>
        /// <param name="tableName">Table name to insert records to.</param>
        /// <returns></returns>
        IIntoClause Into(string tableName);
    }
}
