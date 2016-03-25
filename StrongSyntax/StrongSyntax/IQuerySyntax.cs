using System.Collections.Generic;
using System.Data.SqlClient;

namespace StrongSyntax
{
    public interface IQuerySyntax
    {
        IReadOnlyCollection<SqlParameter> SqlParameters { get; }
    }
}
