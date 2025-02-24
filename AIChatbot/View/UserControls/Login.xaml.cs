using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public IUserBusiness userBusiness = new UserBusiness(new ChatbotDbContext());
        public IPasswordHasher passwordHasher = new PasswordHasher();

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
            if (string.IsNullOrEmpty(txtUsername.txtInput.Text) || string.IsNullOrEmpty(txtPassword.txtInput.Password))
            {
                SetFieldsByMode(!RegisterMode);
                grid.RowDefinitions[2].Height = new(10, GridUnitType.Star);
                txtResult.Text = "All fields are required.";
                return;
            }

            if (!RegisterMode)
            {
                User user = new()
                {
                    Username = txtUsername.txtInput.Text,
                    Password = passwordHasher.Hash(txtPassword.txtInput.Password)
                };

                if (!userBusiness.Find(user))
                {
                    SetFieldsByMode(!RegisterMode);
                    grid.RowDefinitions[2].Height = new(10, GridUnitType.Star);
                    txtResult.Text = "Wrong username/password!";
                    return;
                }

                LoginCompleted(user);
            }

            else
            {
                User user = new()
                {
                    Username = txtUsername.txtInput.Text,
                    Password = passwordHasher.Hash(txtPassword.txtInput.Password)
                };

                if (userBusiness.FindByUsername(user.Username))
                {
                    SetFieldsByMode(!RegisterMode);
                    grid.RowDefinitions[2].Height = new(10, GridUnitType.Star);
                    txtResult.Text = "User with such username already exists!";
                    return;
                }

                RegistrationCompleted(user);
            }
        }

        private void On_BtnRegister_Click(bool obj)
        {
            SetFieldsByMode(RegisterMode);
            RegisterMode = !RegisterMode;
        }

        public void SetFieldsByMode(bool mode)
        {
            txtUsername.txtInput.Text = "";
            txtUsername.tbPlaceholder.Text = "Username";
            txtPassword.txtInput.Password = "";
            txtPassword.tbPlaceholder.Text = "Password";
            txtResult.Text = "";
            grid.RowDefinitions[2].Height = new(4, GridUnitType.Star);

            if (mode)
            {              
                btnLogin.btnClickable.Content = "Login";
                btnRegister.btnClickable.Content = "Register";
            }

            else
            {
                btnLogin.btnClickable.Content = "Register";
                btnRegister.btnClickable.Content = "Go back";
            }
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                On_BtnLogin_Click(true);      
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                On_BtnLogin_Click(true);
            }
        }
    }
}
