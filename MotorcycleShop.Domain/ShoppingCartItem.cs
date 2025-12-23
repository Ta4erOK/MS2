using System;

namespace MotorcycleShop.Domain
{
    /// <summary>
    /// Класс, представляющий позицию корзины
    /// </summary>
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public int ShoppingCartId { get; set; } // Ссылка на корзину
        public int MotorcycleId { get; set; } // Ссылка на мотоцикл
        public int Quantity { get; set; } // Количество (обычно 1 для мотоциклов)
        public DateTime AddedDate { get; set; } // Дата добавления в корзину
        
        // Навигационные свойства
        public ShoppingCart? ShoppingCart { get; set; }
        public Motorcycle? Motorcycle { get; set; }

        // Конструктор по умолчанию
        public ShoppingCartItem()
        {
        }

        // Конструктор с параметрами
        public ShoppingCartItem(int shoppingCartId, int motorcycleId, int quantity = 1)
        {
            ShoppingCartId = shoppingCartId;
            MotorcycleId = motorcycleId;
            Quantity = quantity;
            AddedDate = DateTime.Now;
        }
    }
}