using StrongSyntax.QueryBuilders;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax.DbHelpers
{
    class DmlQueryHelper
    {
        private DbQueryBuilder _queryBuilder;

        public DmlQueryHelper(DbQueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        private SqlCommand CreateCommand(SqlConnection conn)
        {
            SqlCommand command = new SqlCommand();

            command.Connection = conn;
            command.CommandText = _queryBuilder.Query;
            command.Parameters.AddRange(_queryBuilder.SqlParameters.ToArray());

            return command;
        }

        public int Execute()
        {
            int rowsAffected = 0;

            using (var conn = new SqlConnection(_queryBuilder.ConnectionString))
            {
                conn.Open();

                using (var command = CreateCommand(conn))
                {
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected;
        }

        public async Task<int> ExecuteAsync()
        {
            int rowsAffected = 0;

            using (var conn = new SqlConnection(_queryBuilder.ConnectionString))
            {
                conn.Open();

                using (var command = CreateCommand(conn))
                {
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }

            return rowsAffected;
        }
    }
}
