using System.Collections.Generic;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с позициями корзины
    /// </summary>
    public interface IShoppingCartItemRepository
    {
        /// <summary>
        /// Добавление новой позиции корзины
        /// </summary>
        int Add(ShoppingCartItem shoppingCartItem);

        /// <summary>
        /// Получение позиции корзины по ID
        /// </summary>
        ShoppingCartItem GetById(int id);

        /// <summary>
        /// Получение всех позиций корзины
        /// </summary>
        List<ShoppingCartItem> GetAll();

        /// <summary>
        /// Обновление информации о позиции корзины
        /// </summary>
        bool Update(ShoppingCartItem shoppingCartItem);

        /// <summary>
        /// Удаление позиции корзины
        /// </summary>
        bool Delete(int id);
    }
}