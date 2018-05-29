using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Controls;

namespace intra_app.packages.mysql
{
    class mysqlConnection
    {
        private string schema { get; set; }

        private string request { get; set; }

        private string table { get; set; }

        private string additionalQuery { get; set; }

        private DataGrid dataTable { get; set; }

        public mysqlConnection(string request, string table, DataGrid dataTable, string additionalQuery = "")
        {
            this.request = request;
            this.table = table;
            this.dataTable = dataTable;
            this.additionalQuery = additionalQuery;

            getPostData();
        }

        private void getPostData()
        {
            mysqlSettings database = new mysqlSettings();
            string query = String.Format("{0} {1}.{2} {3}", request, schema, table, additionalQuery);

            database.createConnection();

            using (database.initSqlCommand(query))
            {

                database.openConnection();

                dataTable.ItemsSource = database.getData(query).DefaultView;
            }

            database.closeConnection();
        }


    }
}
