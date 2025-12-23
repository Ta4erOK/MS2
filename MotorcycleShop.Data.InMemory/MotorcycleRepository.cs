using System;
using System.Collections.Generic;
using System.Linq;
using MotorcycleShop.Domain;
using MotorcycleShop.Data.Interfaces;

namespace MotorcycleShop.Data.InMemory
{
    /// <summary>
    /// In-memory реализация репозитория для работы с мотоциклами
    /// </summary>
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly List<Motorcycle> _motorcycles = new List<Motorcycle>();
        private int _nextId = 1;

        public int Add(Motorcycle motorcycle)
        {
            if (motorcycle == null)
                throw new ArgumentNullException(nameof(motorcycle));

            motorcycle.Id = _nextId++;
            _motorcycles.Add(motorcycle);
            return motorcycle.Id;
        }

        public Motorcycle GetById(int id)
        {
            return _motorcycles.FirstOrDefault(m => m.Id == id);
        }

        public List<Motorcycle> GetAll()
        {
            return new List<Motorcycle>(_motorcycles);
        }

        public List<Motorcycle> GetAll(MotorcycleFilter filter)
        {
            if (filter == null)
                return GetAll();

            var result = _motorcycles.AsEnumerable();

            // Фильтрация по дате создания
            if (filter.StartDate.HasValue)
            {
                var startDateTime = filter.StartDate.Value.ToDateTime(TimeOnly.MinValue);
                result = result.Where(m => m.CreatedAt >= startDateTime);
            }

            if (filter.EndDate.HasValue)
            {
                var endDateTime = filter.EndDate.Value.ToDateTime(TimeOnly.MaxValue);
                result = result.Where(m => m.CreatedAt <= endDateTime);
            }

            // Фильтрация по бренду
            if (!string.IsNullOrEmpty(filter.Brand))
            {
                result = result.Where(m => m.Brand.Contains(filter.Brand, StringComparison.OrdinalIgnoreCase));
            }

            // Фильтрация по модели
            if (!string.IsNullOrEmpty(filter.Model))
            {
                result = result.Where(m => m.Model.Contains(filter.Model, StringComparison.OrdinalIgnoreCase));
            }

            // Фильтрация по году
            if (filter.Year.HasValue)
            {
                result = result.Where(m => m.Year == filter.Year.Value);
            }

            // Фильтрация по цене
            if (filter.MinPrice.HasValue)
            {
                result = result.Where(m => m.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                result = result.Where(m => m.Price <= filter.MaxPrice.Value);
            }

            // Фильтрация по наличию
            if (filter.InStock.HasValue)
            {
                result = result.Where(m => m.InStock == filter.InStock.Value);
            }

            return result.ToList();
        }

        public bool Update(Motorcycle motorcycle)
        {
            if (motorcycle == null)
                throw new ArgumentNullException(nameof(motorcycle));

            var existing = _motorcycles.FirstOrDefault(m => m.Id == motorcycle.Id);
            if (existing == null)
                return false;

            // Обновляем все свойства
            existing.Brand = motorcycle.Brand;
            existing.Model = motorcycle.Model;
            existing.Year = motorcycle.Year;
            existing.Color = motorcycle.Color;
            existing.EngineVolume = motorcycle.EngineVolume;
            existing.Mileage = motorcycle.Mileage;
            existing.Price = motorcycle.Price;
            existing.Description = motorcycle.Description;
            existing.ImageUrl = motorcycle.ImageUrl;
            existing.InStock = motorcycle.InStock;
            // CreatedAt не изменяем, так как это дата создания

            return true;
        }

        public bool Delete(int id)
        {
            var motorcycle = _motorcycles.FirstOrDefault(m => m.Id == id);
            if (motorcycle == null)
                return false;

            return _motorcycles.Remove(motorcycle);
        }
    }
}