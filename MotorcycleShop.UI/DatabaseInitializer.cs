using Microsoft.EntityFrameworkCore;
using MotorcycleShop.Data.SqlServer;

namespace MotorcycleShop.UI
{
    /// <summary>
    /// Класс для инициализации базы данных
    /// </summary>
    public static class DatabaseInitializer
    {
        public static void InitializeDatabase()
        {
            var options = DatabaseConfiguration.GetDbContextOptions();
            using var context = new MotorcycleShopDbContext(options);
            
            // Создаем базу данных и таблицы, если они не существуют
            context.Database.EnsureCreated();
        }
    }
}