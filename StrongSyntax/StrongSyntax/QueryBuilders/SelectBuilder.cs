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

        public IFromClause LeftJoin(string tableName, string condition)
        {
            return Join("LEFT OUTER", tableName, condition);
        }

        public IFromClause RightJoin(string tableName, string condition)
        {
            return Join("RIGHT OUTER", tableName, condition);
        }

        public IFromClause InnerJoin(string tableName, string condition)
        {
            return Join("INNER", tableName, condition);
        }

        private void ValidateWhereClause(string filter, int paramCount, object[] values)
        {
            this.CheckNullException(filter, "filter");

            if (paramCount != values.Length)
            {
                throw new ArgumentException("Wrong number of parameters were passed to the query.");
            }
        }

        public IWhereClause Where(string filter, params object[] values)
        {
            int paramCount = filter.Count(s => s == '@');

            ValidateWhereClause(filter, paramCount, values);

            for (int i = 0; i < values.Length; i++)
            {
                this._paramList.Add(new SqlParameter("@" + i.ToString(), values[i]));
            }

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

        //public override string ToString()
        //{
        //    StringBuilder sb = new StringBuilder(_query.ToString());

        //    foreach(var s in _select)
        //    {
        //        sb.Replace(string.Format(" AS [{0}]", s), "");
        //    }

        //    return sb.ToString();
        //}
    }
}
