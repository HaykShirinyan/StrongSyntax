using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IOrderByClause : ICompleteQuery
    {
        IOffsetClause Offset(int rowsToSkip);
    }

    public interface IOffsetClause
    {
        IFetchClause Fetch(int rowsToTake);
    }

    public interface IFetchClause : ICompleteQuery
    {

    }
}
