using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;


namespace tSQLt.Services
{
    class DataBaseService
    {
        private string _sqlServiceConnectionString;
        private string _dbName;

        public DataBaseService(string sqlServiceConnectionString)
        {
            _sqlServiceConnectionString = sqlServiceConnectionString;
        }

        public void CreateCleanDataBase(string dbName)
        {
            _dbName = dbName;
            if (this.IsDataBaseExist(_dbName))
            {
                this.DropDataBase();
            }
            this.RunScript(String.Format("CREATE DATABASE {0};", _dbName));
        }

        public void DropDataBase()
        {
            this.RunScript(String.Format("DROP DATABASE {0};", _dbName));
        }

        public void ConfigureForTsqLt()
        {
            var userId = new SqlConnectionStringBuilder(_sqlServiceConnectionString).UserID;
            this.RunScript(String.Format("ALTER DATABASE {0} SET TRUSTWORTHY ON;", _dbName));
            this.RunScript(String.Format("ALTER AUTHORIZATION ON DATABASE::{0} TO {1};", _dbName,
            userId));
        }

        public void RunScript(string script)
        {
            {
                var serverConnection = new ServerConnection() {NonPooledConnection = true, ConnectionString = _sqlServiceConnectionString};
                var server = new Server(serverConnection);
                    try
                    {
                        server.ConnectionContext.ExecuteNonQuery(script);
                    }                    
                    finally
                    {
                        server.ConnectionContext.Disconnect();
                        serverConnection.Disconnect();
                    }
           }      
        }

        public void ExecuteTest(string testName)
        {
            this.RunScript(String.Format("exec tSQLt.Run '{0}'", testName));
        }

        public IEnumerable<TestExecutionResult> GetTestResult(string name)
        {
            var sql = string.Format("SELECT id, class, testcase, name, tranname, result, msg FROM tSQLt.TestResult WHERE name='{0}'", name);
            var tests = new List<TestExecutionResult>();

            using (var sqlConnection = new SqlConnection(_sqlServiceConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tests.Add(new TestExecutionResult
                                (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Class")),
                                reader.GetString(reader.GetOrdinal("TestCase")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetString(reader.GetOrdinal("TranName")),
                                reader.GetString(reader.GetOrdinal("Result")),
                                reader.GetString(reader.GetOrdinal("Msg"))
                                ));
                        }
                    }
                }
            }

            return tests;
        }

        public void SetInitialCatalogTo(string dbName)
        {
            var builder = new SqlConnectionStringBuilder(_sqlServiceConnectionString) { InitialCatalog = dbName };
            _sqlServiceConnectionString = builder.ConnectionString;
        }

        private bool IsDataBaseExist(string dbName)
        {
            var isExistDbSql = String.Format("SELECT COUNT(name) FROM master.dbo.sysdatabases WHERE name='{0}'", dbName);

            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = _sqlServiceConnectionString;
                connection.Open();

                var command = new SqlCommand(isExistDbSql, connection);
                var result = (int)command.ExecuteScalar();
                return result > 0;
            }
        }
    }
}
