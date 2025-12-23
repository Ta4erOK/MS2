using System;
using System.Collections.Generic;
using System.Linq;
using MotorcycleShop.Domain;
using MotorcycleShop.Domain.Statistics;
using MotorcycleShop.Data.Interfaces;

namespace MotorcycleShop.Services
{
    /// <summary>
    /// Сервис для расчёта статистики по продажам мотоциклов
    /// </summary>
    public class StatisticsService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public StatisticsService(IMotorcycleRepository motorcycleRepository, 
            IOrderRepository orderRepository, 
            IOrderItemRepository orderItemRepository)
        {
            _motorcycleRepository = motorcycleRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        /// <summary>
        /// Получить распределение заказов по статусам
        /// </summary>
        public List<OrderStatusStatisticItem> GetOrdersByStatus()
        {
            var orders = _orderRepository.GetAll();

            return orders
                .GroupBy(o => o.Status)
                .Select(g => new OrderStatusStatisticItem
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .OrderBy(s => s.Status)
                .ToList();
        }

        /// <summary>
        /// Получить динамику заказов по месяцам
        /// </summary>
        public List<MonthStatisticItem> GetOrdersByMonth()
        {
            var orders = _orderRepository.GetAll();

            return orders
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new MonthStatisticItem
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(m => m.Year)
                .ThenBy(m => m.Month)
                .ToList();
        }

        /// <summary>
        /// Получить статистику по популярным мотоциклам
        /// </summary>
        public List<PopularMotorcycleStatisticItem> GetPopularMotorcycles()
        {
            var orderItems = _orderItemRepository.GetAll();
            var motorcycles = _motorcycleRepository.GetAll();

            // Объединяем данные о позициях заказов с информацией о мотоциклах
            var orderItemWithMotorcycles = orderItems
                .Join(motorcycles,
                    oi => oi.MotorcycleId,
                    m => m.Id,
                    (oi, m) => new { OrderItem = oi, Motorcycle = m })
                .GroupBy(j => j.Motorcycle.Brand + " " + j.Motorcycle.Model)
                .Select(g => new PopularMotorcycleStatisticItem
                {
                    MotorcycleName = g.Key,
                    Count = g.Sum(j => j.OrderItem.Quantity)
                })
                .OrderByDescending(s => s.Count)
                .ToList();

            return orderItemWithMotorcycles;
        }
    }
}