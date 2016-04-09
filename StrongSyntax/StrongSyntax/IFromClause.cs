using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface IFromClause : ICompleteQuery
    {
        /// <summary>
        /// Appends a left join to the query.
        /// </summary>
        /// <param name="tableName">Table name to join.</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <returns></returns>
        IFromClause LeftJoin(string tableName, string condition);

        /// <summary>
        /// Appends a left join to the query.
        /// </summary>
        /// <param name="tableName">Table name to join.</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="values">Values to use to parametrize the query to avoid SQL injection problems and enable execution plan caching.</param>
        /// <returns></returns>
        IFromClause LeftJoin(string tableName, string condition, params object[] values);

        /// <summary>
        /// Appends a left join to the query.
        /// </summary>
        /// <param name="subSelect">Subselect to join</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="alias">Name of the join.</param>
        /// <returns></returns>
        IFromClause LeftJoin(ICompleteQuery subSelect, string condition, string alias);

        /// <summary>
        /// Appends a left join to the query.
        /// </summary>
        /// <param name="subSelect">Subselect to join</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="alias">Name of the join.</param>
        /// <param name="values">Values to use to parametrize the query to avoid SQL injection problems and enable execution plan caching.</param>
        /// <returns></returns>
        IFromClause LeftJoin(ICompleteQuery subSelect, string condition, string alias, params object[] values);

        /// <summary>
        /// Appends a right join to the query.
        /// </summary>
        /// <param name="tableName">Table name to join.</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <returns></returns>
        IFromClause RightJoin(string tableName, string condition);

        /// <summary>
        /// Appends a right join to the query.
        /// </summary>
        /// <param name="tableName">Table name to join.</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="values">Values to use to parametrize the query to avoid SQL injection problems and enable execution plan caching.</param>
        /// <returns></returns>
        IFromClause RightJoin(string tableName, string condition, params object[] values);

        /// <summary>
        /// Appends a right join to the query.
        /// </summary>
        /// <param name="subSelect">Subselect to join</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="alias">Name of the join.</param>
        /// <returns></returns>
        IFromClause RightJoin(ICompleteQuery subSelect, string condition, string alias);

        /// <summary>
        /// Appends a right join to the query.
        /// </summary>
        /// <param name="subSelect">Subselect to join</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="alias">Name of the join.</param>
        /// <param name="values">Values to use to parametrize the query to avoid SQL injection problems and enable execution plan caching.</param>
        /// <returns></returns>
        IFromClause RightJoin(ICompleteQuery subSelect, string condition, string alias, params object[] values);

        /// <summary>
        /// Appends an inner join to the query.
        /// </summary>
        /// <param name="tableName">Table name to join.</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <returns></returns>
        IFromClause InnerJoin(string tableName, string condition);

        /// <summary>
        /// Appends an inner join to the query.
        /// </summary>
        /// <param name="tableName">Table name to join.</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="values">Values to use to parametrize the query to avoid SQL injection problems and enable execution plan caching.</param>
        /// <returns></returns>
        IFromClause InnerJoin(string tableName, string condition, params object[] values);

        /// <summary>
        /// Appends an inner join to the query.
        /// </summary>
        /// <param name="subSelect">Subselect to join</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="alias">Name of the join.</param>
        /// <returns></returns>
        IFromClause InnerJoin(ICompleteQuery subSelect, string condition, string alias);

        /// <summary>
        /// Appends an inner join to the query.
        /// </summary>
        /// <param name="subSelect">Subselect to join</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="alias">Name of the join.</param>
        /// <param name="values">Values to use to parametrize the query to avoid SQL injection problems and enable execution plan caching.</param>
        /// <returns></returns>
        IFromClause InnerJoin(ICompleteQuery subSelect, string condition, string alias, params object[] values);

        /// <summary>
        /// Appends a where clause to the query.
        /// </summary>
        /// <param name="filter">Filter to use to create the where clause</param>
        /// <param name="values">Values to use to parametrize the query to avoid SQL injection problems and enable execution plan caching.</param>
        /// <returns></returns>
        IWhereClause Where(string filter, params object[] values);

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
