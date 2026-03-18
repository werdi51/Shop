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
    /// Логика взаимодействия для AddOrEditOrder.xaml
    /// </summary>
    public partial class AddOrEditOrder : Page
    {
        private User _currentUser;
        private Order _editingOrder; // если не null, значит редактирование
        private List<TovarSelection> _tovarSelections;

        public AddOrEditOrder(User user) : this()
        {
            _currentUser = user;
            InitializePage(true);
        }

        public AddOrEditOrder(Order order) : this()
        {
            _editingOrder = order;
            _currentUser = order.User; // получаем пользователя из заказа
            InitializePage(false);
        }

        // Пустой конструктор для XAML
        public AddOrEditOrder()
        {
            InitializeComponent();
        }

        private void InitializePage(bool isNewOrder)
        {
            // Загружаем все товары
            var allTovars = DB.Context.Tovar.ToList();

            if (isNewOrder)
            {
                // Новый заказ: все товары с количеством 0
                _tovarSelections = allTovars.Select(t => new TovarSelection
                {
                    Id = t.Id,
                    Name = t.Name,
                    Price = t.Price,
                    Quantity = 0
                }).ToList();
            }
            else
            {
                // Редактирование: проставляем количество из существующих позиций
                var orderItems = _editingOrder.OrderItem.ToDictionary(oi => oi.TovarId);
                _tovarSelections = allTovars.Select(t => new TovarSelection
                {
                    Id = t.Id,
                    Name = t.Name,
                    Price = t.Price,
                    Quantity = orderItems.ContainsKey(t.Id) ? orderItems[t.Id].Count : 0
                }).ToList();
            }

            // Привязываем список к DataGrid
            TovarDataGrid.ItemsSource = _tovarSelections;

            // Обновляем заголовок
            TitleTextBlock.Text = isNewOrder ? "Новый заказ" : $"Редактирование заказа №{_editingOrder.Id}";
        }

        private void CalculateTotal()
        {
            int total = _tovarSelections.Sum(t => (t.Price ?? 0) * t.Quantity);
            TotalTextBlock.Text = $"Итого: {total} руб.";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Собираем позиции с количеством > 0
                var selectedItems = _tovarSelections.Where(t => t.Quantity > 0).ToList();
                if (!selectedItems.Any())
                {
                    MessageBox.Show("Добавьте хотя бы один товар в заказ.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Вычисляем общую сумму
                int totalPrice = selectedItems.Sum(t => (t.Price ?? 0) * t.Quantity);

                Order order;
                if (_editingOrder == null)
                {
                    // Создаём новый заказ
                    order = new Order
                    {
                        UserId = _currentUser.Id,
                        OrderDate = DateTime.Now,
                        TotalPrice = totalPrice,
                        Status = "В работе"

                    };
                    DB.Context.Order.Add(order);
                }
                else
                {
                    // Редактируем существующий: обновляем сумму и удаляем старые позиции
                    order = _editingOrder;
                    order.TotalPrice = totalPrice;

                    // Удаляем все старые позиции (проще, чем сверять)
                    foreach (var item in order.OrderItem.ToList())
                        DB.Context.OrderItem.Remove(item);
                }

                // Сохраняем изменения, чтобы получить Id заказа (для нового)
                DB.Context.SaveChanges();

                // Добавляем новые позиции
                foreach (var sel in selectedItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        TovarId = sel.Id,
                        Count = sel.Quantity
                    };
                    DB.Context.OrderItem.Add(orderItem);
                }

                // Финальное сохранение
                DB.Context.SaveChanges();

                MessageBox.Show("Заказ успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Возвращаемся на предыдущую страницу (например, LKPage)
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        // Обработчик изменения количества для пересчёта итога
        private void Quantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateTotal();
        }
    }
}
