using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.Windows;
using MotorcycleShop.Data.SqlServer;

namespace MotorcycleShop.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Создаем контекст базы данных и создаем базу данных, если она не существует
        var options = DatabaseConfiguration.GetDbContextOptions();
        using var context = new MotorcycleShopDbContext(options);

        // Создаем базу данных, если она не существует
        context.Database.EnsureCreated();



        base.OnStartup(e);
    }
}

