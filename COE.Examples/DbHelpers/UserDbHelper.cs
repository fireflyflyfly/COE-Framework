using System;
using System.Collections.Generic;
using System.Linq;
using Atata;
using COE.Core.Db.Helpers;
using OpenQA.Selenium;

namespace COE.Examples.DbHelpers
{
    public class UserDbHelper : BaseDbHelper
    {
        public UserDbHelper(string connectionString) : base(connectionString) { }

        /// <summary>
        /// QueryRawSqlSingle example
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public int GetUserIdByEmail(string email)
        {
            string query = $"SELECT [ID] FROM dbo.Users WHERE Email = '{email}';";

            int result = QueryRawSqlSingle<int>(query);

            return result;
        }

        /// <summary>
        /// QueryRawSql example
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public List<int> GetUserIdsByLastName(string lastName)
        {
            string query = "SELECT [ID] FROM dbo.Users WHERE [LastName] IN @LastNames;";

            List<int> result = QueryRawSql<int>(query, new Dapper.DynamicParameters(new { LastNames = new string[] { "test1", "Test2" } }));

            return result;
        }

        /// <summary>
        /// ExecuteRawSql example
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="newEmail"></param>
        public void UpdateUserEmail(int userID, string newEmail)
        {
            string updateQuery =
                "UPDATE dbo.Users " +
                $"SET [Email] = '{newEmail}' " +
                $"WHERE [Id] = '{userID}';";

            int affectedRows = ExecuteRawSql(updateQuery);

            if (affectedRows != 1)
            {
                throw new Exception($"Record was not updated, SQL query: {updateQuery}");
            }
        }

    }
}
