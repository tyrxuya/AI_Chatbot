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
    /// Interaction logic for UserChatBubble.xaml
    /// </summary>
    public partial class UserChatBubble : UserControl
    {
        public UserChatBubble()
        {
            InitializeComponent();
        }

        private string bubbleContent;

        public string BubbleContent
        {
            get { return bubbleContent; }
            set
            {
                bubbleContent = value;
                txtResponse.Text = bubbleContent;
            }
        }
    }
}
