using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Controls.Primitives;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Timers;
using System.Diagnostics;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace WpfAppTest2 {
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>

    public partial class MainWindow : Window {
        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void SetCursorPos(int X, int Y);

        private int count = 0;

        Timer timer = new Timer(900000);

        public MainWindow() {
            InitializeComponent();

            SetCursorPos(100, 0);

            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;

            for (int i = 1; i < 9; i++) {
                RadioButton button = this.FindName("button" + i.ToString() + "7") as RadioButton;
                button.IsChecked = true;
            }

            timer.Elapsed += (sender, e) => {
                SetBrightness();
            };

            timer.Start();
        }

        private async void ToggleButton_Checked(object sender, RoutedEventArgs e) {
            RadioButton clickedButton = (RadioButton)sender;

            int row = 0;
            int col = 0;

            for (int i = 1; i < 9; i++) {
                for (int j = 1; j < 8; j++) {
                    if (clickedButton.Name == "button" + i.ToString() + j.ToString()) {
                        row = i;
                        col = j;
                    }

                }
            }

            DateTime currentTime = DateTime.Now;
            Label who = this.FindName("label" + row.ToString() + "0") as Label;
            Label where = this.FindName("label0" + col.ToString()) as Label;

            count++;
            if (count > 8) {
                using (var sw = new System.IO.StreamWriter((@"" + who.Content.ToString() + "_タイムログ.txt"), true)) {
                    sw.Write(currentTime.ToString("yyyy年MM月dd日tthh時mm分ss秒 - " + where.Content.ToString() + "\n"));
                }
                var param = new Hashtable();
                param["who"] = who.Content.ToString();
                param["timeLog"] = currentTime.ToString("yyyy年MM月dd日tthh時mm分ss秒 - " + where.Content.ToString());
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(param);
                var content = new StringContent(json);
                using (var client = new HttpClient()) {
                    var response = await client.PostAsync($"https://script.google.com/macros/s/AKfycbxd1rfqhzXj5JiKVwUcob9fc3HjqxH6ThVuYz_6wSlcu_5Qqtw/exec", content);
                }
            }
        }

        private void ExitButton_Clicked(object sender, RoutedEventArgs e) {
            timer.Stop();
            this.Close();
        }

        private void MinimizeButton_Clicked(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }

        private void SetBrightness() {
            DateTime currentTime = DateTime.Now;

            if (currentTime.Hour > 9 || currentTime.Hour < 6) {
                string cmd = "$monitor = Get-WmiObject -ns root/wmi -class wmiMonitorBrightNessMethods;$monitor.WmiSetBrightness(0, 10)";
                Clipboard.SetText(cmd);
                OpenWithArguments(cmd);
            } else {
                string cmd = "$monitor = Get-WmiObject -ns root/wmi -class wmiMonitorBrightNessMethods;$monitor.WmiSetBrightness(0, 100)";
                Clipboard.SetText(cmd);
                OpenWithArguments(cmd);
            }
        }

        static void OpenWithArguments(string options) {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "PowerShell.exe";
            //PowerShellのWindowを立ち上げずに実行。
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            // 引数optionsをShellのコマンドとして渡す。
            cmd.StartInfo.Arguments = options;
            cmd.Start();
        }
    }
}
