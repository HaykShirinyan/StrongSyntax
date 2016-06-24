using StrongSyntax.DbHelpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax.QueryBuilders
{
    class InsertBuilder : DmlBase, IInsertQuery, IInsertClause, IIntoClause
    {
        private bool _isValueAdded;

        public InsertBuilder(Syntax syntax) 
            : base(syntax)
        {
        }

        public IInsertClause Insert()
        {
            _query.AppendLine("INSERT INTO");

            return this;
        }

        public IInsertClause Insert(params string[] columns)
        {
            _query.AppendLine("INSERT INTO")
               .AppendLine("(");

            var insertCols = this.CreateList(columns);

            _query.AppendLine(insertCols.ToString())
                .AppendLine(")");

            return this;
        }

        public IIntoClause Into(string tableName)
        {
            this.CheckNullException(tableName, "tableName");

            _query.Replace("INSERT INTO", "INSERT INTO " + tableName);

            return this;
        }

        private IEnumerable<string> Parametrize(object[] values)
        {
            List<string> paramsetrizedList = new List<string>();

            for (int i = 0; i < values.Length; i++)
            {
                string paramName = string.Format("@Ins{0}", this._paramList.Count);

                var sqlParam = this.AddParam(paramName, values[i]);

                paramsetrizedList.Add(sqlParam.ParameterName);
            }

            return paramsetrizedList;
        }

        private string GetStringValue(object value)
        {
            if (value is Guid && (Guid)value == Guid.Empty)
            {
                return "NULL";
            }

            return string.Format("'{0}'", value);
        }

        private string GetNumericValue(Type t, object value)
        {
            if (t.IsEnum)
            {
                return ((int)value).ToString();
            }

            return value.ToString();
        }

        private string GetValue(Type t, object value)
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Object:
                    if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return GetValue(Nullable.GetUnderlyingType(t), value);
                    }
                    else if (t == typeof(Guid))
                    {
                        return GetStringValue(value);
                    }
                    break;
                case TypeCode.Boolean:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return GetNumericValue(t, value);
                case TypeCode.DateTime:
                case TypeCode.String:
                case TypeCode.Char:
                    return GetStringValue(value);
            }

            return string.Empty;
        }

        private string GetSqlValue(object val)
        {
            if (val == null || val == DBNull.Value)
            {
                return "NULL";
            }

            return GetValue(val.GetType(), val);
        }

        private IEnumerable<string> GetWithouthParameters(object[] values)
        {
            List<string> valueList = new List<string>();

            foreach (var val in values)
            {
                string sqlValue = GetSqlValue(val);

                valueList.Add(sqlValue);
            }

            return valueList;
        }

        private void AppendValues(StringBuilder values)
        {
            if (_isValueAdded)
            {
                _query.Append(",");
            }
            else
            {
                _query.AppendLine("VALUES");
            }

            _query.AppendLine("(")
                .AppendLine(values.ToString())
                .AppendLine(")");

            _isValueAdded = true;
        }

        private IEnumerable<string> GetValues(object[] values)
        {
            if (this.ParametrizeQuery)
            {
                return Parametrize(values);
            }

            return GetWithouthParameters(values);
        }

        public IIntoClause Values(params object[] values)
        {
            var valuesToAdd = GetValues(values);

            var valueList = this.CreateList(valuesToAdd.ToArray());

            AppendValues(valueList);

            return this;
        }
    }
}
