using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace COE.Core.Db.Helpers
{
    public abstract class BaseDbHelper
    {
        private readonly string _connectionString;

        protected BaseDbHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected T QueryRawSqlSingle<T>(string sql, DynamicParameters parameters = null)
        {
            T result = QueryRawSql<T>(sql, parameters).Single();
            return result;
        }

        protected List<T> QueryRawSql<T>(string sql, DynamicParameters parameters = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    IEnumerable<T> queryResult =
                        connection.Query<T>(sql, parameters, commandType: CommandType.Text, commandTimeout: Int32.MaxValue);

                    List<T> result = queryResult.ToList();
                    result.TrimExcess();

                    connection.Close();

                    return result;
                }
            }
            catch(Exception ex)
            {
                string message = $"Exception while trying to process SQL request: {FormatExceptionMessage(ex, sql)}";
                throw new Exception(message, ex);
            }
        }

        protected int ExecuteRawSql(string sql, DynamicParameters parameters = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    int rowsAffected =
                        connection.Execute(sql, parameters, commandType: CommandType.Text, commandTimeout: Int32.MaxValue);

                    connection.Close();

                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                string message = $"Exception while trying to process SQL request: {FormatExceptionMessage(ex, sql)}";
                throw new Exception(message, ex);
            }
        }

        protected int ExecuteStoredProcedureReturnRecordsAffected(string storedProcedureName, DynamicParameters parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    int result = connection.Execute(storedProcedureName, parameters, commandType: CommandType.StoredProcedure, commandTimeout: Int32.MaxValue);

                    connection.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                string message = $"Exception while running Stored Procedure: { FormatExceptionMessage(ex, $"SP: {storedProcedureName}")}";
                throw new Exception(message, ex);
            }
        }

        protected T ExecuteStoredProcedureSingle<T>(string storedProcedureName, DynamicParameters parameters)
            where T : class
        {
            T result = ExecuteStoredProcedure<T>(storedProcedureName, parameters).Single();
            return result;
        }

        protected List<T> ExecuteStoredProcedure<T>(string storedProcedureName, DynamicParameters parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    IEnumerable<T> queryResult = connection.Query<T>(storedProcedureName, parameters,
                        commandType: CommandType.StoredProcedure, commandTimeout: Int32.MaxValue);
                    List<T> result = queryResult.ToList();
                    result.TrimExcess();

                    connection.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                string message = $"Exception while running Stored Procedure: { FormatExceptionMessage(ex, $"SP: {storedProcedureName}")}";
                throw new Exception(message, ex);
            }
        }

        private string FormatExceptionMessage(Exception ex, string sqlQuery)
        {
            StringBuilder errorMessage = new StringBuilder();

            errorMessage.AppendFormat("\nSource: {0} \nSQL Request: {1} \nMessage: {2} \nStackTrace: \n{3}",
                ex.Source,
                sqlQuery,
                ex.Message,
                ex.StackTrace);

            return errorMessage.ToString();
        }
    }
}
