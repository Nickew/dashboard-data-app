using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for equipment.xaml
    /// </summary>
    public partial class equipment : UserControl
    {
        private string tmpIndex = String.Empty;

        private const string databaseTable = "equipment";

        public equipment()
        {
            InitializeComponent();

            appendDock.Height = 55;

            loadTable();
        }

        private void loadTable()
        {
            packages.mysql.mysqlConnection mySqlConnection = new packages.mysql.mysqlConnection("SELECT id, name as 'Назва обладнання', divisions_id as 'Відділ', description, technical_data FROM", databaseTable, this.dataGrid);
        }

        private void loadRowData(string index)
        {
            packages.mysql.mysqlSettings mySqlSettings = new packages.mysql.mysqlSettings();

            string query = String.Format("SELECT id, name, description, technical_data FROM {0}.{1} WHERE id = {2}",
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
                populateComboBox();
            }
            mySqlSettings.closeConnection();
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

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "id")
            {
                e.Column.MaxWidth = 0;
            }
            if (e.Column.Header.ToString() == "description")
            {
                e.Column.MaxWidth = 0;
            }
            if (e.Column.Header.ToString() == "technical_data")
            {
                e.Column.MaxWidth = 0;
            }
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

            string query = String.Format("UPDATE {0} SET main_sub = @main_sub, name = @name WHERE id = {1}",
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
            populateComboBox();

            controlPanel.Visibility = System.Windows.Visibility.Hidden;
            gridControl.Visibility = System.Windows.Visibility.Visible;

            buttonAdd.Visibility = Visibility.Visible;
            buttonEdit.Visibility = Visibility.Hidden;

            appendDock.Height = 290;
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
                appendDock.Height = 290;

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
            
            #region
            /*
            TextBlock tbTitle = new TextBlock();
            TextBlock tbDivision = new TextBlock();
            TextBox tbDescription = new TextBox();
            TextBox tbTechData = new TextBox();

            Label lbDivision = new Label();
            Label lbDescription = new Label();
            Label lbTechData = new Label();
            Grid grid = new Grid();

            Window wnd = new Window();

            #region Create Window programmatically
            // Grid settings
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions[0].Height = new GridLength(50);
            grid.RowDefinitions[1].Height = new GridLength(40);
            grid.RowDefinitions[2].Height = new GridLength(40);
            grid.RowDefinitions[4].Height = new GridLength(40);
            // Window settings
            wnd.Height = 580;
            wnd.Width = 760;
            wnd.Title = title;
            // TextBlocks
            tbTitle.Text = title;
            tbTitle.Margin = new Thickness(10, 10, 10, 0);
            tbTitle.FontSize = 16;
            tbTitle.FontWeight = FontWeights.Bold;
            tbDivision.Text = division;
            tbDivision.FontWeight = FontWeights.Bold;
            tbDivision.Margin = new Thickness(60, 10, 10, 10);
            tbDescription.Text = description;
            tbDescription.Margin = new Thickness(20, 0, 10, 0);
            tbDescription.IsReadOnly = true;
            tbTechData.Text = techData;
            tbTechData.Margin = new Thickness(20, 0, 10, 10);
            tbTechData.IsReadOnly = true;
            // Labels
            lbDivision.Content = "Відділ: ";
            lbDivision.Margin = new Thickness(10, 5, 0, 0);
            lbDescription.Content = "Опис:";
            lbDescription.Margin = new Thickness(10, 5, 10, 5);
            lbTechData.Content = "Технічні дані:";
            lbTechData.Margin = new Thickness(10, 5, 10, 5);
            // fill grid, window with content
            grid.Children.Add(tbTitle);
            grid.Children.Add(tbDivision);
            grid.Children.Add(tbDescription);
            grid.Children.Add(tbTechData);
            grid.Children.Add(lbDivision);
            grid.Children.Add(lbDescription);
            grid.Children.Add(lbTechData);
            Grid.SetRow(tbTitle, 0);
            Grid.SetRow(tbDivision, 1);
            Grid.SetRow(tbDescription, 3);
            Grid.SetRow(tbTechData, 5);
            Grid.SetRow(lbDivision, 1);
            Grid.SetRow(lbDescription, 2);
            Grid.SetRow(lbTechData, 4);
            wnd.Content = grid;
            #endregion

            wnd.ShowDialog();
             * */
            #endregion

            windows.equipmentPage window = new windows.equipmentPage();
            window.Title = title;
            window.lbTitle.Content = title;
            window.tbDivision.Text = division;
            window.tbDesc.Text = description;
            window.tbTechData.Text = techData;
            window.ShowDialog();
            
        }
    }
}
