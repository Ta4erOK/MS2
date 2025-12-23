using System;
using System.Collections.Generic;

namespace MotorcycleShop.Domain
{
    /// <summary>
    /// Класс, представляющий заказ пользователя
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty; // Номер заказа для отслеживания
        public string CustomerName { get; set; } = string.Empty; // ФИО покупателя
        public string CustomerEmail { get; set; } = string.Empty; // Email покупателя
        public string CustomerPhone { get; set; } = string.Empty; // Телефон покупателя
        public string DeliveryAddress { get; set; } = string.Empty; // Адрес доставки
        public DateTime OrderDate { get; set; } // Дата создания заказа
        public decimal TotalAmount { get; set; } // Общая сумма заказа
        public string Status { get; set; } = "Ожидает оплаты"; // Статус заказа
        public string? Comments { get; set; } // Комментарии к заказу
        
        // Навигационное свойство для позиций заказа
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // Конструктор по умолчанию
        public Order()
        {
        }

        // Конструктор с параметрами
        public Order(string customerName, string customerEmail, string customerPhone, 
            string deliveryAddress, decimal totalAmount, string? comments = null)
        {
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            CustomerPhone = customerPhone;
            DeliveryAddress = deliveryAddress;
            TotalAmount = totalAmount;
            Comments = comments;
            OrderDate = DateTime.Now;
            Status = "Ожидает оплаты";
            
            // Генерация номера заказа (например, дата + случайное число)
            OrderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";
        }
    }
}