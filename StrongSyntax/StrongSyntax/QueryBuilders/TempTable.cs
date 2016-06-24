using StrongSyntax.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax.QueryBuilders
{
     class TempTable<TTable> : InsertBuilder, ITempTable
        where TTable : class, new()
    {
        private IEnumerable<PropertyInfo> _properties;

        public string TableName { get; private set; }

        public TempTable()
            : base(null)
        {
            var tableType = typeof(TTable);

            // We don't need complex properties because they can't be table columns.
            _properties = tableType
                .GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !p.IsNavigationalProperty() && !p.IsCollection());
            
            this.TableName = "#" + tableType.Name;

            CreateTable();
        }

        public TempTable(IEnumerable<TTable> recordList)
            : this()
        {
            FillTable(recordList);
        }       

        private string GetColumnType(Type t, TTable table)
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Object:
                    if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return GetColumnType(Nullable.GetUnderlyingType(t), table) + " NULL";
                    }
                    else if (t == typeof(Guid))
                    {
                        return "UNIQUEIDENTIFIER";
                    }
                    break;
                case TypeCode.Boolean:
                    return "BIT";
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return "INT";
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return "NUMERIC(18, 6)";
                case TypeCode.DateTime:
                    return "DATETIME";
                case TypeCode.String:
                case TypeCode.Char:
                    return "NVARCHAR(MAX)";
            }

            return string.Empty;
        }

        private string[] GetColumns()
        {
            var table = new TTable();
            var columnList = new List<string>();

            foreach (var p in _properties)
            {
                string columnType = GetColumnType(p.PropertyType, table);

                if (!string.IsNullOrEmpty(columnType))
                {
                    string column = string.Format("{0} {1}", p.Name, columnType);

                    columnList.Add(column);
                }
            }

            return columnList.ToArray();
        }

        private void CreateTable()
        {
            _query.AppendFormat("CREATE TABLE {0}", this.TableName)
                .AppendLine()
                .AppendLine("(");

            var columns = GetColumns();

            var sb = this.CreateList(columns);

            _query.AppendLine(sb.ToString())
                .AppendLine(");");
        }

        private object[] GetRecordValues(TTable record)
        {
            var valuesList = new List<object>();

            foreach (var p in _properties)
            {
                var value = p.GetValue(record);

                valuesList.Add(value);
            }

            return valuesList.ToArray();
        }

        public int FillTable(IEnumerable<TTable> recordList)
        {
            int rowsAffected = 0;

            var columns = _properties
                .Select(p => p.Name)
                .ToArray();

            var into = this.Insert(columns).Into(this.TableName);

            foreach (var record in recordList)
            {
                var recordValues = GetRecordValues(record);

                into = into.Values(recordValues);

                rowsAffected++;
            }

            return rowsAffected;
        }
    }
}
