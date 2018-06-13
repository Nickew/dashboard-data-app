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
using intra_app.viewModels;

namespace intra_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Window DLL
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]

        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]

        public static extern bool ReleaseCapture();
        #endregion

        public const int AUTHORIZED_USERID = 0;
        public const string AUTHORIZED_USERNAME = "Nickew";
        public bool isAuthorized = true;

        public MainWindow()
        {
            InitializeComponent();
            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata
            {
                DefaultValue = FindResource(typeof(Window))
            });
        }

        private void wndHeader_Click(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                Window window = Window.GetWindow(this);
                var wih = new WindowInteropHelper(window);
                IntPtr hWnd = wih.Handle;
                ReleaseCapture();
                SendMessage(hWnd, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void subDir_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new DivisionViewModel();
        }

        private void employeesDir_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new EmployeeViewModel();
        }

        private void equipmentDir_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new EquipmentViewModel();
        }

        private void servicesDir_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new ServiceViewModel();
        }

        private void ordersDir_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new OrderViewModel();
        }

        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void maximizeWindow_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.ResizeMode = ResizeMode.NoResize;
                this.WindowState = System.Windows.WindowState.Maximized;
                this.MaxHeight = SystemParameters.WorkArea.Height;
                this.MaxWidth = SystemParameters.WorkArea.Width;
                maximizeWindowText.Text = "";
            }
            else
            {
                this.WindowState = WindowState.Normal;
                maximizeWindowText.Text = "";
            }
        }

        private void minimizeWindow_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void closeWindow_MouseEnter(object sender, MouseEventArgs e)
        {
            closeWindow.Background = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFB8A8A"));
            closeWindowText.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
        }

        private void closeWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            closeWindow.Background = Brushes.Transparent;
            closeWindowText.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBDCFDD"));
        }

        private void maximizeWindow_MouseEnter(object sender, MouseEventArgs e)
        {
            maximizeWindow.Background = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffe9f4fd"));
            maximizeWindowText.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBDCFDD"));
        }

        private void maximizeWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            maximizeWindow.Background = Brushes.Transparent;
            maximizeWindowText.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBDCFDD"));
        }

        private void minimizeWindow_MouseEnter(object sender, MouseEventArgs e)
        {
            minimizeWindow.Background = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffe9f4fd"));
            minimizeWindowText.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBDCFDD"));
        }

        private void minimizeWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            minimizeWindow.Background = Brushes.Transparent;
            minimizeWindowText.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBDCFDD"));
        }
    }
}