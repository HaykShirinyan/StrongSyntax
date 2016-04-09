using StrongSyntax.DbHelpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace StrongSyntax.QueryBuilders
{
    class SelectBuilder : QueryBuilderBase, IDynamicQuery, ISelectClause, IFromClause, IWhereClause, IGroupByClause, IOrderByClause, IOffsetClause, IFetchClause
    {
        public SelectBuilder(Syntax syntax)
            : base(syntax)
        {
        }

        public ISelectClause Select(params string[] selector)
        {
            this._select = selector;

            this._query.AppendLine("SELECT");

            var sb = this.CreateList(_select, true);

            this._query.AppendLine(sb.ToString());

            return this;
        }

        public IFromClause From(string tableName)
        {
            this.CheckNullException(tableName, "tableName");

            this._query.AppendFormat("FROM {0}", tableName)
                .AppendLine();

            return this;
        }

        private void ValidatSubselect(ICompleteQuery subSelect, string alias)
        {
            this.CheckNullException(subSelect, "subSelect");
            this.CheckNullException(alias, "alias");
        }

        public IFromClause From(ICompleteQuery subSelect, string alias)
        {
            ValidatSubselect(subSelect, alias);

            this._paramList.AddRange(subSelect.SqlParameters);

            this._query.AppendLine("FROM")
                .AppendLine("(")
                .Append(subSelect.ToString())
                .AppendFormat(") AS [{0}]", alias)
                .AppendLine();

            return this;
        }

        private IFromClause Join(string join, string tableName, string condition)
        {
            this.CheckNullException(tableName, "tableName");
            this.CheckNullException(condition, "condition");

            this._query.AppendFormat("{0} JOIN {1}", join, tableName)
                .AppendLine()
                .AppendFormat("    ON {0}", condition)
                .AppendLine();

            return this;
        }

        private IFromClause Join(string join, ICompleteQuery subSelect, string condition, string alias)
        {
            ValidatSubselect(subSelect, alias);
            this.CheckNullException(condition, "condition");

            this._paramList.AddRange(subSelect.SqlParameters);

            this._query.AppendFormat("{0} JOIN", join)
                .AppendLine()
                .AppendLine("(")
                .AppendFormat("\t{0}", subSelect.ToString())
                .AppendFormat(") AS {0}", alias)
                .AppendLine()
                .AppendFormat("\tON {0}", condition)
                .AppendLine();

            return this;
        }

        public IFromClause LeftJoin(string tableName, string condition)
        {
            return Join("LEFT OUTER", tableName, condition);
        }

        public IFromClause LeftJoin(string tableName, string condition, params object[] values)
        {
            CreateFilterParams(condition, values);

            return Join("LEFT OUTER", tableName, condition);
        }

        public IFromClause LeftJoin(ICompleteQuery subSelect, string condition, string alias)
        {
            return Join("LEFT OUTER", subSelect, condition, alias);
        }

        public IFromClause LeftJoin(ICompleteQuery subSelect, string condition, string alias, params object[] values)
        {
            CreateFilterParams(condition, values);

            return Join("LEFT OUTER", subSelect, condition, alias);
        }

        public IFromClause RightJoin(string tableName, string condition)
        {
            return Join("RIGHT OUTER", tableName, condition);
        }

        public IFromClause RightJoin(string tableName, string condition, params object[] values)
        {
            CreateFilterParams(condition, values);

            return Join("RIGHT OUTER", tableName, condition);
        }

        public IFromClause RightJoin(ICompleteQuery subSelect, string condition, string alias)
        {
            return Join("RIGHT OUTER", subSelect, condition, alias);
        }

        public IFromClause RightJoin(ICompleteQuery subSelect, string condition, string alias, params object[] values)
        {
            CreateFilterParams(condition, values);

            return Join("RIGHT OUTER", subSelect, condition, alias);
        }

        public IFromClause InnerJoin(string tableName, string condition)
        {
            return Join("INNER", tableName, condition);
        }

        public IFromClause InnerJoin(string tableName, string condition, params object[] values)
        {
            CreateFilterParams(condition, values);

            return Join("INNER OUTER", tableName, condition);
        }

        public IFromClause InnerJoin(ICompleteQuery subSelect, string condition, string alias)
        {
            return Join("INNER", subSelect, condition, alias);
        }

        public IFromClause InnerJoin(ICompleteQuery subSelect, string condition, string alias, params object[] values)
        {
            CreateFilterParams(condition, values);

            return Join("INNER OUTER", subSelect, condition, alias);
        }

        private void ValidateWhereClause(string filter, int paramCount, object[] values)
        {
            this.CheckNullException(filter, "filter");

            if (paramCount != values.Length)
            {
                throw new ArgumentException("Wrong number of parameters were passed to the query.");
            }
        }

        private void CreateFilterParams(string filter, object[] values)
        {
            int paramCount = filter.Count(s => s == '@');

            ValidateWhereClause(filter, paramCount, values);

            foreach (var val in values)
            {
                this._paramList.Add(new SqlParameter("@" + this._paramList.Count.ToString(), val));
            }
        }

        public IWhereClause Where(string filter, params object[] values)
        {
            CreateFilterParams(filter, values);

            this._query.AppendFormat("WHERE {0}", filter)
                .AppendLine();

            return this;
        }

        public IGroupByClause GroupBy(params string[] groupings)
        {
            this._query.AppendLine("GROUP BY");

            var groupBy = this.CreateList(groupings);

            this._query.AppendLine(groupBy.ToString());

            return this;
        }

        private IOrderByClause OrderBy(string order, params string[] orderList)
        {
            this._query.AppendLine("ORDER BY");

            var sb = this.CreateList(orderList);

            this._query.Append(sb.ToString())
                .Append(" ")
                .AppendLine(order);

            return this;
        }

        public IOrderByClause OrderBy(params string[] orderList)
        {
            return OrderBy("ASC", orderList);
        }

        public IOrderByClause OrderByDescending(params string[] orderList)
        {
            return OrderBy("DESC", orderList);
        }

        public IOffsetClause Offset(int rowsToSkip)
        {
            this._query.AppendFormat("OFFSET {0} ROWS", rowsToSkip)
                .AppendLine();

            return this;
        }

        public IFetchClause Fetch(int rowsToTake)
        {
            this._query.AppendFormat("FETCH NEXT {0} ROWS ONLY", rowsToTake)
                .AppendLine();

            return this;
        }

        public IQueryReader<TEntity> PrepareReader<TEntity>()
            where TEntity : class, new()
        {
            return new DbQueryReader<TEntity>(this);
        }
    }
}
