using System;

namespace MotorcycleShop.Domain
{
    /// <summary>
    /// Класс, представляющий платеж по заказу
    /// </summary>
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // Ссылка на заказ
        public DateTime PaymentDate { get; set; } // Дата и время платежа
        public decimal Amount { get; set; } // Сумма платежа
        public string PaymentMethod { get; set; } = "Карта"; // Способ оплаты ("Карта")
        public string CardLastFour { get; set; } = string.Empty; // Последние 4 цифры карты
        public string? TransactionId { get; set; } // Идентификатор транзакции платежной системы
        public string Status { get; set; } = "Ожидает"; // Статус платежа ("Успешно", "Ошибка", "Ожидает")

        // Навигационное свойство
        public Order? Order { get; set; }

        // Конструктор по умолчанию
        public Payment()
        {
        }

        // Конструктор с параметрами
        public Payment(int orderId, decimal amount, string cardLastFour, string? transactionId = null)
        {
            OrderId = orderId;
            Amount = amount;
            CardLastFour = cardLastFour;
            TransactionId = transactionId;
            PaymentDate = DateTime.Now;
            Status = "Ожидает";
        }
    }
}