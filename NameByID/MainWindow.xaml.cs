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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace NameByID
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IdTextBox.Focus();
            this.KeyDown += new KeyEventHandler(ReturnPressListener);
        }

         private void SearchButton_Click(object sender, RoutedEventArgs args)
        {
            NbtstatSearch();
        }

        private void ReturnPressListener(object sender, KeyEventArgs key)
        {
            if(key.Key.ToString() == "Return")
            {
                NbtstatSearch();
            }
        }

        async private void NbtstatSearch()
        {

            try
            {
                SearchProgressBar.IsIndeterminate = true;
                SearchButton.IsEnabled = false;
                NbtstatResult.Items.Clear();
                int idNumber = Convert.ToInt32(IdTextBox.Text);
                ProcessStartInfo nbtStat = new ProcessStartInfo(@"c:\windows\sysnative\nbtstat.exe", "-A 97.64.169." + idNumber);
                nbtStat.RedirectStandardOutput = true;
                nbtStat.UseShellExecute = false;

                nbtStat.CreateNoWindow = true;

                Process proc = new Process();
                proc.StartInfo = nbtStat;
                string result = await Task.Run(() =>
                {
                    proc.Start();
                    string tempresult = proc.StandardOutput.ReadToEnd();

                    proc.WaitForExit();
                    return tempresult;
                });

                //Console.WriteLine(result);
                NbtstatResult.Items.Add(result);
                SearchButton.IsEnabled = true;
                SearchProgressBar.IsIndeterminate = false;

            }
            catch (Exception ex)
            {
                if (ex.Message == "Input string was not in a correct format.")
                {
                    SearchProgressBar.IsIndeterminate = false;
                    SearchButton.IsEnabled = true;
                    MessageBox.Show("Please insert a number for the ID", "ID is not a number");
                }
            }
        }
    }
}
