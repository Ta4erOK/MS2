using System;
using System.Windows;
using System.Windows.Controls;
using MotorcycleShop.Domain;

namespace MotorcycleShop.UI
{
    /// <summary>
    /// Логика взаимодействия для OrderStatusWindow.xaml
    /// Форма для просмотра статуса заказа
    /// </summary>
    public partial class OrderStatusWindow : Window
    {
        private readonly int? _orderId;
        private readonly MotorcycleShop.Data.SqlServer.OrderSqlServerRepository _orderRepository;
        private readonly MotorcycleShop.Data.SqlServer.MotorcycleSqlServerRepository _motorcycleRepository;

        public OrderStatusWindow(int orderId = 0)
        {
            InitializeComponent();
            
            _orderId = orderId > 0 ? orderId : null;
            
            // Инициализация репозиториев
            var options = DatabaseConfiguration.GetDbContextOptions();
            var context = new MotorcycleShop.Data.SqlServer.MotorcycleShopDbContext(options);
            _orderRepository = new MotorcycleShop.Data.SqlServer.OrderSqlServerRepository(context);
            _motorcycleRepository = new MotorcycleShop.Data.SqlServer.MotorcycleSqlServerRepository(context);
            
            if (_orderId.HasValue && _orderId.Value > 0)
            {
                LoadOrderInfo(_orderId.Value);
            }
        }

        /// <summary>
        /// Загрузка информации о заказе по ID
        /// </summary>
        private void LoadOrderInfo(int orderId)
        {
            try
            {
                var order = _orderRepository.GetById(orderId);
                if (order != null)
                {
                    // Заполняем информацию о заказе
                    StatusTextBlock.Text = order.Status;
                    OrderDateTextBlock.Text = order.OrderDate.ToString("dd.MM.yyyy HH:mm");
                    OrderAmountTextBlock.Text = order.TotalAmount.ToString("C");
                    
                    // Загружаем позиции заказа (в демонстрационных целях создаем фиктивные данные)
                    // В реальном приложении эти данные будут загружаться из базы данных
                    var orderItems = new System.Collections.ObjectModel.ObservableCollection<OrderItem>();
                    
                    // Добавляем один элемент для демонстрации
                    var motorcycle = _motorcycleRepository.GetById(1); // Берем первый мотоцикл
                    if (motorcycle != null)
                    {
                        var orderItem = new OrderItem(order.Id, motorcycle.Id, motorcycle.Price, 1);
                        orderItem.Motorcycle = motorcycle;
                        orderItems.Add(orderItem);
                    }
                    
                    OrderItemsDataGrid.ItemsSource = orderItems;
                    
                    // Блокируем поле ввода номера заказа, если мы уже открыли конкретный заказ
                    OrderNumberTextBox.IsEnabled = false;
                    OrderNumberTextBox.Text = order.OrderNumber;
                }
                else
                {
                    MessageBox.Show("Заказ не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке информации о заказе: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработка нажатия кнопки "Поиск"
        /// </summary>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OrderNumberTextBox.Text))
            {
                MessageBox.Show("Введите номер заказа", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // В реальном приложении здесь будет поиск заказа по номеру
            // Пока что просто покажем сообщение
            MessageBox.Show($"Поиск заказа с номером {OrderNumberTextBox.Text} не реализован в демонстрационной версии.\n\n" +
                           "В реальном приложении здесь будет реализована функция поиска заказа по его номеру.", 
                           "Поиск заказа", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Обработка нажатия кнопки "Назад"
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Обработка нажатия кнопки "Новый заказ"
        /// </summary>
        private void NewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Обработка нажатия кнопки "Закрыть"
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}