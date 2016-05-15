using System.Collections.Generic;
using System.Data.SqlClient;

namespace StrongSyntax
{
    public interface IQuerySyntax
    {
        /// <summary>
        /// Gets the SQL parameters that are associated with the generated query.
        /// </summary>
        ICollection<SqlParameter> SqlParameters { get; }
    }
}
