namespace MotorcycleShop.Domain.Statistics
{
    /// <summary>
    /// Класс для хранения статистики по статусам заказов
    /// </summary>
    public record OrderStatusStatisticItem
    {
        public required string Status { get; set; }
        public required int Count { get; set; }
    }
}