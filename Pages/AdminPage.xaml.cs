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
using System.Data.Entity;

namespace Pract_SQL3.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage(User selectedUser)
        {
            InitializeComponent();
            DataContext = selectedUser;
            DGrid.ItemsSource = DB.Context.User.ToList();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AutorisationPage());
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

            Button button = sender as Button;
            User user = button.DataContext as User;
            if (user == null)
            {
                return;
            }
            try
            {

                //user.admin==true? false : true;
                //if (user.Admin)
                //{
                //    user.Admin = false;
                //}
                //else
                //{
                //    user.Admin = true;
                //}
                //if (user.AccessId == 3)
                //{
                //    user.AccessId = 1;
                //}

                //if (user.AccessId == 2)
                //{
                //    user.AccessId = 3;
                //}

                //if (user.AccessId == 1)
                //{
                //    user.AccessId = 2;
                //}


                switch (user.AccessId)
                {
                    case 1:
                        {
                            user.AccessId = 2;
                            break;
                        }

                    case 2:
                        {
                            user.AccessId = 3;
                            break;
                        }

                    case 3:
                        {
                            user.AccessId = 1;
                            break;
                        }
                }



                DB.Context.SaveChanges();
                DGrid.ItemsSource = DB.Context.User.Include(u => u.Access).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка {ex.Message}");
            }
        }
    }
}
