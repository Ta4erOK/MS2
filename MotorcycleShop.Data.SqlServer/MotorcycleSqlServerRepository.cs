using Microsoft.EntityFrameworkCore;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.SqlServer
{
    /// <summary>
    /// Реализация репозитория для работы с мотоциклами в SQL Server
    /// </summary>
    public class MotorcycleSqlServerRepository : BaseRepository<Motorcycle>, IMotorcycleRepository
    {
        public MotorcycleSqlServerRepository(MotorcycleShopDbContext context) : base(context)
        {
        }

        public int Add(Motorcycle motorcycle)
        {
            if (motorcycle == null)
                throw new ArgumentNullException(nameof(motorcycle));

            _dbSet.Add(motorcycle);
            _context.SaveChanges();
            return motorcycle.Id;
        }

        public Motorcycle GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public List<Motorcycle> GetAll()
        {
            return _dbSet.ToList();
        }

        public List<Motorcycle> GetAll(MotorcycleFilter filter)
        {
            if (filter == null)
                return GetAll();

            var query = _dbSet.AsQueryable();

            // Фильтрация по дате создания
            if (filter.StartDate.HasValue)
            {
                var startDateTime = filter.StartDate.Value.ToDateTime(TimeOnly.MinValue);
                query = query.Where(m => m.CreatedAt >= startDateTime);
            }

            if (filter.EndDate.HasValue)
            {
                var endDateTime = filter.EndDate.Value.ToDateTime(TimeOnly.MaxValue);
                query = query.Where(m => m.CreatedAt <= endDateTime);
            }

            // Фильтрация по бренду
            if (!string.IsNullOrEmpty(filter.Brand))
            {
                query = query.Where(m => m.Brand.Contains(filter.Brand, StringComparison.OrdinalIgnoreCase));
            }

            // Фильтрация по модели
            if (!string.IsNullOrEmpty(filter.Model))
            {
                query = query.Where(m => m.Model.Contains(filter.Model, StringComparison.OrdinalIgnoreCase));
            }

            // Фильтрация по году
            if (filter.Year.HasValue)
            {
                query = query.Where(m => m.Year == filter.Year.Value);
            }

            // Фильтрация по цене
            if (filter.MinPrice.HasValue)
            {
                query = query.Where(m => m.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(m => m.Price <= filter.MaxPrice.Value);
            }

            // Фильтрация по наличию
            if (filter.InStock.HasValue)
            {
                query = query.Where(m => m.InStock == filter.InStock.Value);
            }

            return query.ToList();
        }

        public bool Update(Motorcycle motorcycle)
        {
            if (motorcycle == null)
                throw new ArgumentNullException(nameof(motorcycle));

            var existing = _dbSet.Find(motorcycle.Id);
            if (existing == null)
                return false;

            _context.Entry(existing).CurrentValues.SetValues(motorcycle);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var motorcycle = _dbSet.Find(id);
            if (motorcycle == null)
                return false;

            _dbSet.Remove(motorcycle);
            _context.SaveChanges();
            return true;
        }
    }
}