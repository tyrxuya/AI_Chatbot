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

        private bool registerMode = false;

        public bool RegisterMode
        {
            get { return registerMode; }
            set { registerMode = value; }
        }

        public Login()
        {
            InitializeComponent();
        }

        private void On_BtnLogin_Click(bool obj)
        {
            if (!RegisterMode)
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

            else
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

                    if (userBusiness.FindByUsername(user.Username))
                    {
                        MessageBox.Show("User already exists");
                        SetFieldsByMode(!RegisterMode);
                        return;
                    }

                    RegistrationCompleted(user);
                }
            }
        }

        private void On_BtnRegister_Click(bool obj)
        {
            SetFieldsByMode(RegisterMode);
            RegisterMode = !RegisterMode;
        }

        public void SetFieldsByMode(bool mode)
        {
            if (mode)
            {
                txtUsername.txtInput.Text = "";
                txtUsername.tbPlaceholder.Text = "Username";
                txtPassword.txtInput.Password = "";
                txtPassword.tbPlaceholder.Text = "Password";
                btnLogin.btnClickable.Content = "Login";
                btnRegister.btnClickable.Content = "Register";
            }

            else
            {
                txtUsername.txtInput.Text = "";
                txtUsername.tbPlaceholder.Text = "Username";
                txtPassword.txtInput.Password = "";
                txtPassword.tbPlaceholder.Text = "Password";
                btnLogin.btnClickable.Content = "Register";
                btnRegister.btnClickable.Content = "Go back";
            }
        }
    }
}
