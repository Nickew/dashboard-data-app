using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace intra_app.packages.mysql
{
    class mysqlSettings
    {
        private MySqlConnection dbConnection;
        private DataTable dataTable;

        public const string dbServer = "localhost";
        public const string dbSchema = "intra";
        public const string dbUser = "nickew";
        public const string dbUserPassword = "qwerty";
        
        public static readonly string connectionString = String.Format("SERVER={0};DATABASE={1};username={2};PASSWORD={3}",
            dbServer,
            dbSchema,
            dbUser,
            dbUserPassword
        );

        public mysqlSettings(string server, string schema, string username, string password)
        {
            dbConnection = createConnection(server, schema, username, password);
        }

        public mysqlSettings() : this(dbServer, dbSchema, dbUser, dbUserPassword)
        {

        }

        public MySqlConnection createConnection(string server = dbServer, string schema = dbSchema, string username = dbUser, string password = dbUserPassword)
        {
            var connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3}", server, schema, username, password);
            return new MySqlConnection(connectionString);
        }

        public void openConnection()
        {
            if (dbConnection != null && dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }
        }

        public void closeConnection()
        {
            if (dbConnection != null && dbConnection.State == ConnectionState.Open)
            {
                dbConnection.Close();
            }
        }

        public MySqlCommand initSqlCommand(string query)
        {
            var mysqlCommand = new MySqlCommand(query, dbConnection);
            return mysqlCommand;
        }

        public DataTable getData(string query)
        {
            dataTable = new DataTable();
            var dataAdapter = new MySqlDataAdapter { SelectCommand = initSqlCommand(query) };

            dataAdapter.Fill(dataTable);
            return dataTable;
        }


    }
}
