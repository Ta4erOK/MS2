using Microsoft.EntityFrameworkCore;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.SqlServer
{
    /// <summary>
    /// Реализация репозитория для работы с заказами в SQL Server
    /// </summary>
    public class OrderSqlServerRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderSqlServerRepository(MotorcycleShopDbContext context) : base(context)
        {
        }

        public int Add(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _dbSet.Add(order);
            _context.SaveChanges();
            return order.Id;
        }

        public Order GetById(int id)
        {
            return _dbSet.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == id);
        }

        public List<Order> GetAll()
        {
            return _dbSet.Include(o => o.OrderItems).ToList();
        }

        public List<Order> GetAll(OrderFilter filter)
        {
            if (filter == null)
                return GetAll();

            var query = _dbSet.AsQueryable();

            // Фильтрация по дате создания
            if (filter.StartDate.HasValue)
            {
                var startDateTime = filter.StartDate.Value.ToDateTime(TimeOnly.MinValue);
                query = query.Where(o => o.OrderDate >= startDateTime);
            }

            if (filter.EndDate.HasValue)
            {
                var endDateTime = filter.EndDate.Value.ToDateTime(TimeOnly.MaxValue);
                query = query.Where(o => o.OrderDate <= endDateTime);
            }

            // Фильтрация по имени клиента
            if (!string.IsNullOrEmpty(filter.CustomerName))
            {
                query = query.Where(o => o.CustomerName.Contains(filter.CustomerName, StringComparison.OrdinalIgnoreCase));
            }

            // Фильтрация по статусу
            if (!string.IsNullOrEmpty(filter.Status))
            {
                query = query.Where(o => o.Status.Contains(filter.Status, StringComparison.OrdinalIgnoreCase));
            }

            // Фильтрация по сумме
            if (filter.MinAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount >= filter.MinAmount.Value);
            }

            if (filter.MaxAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount <= filter.MaxAmount.Value);
            }

            return query.Include(o => o.OrderItems).ToList();
        }

        public bool Update(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var existing = _dbSet.Find(order.Id);
            if (existing == null)
                return false;

            _context.Entry(existing).CurrentValues.SetValues(order);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var order = _dbSet.Find(id);
            if (order == null)
                return false;

            _dbSet.Remove(order);
            _context.SaveChanges();
            return true;
        }
    }
}