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
using System.Windows.Interop;
using System.Collections.ObjectModel;

namespace intra_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]

        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]

        public static extern bool ReleaseCapture();

        public class Data
        {
            public int Test { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void subDir_Click(object sender, RoutedEventArgs e)
        {
            packages.mysql.mysqlConnection mySqlConnection = new packages.mysql.mysqlConnection("SELECT * FROM", "sub_dir", this.dataGrid);  
        }

        private void tryBetter_Click(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
