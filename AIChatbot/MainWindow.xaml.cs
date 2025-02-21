using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AIChatbot.Business;
using AIChatbot.Data.Models;
using AIChatbot.View;

namespace AIChatbot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UserBusiness userBusiness = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_LoginCompleted(Data.Models.User obj)
        {
            new Chat(obj).Show();
            this.Close();
        }

        private void login_RegistrationCompleted(Data.Models.User obj)
        {
            userBusiness.Add(obj);
        }
    }
}