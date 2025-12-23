using System;

namespace MotorcycleShop.Data.Interfaces
{
    /// <summary>
    /// Класс фильтра для заказов
    /// </summary>
    public record OrderFilter
    {
        public static OrderFilter Empty => new();

        // Параметры фильтрации по дате
        public DateOnly? StartDate { get; init; }
        public DateOnly? EndDate { get; init; }

        // Дополнительные параметры фильтрации
        public string? CustomerName { get; init; }
        public string? Status { get; init; }
        public decimal? MinAmount { get; init; }
        public decimal? MaxAmount { get; init; }
    }
}