using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using KeyboardHook;

namespace Stopwatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StopWatch _watch;
        private HookManager _hooks;

        public MainWindow()
        {
            InitializeComponent();

            _watch = new StopWatch(DispatchUpdate);

            // Set up high level hooks
            _hooks = new HookManager();
            _hooks.RegisterHook(Key.S, OnSKey);
            _hooks.RegisterHook(Key.R, OnRKey);
            _hooks.Listen();


            // On clicks
            StartBtn.Click += (sender, e) => _watch.Start();
            PauseBtn.Click += (sender, e) => _watch.Pause();
            StopBtn.Click += (sender, e) => _watch.Reset();

            // We don't want to leave our threads hanging in bare nothingness
            this.Closed += (sender, args) =>
            {
                _watch.Reset();
                _hooks.StopListening();
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

        private void OnSKey(Key key)
        {
            if (_watch.Running) {
                _watch.Pause();
            } else {
                _watch.Start();
            }
        }

        private void OnRKey(Key key)
        {
            _watch.Reset();

        }
    }
}
