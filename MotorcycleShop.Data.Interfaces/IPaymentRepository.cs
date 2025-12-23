using System.Collections.Generic;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с платежами
    /// </summary>
    public interface IPaymentRepository
    {
        /// <summary>
        /// Добавление нового платежа
        /// </summary>
        int Add(Payment payment);

        /// <summary>
        /// Получение платежа по ID
        /// </summary>
        Payment GetById(int id);

        /// <summary>
        /// Получение всех платежей
        /// </summary>
        List<Payment> GetAll();

        /// <summary>
        /// Обновление информации о платеже
        /// </summary>
        bool Update(Payment payment);

        /// <summary>
        /// Удаление платежа
        /// </summary>
        bool Delete(int id);
    }
}