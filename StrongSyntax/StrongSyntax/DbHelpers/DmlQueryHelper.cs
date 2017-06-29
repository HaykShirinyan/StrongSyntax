using StrongSyntax.QueryBuilders;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax.DbHelpers
{
    class DmlQueryHelper
    {
        private QueryBuilderBase _queryBuilder;

        public DmlQueryHelper(QueryBuilderBase queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        private SqlCommand CreateCommand(SqlConnection conn, SqlTransaction transaction)
        {
            SqlCommand command = new SqlCommand();

            command.Connection = conn;
            command.CommandText = _queryBuilder.RawQuery;
            command.CommandTimeout = _queryBuilder.Timeout;
            command.Parameters.AddRange(_queryBuilder.SqlParameters.ToArray());

            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            return command;
        }

        public int Execute(DbTransaction transaction)
        {
            int rowsAffected = 0;

            using (var conn = new SqlConnection(_queryBuilder.ConnectionString))
            {
                conn.Open();

                using (var command = CreateCommand(conn, transaction as SqlTransaction))
                {
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected;
        }

        public async Task<int> ExecuteAsync(DbTransaction transaction)
        {
            int rowsAffected = 0;

            using (var conn = new SqlConnection(_queryBuilder.ConnectionString))
            {
                conn.Open();

                using (var command = CreateCommand(conn, transaction as SqlTransaction))
                {
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }

            return rowsAffected;
        }
    }
}
