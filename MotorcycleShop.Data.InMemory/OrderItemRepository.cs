using System;
using System.Collections.Generic;
using System.Linq;
using MotorcycleShop.Domain;
using MotorcycleShop.Data.Interfaces;

namespace MotorcycleShop.Data.InMemory
{
    /// <summary>
    /// In-memory реализация репозитория для работы с позициями заказа
    /// </summary>
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly List<OrderItem> _orderItems = new List<OrderItem>();
        private int _nextId = 1;

        public int Add(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            orderItem.Id = _nextId++;
            _orderItems.Add(orderItem);
            return orderItem.Id;
        }

        public OrderItem GetById(int id)
        {
            return _orderItems.FirstOrDefault(oi => oi.Id == id);
        }

        public List<OrderItem> GetAll()
        {
            return new List<OrderItem>(_orderItems);
        }

        public bool Update(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            var existing = _orderItems.FirstOrDefault(oi => oi.Id == orderItem.Id);
            if (existing == null)
                return false;

            // Обновляем все свойства
            existing.OrderId = orderItem.OrderId;
            existing.MotorcycleId = orderItem.MotorcycleId;
            existing.Quantity = orderItem.Quantity;
            existing.UnitPrice = orderItem.UnitPrice;

            return true;
        }

        public bool Delete(int id)
        {
            var orderItem = _orderItems.FirstOrDefault(oi => oi.Id == id);
            if (orderItem == null)
                return false;

            return _orderItems.Remove(orderItem);
        }
    }
}