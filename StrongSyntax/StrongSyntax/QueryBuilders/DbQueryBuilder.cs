using StrongSyntax.DbHelpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace StrongSyntax.QueryBuilders
{
    class DbQueryBuilder : IDynamicQuery, ISelectClause, IFromClause, IWhereClause, IGroupByClause, IOrderByClause, IOffsetClause, IFetchClause
    {
        protected StringBuilder _query = new StringBuilder();
        protected Syntax _syntax;
        protected string[] _select;
        protected List<SqlParameter> _paramList = new List<SqlParameter>();

        public IReadOnlyCollection<SqlParameter> SqlParameters
        {
            get
            {
                return _paramList;
            }
        }

        public string ConnectionString
        {
            get
            {
                return _syntax.ConnectionString;
            }
        }

        public int Timeout
        {
            get
            {
                if (_syntax.Timout > 0)
                {
                    return _syntax.Timout;
                }

                return 30000;
            }
        }

        public string Query
        {
            get
            {
                return _query.ToString();
            }
        }

        public DbQueryBuilder(Syntax syntax)
        {
            _syntax = syntax;
        }

        protected void CheckNullException(string arg, string argName)
        {
            if (string.IsNullOrEmpty(arg))
            {
                throw new ArgumentNullException(argName);
            }
        }

        protected StringBuilder CreateList(string[] selector)
        {
            return CreateList(selector, false);
        }

        private void AddAlias(StringBuilder sb, string s, bool autoAlias)
        {
            if (autoAlias && s.IndexOf(" AS ", StringComparison.OrdinalIgnoreCase) < 0)
            {
                sb.AppendFormat(" AS [{0}]", s);
            }
        }

        protected StringBuilder CreateList(string[] selector, bool autoAlias)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("\t")
                .Append(selector[0]);

            AddAlias(sb, selector[0], autoAlias);

            for (int i = 1; i < selector.Length; i++)
            {
                sb.AppendLine()
                    .Append("\t,")
                    .Append(selector[i]);

                AddAlias(sb, selector[i], autoAlias);
            }

            return sb;
        }

        public ISelectClause Select(params string[] selector)
        {
            _select = selector;

            _query.AppendLine("SELECT");

            var sb = CreateList(_select, true);

            _query.AppendLine(sb.ToString());

            return this;
        }

        public IFromClause From(string tableName)
        {
            CheckNullException(tableName, "tableName");

            _query.AppendFormat("FROM {0}", tableName)
                .AppendLine();

            return this;
        }

        private IFromClause Join(string join, string tableName, string condition)
        {
            CheckNullException(tableName, "tableName");
            CheckNullException(condition, "condition");

            _query.AppendFormat("{0} JOIN {1}", join, tableName)
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
            CheckNullException(filter, "filter");

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
                _paramList.Add(new SqlParameter("@" + i.ToString(), values[i]));
            }

            _query.AppendFormat("WHERE {0}", filter)
                .AppendLine();

            return this;
        }

        public IGroupByClause GroupBy(params string[] groupings)
        {
            _query.AppendLine("GROUP BY");

            var groupBy = CreateList(groupings);

            _query.AppendLine(groupBy.ToString());

            return this;
        }

        private IOrderByClause OrderBy(string order, params string[] orderList)
        {
            _query.AppendLine("ORDER BY");

            var sb = this.CreateList(orderList);

            _query.Append(sb.ToString())
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
            _query.AppendFormat("OFFSET {0} ROWS", rowsToSkip)
                .AppendLine();

            return this;
        }

        public IFetchClause Fetch(int rowsToTake)
        {
            _query.AppendFormat("FETCH NEXT {0} ROWS ONLY", rowsToTake)
                .AppendLine();

            return this;
        }

        public IQueryReader<TEntity> PrepareReader<TEntity>()
            where TEntity : class, new()
        {
            return new DbQueryReader<TEntity>(this);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(_query.ToString());

            foreach(var s in _select)
            {
                sb.Replace(string.Format(" AS [{0}]", s), "");
            }

            return sb.ToString();
        }
    }
}
