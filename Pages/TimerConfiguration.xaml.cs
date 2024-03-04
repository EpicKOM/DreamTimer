using System.Windows;
using System.Windows.Controls;

namespace DreamTimer.Pages
{
    /// <summary>
    /// Logique d'interaction pour TimerConfiguration.xaml
    /// </summary>
    public partial class TimerConfiguration : Page
    {
        private readonly Frame navigationFrame;
        public DateTime TotalTime { get; set; } = new DateTime(1, 1, 1, 0, 30, 0);
        public bool DreamMode { get; set; }

        public TimerConfiguration(Frame _navigationFrame)
        {
            InitializeComponent();
            this.navigationFrame = _navigationFrame;           
            this.DataContext = this;
        }

        //--CONTROLS CLICK-----------------------------------------------------------------------------------------------
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            CountdownProgress countDownProgress = new(navigationFrame, TotalTime.TimeOfDay, DreamMode);

            if (DreamMode)
            {
                OverlayWindow overlayWindow = new(countDownProgress);
                overlayWindow.Show();
            }

            navigationFrame.Navigate(countDownProgress);
        }
    }
}
