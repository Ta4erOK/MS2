namespace MotorcycleShop.Domain
{
    /// <summary>
    /// Перечисление статусов заказа
    /// </summary>
    public enum OrderStatus
    {
        PendingPayment = 0,    // Ожидает оплаты
        Paid = 1,              // Оплачен
        Processing = 2,        // В обработке
        Shipped = 3,           // Доставляется
        Completed = 4,         // Завершен
        Canceled = 5           // Отменен
    }
}