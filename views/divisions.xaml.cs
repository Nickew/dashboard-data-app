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
        public divisions()
        {
            InitializeComponent();
            packages.mysql.mysqlConnection mySqlConnection = new packages.mysql.mysqlConnection("SELECT main_sub as 'Код главного отдела', name as 'Название отдела' FROM", "divisions", this.dataGrid);
        }
    }
}
