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
    /// Interaction logic for employees.xaml
    /// </summary>
    public partial class employees : UserControl
    {
        public employees()
        {
            InitializeComponent();
            packages.mysql.mysqlConnection mySqlConnection = new packages.mysql.mysqlConnection("SELECT firstname as 'Імя', surname as 'Призвище', patronymic as 'По батькові', divisions.name as 'Відділ' FROM", "employees", this.dataGrid, "JOIN divisions ON employees.DIVISIONS_id = divisions.id");
        }
    }
}
