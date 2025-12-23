using Microsoft.EntityFrameworkCore;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.SqlServer
{
    /// <summary>
    /// Реализация репозитория для работы с позициями корзины в SQL Server
    /// </summary>
    public class ShoppingCartItemSqlServerRepository : BaseRepository<ShoppingCartItem>, IShoppingCartItemRepository
    {
        public ShoppingCartItemSqlServerRepository(MotorcycleShopDbContext context) : base(context)
        {
        }

        public int Add(ShoppingCartItem shoppingCartItem)
        {
            if (shoppingCartItem == null)
                throw new ArgumentNullException(nameof(shoppingCartItem));

            _dbSet.Add(shoppingCartItem);
            _context.SaveChanges();
            return shoppingCartItem.Id;
        }

        public ShoppingCartItem GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public List<ShoppingCartItem> GetAll()
        {
            return _dbSet.ToList();
        }

        public bool Update(ShoppingCartItem shoppingCartItem)
        {
            if (shoppingCartItem == null)
                throw new ArgumentNullException(nameof(shoppingCartItem));

            var existing = _dbSet.Find(shoppingCartItem.Id);
            if (existing == null)
                return false;

            _context.Entry(existing).CurrentValues.SetValues(shoppingCartItem);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var shoppingCartItem = _dbSet.Find(id);
            if (shoppingCartItem == null)
                return false;

            _dbSet.Remove(shoppingCartItem);
            _context.SaveChanges();
            return true;
        }
    }
}