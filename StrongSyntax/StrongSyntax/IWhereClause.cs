using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IWhereClause : ICompleteQuery
    {
        ICompleteQuery GroupBy(params string[] groupings);
    }
}
