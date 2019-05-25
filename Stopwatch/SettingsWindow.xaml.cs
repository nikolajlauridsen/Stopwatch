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
using Timers;
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

        public Action ApplySettings;
        public SettingsWindow(StopWatch watch, HookManager hooks)
        {
            InitializeComponent();

            _hooks = hooks;
            _watch = watch;

            KeybindingsToggle.IsChecked = hooks.Listening;
            DigitsBox.SelectedIndex = Settings.Default.MiliDigits;
            DelayBox.Text = Settings.Default.UpdateDelay.ToString();

            Startbind.Text = Settings.Default.StartKey;
            Resetbind.Text = Settings.Default.ResetKey;

            KeybindingsToggle.Checked += (sender, e) =>
            {
                Settings.Default.GlobalKeybinds = true;
                _hooks.Listen();
            };

            KeybindingsToggle.Unchecked += (sender, e) =>
            {
                Settings.Default.GlobalKeybinds = false;
                _hooks.StopListening();
            };

            DigitsBox.SelectionChanged += (sender, e) =>
            {
                // Jesus christ this is ugly and smells, but ey, it works
                int digits = int.Parse(((ComboBoxItem) DigitsBox.SelectedItem).Content.ToString());
                Settings.Default.MiliDigits = digits;
                _watch.SetMilisecondDigits(digits);
                _watch.ForceUpdate();
            };

            DelayBox.TextChanged += (sender, e) =>
            {
                int milli;
                if (int.TryParse(DelayBox.Text, out milli))
                {
                    _watch.UpdateDelay = milli;
                    Settings.Default.UpdateDelay = milli;
                }
            };

            Startbind.KeyUp += (sender, e) =>
            {
                if (Startbind.IsFocused)
                {
                    KeyConverter keyConverter = new KeyConverter();
                    _hooks.DisableHook((Key)keyConverter.ConvertFromString(Settings.Default.StartKey));
                    string startKey = keyConverter.ConvertToString(e.Key);
                    Startbind.Text = startKey;
                    e.Handled = true;
                    Settings.Default.StartKey = startKey;
                    ApplySettings.Invoke();
                }
            };

            Resetbind.KeyUp += (sender, e) => 
            {
                if (Resetbind.IsFocused) {
                    KeyConverter keyConverter = new KeyConverter();
                    _hooks.DisableHook((Key)keyConverter.ConvertFromString(Settings.Default.ResetKey));
                    string resetKey = keyConverter.ConvertToString(e.Key);
                    Resetbind.Text = resetKey;
                    e.Handled = true;
                    Settings.Default.ResetKey = resetKey;
                    ApplySettings.Invoke();
                }
            };

            this.Closed += (sender, e) => Settings.Default.Save();
        }

    }
}
