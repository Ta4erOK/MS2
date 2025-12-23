using System.Collections.Generic;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с мотоциклами
    /// </summary>
    public interface IMotorcycleRepository
    {
        /// <summary>
        /// Добавление нового мотоцикла
        /// </summary>
        int Add(Motorcycle motorcycle);

        /// <summary>
        /// Получение мотоцикла по ID
        /// </summary>
        Motorcycle GetById(int id);

        /// <summary>
        /// Получение всех мотоциклов
        /// </summary>
        List<Motorcycle> GetAll();

        /// <summary>
        /// Получение мотоциклов с фильтрацией
        /// </summary>
        List<Motorcycle> GetAll(MotorcycleFilter filter);

        /// <summary>
        /// Обновление информации о мотоцикле
        /// </summary>
        bool Update(Motorcycle motorcycle);

        /// <summary>
        /// Удаление мотоцикла
        /// </summary>
        bool Delete(int id);
    }
}