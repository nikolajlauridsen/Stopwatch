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

namespace Stopwatch
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private HookManager _hooks;
        public SettingsWindow(HookManager hooks)
        {
            InitializeComponent();
            _hooks = hooks;

            KeybindingsToggle.IsChecked = hooks.Listening;
            KeybindingsToggle.Checked += (sender, e) => _hooks.Listen();
            KeybindingsToggle.Unchecked += (sender, e) => _hooks.StopListening();
        }
    }
}
