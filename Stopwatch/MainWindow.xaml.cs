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
            _hooks.RegisterHook(Key.S, key => Dispatcher.Invoke(() => OnStartPause(_hooks, null)));
            _hooks.RegisterHook(Key.R, key => Dispatcher.Invoke(() => OnReset(_hooks, null)));
            _hooks.Listen();


            // On clicks
            StartBtn.Click += OnStartPause;
            StopBtn.Click += OnReset;

            BindingsCheck.Checked += (sender, e) => _hooks.Listen();
            BindingsCheck.Unchecked += (sender, e) => _hooks.StopListening();
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

        private void OnReset(object sender, EventArgs e)
        {
            _watch.Reset();
            StartBtn.Content = "Start";
        }

        private void OnStartPause(object sender, EventArgs e)
        {
            if (_watch.Running)
            {
                // The watch is running, pause it and switch the button to start
                _watch.Pause();
                StartBtn.Content = "Start";
            } else if (!_watch.Running)
            {
                // The watch is paused, start it and change button to pause
                _watch.Start();
                StartBtn.Content = "Pause";
            }
        }
    }
}
