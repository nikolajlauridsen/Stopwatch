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
using System.Windows.Shapes;

using KeyboardHook;
using Stopwatch.Properties;

namespace Stopwatch
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private HookManager _hooks;
        private StopWatch _watch;
        public SettingsWindow(StopWatch watch, HookManager hooks)
        {
            InitializeComponent();
            _hooks = hooks;
            _watch = watch;

            KeybindingsToggle.IsChecked = hooks.Listening;
            DigitsBox.SelectedIndex = Settings.Default.MiliDigits;

            KeybindingsToggle.Checked += (sender, e) =>
            {
                Settings.Default.GlobalKeybinds = true;
                Settings.Default.Save();
                _hooks.Listen();
            };

            KeybindingsToggle.Unchecked += (sender, e) =>
            {
                Settings.Default.GlobalKeybinds = false;
                Settings.Default.Save();
                _hooks.StopListening();
            };

            DigitsBox.SelectionChanged += (sender, e) =>
            {
                // Jesus christ this is ugly and smells, but ey, it works
                int digits = int.Parse(((ComboBoxItem) DigitsBox.SelectedItem).Content.ToString());
                Settings.Default.MiliDigits = digits;
                Settings.Default.Save();
                _watch.SetMilisecondDigits(digits);
            };
        }
    }
}
