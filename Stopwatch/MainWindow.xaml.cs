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
        private HookManager _hooks;

        public MainWindow()
        {
            InitializeComponent();
            _watch = new StopWatch();
            _hooks = new HookManager();
            ContentFame.Navigate(new StopwatchPage(_watch, _hooks));

            SettingsBtn.Click += (sender, e) => new SettingsWindow(_watch, _hooks).Show();
        }
        
    }
}
