namespace MotorcycleShop.Domain
{
    /// <summary>
    /// Класс, представляющий позицию заказа
    /// </summary>
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // Ссылка на заказ
        public int MotorcycleId { get; set; } // Ссылка на мотоцикл
        public int Quantity { get; set; } // Количество (обычно 1 для мотоциклов)
        public decimal UnitPrice { get; set; } // Цена за единицу на момент заказа
        
        // Навигационные свойства
        public Order? Order { get; set; }
        public Motorcycle? Motorcycle { get; set; }

        // Конструктор по умолчанию
        public OrderItem()
        {
        }

        // Конструктор с параметрами
        public OrderItem(int orderId, int motorcycleId, decimal unitPrice, int quantity = 1)
        {
            OrderId = orderId;
            MotorcycleId = motorcycleId;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
    }
}