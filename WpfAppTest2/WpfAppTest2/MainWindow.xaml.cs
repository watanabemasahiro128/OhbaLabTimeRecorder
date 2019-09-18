using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;

namespace WpfAppTest2 {
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>

    public partial class MainWindow : Window {
        private int count = 0;

        public MainWindow() {
            InitializeComponent();

            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;

            for (int i = 1; i < 9; i++) {
                RadioButton button = this.FindName("button" + i.ToString() + "7") as RadioButton;
                button.IsChecked = true;
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e) {
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
                using (var sw = new System.IO.StreamWriter((@"" + who.Content + "_タイムログ.txt"), true)) {
                    sw.Write(currentTime.ToString("yyyy年MM月dd日tthh時mm分ss秒 - " + where.Content + "\n"));
                }
            }

            Console.WriteLine(count);
        }

        private void ExitButton_Clicked(object sender, RoutedEventArgs e) {
            this.Close();
        }
        private void MinimizeButton_Clicked(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }
    }
}
