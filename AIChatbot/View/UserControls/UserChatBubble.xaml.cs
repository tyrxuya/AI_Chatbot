using System.Windows.Controls;

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
