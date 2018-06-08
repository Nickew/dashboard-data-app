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
using MySql.Data.MySqlClient;

namespace intra_app.views
{
    /// <summary>
    /// Interaction logic for employees.xaml
    /// </summary>
    public partial class employees : UserControl
    {
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
        }

        private void buttonAdd_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonAdd.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF81a3ca"));
        }

        private void buttonAdd_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonAdd.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF607C9D"));
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

        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
