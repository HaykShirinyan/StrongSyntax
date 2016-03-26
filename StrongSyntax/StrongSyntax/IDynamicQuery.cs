using System;
using System.Data;

namespace StrongSyntax
{
    public interface IDynamicQuery : IQuerySyntax
    {
        /// <summary>
        /// Appends a select clause to the query.
        /// </summary>
        /// <param name="selector">Column to select.</param>
        /// <returns></returns>
        ISelectClause Select(params string[] selector);
    }
}
