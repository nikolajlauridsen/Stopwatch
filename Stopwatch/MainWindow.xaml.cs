using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        private int _selectedPage;
        private List<Page> _pages = new List<Page>();

        public MainWindow()
        {
            InitializeComponent();
            _watch = new StopWatch();
            _timer = new CountdownTimer();
            _hooks = new HookManager();
            _pages.Add(new StopwatchPage(_watch, _hooks));
            _pages.Add(new CountdownPage(_timer));
            ContentFrame.Navigate(_pages[0]);


            SettingsBtn.Click += (sender, e) =>
            {
                SettingsWindow settingsWin = new SettingsWindow(_watch, _hooks);
                settingsWin.ApplySettings += ((StopwatchPage) _pages[0]).ApplySettings;
                settingsWin.ApplySettings += ((CountdownPage) _pages[1]).ApplySettings;
                settingsWin.Show();
            };

            SwapBtn.Click += (sender, e) =>
            {
                if (_selectedPage == 0)
                {
                    ContentFrame.Navigate(_pages[1]);
                    _selectedPage = 1;
                    SwapBtn.Content = "Stopwatch";
                    if(_hooks.Listening) _hooks.StopListening();
                } else if (_selectedPage == 1)
                {
                    ContentFrame.Navigate(_pages[0]);
                    _selectedPage = 0;
                    if(Settings.Default.GlobalKeybinds && !_hooks.Listening) _hooks.Listen();
                    SwapBtn.Content = "Timer";
                }
            };
        }
        
    }
}
