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
using AIChatbot.API;
using AIChatbot.Business;
using AIChatbot.Data;
using AIChatbot.Data.Models;

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
