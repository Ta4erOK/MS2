using System;

namespace MotorcycleShop.Data.Interfaces
{
    /// <summary>
    /// Класс фильтра для мотоциклов
    /// </summary>
    public record MotorcycleFilter
    {
        public static MotorcycleFilter Empty => new();

        // Параметры фильтрации по дате
        public DateOnly? StartDate { get; init; }
        public DateOnly? EndDate { get; init; }

        // Дополнительные параметры фильтрации
        public string? Brand { get; init; }
        public string? Model { get; init; }
        public int? Year { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public bool? InStock { get; init; }
    }
}