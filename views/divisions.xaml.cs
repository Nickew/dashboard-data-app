using intra_app.viewModels;
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

namespace intra_app.views
{
    /// <summary>
    /// Interaction logic for divisions.xaml
    /// </summary>
    public partial class divisions : UserControl
    {
        packages.setters.Division div = new packages.setters.Division();
        public string index = String.Empty;
        public divisions()
        {
            InitializeComponent();
            packages.mysql.mysqlConnection mySqlConnection = new packages.mysql.mysqlConnection("SELECT id, main_sub as 'Код главного отдела', name as 'Название отдела' FROM", "divisions", this.dataGrid);
        }

        public void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            div.item = dataGrid.SelectedItem;
            index = (dataGrid.SelectedCells[0].Column.GetCellContent(div.item) as TextBlock).Text;
            deleteSelectedRow();
        }

        public void deleteSelectedRow() // doesn't work
        {
            MessageBox.Show(this.index);
        }
    }
}
