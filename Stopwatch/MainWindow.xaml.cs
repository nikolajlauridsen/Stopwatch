using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using KeyboardHook;
using Timers;
using Stopwatch.Properties;

namespace Stopwatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StopWatch _watch;
        private CountdownTimer _timer;
        private HookManager _hooks;

        public MainWindow()
        {
            InitializeComponent();
            _watch = new StopWatch();
            _timer = new CountdownTimer();
            _hooks = new HookManager();
            // ContentFame.Navigate(new StopwatchPage(_watch, _hooks));
            ContentFame.Navigate(new CountdownPage(_timer));

            SettingsBtn.Click += (sender, e) => new SettingsWindow(_watch, _hooks).Show();
        }
        
    }
}
