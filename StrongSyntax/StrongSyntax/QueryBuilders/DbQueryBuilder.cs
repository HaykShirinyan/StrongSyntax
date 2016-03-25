using StrongSyntax.DbHelpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace StrongSyntax.QueryBuilders
{
    class DbQueryBuilder : IDynamicQuery, ISelectClause, IFromClause, IWhereClause
    {
        protected StringBuilder _query = new StringBuilder();
        protected string[] _select;
        protected List<SqlParameter> _paramList = new List<SqlParameter>();
        protected string _connectionString;
        protected Dictionary<Type, Delegate> _projectDict = new Dictionary<Type, Delegate>();

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
                return _connectionString;
            }
        }

        public string Query
        {
            get
            {
                return _query.ToString();
            }
        }

        public DbQueryBuilder(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected StringBuilder CreateList(string[] selector)
        {
            return CreateList(selector, false);
        }

        private void Addlias(StringBuilder sb, string s, bool autoAlias)
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

            Addlias(sb, selector[0], autoAlias);

            for (int i = 1; i < selector.Length; i++)
            {
                sb.AppendLine()
                    .Append("\t,")
                    .Append(selector[i]);

                Addlias(sb, selector[i], autoAlias);
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
            _query.AppendFormat("FROM {0}", tableName)
                .AppendLine();

            return this;
        }

        private IFromClause Join(string join, string tableName, string condition)
        {
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

        public IWhereClause Where(string filter, params object[] values)
        {
            int paramCount = filter.Count(s => s == '@');

            for (int i = 0; i < values.Length; i++)
            {
                _paramList.Add(new SqlParameter("@" + i.ToString(), values[i]));
            }

            _query.AppendFormat("WHERE {0}", filter)
                .AppendLine();

            return this;
        }

        public ICompleteQuery GroupBy(params string[] groupings)
        {
            _query.AppendLine("GROUP BY");

            var groupBy = CreateList(groupings);

            _query.AppendLine(groupBy.ToString());

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
