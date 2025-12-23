using System.Collections.Generic;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с корзиной
    /// </summary>
    public interface IShoppingCartRepository
    {
        /// <summary>
        /// Добавление новой корзины
        /// </summary>
        int Add(ShoppingCart shoppingCart);

        /// <summary>
        /// Получение корзины по ID
        /// </summary>
        ShoppingCart GetById(int id);

        /// <summary>
        /// Получение всех корзин
        /// </summary>
        List<ShoppingCart> GetAll();

        /// <summary>
        /// Обновление информации о корзине
        /// </summary>
        bool Update(ShoppingCart shoppingCart);

        /// <summary>
        /// Удаление корзины
        /// </summary>
        bool Delete(int id);
    }
}