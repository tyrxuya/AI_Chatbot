using System.Windows;
using System.Windows.Controls;

namespace AIChatbot.View.UserControls
{
    /// <summary>
    /// Interaction logic for ChatroomOption.xaml
    /// </summary>
    public partial class ChatroomOption : UserControl
    {
        public event Action<string> ShowChatroom;
        public event Action<string> DeleteChatroom;

        public ChatroomOption()
        {
            InitializeComponent();
        }

        private void btnShowChatroom_Click(object sender, RoutedEventArgs e)
        {
            ShowChatroom(btnShowChatroom.Content.ToString());
        }

        private void btnDeleteChatroom_OnClick(bool obj)
        {
            DeleteChatroom(btnShowChatroom.Content.ToString());
        }
    }
}
