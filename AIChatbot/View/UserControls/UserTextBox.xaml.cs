using System.Windows;
using System.Windows.Controls;

namespace AIChatbot.View.UserControls
{
    /// <summary>
    /// Interaction logic for UserTextBox.xaml
    /// </summary>
    public partial class UserTextBox : UserControl
    {
        public UserTextBox()
        {
            InitializeComponent();
        }

        private string placeholder;

        public string Placeholder
        {
            get { return placeholder; }
            set
            {
                placeholder = value;
                tbPlaceholder.Text = placeholder;
            }
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtInput.Text))
            {
                tbPlaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                tbPlaceholder.Visibility = Visibility.Hidden;
            }
        }
    }
}
