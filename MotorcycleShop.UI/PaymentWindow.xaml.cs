using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using MotorcycleShop.Domain;

namespace MotorcycleShop.UI
{
    /// <summary>
    /// Логика взаимодействия для PaymentWindow.xaml
    /// Форма для оплаты заказа банковской картой
    /// </summary>
    public partial class PaymentWindow : Window
    {
        private readonly int _orderId;
        private Order _order;
        private readonly MotorcycleShop.Data.SqlServer.PaymentSqlServerRepository _paymentRepository;
        private readonly MotorcycleShop.Data.SqlServer.OrderSqlServerRepository _orderRepository;

        public PaymentWindow(int orderId)
        {
            InitializeComponent();

            _orderId = orderId;

            // Инициализация репозиториев
            var options = DatabaseConfiguration.GetDbContextOptions();
            var context = new MotorcycleShop.Data.SqlServer.MotorcycleShopDbContext(options);
            _paymentRepository = new MotorcycleShop.Data.SqlServer.PaymentSqlServerRepository(context);
            _orderRepository = new MotorcycleShop.Data.SqlServer.OrderSqlServerRepository(context);

            LoadOrderInfo();
        }

        /// <summary>
        /// Загрузка информации о заказе
        /// </summary>
        private void LoadOrderInfo()
        {
            try
            {
                _order = _orderRepository.GetById(_orderId);
                if (_order != null)
                {
                    OrderNumberTextBlock.Text = _order.OrderNumber;
                    OrderAmountTextBlock.Text = _order.TotalAmount.ToString("N2") + " руб.";
                    AmountToPayTextBlock.Text = _order.TotalAmount.ToString("N2") + " руб.";
                }
                else
                {
                    MessageBox.Show("Заказ не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке информации о заказе: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        /// <summary>
        /// Проверка номера карты
        /// </summary>
        private bool ValidateCardNumber(string cardNumber)
        {
            // Убираем пробелы
            cardNumber = cardNumber.Replace(" ", "");

            // Проверяем, что строка содержит только цифры и имеет длину 16
            return cardNumber.Length == 16 && Regex.IsMatch(cardNumber, @"^\d{16}$");
        }

        /// <summary>
        /// Проверка срока действия карты
        /// </summary>
        private bool ValidateExpiryDate(string expiryDate)
        {
            // Проверяем формат MM/YY или MMYY
            if (!Regex.IsMatch(expiryDate, @"^(0[1-9]|1[0-2])\/?(\d{2})$"))
            {
                return false;
            }

            // Убираем слэш для парсинга
            string cleanDate = expiryDate.Replace("/", "");
            if (cleanDate.Length != 4)
            {
                return false;
            }

            // Парсим месяц и год
            int month = int.Parse(cleanDate.Substring(0, 2));
            int year = int.Parse(cleanDate.Substring(2, 2));
            int fullYear = 2000 + year; // Преобразуем 2-digit год в 4-digit

            // Создаем дату - первый день следующего месяца
            DateTime expiry = new DateTime(fullYear, month, 1).AddMonths(1);

            // Проверяем, что срок действия больше текущей даты
            return expiry > DateTime.Now;
        }

        /// <summary>
        /// Обработчик изменения текста в поле номера карты
        /// </summary>
        private void CardNumberTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var textBox = sender as System.Windows.Controls.TextBox;
            if (textBox == null) return;

            // Сохраняем текущую позицию курсора
            int caretIndex = textBox.CaretIndex;

            // Получаем текущий текст и удаляем все пробелы
            string text = textBox.Text.Replace(" ", "");

            // Ограничиваем длину до 16 символов
            if (text.Length > 16)
            {
                text = text.Substring(0, 16);
            }

            // Форматируем с пробелами каждые 4 цифры
            string formattedText = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (i > 0 && i % 4 == 0)
                {
                    formattedText += " ";
                }
                formattedText += text[i];
            }

            // Обновляем текст, если он изменился
            if (textBox.Text != formattedText)
            {
                textBox.Text = formattedText;

                // Корректируем позицию курсора
                int newCaretIndex = caretIndex;
                if (formattedText.Length > text.Length)
                {
                    // Если добавился пробел, корректируем позицию
                    int spacesAdded = formattedText.Count(c => c == ' ');
                    newCaretIndex = Math.Min(formattedText.Length, caretIndex + spacesAdded);
                }

                textBox.CaretIndex = newCaretIndex;
            }
        }

        /// <summary>
        /// Обработчик изменения текста в поле срока действия
        /// </summary>
        private void ExpiryDateTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var textBox = sender as System.Windows.Controls.TextBox;
            if (textBox == null) return;

            // Сохраняем текущую позицию курсора
            int caretIndex = textBox.CaretIndex;

            // Получаем текущий текст и удаляем все слэши
            string text = textBox.Text.Replace("/", "");

            // Ограничиваем длину до 4 символов
            if (text.Length > 4)
            {
                text = text.Substring(0, 4);
            }

            // Форматируем с добавлением слэша после 2-х символов
            string formattedText = text;
            if (text.Length > 2)
            {
                formattedText = text.Substring(0, 2) + "/" + text.Substring(2);
            }

            // Обновляем текст, если он изменился
            if (textBox.Text != formattedText)
            {
                textBox.Text = formattedText;

                // Корректируем позицию курсора
                int newCaretIndex = caretIndex;
                if (formattedText.Length > text.Length && caretIndex >= 2)
                {
                    // Если добавился слэш, корректируем позицию
                    newCaretIndex = Math.Min(formattedText.Length, caretIndex + 1);
                }

                textBox.CaretIndex = newCaretIndex;
            }
        }

        /// <summary>
        /// Проверка CVV
        /// </summary>
        private bool ValidateCvv(string cvv)
        {
            return cvv.Length == 3 && Regex.IsMatch(cvv, @"^\d{3}$");
        }

        /// <summary>
        /// Обработка нажатия кнопки "Оплатить"
        /// </summary>
        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация данных карты
            if (!ValidateCardNumber(CardNumberTextBox.Text))
            {
                MessageBox.Show("Введите корректный номер карты (16 цифр)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!ValidateExpiryDate(ExpiryDateTextBox.Text))
            {
                MessageBox.Show("Введите корректный срок действия (MM/YY)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!ValidateCvv(CvvTextBox.Text))
            {
                MessageBox.Show("Введите корректный CVV код (3 цифры)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CardholderNameTextBox.Text))
            {
                MessageBox.Show("Введите имя владельца карты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Имитация обработки платежа
                // В реальном приложении здесь будет вызов платежного шлюза
                
                // Получаем последние 4 цифры карты
                string cardNumber = CardNumberTextBox.Text.Replace(" ", "");
                string lastFourDigits = cardNumber.Substring(cardNumber.Length - 4);
                
                // Создаем платеж
                var payment = new Payment(_orderId, _order.TotalAmount, lastFourDigits);
                payment.PaymentMethod = "Карта";
                
                // Сохраняем платеж в базу данных
                var paymentId = _paymentRepository.Add(payment);
                
                if (paymentId > 0)
                {
                    // Обновляем статус заказа
                    _order.Status = "Оплачен";
                    _orderRepository.Update(_order);

                    MessageBox.Show("Оплата прошла успешно! Ваш заказ обрабатывается.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка при обработке платежа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обработке платежа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработка нажатия кнопки "Назад"
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Обработка нажатия кнопки "Отмена"
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}