using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DreamTimer.Pages
{
    /// <summary>
    /// Logique d'interaction pour CountdownProgress.xaml
    /// </summary>
    public partial class CountdownProgress : Page, INotifyPropertyChanged
    {
        // Timer
        private readonly DispatcherTimer dreamTimer = new();

        // Frame
        private readonly Frame navigationFrame;

        // Binding Variable
        public TimeSpan TotalTime { get; set; }
        public bool IsPausedCountdown { get; set; }

        private bool _progressIsIndeterminate;
        public bool ProgressIsIndeterminate
        {
            get { return _progressIsIndeterminate; }
            set { _progressIsIndeterminate = value; OnPropertyChanged(nameof(ProgressIsIndeterminate)); }
        }

        private bool _dreamMode;
        public bool DreamMode
        {
            get { return _dreamMode; }
            set { _dreamMode = value; OnPropertyChanged(nameof(DreamMode)); }
        }

        private string _currentTime;
        public string CurrentTime
        {
            get { return _currentTime; }
            set { _currentTime = value; OnPropertyChanged(nameof(CurrentTime)); }
        }

        private double _progressValue;
        public double ProgressValue
        {
            get { return _progressValue; }
            set { _progressValue = value; OnPropertyChanged(nameof(ProgressValue)); }
        }

        private string _remainingTimePercentage;
        public string RemainingTimePercentage
        {
            get { return _remainingTimePercentage; }
            set { _remainingTimePercentage = value; OnPropertyChanged(nameof(RemainingTimePercentage)); }
        }

        private readonly double totalSeconds;
        private double currentSeconds;
        private DateTime startTime;
        private TimeSpan elapsedTime;
        private const int TimeInterval = 80;

        public event PropertyChangedEventHandler? PropertyChanged;

        public CountdownProgress(Frame _navigationFrame, TimeSpan _time, bool _dreamMode)
        {
            this.navigationFrame = _navigationFrame;
            this.TotalTime = _time;
            this.CurrentTime = _time.ToString(@"hh\:mm\:ss");
            this.totalSeconds = TotalTime.TotalSeconds;
            this.currentSeconds = totalSeconds;
            this.ProgressValue = 100.0;
            this.RemainingTimePercentage = $"100";
            this.DreamMode = _dreamMode;

            InitializeComponent();

            this.DataContext = this;

            TimerManager();
        }

        /// <summary>
        /// Initializes and starts a timer for managing countdown functionality.
        /// </summary>
        private void TimerManager()
        {
            dreamTimer.Interval = TimeSpan.FromMilliseconds(TimeInterval);
            dreamTimer.Tick += Countdown_Tick;

            startTime = DateTime.Now;

            dreamTimer.Start();
        }

        private void Countdown_Tick(object? sender, EventArgs e)
        {
            double elapsedTimeInSeconds;
            double timePercentage;
            double roundedTimePercentage;            

            // Calculate the elapsed time since the start of the countdown
            elapsedTime = DateTime.Now - startTime;

            // Convert the elapsed time to seconds
            elapsedTimeInSeconds = elapsedTime.TotalSeconds;

            currentSeconds = totalSeconds - elapsedTimeInSeconds;

            // If the countdown is finished: Shut down the computer
            if (currentSeconds <= 0)
            {
                CurrentTime = $"00:00:00";
                RemainingTimePercentage = $"0";
                dreamTimer.Stop();

                // Shut down the computer after 5s
                Process.Start("shutdown", "/s /t 5");

                return;
            }

            // Display the remaining time in the format hh:mm:ss
            TimeSpan currentTimeSpan = TimeSpan.FromSeconds(currentSeconds).Add(TimeSpan.FromSeconds(1));
            CurrentTime = currentTimeSpan.ToString(@"hh\:mm\:ss");

            // Display the remaining percentage
            timePercentage = CountdownPercentage(elapsedTimeInSeconds, totalSeconds);
            roundedTimePercentage = Math.Ceiling(timePercentage);
            RemainingTimePercentage = $"{roundedTimePercentage}";

            ProgressValue = timePercentage;
        }

        /// <summary>
        /// Calculates the percentage of time remaining in a countdown.
        /// </summary>
        /// <param name="_currentSeconds">The number of seconds elapsed.</param>
        /// <param name="_totalSeconds">The total duration of the countdown in seconds.</param>
        /// <returns>The percentage of time remaining in the countdown.</returns>
        private static double CountdownPercentage(double _currentSeconds, double _totalSeconds)
        {
            double countdownPercentage = 100 - (_currentSeconds * 100 / _totalSeconds);
            return countdownPercentage;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //--CONTROLS CLICK-----------------------------------------------------------------------------------------------
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            dreamTimer.Stop();   

            if (navigationFrame.NavigationService.CanGoBack)
            {
                navigationFrame.NavigationService.GoBack();
                return;
            }

            navigationFrame.Navigate(new TimerConfiguration(navigationFrame));
        }

        private void StartPause_Click(object sender, RoutedEventArgs e)
        {
            if (IsPausedCountdown)
            {
                dreamTimer.Stop();

                ProgressIsIndeterminate = true;
                ProgressValue = 0;
            }

            else
            {
                ProgressIsIndeterminate = false;
                ProgressValue = CountdownPercentage(currentSeconds, totalSeconds);

                startTime = DateTime.Now - elapsedTime;

                dreamTimer.Start();
            }
        }

        private void DreamMode_Click(object sender, RoutedEventArgs e)
        {
            if (DreamMode)
            {
                OverlayWindow overlayWindow = new(this);
                overlayWindow.Show();
            }           
        }
    }
}