using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для LKPage.xaml
    /// </summary>
    public partial class LKPage : Page
    {
        private User _currentUser;


        public LKPage(User selectedUser)
        {
            InitializeComponent();
            _currentUser = selectedUser;
            DataContext = _currentUser; 
            LoadUserOrders();
        }

        private void LoadUserOrders()
        {
            try
            {
                var orders = DB.Context.Order
                    .Where(o => o.UserId == _currentUser.Id)
                    .Include(o => o.OrderItem.Select(oi => oi.Tovar)) 
                    .ToList();

                OrderList.ItemsSource = orders;


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AutorisationPage());
        }

        private void AddOrder(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddOrEditOrder(_currentUser));
        }

        private void OrderList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            var selectedOrder = OrderList.SelectedItem as Order;
            if (selectedOrder != null && selectedOrder.Status == "В работе")
            {
                NavigationService.Navigate(new AddOrEditOrder(selectedOrder));
            }
            else
            {
                MessageBox.Show("заказ уже принят");
            }

        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                LoadUserOrders(); 
            }
        }
    }
}
