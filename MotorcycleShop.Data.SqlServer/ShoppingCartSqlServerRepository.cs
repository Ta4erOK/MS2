using Microsoft.EntityFrameworkCore;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.SqlServer
{
    /// <summary>
    /// Реализация репозитория для работы с корзинами в SQL Server
    /// </summary>
    public class ShoppingCartSqlServerRepository : BaseRepository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartSqlServerRepository(MotorcycleShopDbContext context) : base(context)
        {
        }

        public int Add(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
                throw new ArgumentNullException(nameof(shoppingCart));

            _dbSet.Add(shoppingCart);
            _context.SaveChanges();
            return shoppingCart.Id;
        }

        public ShoppingCart GetById(int id)
        {
            return _dbSet.Include(sc => sc.CartItems).FirstOrDefault(sc => sc.Id == id);
        }

        public List<ShoppingCart> GetAll()
        {
            return _dbSet.Include(sc => sc.CartItems).ToList();
        }

        public bool Update(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
                throw new ArgumentNullException(nameof(shoppingCart));

            var existing = _dbSet.Find(shoppingCart.Id);
            if (existing == null)
                return false;

            _context.Entry(existing).CurrentValues.SetValues(shoppingCart);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var shoppingCart = _dbSet.Find(id);
            if (shoppingCart == null)
                return false;

            _dbSet.Remove(shoppingCart);
            _context.SaveChanges();
            return true;
        }
    }
}