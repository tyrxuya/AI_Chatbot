using System;
using System.ClientModel;
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
using System.Windows.Shapes;
using AIChatbot.Data;
using AIChatbot.Data.Models;
using AIChatbot.View.UserControls;
using OpenAI;
using OpenAI.Chat;

namespace AIChatbot.View
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : Window
    {
        private User activeUser;
        private ChatClient chatService;
        private List<ChatMessage> messages;

        public User ActiveUser
        {
            get { return activeUser; }
            set { activeUser = value; }
        }

        public ChatClient ChatService
        {
            get { return chatService; }
            set { chatService = value; }
        }

        public List<ChatMessage> Messages
        {
            get { return new List<ChatMessage>(messages); }
        }

        public Chat(User user)
        {
            InitializeComponent();
            ActiveUser = user;
            OpenAIClient client = new(new ApiKeyCredential(APIConstants.OPENAI_API_KEY));
            ChatService = client.GetChatClient("gpt-4o");
            messages = new List<ChatMessage>();
        }

        private async void btnSend_OnClick(bool obj)
        {
            svBar.ScrollToEnd();
            string rawMessage = txtPrompt.txtInput.Text;
            txtPrompt.txtInput.Text = "";
            txtPrompt.tbPlaceholder.Text = "Type here";
            UserChatMessage message = new(rawMessage);
            AddMessage(message);

            UserChatBubble userBubble = new();
            userBubble.txtResponse.Text = rawMessage;
            spChat.Children.Add(userBubble);

            var stream = ChatService.CompleteChatStreamingAsync(messages);
            ResponseChatBubble responseBubble = new();
            int index = spChat.Children.Add(responseBubble);

            StringBuilder sb = new();
            await foreach (var update in stream)
            {
                foreach (var content in update.ContentUpdate)
                {
                    responseBubble.txtResponse.Text += content.Text;
                    sb.Append(content.Text);
                    svBar.ScrollToEnd();
                }
            };

            AddMessage(new AssistantChatMessage(sb.ToString()));
        }

        private void btnLogout_OnClick(bool obj)
        {
            activeUser = null;
            new MainWindow().Show();
            this.Close();
        }

        public void AddMessage(ChatMessage message)
        {
            messages.Add(message);
        }

        private void txtPrompt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSend_OnClick(true);
            }
        }
    }
}
