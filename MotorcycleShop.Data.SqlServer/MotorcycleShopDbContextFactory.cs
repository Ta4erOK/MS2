using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MotorcycleShop.Data.SqlServer
{
    /// <summary>
    /// Фабрика для создания DbContext при создании миграций
    /// </summary>
    public class MotorcycleShopDbContextFactory : IDesignTimeDbContextFactory<MotorcycleShopDbContext>
    {
        public MotorcycleShopDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.database.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<MotorcycleShopDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new MotorcycleShopDbContext(optionsBuilder.Options);
        }
    }
}