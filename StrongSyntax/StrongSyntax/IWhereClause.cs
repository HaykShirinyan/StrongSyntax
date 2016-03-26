using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IWhereClause : ICompleteQuery
    {
        /// <summary>
        /// Appends a group by clause to the query.
        /// </summary>
        /// <param name="groupings">Column to group the returned dataset by.</param>
        /// <returns></returns>
        IGroupByClause GroupBy(params string[] groupings);
    }
}
