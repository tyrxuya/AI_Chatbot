using System.Windows.Controls;

namespace AIChatbot.View.UserControls
{
    /// <summary>
    /// Interaction logic for ResponseChatBubble.xaml
    /// </summary>
    public partial class ResponseChatBubble : UserControl
    {
        public ResponseChatBubble()
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
