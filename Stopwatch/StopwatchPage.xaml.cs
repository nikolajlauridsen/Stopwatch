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
using KeyboardHook;
using Stopwatch.Properties;
using Timers;

namespace Stopwatch
{
    /// <summary>
    /// Interaction logic for StopwatchPage.xaml
    /// </summary>
    public partial class StopwatchPage : Page
    {
        private StopWatch _watch;
        private HookManager _hooks;

        public StopwatchPage(StopWatch watch, HookManager hooks)
        {
            InitializeComponent();
            _watch = watch;
            _watch.SetUpdateEvent(DispatchUpdate);
            
            timeLbl.Content = _watch.Elapsed.ToString(_watch.FormatString);
            // Set up high level hooks
            _hooks = hooks;
            _hooks.RegisterHook(Settings.Default.StartKey, key => Dispatcher.Invoke(() => OnStartPause(_hooks, null)));
            _hooks.RegisterHook(Settings.Default.ResetKey, key => Dispatcher.Invoke(() => OnReset(_hooks, null)));

            applySettings();
            UpdateLabel(new TimeSpan(0));

            // On clicks
            StartBtn.Click += OnStartPause;
            StopBtn.Click += OnReset;
        }

        private void applySettings()
        {
            _watch.SetMilisecondDigits(Settings.Default.MiliDigits);
            _watch.UpdateDelay = Settings.Default.UpdateDelay;
            if (Settings.Default.GlobalKeybinds) {
                _hooks.Listen();
            }
        }

        private void DispatchUpdate(TimeSpan time)
        {
            try {
                timeLbl.Dispatcher.Invoke(() => UpdateLabel(time));
            } catch (TaskCanceledException) {
                // This occurs when the program is closed, no reason to do anything
            }

        }

        private void UpdateLabel(TimeSpan time)
        {
            timeLbl.Content = time.ToString(_watch.FormatString);
        }

        private void OnReset(object sender, EventArgs e)
        {
            _watch.Reset();
            StartBtn.Content = "Start";
        }

        private void OnStartPause(object sender, EventArgs e)
        {
            if (_watch.Running) {
                // The watch is running, pause it and switch the button to start
                _watch.Pause();
                StartBtn.Content = "Start";
            } else if (!_watch.Running) {
                // The watch is paused, start it and change button to pause
                _watch.Start();
                StartBtn.Content = "Pause";
            }
        }
    }
}
