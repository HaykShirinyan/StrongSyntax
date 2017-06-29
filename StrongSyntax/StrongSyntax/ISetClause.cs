using StrongSyntax.DbHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax
{
    public interface ISetClause : ICompleteDml
    {
        /// <summary>
        /// Appends a where clause to the query.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        ICompleteDml Where(string filter, params object[] values);

        /// <summary>
        /// Appends a from clause to the query.
        /// </summary>
        /// <param name="tableName">Table name to add with from clause.</param>
        /// <returns></returns>
        IUpdateFromClause From(string tableName);

        /// <summary>
        /// Appends a from clause to the query.
        /// </summary>
        /// <param name="subSelect">Subselect to us in from clause.</param>
        /// <param name="alias">Name of the subselect.</param>
        /// <returns></returns>
        IUpdateFromClause From(ICompleteQuery subSelect, string alias);

        IUpdateFromClause From(ITempTable tempTable, string alias);
    }

    public interface IUpdateFromClause : ICompleteDml
    {
        /// <summary>
        /// Appends a left join to the query.
        /// </summary>
        /// <param name="tableName">Table name to join.</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <returns></returns>
        IUpdateFromClause LeftJoin(string tableName, string condition);

        /// <summary>
        /// Appends a left join to the query.
        /// </summary>
        /// <param name="tableName">Table name to join.</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="values">Values to use to parametrize the query to avoid SQL injection problems and enable execution plan caching.</param>
        /// <returns></returns>
        IUpdateFromClause LeftJoin(string tableName, string condition, params object[] values);

        /// <summary>
        /// Appends a left join to the query.
        /// </summary>
        /// <param name="subSelect">Subselect to join</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="alias">Name of the join.</param>
        /// <returns></returns>
        IUpdateFromClause LeftJoin(ICompleteQuery subSelect, string condition, string alias);

        /// <summary>
        /// Appends a left join to the query.
        /// </summary>
        /// <param name="subSelect">Subselect to join</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="alias">Name of the join.</param>
        /// <param name="values">Values to use to parametrize the query to avoid SQL injection problems and enable execution plan caching.</param>
        /// <returns></returns>
        IUpdateFromClause LeftJoin(ICompleteQuery subSelect, string condition, string alias, params object[] values);

        /// <summary>
        /// Appends a right join to the query.
        /// </summary>
        /// <param name="tableName">Table name to join.</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <returns></returns>
        IUpdateFromClause RightJoin(string tableName, string condition);

        /// <summary>
        /// Appends a right join to the query.
        /// </summary>
        /// <param name="tableName">Table name to join.</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="values">Values to use to parametrize the query to avoid SQL injection problems and enable execution plan caching.</param>
        /// <returns></returns>
        IUpdateFromClause RightJoin(string tableName, string condition, params object[] values);

        /// <summary>
        /// Appends a right join to the query.
        /// </summary>
        /// <param name="subSelect">Subselect to join</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="alias">Name of the join.</param>
        /// <returns></returns>
        IUpdateFromClause RightJoin(ICompleteQuery subSelect, string condition, string alias);

        /// <summary>
        /// Appends a right join to the query.
        /// </summary>
        /// <param name="subSelect">Subselect to join</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="alias">Name of the join.</param>
        /// <param name="values">Values to use to parametrize the query to avoid SQL injection problems and enable execution plan caching.</param>
        /// <returns></returns>
        IUpdateFromClause RightJoin(ICompleteQuery subSelect, string condition, string alias, params object[] values);

        /// <summary>
        /// Appends an inner join to the query.
        /// </summary>
        /// <param name="tableName">Table name to join.</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <returns></returns>
        IUpdateFromClause InnerJoin(string tableName, string condition);

        /// <summary>
        /// Appends an inner join to the query.
        /// </summary>
        /// <param name="tableName">Table name to join.</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="values">Values to use to parametrize the query to avoid SQL injection problems and enable execution plan caching.</param>
        /// <returns></returns>
        IUpdateFromClause InnerJoin(string tableName, string condition, params object[] values);

        /// <summary>
        /// Appends an inner join to the query.
        /// </summary>
        /// <param name="subSelect">Subselect to join</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="alias">Name of the join.</param>
        /// <returns></returns>
        IUpdateFromClause InnerJoin(ICompleteQuery subSelect, string condition, string alias);

        /// <summary>
        /// Appends an inner join to the query.
        /// </summary>
        /// <param name="subSelect">Subselect to join</param>
        /// <param name="condition">Condition on which to join the table</param>
        /// <param name="alias">Name of the join.</param>
        /// <param name="values">Values to use to parametrize the query to avoid SQL injection problems and enable execution plan caching.</param>
        /// <returns></returns>
        IUpdateFromClause InnerJoin(ICompleteQuery subSelect, string condition, string alias, params object[] values);

        /// <summary>
        /// Appends a where clause to the query.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        ICompleteDml Where(string filter, params object[] values);
    }
}
