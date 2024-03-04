using DreamTimer.Pages;
using System.Windows;
using System.Windows.Media;

namespace DreamTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigationFrame.Navigate(new TimerConfiguration(NavigationFrame));
        }

        // Changing the Toolbar color depending if the application has focus or not
        private void WindowActivated(object sender, EventArgs e)
        {
            ToolBar.Background = (SolidColorBrush)this.FindResource("NavBarColor");
        }

        private void WindowDeactivated(object sender, EventArgs e)
        {
            ToolBar.Background = (SolidColorBrush)this.FindResource("DisabledWindowColor");
        }
    }
}