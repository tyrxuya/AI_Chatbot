using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using AIChatbot.Business;
using AIChatbot.Data.Models;

namespace AIChatbot.View.UserControls
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public UserBusiness userBusiness = new();

        public event Action<User> LoginCompleted;
        public event Action<User> RegistrationCompleted;

        public Login()
        {
            InitializeComponent();
        }

        private void On_BtnLogin_Click(bool obj)
        {
            byte[] data = Encoding.ASCII.GetBytes(txtPassword.txtInput.Password);
            using (var sha256 = SHA256.Create())
            {
                byte[] digest = sha256.ComputeHash(data);
                string hash = Encoding.ASCII.GetString(digest);

                User user = new User
                {
                    Username = txtUsername.txtInput.Text,
                    Password = hash
                };

                if (userBusiness.Find(user))
                {
                    LoginCompleted(user);
                }
            }
            
        }

        private void On_BtnRegister_Click(bool obj)
        {
            byte[] data = Encoding.ASCII.GetBytes(txtPassword.txtInput.Password);
            using (var sha256 = SHA256.Create())
            {
                byte[] digest = sha256.ComputeHash(data);
                string hash = Encoding.ASCII.GetString(digest);

                User user = new()
                {
                    Username = txtUsername.txtInput.Text,
                    Password = hash
                };

                RegistrationCompleted(user);
            }
        }
    }
}
