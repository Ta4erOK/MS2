using System;
using System.Collections.Generic;
using System.Linq;
using MotorcycleShop.Domain;
using MotorcycleShop.Data.Interfaces;

namespace MotorcycleShop.Data.InMemory
{
    /// <summary>
    /// In-memory реализация репозитория для работы с заказами
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new List<Order>();
        private int _nextId = 1;

        public int Add(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            order.Id = _nextId++;
            _orders.Add(order);
            return order.Id;
        }

        public Order GetById(int id)
        {
            return _orders.FirstOrDefault(o => o.Id == id);
        }

        public List<Order> GetAll()
        {
            return new List<Order>(_orders);
        }

        public List<Order> GetAll(OrderFilter filter)
        {
            if (filter == null)
                return GetAll();

            var result = _orders.AsEnumerable();

            // Фильтрация по дате создания
            if (filter.StartDate.HasValue)
            {
                var startDateTime = filter.StartDate.Value.ToDateTime(TimeOnly.MinValue);
                result = result.Where(o => o.OrderDate >= startDateTime);
            }

            if (filter.EndDate.HasValue)
            {
                var endDateTime = filter.EndDate.Value.ToDateTime(TimeOnly.MaxValue);
                result = result.Where(o => o.OrderDate <= endDateTime);
            }

            // Фильтрация по имени клиента
            if (!string.IsNullOrEmpty(filter.CustomerName))
            {
                result = result.Where(o => o.CustomerName.Contains(filter.CustomerName, StringComparison.OrdinalIgnoreCase));
            }

            // Фильтрация по статусу
            if (!string.IsNullOrEmpty(filter.Status))
            {
                result = result.Where(o => o.Status.Contains(filter.Status, StringComparison.OrdinalIgnoreCase));
            }

            // Фильтрация по сумме
            if (filter.MinAmount.HasValue)
            {
                result = result.Where(o => o.TotalAmount >= filter.MinAmount.Value);
            }

            if (filter.MaxAmount.HasValue)
            {
                result = result.Where(o => o.TotalAmount <= filter.MaxAmount.Value);
            }

            return result.ToList();
        }

        public bool Update(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var existing = _orders.FirstOrDefault(o => o.Id == order.Id);
            if (existing == null)
                return false;

            // Обновляем все свойства
            existing.OrderNumber = order.OrderNumber;
            existing.CustomerName = order.CustomerName;
            existing.CustomerEmail = order.CustomerEmail;
            existing.CustomerPhone = order.CustomerPhone;
            existing.DeliveryAddress = order.DeliveryAddress;
            existing.OrderDate = order.OrderDate;
            existing.TotalAmount = order.TotalAmount;
            existing.Status = order.Status;
            existing.Comments = order.Comments;
            // OrderItems не обновляем здесь, для этого есть отдельный репозиторий

            return true;
        }

        public bool Delete(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
                return false;

            return _orders.Remove(order);
        }
    }
}