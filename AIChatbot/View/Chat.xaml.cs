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
using AIChatbot.API;
using AIChatbot.Business;
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
        private IChatroomBusiness chatroomBusiness = new ChatroomBusiness(new ChatbotDbContext());
        private IMessageBusiness messageBusiness = new MessageBusiness(new ChatbotDbContext());

        private User activeUser;
        private Chatroom activeChatroom;
        private ChatClient chatService;

        public User ActiveUser
        {
            get { return activeUser; }
            set { activeUser = value; }
        }

        public Chatroom ActiveChatroom
        {
            get { return activeChatroom; }
            set { activeChatroom = value; }
        }

        public ChatClient ChatService
        {
            get { return chatService; }
            set { chatService = value; }
        }

        public Chat(User user)
        {
            InitializeComponent();
            ActiveUser = user;
            OpenAIClient client = new OpenAIClient(new ApiKeyCredential(APIConstants.OPENAI_API_KEY));
            ChatService = client.GetChatClient("gpt-4o");
            ReloadChatrooms();
            if (chatroomBusiness.FindByUser(ActiveUser).Count > 0)
            {
                ActiveChatroom = chatroomBusiness.FindByUser(user).Last();
                ChatOption_ShowChatroom(ActiveChatroom.Title);
            }
        }

        private async void btnSend_OnClick(bool obj)
        {
            svChatBar.ScrollToEnd();

            string rawMessage = txtPrompt.txtInput.Text;
            txtPrompt.txtInput.Text = "";
            txtPrompt.tbPlaceholder.Text = "Type here";

            UserChatMessage message = new(rawMessage);
            Message userMessage = new()
            {
                Content = rawMessage,
                ChatroomId = activeChatroom.Id,
                Type = APIConstants.USER_MESSAGE
            };
            messageBusiness.Add(userMessage);

            var context = messageBusiness.GetByChatroom(ActiveChatroom)
                .ConvertAll<ChatMessage>(m => m.Type == APIConstants.USER_MESSAGE ? new UserChatMessage(m.Content) : new AssistantChatMessage(m.Content));

            if (context.Count == 1)
            {
                chatroomBusiness.UpdateTitle(ActiveChatroom.Id, rawMessage);
                ReloadChatrooms();
            }

            UserChatBubble userBubble = new();
            userBubble.txtResponse.Text = rawMessage;
            spChat.Children.Add(userBubble);

            var stream = ChatService.CompleteChatStreamingAsync(context);
            ResponseChatBubble responseBubble = new();
            int index = spChat.Children.Add(responseBubble);

            StringBuilder sb = new();
            await foreach (var update in stream)
            {
                foreach (var content in update.ContentUpdate)
                {
                    responseBubble.txtResponse.Text += content.Text;
                    sb.Append(content.Text);
                    svChatBar.ScrollToEnd();
                }
            };

            Message aiMessage = new()
            {
                Content = sb.ToString(),
                ChatroomId = activeChatroom.Id,
                Type = APIConstants.AI_MESSAGE
            };

            messageBusiness.Add(aiMessage);

            context.Add(new AssistantChatMessage(aiMessage.Content));
        }

        private void btnLogout_OnClick(bool obj)
        {
            activeUser = null;
            new MainWindow().Show();
            this.Close();
        }

        private void txtPrompt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSend_OnClick(true);
            }
        }

        private void btnAddChatroom_OnClick(bool obj)
        {
            int chatroomsCount = chatroomBusiness.FindByUser(ActiveUser).Count + 1;

            Chatroom chatroom = new()
            {
                Title = chatroomsCount.ToString(),
                UserId = activeUser.Id,
            };

            chatroomBusiness.Add(chatroom);

            ChatroomOption chatroomOption = new ChatroomOption();
            chatroomOption.btnShowChatroom.Content = chatroom.Title;
            chatroomOption.ShowChatroom += ChatOption_ShowChatroom;
            chatroomOption.DeleteChatroom += ChatOption_DeleteChatroom;
            spChatrooms.Children.Add(chatroomOption);

            ChatOption_ShowChatroom(chatroom.Title);
        }

        private void ChatOption_ShowChatroom(string title)
        {
            Chatroom? chatroom = chatroomBusiness.FindByUserAndTitle(activeUser, title);

            if (chatroom == null)
            {
                return;
            }

            ActiveChatroom = chatroom;

            ReloadMessages(chatroom);
        }

        private void ChatOption_DeleteChatroom(string title)
        {
            Chatroom? chatroom = chatroomBusiness.FindByUserAndTitle(ActiveUser, title);

            if (chatroom == null)
            {
                return;
            }

            if (ActiveChatroom == chatroom)
            {
                List<Chatroom> chatrooms = chatroomBusiness.FindByUser(ActiveUser);
                ActiveChatroom = chatrooms.ElementAt(chatrooms.Count - 2);
                ReloadMessages(ActiveChatroom);
            }

            chatroomBusiness.Remove(chatroom.Id);

            ReloadChatrooms();
        }

        private void ReloadChatrooms()
        {
            spChatrooms.Children.Clear();

            foreach (Chatroom chatroom in chatroomBusiness.FindByUser(activeUser))
            {
                ChatroomOption chatroomOption = new ChatroomOption();
                chatroomOption.btnShowChatroom.Content = chatroom.Title;
                chatroomOption.ShowChatroom += ChatOption_ShowChatroom;
                chatroomOption.DeleteChatroom += ChatOption_DeleteChatroom;

                spChatrooms.Children.Add(chatroomOption);
            }
        }

        private void ReloadMessages(Chatroom chatroom)
        {
            spChat.Children.Clear();

            foreach (Message message in messageBusiness.GetByChatroom(chatroom))
            {
                if (message.Type == APIConstants.USER_MESSAGE)
                {
                    UserChatBubble chatBubble = new UserChatBubble();
                    chatBubble.txtResponse.Text = message.Content;
                    spChat.Children.Add(chatBubble);
                }

                else if (message.Type == APIConstants.AI_MESSAGE)
                {
                    ResponseChatBubble chatBubble = new ResponseChatBubble();
                    chatBubble.txtResponse.Text = message.Content;
                    spChat.Children.Add(chatBubble);
                }

                else
                {
                    continue;
                }
            }
        }
    }
}
