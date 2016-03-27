using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface ISelectClause : IQuerySyntax
    {
        /// <summary>
        /// Appends a from clause to the query.
        /// </summary>
        /// <param name="tableName">Table name to add with from clause.</param>
        /// <returns></returns>
        IFromClause From(string tableName);

        IFromClause From(ICompleteQuery subSelect, string alias);
    }
}
