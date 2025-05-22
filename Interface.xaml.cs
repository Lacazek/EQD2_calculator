using System.Windows;


namespace EQD2_Calculator
{
    /// <summary>
    /// Logique d'interaction pour Interface.xaml
    /// </summary>
    public partial class Interface : Window
    {
        private model _m;
        internal Interface(model m)
        {
            InitializeComponent();
            _m = m;
            AlphaBeta.Text = "2";
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ShowDialog();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
                _m.allowEQD2s = true;
        }
        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
                _m.allowEQD2s = false;
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            _m.allowBED = true;
        }
        private void CheckBox_UnChecked_1(object sender, RoutedEventArgs e)
        {
            _m.allowBED = false;
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                _m.AlphaBeta = double.Parse(AlphaBeta.Text.ToString());
            }
            catch
            {

            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
