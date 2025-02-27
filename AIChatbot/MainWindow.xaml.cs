using System.Windows;
using AIChatbot.API;
using AIChatbot.Business;
using AIChatbot.Data;
using AIChatbot.Data.Models;
using AIChatbot.View;

namespace AIChatbot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IUserBusiness userBusiness = new UserBusiness(new ChatbotDbContext());

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_LoginCompleted(User obj)
        {
            new Chat(obj).Show();
            this.Close();
        }

        private void login_RegistrationCompleted(User obj)
        {
            userBusiness.Add(obj);
            new Chat(obj).Show();
            this.Close();
        }
    }
}