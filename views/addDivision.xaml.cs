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
    /// Interaction logic for addDivision.xaml
    /// </summary>
    public partial class addDivision : UserControl
    {
        private const string databaseTable = "divisions";

        public addDivision()
        {
            InitializeComponent();

            populateComboBox();

            if (controlPanel.Visibility == System.Windows.Visibility.Visible)
            {
                this.Height = 55;
            }
        }

        private void populateComboBox()
        {
            packages.mysql.mysqlSettings database = new packages.mysql.mysqlSettings();
            string query = String.Format("SELECT id, name FROM {0}.{1}",
                packages.mysql.mysqlSettings.dbSchema,
                databaseTable
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

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            string query = String.Format("INSERT INTO {0}.{1}(MAIN_SUB, NAME) VALUES(@main_sub, @name)",
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

                            if (inputDivision.SelectedValue == null)
                            {
                                mySqlCommand.Parameters.AddWithValue("@main_sub", null);
                                mySqlCommand.Parameters.AddWithValue("@name", inputName.Text);
                            }
                            else
                            {
                                mySqlCommand.Parameters.AddWithValue("@main_sub", Convert.ToInt32(inputDivision.SelectedValue));
                                mySqlCommand.Parameters.AddWithValue("@name", inputName.Text);
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

        private void inputName_LeftButton(object sender, MouseButtonEventArgs e)
        {
            if (inputName.Text == "Название отдела")
            {
                inputName.Text = "";
            }
        }

        private void toggle_Click(object sender, RoutedEventArgs e)
        {
            if (inputName.Visibility == System.Windows.Visibility.Visible) {
                inputName.Visibility = System.Windows.Visibility.Hidden;
                inputDivision.Visibility = System.Windows.Visibility.Collapsed;
                buttonAdd.Visibility = System.Windows.Visibility.Collapsed;
                toggleText.Text = "Показать";
                
            } else {
                inputName.Visibility = System.Windows.Visibility.Visible;
                inputDivision.Visibility = System.Windows.Visibility.Visible;
                buttonAdd.Visibility = System.Windows.Visibility.Visible;
                toggleText.Text = "Скрыть";
            }

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
            this.Height = 100;
        }

        private void buttonDeleteEntry_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonEditEntry_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
