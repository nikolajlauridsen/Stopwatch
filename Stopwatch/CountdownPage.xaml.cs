using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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
using Timers;

namespace Stopwatch
{
    /// <summary>
    /// Interaction logic for CountdownPage.xaml
    /// </summary>
    public partial class CountdownPage : Page
    {
        private CountdownTimer _timer;
        public CountdownPage(CountdownTimer timer)
        {
            InitializeComponent();

            _timer = timer;
            _timer.SetUpdateAction(span => Dispatcher.Invoke(() =>_updateTime(span)));
            _timer.SetOnFinishedAction(() => Dispatcher.Invoke(()=> StartBtn.Content = "Start"));
            StartBtn.Click += (sender, e) => _startTimer();
        }

        private void _updateTime(TimeSpan remainingTime)
        {
            HoursBox.Text = remainingTime.Hours.ToString("00");
            MinutesBox.Text = remainingTime.Minutes.ToString("00");
            SecondsBox.Text = remainingTime.Seconds.ToString("00");

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
