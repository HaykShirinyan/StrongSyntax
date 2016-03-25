using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IFromClause : ICompleteQuery
    {
        IFromClause LeftJoin(string tableName, string condition);
        IFromClause RightJoin(string tableName, string condition);
        IFromClause InnerJoin(string tableName, string condition);
        IWhereClause Where(string filter, params object[] values);
        ICompleteQuery GroupBy(params string[] groupings);
    }
}
