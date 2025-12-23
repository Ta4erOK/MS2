using Microsoft.EntityFrameworkCore;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.SqlServer
{
    /// <summary>
    /// Реализация репозитория для работы с позициями заказа в SQL Server
    /// </summary>
    public class OrderItemSqlServerRepository : BaseRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemSqlServerRepository(MotorcycleShopDbContext context) : base(context)
        {
        }

        public int Add(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            _dbSet.Add(orderItem);
            _context.SaveChanges();
            return orderItem.Id;
        }

        public OrderItem GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public List<OrderItem> GetAll()
        {
            return _dbSet.ToList();
        }

        public bool Update(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            var existing = _dbSet.Find(orderItem.Id);
            if (existing == null)
                return false;

            _context.Entry(existing).CurrentValues.SetValues(orderItem);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var orderItem = _dbSet.Find(id);
            if (orderItem == null)
                return false;

            _dbSet.Remove(orderItem);
            _context.SaveChanges();
            return true;
        }
    }
}