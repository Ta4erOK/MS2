using System.Collections.Generic;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с заказами
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Добавление нового заказа
        /// </summary>
        int Add(Order order);

        /// <summary>
        /// Получение заказа по ID
        /// </summary>
        Order GetById(int id);

        /// <summary>
        /// Получение всех заказов
        /// </summary>
        List<Order> GetAll();

        /// <summary>
        /// Получение заказов с фильтрацией
        /// </summary>
        List<Order> GetAll(OrderFilter filter);

        /// <summary>
        /// Обновление информации о заказе
        /// </summary>
        bool Update(Order order);

        /// <summary>
        /// Удаление заказа
        /// </summary>
        bool Delete(int id);
    }
}