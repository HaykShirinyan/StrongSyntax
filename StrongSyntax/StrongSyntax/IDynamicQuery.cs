using System;
using System.Data;

namespace StrongSyntax
{
    public interface IDynamicQuery : IQuerySyntax
    {
        /// <summary>
        /// Appends a select clause to the query.
        /// </summary>
        /// <param name="selector">Columns to select.</param>
        /// <returns></returns>
        ISelectClause Select(params string[] selector);

        /// <summary>
        /// Creates a subselect in the main select statement.
        /// </summary>
        /// <param name="selector">Columns to select.</param>
        /// <returns></returns>
        ISelectClause SubSelect(params string[] selector);
    }
}
