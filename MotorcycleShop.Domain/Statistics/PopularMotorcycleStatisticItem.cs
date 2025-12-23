namespace MotorcycleShop.Domain.Statistics
{
    /// <summary>
    /// Класс для хранения статистики по популярным мотоциклам
    /// </summary>
    public record PopularMotorcycleStatisticItem
    {
        public required string MotorcycleName { get; set; }
        public required int Count { get; set; }
    }
}