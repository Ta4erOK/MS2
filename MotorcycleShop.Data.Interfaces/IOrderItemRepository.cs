using System.Collections.Generic;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с позициями заказа
    /// </summary>
    public interface IOrderItemRepository
    {
        /// <summary>
        /// Добавление новой позиции заказа
        /// </summary>
        int Add(OrderItem orderItem);

        /// <summary>
        /// Получение позиции заказа по ID
        /// </summary>
        OrderItem GetById(int id);

        /// <summary>
        /// Получение всех позиций заказа
        /// </summary>
        List<OrderItem> GetAll();

        /// <summary>
        /// Обновление информации о позиции заказа
        /// </summary>
        bool Update(OrderItem orderItem);

        /// <summary>
        /// Удаление позиции заказа
        /// </summary>
        bool Delete(int id);
    }
}