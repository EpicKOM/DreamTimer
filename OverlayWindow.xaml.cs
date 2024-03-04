using DreamTimer.Classes;
using DreamTimer.Pages;
using System.Windows;
using System.Windows.Input;

namespace DreamTimer
{
    /// <summary>
    /// Logique d'interaction pour WindowTest.xaml
    /// </summary>
    public partial class OverlayWindow : Window
    {
        readonly Dictionary<string, double> fadeOutSettings = new()
        {
            { "from", 0 },
            { "to", 0.80 },
            { "duration", 1.0 },
        };

        readonly Dictionary<string, double> fadeInSettings = new()
        {
            { "from", 0.80 },
            { "to", 0 },
            { "duration", 0.3 },
        };

        private readonly CountdownProgress countdownProgress;
        public OverlayWindow(CountdownProgress _countdownProgress)
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;

            Animation.FadeAnimation(fadeOutSettings, this, null);

            this.countdownProgress = _countdownProgress;
        }

        private new void KeyDownEvent(object sender, KeyEventArgs e)
        {
            Animation.FadeAnimation(fadeInSettings, this, CloseOverlay);           
        }

        private void CloseOverlay()
        {
            countdownProgress.DreamMode = false;
            this.Close();
        }
    }
}
