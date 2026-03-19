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
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private User _currentUser = new User();
        public RegisterPage()
        {
            InitializeComponent();
            DataContext = _currentUser;
        }
        StringBuilder errors = new StringBuilder();


        private void Register_clisk(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_currentUser.Fio))
                errors.AppendLine("Заполните фио");

            if (string.IsNullOrWhiteSpace(_currentUser.Login))
                errors.AppendLine("Заполните логин");

            if (string.IsNullOrWhiteSpace(_currentUser.Password))
                errors.AppendLine("Заполните пароль");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentUser.AccessId = 3;

            if (_currentUser.Id == 0)
            {
                DB.Context.User.Add(_currentUser);
            }

            try
            {
                DB.Context.SaveChanges();
                MessageBox.Show("Saved");
                NavigationService.Navigate(new LKPage(DataContext as User));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }

        }

        private void Auto_clisk(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AutorisationPage());
        }
    }
}
