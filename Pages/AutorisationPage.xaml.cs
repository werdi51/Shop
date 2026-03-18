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

namespace Pract_SQL3.Pages
{
    /// <summary>
    /// Логика взаимодействия для AutorisationPage.xaml
    /// </summary>
    public partial class AutorisationPage : Page
    {
        private User _currentUser = new User();
        public AutorisationPage()
        {
            InitializeComponent();
            DataContext = _currentUser;
        }
        StringBuilder errors = new StringBuilder();

        private void Register_clisk(object sender, RoutedEventArgs e)
        {
            var existingUser = DB.Context.User.FirstOrDefault(u => u.Login == _currentUser.Login && u.Password == _currentUser.Password);

            if (existingUser == null)
            {
                errors.AppendLine("неверный логин или пароль");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            MessageBox.Show("добро пожаловать");


            //if (existingUser.Admin == true)
            //{
            //    NavigationService.Navigate(new AdminPage(selectedUser: existingUser));
            //}
            //else
            //{
            //    NavigationService.Navigate(new LKPage(selectedUser: existingUser));
            //}

            if (existingUser.AccessId == 1)
            {
                NavigationService.Navigate(new AdminPage(selectedUser: existingUser));
            }

            if (existingUser.AccessId == 2)
            {
                NavigationService.Navigate(new AdminPage(selectedUser: existingUser)); ///////////////
            }

            if (existingUser.AccessId == 3)
            {
                NavigationService.Navigate(new LKPage(selectedUser: existingUser));
            }

        }

        private void Auto_clisk(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }
    }
}
