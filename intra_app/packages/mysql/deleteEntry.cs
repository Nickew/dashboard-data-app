using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Controls;
using System.Windows;

namespace intra_app.packages.mysql
{
    class deleteEntry
    {
        private string table { get; set; }

        private string index { get; set; }

        private DataGrid dataGrid { get; set; }

        public deleteEntry(string table, string index, DataGrid dataGrid)
        {
            this.table = table;
            this.index = index;
            this.dataGrid = dataGrid;

            deleteMethod();
        }

        public void deleteMethod()
        {
            try
            {
                packages.mysql.mysqlSettings mySqlSettings = new packages.mysql.mysqlSettings();

                string query = String.Format("DELETE FROM {0}.{1} WHERE {2} = {3}",
                    packages.mysql.mysqlSettings.dbSchema,
                    table,
                    "id",
                    index);

                using (MySqlConnection mySqlConnection = new MySqlConnection(packages.mysql.mysqlSettings.connectionString))
                {
                    using (MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        mySqlConnection.Open();
                        dataGrid.ItemsSource = mySqlSettings.getData(query).DefaultView;
                    }
                    mySqlConnection.Close();
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1451)
                {
                    MessageBox.Show("Успадковані дані неможливо видалити.", "Помилка при видаленні", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message, ex.Source);
                }
            }
        }
    }
}
