using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace intra_app.views
{
    /// <summary>
    /// Interaction logic for orders.xaml
    /// </summary>
    public partial class orders : UserControl
    {
        private string tmpIndex = String.Empty;

        private const string databaseTable = "orders";

        public orders()
        {
            InitializeComponent();

            appendDock.Height = 55;

            loadTable();

            loadServices();
        }

        private void loadTable()
        {
            packages.mysql.mysqlConnection mySqlConnection = new packages.mysql.mysqlConnection("SELECT id as '№', quantity as 'Кількість', client_name as 'Імя клієнта', publish_date as 'Дата замовлення', completion_date as 'Дата виконання' FROM", databaseTable, this.dataGrid);
        }

        private void loadRowData(string index)
        {
            packages.mysql.mysqlSettings mySqlSettings = new packages.mysql.mysqlSettings();

            string query = String.Format("SELECT id, quantity, client_name, completion_date, description, SERVICES_id FROM {0}.{1} WHERE id = {2}",
                packages.mysql.mysqlSettings.dbSchema,
                databaseTable,
                index);

            mySqlSettings.createConnection();

            using (mySqlSettings.initSqlCommand(query))
            {
                mySqlSettings.openConnection();

                using (MySqlDataReader reader = mySqlSettings.initSqlCommand(query).ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inputQuantity.Text = (reader["quantity"].ToString());
                        inputName.Text = (reader["client_name"].ToString());
                        inputDesc.Text = (reader["description"].ToString());
                        inputDate.Text = (reader["completion_date"].ToString());
                        listServices.SelectedValue = (reader["SERVICES_id"]);
                    }
                }
            }
            mySqlSettings.closeConnection();
        }

        private void loadServices()
        {
            packages.mysql.mysqlSettings database = new packages.mysql.mysqlSettings();

            string query = String.Format("SELECT id, name, price_per_unit FROM {0}.{1}",
                packages.mysql.mysqlSettings.dbSchema,
                "services"
            );

            database.createConnection();

            using (database.initSqlCommand(query))
            {
                database.openConnection();

                listServices.SelectedValuePath = "id";
                listServices.DisplayMemberPath = "name";
                listServices.ItemsSource = database.getData(query).DefaultView;

            }
            database.closeConnection();

        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "№")
            {
                e.Column.MaxWidth = 75;
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            string query = String.Format("INSERT INTO {0}.{1}(client_name, description, SERVICES_id, completion_date, quantity) VALUES(@client, @desc, @service, @date_end, @quantity)",
                packages.mysql.mysqlSettings.dbSchema,
                databaseTable
            );

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(packages.mysql.mysqlSettings.connectionString))
                {
                    using (MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        mySqlConnection.Open();

                        mySqlCommand.Parameters.AddWithValue("@client", inputName.Text);
                        mySqlCommand.Parameters.AddWithValue("@desc", inputDesc.Text);
                        mySqlCommand.Parameters.AddWithValue("@service", listServices.SelectedValue);
                        mySqlCommand.Parameters.AddWithValue("@date_end", inputDate.Text);
                        mySqlCommand.Parameters.AddWithValue("@quantity", inputQuantity.Text);

                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            loadTable();
        }

        private void buttonAdd_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonAdd.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF81a3ca"));
        }

        private void buttonAdd_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonAdd.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF607C9D"));
        }

        private void buttonEdit_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonEdit.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF81a3ca"));
        }

        private void buttonEdit_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonEdit.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF607C9D"));
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            packages.mysql.mysqlSettings mySqlSettings = new packages.mysql.mysqlSettings();

            string query = String.Format("UPDATE {0} SET client_name = @client, description = @desc, SERVICES_id = @service, completion_date = @date_end, quantity = @quantity WHERE id = {1}",
                databaseTable,
                tmpIndex);

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(packages.mysql.mysqlSettings.connectionString))
                {
                    using (MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        mySqlConnection.Open();

                        mySqlCommand.Parameters.AddWithValue("@client", inputName.Text);
                        mySqlCommand.Parameters.AddWithValue("@desc", inputDesc.Text);
                        mySqlCommand.Parameters.AddWithValue("@date_end", inputDate.Text);
                        mySqlCommand.Parameters.AddWithValue("@service", listServices.SelectedValue);
                        mySqlCommand.Parameters.AddWithValue("@quantity", inputQuantity.Text);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            loadTable();
        }

        private void inputName_LeftButton(object sender, MouseButtonEventArgs e)
        {
            if (inputName.Text == "Название отдела")
            {
                inputName.Text = "";
            }
        }

        private void toggle_Click(object sender, RoutedEventArgs e)
        {
            controlPanel.Visibility = System.Windows.Visibility.Visible;
            gridControl.Visibility = System.Windows.Visibility.Hidden;
            appendDock.Height = 55;

        }

        private void buttonAddEntry_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonAddEntry.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF81a3ca"));
        }

        private void buttonEditEntry_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonEditEntry.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF81a3ca"));
        }

        private void buttonEditEntry_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonEditEntry.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF607C9D"));
        }

        private void buttonDeleteEntry_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonDeleteEntry.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFB8A8A"));
        }

        private void buttonDeleteEntry_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonDeleteEntry.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF607C9D"));
        }
        private void buttonAddEntry_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonAddEntry.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF607C9D"));
        }
        private void buttonAddEntry_Click(object sender, RoutedEventArgs e)
        {
            inputName.Text = "Ім'я клієнта";
            inputDesc.Text = "Опис (не обов'язково)";
            inputDate.Text = DateTime.Now.ToString();

            controlPanel.Visibility = System.Windows.Visibility.Hidden;
            gridControl.Visibility = System.Windows.Visibility.Visible;

            buttonAdd.Visibility = Visibility.Visible;
            buttonEdit.Visibility = Visibility.Hidden;

            appendDock.Height = 300;
        }

        private void buttonDeleteEntry_Click(object sender, RoutedEventArgs e)
        {
            var cell = dataGrid.SelectedCells[0];
            var index = (cell.Column.GetCellContent(cell.Item) as TextBlock).Text;

            packages.mysql.deleteEntry deleteSelectedRow = new packages.mysql.deleteEntry(databaseTable, index, dataGrid);

            loadTable();
        }

        public void buttonEditEntry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cell = dataGrid.SelectedCells[0];
                var index = (cell.Column.GetCellContent(cell.Item) as TextBlock).Text;

                tmpIndex = index;

                controlPanel.Visibility = System.Windows.Visibility.Hidden;
                gridControl.Visibility = System.Windows.Visibility.Visible;
                appendDock.Height = 300;

                buttonAdd.Visibility = Visibility.Hidden;
                buttonEdit.Visibility = Visibility.Visible;

                loadRowData(index);

            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var frame = stackTrace.GetFrame(0);
                var line = frame.GetFileLineNumber();

                if (line == 0)
                {
                    MessageBox.Show("Оберіть запис для редагування", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }

        private void dataGrid_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var title = (dataGrid.SelectedCells[1].Column.GetCellContent(dataGrid.SelectedCells[1].Item) as TextBlock).Text;
            var division = (dataGrid.SelectedCells[2].Column.GetCellContent(dataGrid.SelectedCells[2].Item) as TextBlock).Text;
            var description = (dataGrid.SelectedCells[3].Column.GetCellContent(dataGrid.SelectedCells[3].Item) as TextBlock).Text;
            var techData = (dataGrid.SelectedCells[4].Column.GetCellContent(dataGrid.SelectedCells[4].Item) as TextBlock).Text;

            windows.orderPage window = new windows.orderPage();
            window.Title = title;

            window.ShowDialog();
        }
    }
}
