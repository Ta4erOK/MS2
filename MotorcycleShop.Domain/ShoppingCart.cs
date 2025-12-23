using System;
using System.Collections.Generic;

namespace MotorcycleShop.Domain
{
    /// <summary>
    /// Класс, представляющий корзину пользователя
    /// </summary>
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string SessionId { get; set; } = string.Empty; // Идентификатор сессии пользователя
        public DateTime CreatedDate { get; set; } // Дата создания корзины
        public DateTime LastModified { get; set; } // Дата последнего изменения
        public decimal TotalAmount { get; set; } // Общая сумма товаров в корзине
        
        // Навигационное свойство для позиций корзины
        public List<ShoppingCartItem> CartItems { get; set; } = new List<ShoppingCartItem>();

        // Конструктор по умолчанию
        public ShoppingCart()
        {
        }

        // Конструктор с параметрами
        public ShoppingCart(string sessionId)
        {
            SessionId = sessionId;
            CreatedDate = DateTime.Now;
            LastModified = DateTime.Now;
            TotalAmount = 0;
        }
    }
}