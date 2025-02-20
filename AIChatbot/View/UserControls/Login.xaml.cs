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

namespace AIChatbot.View.UserControls
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public event Action<object> LoginCompleted;
        public Login()
        {
            InitializeComponent();
        }

        private void On_BtnLogin_Click(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void On_BtnRegister_Click(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
