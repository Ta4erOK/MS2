using System;
using System.Windows;
using MotorcycleShop.Domain;

namespace MotorcycleShop.UI
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// Форма для оформления заказа
    /// </summary>
    public partial class OrderWindow : Window
    {
        private decimal _totalAmount;
        private readonly MotorcycleShop.Data.SqlServer.OrderSqlServerRepository _orderRepository;
        private readonly MotorcycleShop.Data.SqlServer.MotorcycleSqlServerRepository _motorcycleRepository;

        public OrderWindow()
        {
            InitializeComponent();
            
            // Инициализация репозиториев
            var options = DatabaseConfiguration.GetDbContextOptions();
            var context = new MotorcycleShop.Data.SqlServer.MotorcycleShopDbContext(options);
            _orderRepository = new MotorcycleShop.Data.SqlServer.OrderSqlServerRepository(context);
            _motorcycleRepository = new MotorcycleShop.Data.SqlServer.MotorcycleSqlServerRepository(context);
            
            // Установка тестовой суммы
            _totalAmount = 1500000; // 1.5 млн рублей для теста
            TotalAmountTextBlock.Text = _totalAmount.ToString("C");
        }

        /// <summary>
        /// Обработка нажатия кнопки "Оплатить"
        /// </summary>
        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка обязательных полей
            if (string.IsNullOrWhiteSpace(CustomerNameTextBox.Text))
            {
                MessageBox.Show("Введите ФИО покупателя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CustomerEmailTextBox.Text))
            {
                MessageBox.Show("Введите email", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CustomerPhoneTextBox.Text))
            {
                MessageBox.Show("Введите телефон", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(DeliveryAddressTextBox.Text))
            {
                MessageBox.Show("Введите адрес доставки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Создание заказа
                var order = new Order(
                    CustomerNameTextBox.Text.Trim(),
                    CustomerEmailTextBox.Text.Trim(),
                    CustomerPhoneTextBox.Text.Trim(),
                    DeliveryAddressTextBox.Text.Trim(),
                    _totalAmount,
                    CommentTextBox.Text.Trim()
                );

                // Сохранение заказа в базу данных
                var orderId = _orderRepository.Add(order);
                
                if (orderId > 0)
                {
                    MessageBox.Show($"Заказ #{order.OrderNumber} успешно создан! Теперь перейдите к оплате.", 
                        "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Открываем окно оплаты
                    var paymentWindow = new PaymentWindow(orderId);
                    paymentWindow.ShowDialog();
                    
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не удалось создать заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании заказа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработка нажатия кнопки "Назад"
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}