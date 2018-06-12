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
    /// Interaction logic for services.xaml
    /// </summary>
    public partial class services : UserControl
    {
        private string tmpIndex = String.Empty;

        private const string databaseTable = "services";

        public services()
        {
            InitializeComponent();

            appendDock.Height = 55;

            loadTable();
        }

        private void loadTable()
        {
            packages.mysql.mysqlConnection mySqlConnection = new packages.mysql.mysqlConnection("SELECT id, name as 'Назва послуги', price_per_unit as 'Ціна за одиницю' FROM", databaseTable, this.dataGrid);
        }

        private void loadRowData(string index)
        {
            packages.mysql.mysqlSettings mySqlSettings = new packages.mysql.mysqlSettings();

            string query = String.Format("SELECT id, main_sub, name FROM {0}.{1} WHERE id = {2}",
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
                        inputName.Text = (reader["name"].ToString());
                    }
                }
            }
            mySqlSettings.closeConnection();
        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "id")
            {
                e.Column.MaxWidth = 0;
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            string query = String.Format("INSERT INTO {0}.{1}(name, price_per_unit) VALUES(@name, @price)",
                packages.mysql.mysqlSettings.dbSchema,
                databaseTable
            );

            if (inputName.Text != "Название отдела" && inputName.Text != "")
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(packages.mysql.mysqlSettings.connectionString))
                    {
                        using (MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection))
                        {
                            mySqlConnection.Open();

                            mySqlCommand.Parameters.AddWithValue("@name", inputName.Text);
                            mySqlCommand.Parameters.AddWithValue("@price", inputPrice.Text);

                            mySqlCommand.ExecuteNonQuery();
                            mySqlConnection.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Укажите название нового отдела", "Syntax Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
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

            string query = String.Format("UPDATE {0} SET name = @name, price_per_unit = @price WHERE id = {1}",
                databaseTable,
                tmpIndex);

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(packages.mysql.mysqlSettings.connectionString))
                {
                    using (MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        mySqlConnection.Open();

                        mySqlCommand.Parameters.AddWithValue("@name", inputName.Text);
                        mySqlCommand.Parameters.AddWithValue("@price", inputPrice.Text);

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
            if (inputName.Text == "Назва послуги")
            {
                inputName.Text = "";
            }
        }

        private void inputPrice_LeftButton(object sender, MouseButtonEventArgs e)
        {
            if (inputName.Text == "Ціна за одиницю")
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
            controlPanel.Visibility = System.Windows.Visibility.Hidden;
            gridControl.Visibility = System.Windows.Visibility.Visible;

            buttonAdd.Visibility = Visibility.Visible;
            buttonEdit.Visibility = Visibility.Hidden;

            appendDock.Height = 100;
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
                appendDock.Height = 100;

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
    }
}
