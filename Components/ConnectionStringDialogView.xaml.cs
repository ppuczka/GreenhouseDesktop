using System.Windows;

namespace GreenhouseDesktopApp.Components
{
    public partial class ConnectionStringDialog : Window
    {
        public string ConnectionString { get; private set; } = string.Empty;

        public ConnectionStringDialog(string initialValue = "")
        {
            InitializeComponent();
            ConnectionStringBox.Text = initialValue;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectionString = ConnectionStringBox.Text.Trim();
            DialogResult = true;
            Close();
        }
    }
}
