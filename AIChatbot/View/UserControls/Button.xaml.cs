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
