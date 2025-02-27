using System.Windows;
using System.Windows.Controls;

namespace AIChatbot.View.UserControls
{
    /// <summary>
    /// Interaction logic for Button.xaml
    /// </summary>
    public partial class Button : UserControl
    {
        public event Action<bool> OnClick;

        public Button()
        {
            InitializeComponent();
        }

        private string btnContent;

        public string BtnContent
        {
            get { return btnContent; }
            set 
            { 
                btnContent = value;
                btnClickable.Content = btnContent;
            }
        }

        private void btnClickable_Click(object sender, RoutedEventArgs e)
        {
            OnClick(true);
        }
    }
}
