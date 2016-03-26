using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IOrderByClause : ICompleteQuery
    {
        /// <summary>
        /// Append offset clause to the query.
        /// </summary>
        /// <param name="rowsToSkip">Number of rows to skip when selecting data,</param>
        /// <returns></returns>
        IOffsetClause Offset(int rowsToSkip);
    }

    public interface IOffsetClause
    {
        /// <summary>
        /// Appends a fetch clause to the query.
        /// </summary>
        /// <param name="rowsToTake">Number of rows to take when selecting data.</param>
        /// <returns></returns>
        IFetchClause Fetch(int rowsToTake);
    }

    public interface IFetchClause : ICompleteQuery
    {

    }
}
