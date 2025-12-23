using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MotorcycleShop.UI
{
    /// <summary>
    /// Класс для настройки подключения к базе данных
    /// </summary>
    public static class DatabaseConfiguration
    {
        public static DbContextOptions<MotorcycleShop.Data.SqlServer.MotorcycleShopDbContext> GetDbContextOptions()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.database.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? 
                "Server=(localdb)\\MSSQLLocalDB;Database=MotorcycleShopDB;Integrated Security=True;TrustServerCertificate=True;";

            var optionsBuilder = new DbContextOptionsBuilder<MotorcycleShop.Data.SqlServer.MotorcycleShopDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return optionsBuilder.Options;
        }
    }
}