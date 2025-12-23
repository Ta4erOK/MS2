using System;

namespace MotorcycleShop.Domain
{
    /// <summary>
    /// Класс, представляющий мотоцикл в каталоге
    /// </summary>
    public class Motorcycle
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty; // Марка мотоцикла (Yamaha, Honda, Kawasaki, etc.)
        public string Model { get; set; } = string.Empty; // Модель мотоцикла
        public int Year { get; set; } // Год выпуска
        public string Color { get; set; } = string.Empty; // Цвет
        public decimal EngineVolume { get; set; } // Объем двигателя (см³)
        public int Mileage { get; set; } // Пробег (км)
        public decimal Price { get; set; } // Цена
        public string? Description { get; set; } // Описание мотоцикла
        public string? ImageUrl { get; set; } // URL изображения
        public bool InStock { get; set; } // Доступен ли для заказа
        public DateTime CreatedAt { get; set; } // Дата добавления в каталог

        // Конструктор по умолчанию
        public Motorcycle()
        {
        }

        // Конструктор с параметрами
        public Motorcycle(string brand, string model, int year, string color, 
            decimal engineVolume, int mileage, decimal price, string? description = null, 
            string? imageUrl = null, bool inStock = true)
        {
            Brand = brand;
            Model = model;
            Year = year;
            Color = color;
            EngineVolume = engineVolume;
            Mileage = mileage;
            Price = price;
            Description = description;
            ImageUrl = imageUrl;
            InStock = inStock;
            CreatedAt = DateTime.Now;
        }
    }
}