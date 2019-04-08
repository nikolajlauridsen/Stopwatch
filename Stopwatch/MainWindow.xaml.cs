using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Stopwatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StopWatch _watch;
        public MainWindow()
        {
            InitializeComponent();

            _watch = new StopWatch(DispatchUpdate);
            StartBtn.Click += (sender, e) => _watch.Start();
            PauseBtn.Click += (sender, e) => _watch.Pause();
            StopBtn.Click += (sender, e) => _watch.Stop();
        }

        private void DispatchUpdate(TimeSpan time)
        {
            try
            {
                timeLbl.Dispatcher.Invoke(() => UpdateLabel(time));
            }
            catch (TaskCanceledException)
            {

            }
            
        }

        private void UpdateLabel(TimeSpan time)
        {
            timeLbl.Content = time.ToString(@"hh\:mm\:ss\.ff");
        }
    }
}
