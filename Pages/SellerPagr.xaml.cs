using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Pract_SQL3.Pages
{
    public partial class SellerPagr : Page
    {
        public SellerPagr()
        {
            InitializeComponent();
            RefreshData();
        }

        private void RefreshData()
        {
            try
            {
                var allOrders = DB.Context.Order
                    .Include(o => o.OrderItem.Select(oi => oi.Tovar))
                    .Include(o => o.User) 
                    .ToList();

                NewOrderList.ItemsSource = allOrders.Where(o => o.Status == "В работе").ToList();
                WorkOrderList.ItemsSource = allOrders.Where(o => o.Status == "Выполняется").ToList();
                CompleteOrderList.ItemsSource = allOrders.Where(o => o.Status == "Выполнен").ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке заказов: " + ex.Message);
            }

        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                DB.Context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                RefreshData();
            }
        }

        private void OrderList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var list = sender as ListView;
            if (list?.SelectedItem is Order selectedOrder)
            {
                NavigationService.Navigate(new AddOrEditOrder(selectedOrder));
            }
        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AutorisationPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var button = sender as Button;

            if (button?.DataContext is Order selectedOrder)
            {
                if (selectedOrder.Status == "В работе")
                {
                    selectedOrder.Status = "Выполняется";
                }
                else if (selectedOrder.Status == "Выполняется")
                {
                    selectedOrder.Status = "Выполнен";
                }
                else
                {
                    MessageBox.Show("Заказ уже завершен!");
                    return;
                }

                try
                {
                    DB.Context.SaveChanges();

                    RefreshData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении статуса: {ex.Message}");
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddTovar());
        }
    }
    }
