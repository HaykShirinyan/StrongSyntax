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

        /// <summary>
        /// Appends an ascending order by clause to the query.
        /// </summary>
        /// <param name="orderList">Column to use to sort the returned dataset.</param>
        /// <returns></returns>
        IOrderByClause OrderBy(params string[] orderList);

        /// <summary>
        /// Appends a descending order by clause to the query.
        /// </summary>
        /// <param name="orderList">Column to use to sort the returned dataset.</param>
        /// <returns></returns>
        IOrderByClause OrderByDescending(params string[] orderList);
    }
}
