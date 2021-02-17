using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Nimb3s.Automaton.DataAccess
{
    public class SqlDataAccess
    {
        public List<T> GetData<T, U>(string sqlStatement, U parameter, string connectionString)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStatement, parameter).ToList();
                return rows;
            }
        }
    }
}
