using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Stopwatch.Interfaces;

using KeyboardHook;
using Timers;
using Stopwatch.Properties;

namespace Stopwatch
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, SettingsPublisher
    {
        private HookManager _hooks;
        private List<SettingsSubscriber> _subscribers = new List<SettingsSubscriber>();

        public SettingsWindow(HookManager hooks)
        {
            InitializeComponent();

            _hooks = hooks;

            KeybindingsToggle.IsChecked = hooks.Listening;
            TimerKeybindingsToggle.IsChecked = Settings.Default.TimerKeys;
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
                NotifySubscribers();
            };

            DelayBox.TextChanged += (sender, e) =>
            {
                int milli;
                if (int.TryParse(DelayBox.Text, out milli))
                {
                    Settings.Default.UpdateDelay = milli;
                    NotifySubscribers();
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
                    NotifySubscribers();
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
                    NotifySubscribers();
                }
            };

            this.Closed += (sender, e) => Settings.Default.Save();

            TimerKeybindingsToggle.Checked += (sender, e) =>
            {
                Settings.Default.TimerKeys = true;
                NotifySubscribers();
            };
            TimerKeybindingsToggle.Unchecked += (sender, e) =>
            {
                Settings.Default.TimerKeys = false;
                NotifySubscribers();
            };
        }

        public void AddSubscriber(SettingsSubscriber subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public void NotifySubscribers()
        {
            foreach(SettingsSubscriber sub in _subscribers)
            {
                sub.SettingsChanged();
            }
        }
    }
}
