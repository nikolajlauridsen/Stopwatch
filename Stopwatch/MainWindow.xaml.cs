using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Stopwatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StopWatch _watch;
        private bool _listening = true; // Will listen for keystrokes while true

        public MainWindow()
        {
            InitializeComponent();

            _watch = new StopWatch(DispatchUpdate);

            // Set up high level hooks
            Thread listener = new Thread(KeyHandler);
            listener.SetApartmentState(ApartmentState.STA);
            listener.Start();

            // On clicks
            StartBtn.Click += (sender, e) => _watch.Start();
            PauseBtn.Click += (sender, e) => _watch.Pause();
            StopBtn.Click += (sender, e) => _watch.Stop();

            // We don't want to leave our threads hanging in bare nothingness
            this.Closed += (sender, args) =>
            {
                _watch.Stop();
                _listening = false;
            };
        }

        private void DispatchUpdate(TimeSpan time)
        {
            try
            {
                timeLbl.Dispatcher.Invoke(() => UpdateLabel(time));
            }
            catch (TaskCanceledException)
            {
                // This occurs when the program is closed, no reason to do anything
            }
            
        }

        private void UpdateLabel(TimeSpan time)
        {
            timeLbl.Content = time.ToString(@"hh\:mm\:ss\.ff");
        }

        private void KeyHandler()
        {
            while (_listening)
            {
                Thread.Sleep(50);  // Give the poor CPU a chance
                if (Keyboard.IsKeyDown(Key.P))
                {
                    _watch.Pause();
                } else if (Keyboard.IsKeyDown(Key.S))S
                {
                    _watch.Start();
                } else if (Keyboard.IsKeyDown(Key.R))
                {
                    _watch.Stop();
                }
            }
            
        }
    }
}
