using StrongSyntax.QueryBuilders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax.DbHelpers
{
    class DbQueryReader<TEntity> : IQueryReader<TEntity>
        where TEntity : class, new()
    {
        private SelectBuilder _queryBuilder;

        public DbQueryReader(SelectBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        private bool IsNavigationalProperty(PropertyInfo p)
        {
            var type = p.PropertyType;

            if ((type.IsClass) && type != typeof(string))
            {
                return true;
            }

            return false;
        }

        private string GetColumnName(PropertyInfo p)
        {
            string name = p.Name;

            if (IsNavigationalProperty(p))
            {
                name = p.PropertyType.Name + p.Name;
            }

            return name;
        }

        private bool HasColumn(IDataRecord reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasColumn(IDataRecord reader, IEnumerable<string> columnNames)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);

                if (columnNames.Contains(name.Substring(0, name.IndexOf(".")), StringComparer.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private void SetValue(object entity, object value, PropertyInfo p)
        {
            if (value != null && value != DBNull.Value)
            {
                Type convertTo = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;

                p.SetValue(entity, Convert.ChangeType
                    (value, convertTo, CultureInfo.InvariantCulture), null);
            }
        }

        private void ParseProperty(TEntity entity, IDataRecord reader, PropertyInfo p)
        {
            if (HasColumn(reader, p.Name))
            {
                var value = reader[p.Name];

                if (value != null && value != DBNull.Value)
                {
                    SetValue(entity, value, p);
                }
            }
        }

        private void SetNavigationalProperty(object entity, object value, PropertyInfo[] props, string[] property)
        {
            var prop = props.SingleOrDefault(p => (p.PropertyType.Name + "s").Equals(property[0], StringComparison.OrdinalIgnoreCase));

            if (prop != null)
            {
                var propInstance = prop.GetValue(entity);

                if (propInstance == null)
                {
                    propInstance = Activator.CreateInstance(prop.PropertyType);
                }

                SetValue(propInstance, value, propInstance.GetType().GetProperty(property[1]));

                prop.SetValue(entity, propInstance);
            }
        }

        private void SetProperty(TEntity entity, string name, object value, PropertyInfo[] props)
        {
            if (name.IndexOf(typeof(TEntity).Name, StringComparison.OrdinalIgnoreCase) > -1)
            {
                string propName = name.Substring(name.IndexOf('.') + 1);

                var prop = props.SingleOrDefault(p => p.Name.Equals(propName, StringComparison.OrdinalIgnoreCase));

                if (prop != null)
                {
                    SetValue(entity, value, prop);
                }
            }
            else
            {
                SetNavigationalProperty(entity, value, props, name.Split('.'));
            }
        }

        private TEntity ParseRecord(IDataRecord reader)
        {
            TEntity entity = new TEntity();

            var props = typeof(TEntity).GetProperties();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);
                object value = reader.GetValue(i);

                SetProperty(entity, name, value, props);
            }

            return entity;
        }

        private SqlCommand CreateCommand(SqlConnection conn)
        {
            SqlCommand command = new SqlCommand();

            command.CommandText = _queryBuilder.RawQuery;
            command.CommandTimeout = _queryBuilder.Timeout;
            command.Connection = conn;

            if (_queryBuilder.SqlParameters.Count > 0)
            {
                command.Parameters.AddRange(_queryBuilder.SqlParameters.ToArray());
            }

            return command;
        }

        public ICollection<TDestination> Project<TDestination>(Func<TEntity, TDestination> projection) 
            where TDestination : class
        {
            if (projection == null)
            {
                throw new ArgumentNullException("projection");
            }

            List<TDestination> entityList = new List<TDestination>();

            using (var conn = new SqlConnection(_queryBuilder.ConnectionString))
            {
                conn.Open();

                using (var command = CreateCommand(conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entity = ParseRecord(reader);

                            entityList.Add(projection(entity));
                        }
                    }
                }
            }

            return entityList;
        }

        public async Task<ICollection<TDestination>> ProjectAsync<TDestination>(Func<TEntity, TDestination> projection)
            where TDestination : class
        {
            if (projection == null)
            {
                throw new ArgumentNullException("projection");
            }

            List<TDestination> entityList = new List<TDestination>();

            using (var conn = new SqlConnection(_queryBuilder.ConnectionString))
            {
                conn.Open();

                using (var command = CreateCommand(conn))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var entity = ParseRecord(reader);

                            entityList.Add(projection(entity));
                        }
                    }
                }
            }

            return entityList;
        }

        public ICollection<TEntity> Read()
        {
            List<TEntity> entityList = new List<TEntity>();

            using (var conn = new SqlConnection(_queryBuilder.ConnectionString))
            {
                conn.Open();

                using (var command = CreateCommand(conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            entityList.Add(ParseRecord(reader));
                        }
                    }
                }
            }

            return entityList;
        }

        public async Task<ICollection<TEntity>> ReadAsync()
        {
            List<TEntity> entityList = new List<TEntity>();

            using (var conn = new SqlConnection(_queryBuilder.ConnectionString))
            {
                conn.Open();

                using (var command = CreateCommand(conn))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            entityList.Add(ParseRecord(reader));
                        }
                    }
                }
            }

            return entityList;
        }
    }
}
