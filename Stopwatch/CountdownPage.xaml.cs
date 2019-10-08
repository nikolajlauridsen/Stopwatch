using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Remoting.Channels;
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
using System.Xml.Serialization;
using Stopwatch.Properties;
using Timers;

namespace Stopwatch
{
    /// <summary>
    /// Interaction logic for CountdownPage.xaml
    /// </summary>
    public partial class CountdownPage : Page
    {
        private static string _warningColor = "PrimaryWarning";
        private static string _foreColor = "PrimaryFore";
        private CountdownTimer _timer;
        public CountdownPage(CountdownTimer timer)
        {
            InitializeComponent();

            _timer = timer;
            _timer.SetUpdateAction(span => Dispatcher.Invoke(() => _updateTime(span)));
            // TODO: Reset color here....
            _timer.SetOnStoppedAction(() => Dispatcher.Invoke(()=> {StartBtn.Content = "Start"));
            _timer.SetOnFinishedAction(() => Dispatcher.Invoke(()=>
            {
                UserControl uc = new UserControl();
                Brush warningBrush = (Brush)uc.TryFindResource(_warningColor);
                HoursBox.Foreground = warningBrush;
                HourSeperator.Foreground = warningBrush;
                MinuteSeperator.Foreground = warningBrush;
                MinutesBox.Foreground = warningBrush;
                SecondsBox.Foreground = warningBrush;
            }));
            _timer.AutoStop = false;
            StartBtn.Click += (sender, e) => _startTimer();
            ResetBtn.Click += (sender, e) => _ResetTimer();
            ApplySettings();
        }

        public void ApplySettings()
        {
            if (Settings.Default.TimerKeys)
            {
                this.KeyUp += _handleKeyUp;
            }
            else
            {
                this.KeyUp -= _handleKeyUp;
            }
        }

        private void _handleKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S)
            {
                _startTimer();
            } else if (e.Key == Key.R)
            {
                _ResetTimer();
            }
        }

        private void _updateTime(TimeSpan remainingTime)
        {
            if (_timer.Running)
            {
                HoursBox.Text = Math.Abs(remainingTime.Hours).ToString("00");
                MinutesBox.Text = Math.Abs(remainingTime.Minutes).ToString("00");
                SecondsBox.Text = Math.Abs(remainingTime.Seconds).ToString("00");
            }
        }

        private void _startTimer()
        {
            if (!_timer.Running)
            {
                int hours, minutes, seconds;
                hours = int.Parse(HoursBox.Text);
                minutes = int.Parse(MinutesBox.Text);
                seconds = int.Parse(SecondsBox.Text);
                _timer.Start(hours, minutes, seconds);
                StartBtn.Content = "Pause";
            }
            else
            {
                _timer.Pause();
                StartBtn.Content = "Start";
            }
        }

        private void _ResetTimer()
        {
            if (_timer.Running) { 
                _timer.Pause();
                StartBtn.Content = "Start";
            }

            HoursBox.Text = "00";
            MinutesBox.Text = "00";
            SecondsBox.Text = "00";
        }

        private void validateMinutesSeconds(object sender, TextCompositionEventArgs e)
        {
            TextBox selectedBox = sender as TextBox;
            if (selectedBox.SelectedText == selectedBox.Text) selectedBox.Text = "";;
            e.Handled = !isMinutesSeconds((selectedBox.Text + e.Text));
        }

        private void validateHours(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsHours((sender as TextBox).Text + e.Text);
        }

        private void filterSpace(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) {
                e.Handled = true;
            }
        }

        private static bool isMinutesSeconds(string str)
        {
            int input;
            return int.TryParse(str, out input) && input >= 0 && input <= 59;
        }

        private static bool IsHours(string str)
        {
            int input;
            return int.TryParse(str, out input) && input >= 0;
        }

    }
}
