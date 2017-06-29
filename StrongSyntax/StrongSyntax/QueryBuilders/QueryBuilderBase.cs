using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax.QueryBuilders
{
    abstract class QueryBuilderBase
    {
        protected StringBuilder _query = new StringBuilder();
        protected Syntax _syntax;
        protected string[] _select;
        protected List<SqlParameter> _paramList = new List<SqlParameter>();

        public ICollection<SqlParameter> SqlParameters
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

        public string RawQuery
        {
            get
            {
                return _query.ToString();
            }
        }

        public StringBuilder Query
        {
            get
            {
                return _query;
            }
            internal set
            {
                _query = value;
            }
        }

        public QueryBuilderBase(Syntax syntax)
        {
            _syntax = syntax;
        }

        protected void CheckNullException<TArg>(TArg arg, string argName)
            where TArg : class
        {
            if (arg == null)
            {
                throw new ArgumentNullException(argName);
            }
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

        protected SqlParameter AddParam(string name, object val)
        {
            val = val ?? DBNull.Value;

            var sqlParam = new SqlParameter(name, val);

            _paramList.Add(sqlParam);

            return sqlParam;
        }

        private string GetParamName()
        {
            int paramCount = _paramList
                .Count(p => !p.ParameterName.StartsWith("@Set", StringComparison.OrdinalIgnoreCase)
                    && !p.ParameterName.StartsWith("@Ins", StringComparison.OrdinalIgnoreCase)
                );

            return "@" + paramCount;
        }

        protected SqlParameter AddParam(object val)
        {
            val = val ?? DBNull.Value;

            var sqlParam = new SqlParameter(GetParamName(), val);

            _paramList.Add(sqlParam);

            return sqlParam;
        }

        protected SqlParameter AddParam(SqlParameter param)
        {
            _paramList.Add(param);

            return param;
        }

        public override string ToString()
        {
            if (_select != null)
            {
                StringBuilder sb = new StringBuilder(_query.ToString());

                foreach (var s in _select)
                {
                    sb.Replace(string.Format(" AS [{0}]", s), "");
                }

                return sb.ToString();
            }

            return _query.ToString();
        }
    }
}
