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
    /// Interaction logic for employees.xaml
    /// </summary>
    public partial class employees : UserControl
    {
        private string tmpIndex = String.Empty;

        private const string databaseTable = "employees";
        private const string foreignTable = "divisions";

        public employees()
        {
            InitializeComponent();

            appendDock.Height = 55;

            loadTable();
        }

        private void loadTable()
        {
            packages.mysql.mysqlConnection mySqlConnection = new packages.mysql.mysqlConnection("SELECT employees.id, firstname as 'Імя', surname as 'Призвище', patronymic as 'По батькові', divisions.name as 'Відділ' FROM", "employees", this.dataGrid, "JOIN divisions ON employees.DIVISIONS_id = divisions.id");
        }

        private void loadRowData(string index)
        {
            packages.mysql.mysqlSettings mySqlSettings = new packages.mysql.mysqlSettings();

            string query = String.Format("SELECT employees.id, firstname, surname, patronymic FROM {0}.{1} WHERE employees.id = {2}",
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
                        inputFirstName.Text = (reader["firstname"].ToString());
                        inputSurName.Text = (reader["surname"].ToString());
                        inputPatronymic.Text = (reader["patronymic"].ToString());
                    }
                }
                populateComboBox();
            }
            mySqlSettings.closeConnection();
        }

        private void populateComboBox()
        {
            packages.mysql.mysqlSettings database = new packages.mysql.mysqlSettings();
            string query = String.Format("SELECT id, name FROM {0}.{1}",
                packages.mysql.mysqlSettings.dbSchema,
                foreignTable
            );

            database.createConnection();

            using (database.initSqlCommand(query))
            {
                database.openConnection();

                inputDivision.DisplayMemberPath = "name";
                inputDivision.SelectedValuePath = "id";
                inputDivision.ItemsSource = database.getData(query).DefaultView;

            }
            database.closeConnection();

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
            string query = String.Format("INSERT INTO {0}.{1}(FIRSTNAME, SURNAME, PATRONYMIC, DIVISIONS_ID) VALUES(@firstname, @surname, @patronymic, @division)",
                packages.mysql.mysqlSettings.dbSchema,
                databaseTable
            );
            if (inputFirstName.Text != "Ім'я" && inputFirstName.Text != "" && inputSurName.Text != "Призвище" && inputSurName.Text != "" && inputPatronymic.Text != "По батькові" && inputPatronymic.Text != "")
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(packages.mysql.mysqlSettings.connectionString))
                    {
                        using (MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection))
                        {
                            mySqlConnection.Open();

                            if (inputDivision.SelectedValue == null)
                            {
                                mySqlCommand.Parameters.AddWithValue("@division", null);
                                mySqlCommand.Parameters.AddWithValue("@firstname", inputFirstName.Text);
                                mySqlCommand.Parameters.AddWithValue("@surname", inputSurName.Text);
                                mySqlCommand.Parameters.AddWithValue("@patronymic", inputPatronymic.Text);
                            }
                            else
                            {
                                mySqlCommand.Parameters.AddWithValue("@division", Convert.ToInt32(inputDivision.SelectedValue));
                                mySqlCommand.Parameters.AddWithValue("@firstname", inputFirstName.Text);
                                mySqlCommand.Parameters.AddWithValue("@surname", inputSurName.Text);
                                mySqlCommand.Parameters.AddWithValue("@patronymic", inputPatronymic.Text);
                            }

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

            string query = String.Format("UPDATE {0} SET divisions_id = @division, firstname = @firstname, surname = @surname, patronymic = @patronymic WHERE {0}.id = {1}",
                databaseTable,
                tmpIndex);

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(packages.mysql.mysqlSettings.connectionString))
                {
                    using (MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        mySqlConnection.Open();

                        if (inputDivision.SelectedValue == null)
                        {
                            mySqlCommand.Parameters.AddWithValue("@division", null);
                            mySqlCommand.Parameters.AddWithValue("@firstname", inputFirstName.Text);
                            mySqlCommand.Parameters.AddWithValue("@surname", inputSurName.Text);
                            mySqlCommand.Parameters.AddWithValue("@patronymic", inputPatronymic.Text);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("@division", Convert.ToInt32(inputDivision.SelectedValue));
                            mySqlCommand.Parameters.AddWithValue("@firstname", inputFirstName.Text);
                            mySqlCommand.Parameters.AddWithValue("@surname", inputSurName.Text);
                            mySqlCommand.Parameters.AddWithValue("@patronymic", inputPatronymic.Text);
                        }

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

        private void inputFirstName_LeftButton(object sender, MouseButtonEventArgs e)
        {
            if (inputFirstName.Text == "Ім'я")
            {
                inputFirstName.Text = "";
            }
        }

        private void inputSurName_LeftButton(object sender, MouseButtonEventArgs e)
        {
            if (inputSurName.Text == "Призвище")
            {
                inputSurName.Text = "";
            }
        }

        private void inputPatronymic_LeftButton(object sender, MouseButtonEventArgs e)
        {
            if (inputPatronymic.Text == "По батькові")
            {
                inputPatronymic.Text = "";
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
            populateComboBox();

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

        private void buttonEditEntry_Click(object sender, RoutedEventArgs e)
        {
            try {
                var cell = dataGrid.SelectedCells[0];
                var index = (cell.Column.GetCellContent(cell.Item) as TextBlock).Text;

                tmpIndex = index;

                controlPanel.Visibility = System.Windows.Visibility.Hidden;
                gridControl.Visibility = System.Windows.Visibility.Visible;
                appendDock.Height = 100;

                buttonAdd.Visibility = Visibility.Hidden;
                buttonEdit.Visibility = Visibility.Visible;

                loadRowData(index);

            } catch (Exception ex) {
                var stackTrace = new StackTrace(ex, true);
                var frame = stackTrace.GetFrame(0);
                var line = frame.GetFileLineNumber();

                if (line == 0)
                {
                    MessageBox.Show("Оберіть запис для редагування", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
