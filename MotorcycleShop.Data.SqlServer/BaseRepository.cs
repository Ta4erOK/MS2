using Microsoft.EntityFrameworkCore;
using MotorcycleShop.Data.Interfaces;

namespace MotorcycleShop.Data.SqlServer
{
    /// <summary>
    /// Базовый класс для SQL Server репозиториев
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    public abstract class BaseRepository<TEntity> where TEntity : class
    {
        protected readonly MotorcycleShopDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(MotorcycleShopDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
    }
}