using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MotorcycleShop.Domain;

namespace MotorcycleShop.UI;

/// <summary>
/// Логика взаимодействия для MainWindow.xaml
/// Главная форма приложения с каталогом мотоциклов и навигацией
/// </summary>
public partial class MainWindow : Window
{
    // Список мотоциклов для отображения
    private List<Motorcycle> _motorcycles;
    // Список всех мотоциклов (для фильтрации)
    private List<Motorcycle> _allMotorcycles;

    public MainWindow()
    {
        InitializeComponent();
        LoadMotorcycles();
        SetupFilters();
    }

    /// <summary>
    /// Загрузка мотоциклов из репозитория
    /// </summary>
    private void LoadMotorcycles()
    {
        try
        {
            // Создаем контекст базы данных
            var options = DatabaseConfiguration.GetDbContextOptions();
            using var context = new MotorcycleShop.Data.SqlServer.MotorcycleShopDbContext(options);

            // Создаем репозиторий
            var repository = new MotorcycleShop.Data.SqlServer.MotorcycleSqlServerRepository(context);

            _motorcycles = repository.GetAll();
            _allMotorcycles = new List<Motorcycle>(_motorcycles);
            MotorcyclesDataGrid.ItemsSource = _motorcycles;
            UpdateStatus($"Загружено {_motorcycles.Count} мотоциклов");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при загрузке мотоциклов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            _motorcycles = new List<Motorcycle>();
            _allMotorcycles = new List<Motorcycle>();
            MotorcyclesDataGrid.ItemsSource = _motorcycles;
        }
    }

    /// <summary>
    /// Настройка фильтров
    /// </summary>
    private void SetupFilters()
    {
        // Заполнение фильтра по марке
        var brands = new List<string>();
        foreach (var motorcycle in _allMotorcycles)
        {
            if (!brands.Contains(motorcycle.Brand))
            {
                brands.Add(motorcycle.Brand);
            }
        }
        brands.Sort();
        BrandFilterComboBox.ItemsSource = brands;

        // Заполнение фильтра по году
        var years = new List<int>();
        foreach (var motorcycle in _allMotorcycles)
        {
            if (!years.Contains(motorcycle.Year))
            {
                years.Add(motorcycle.Year);
            }
        }
        years.Sort();
        YearFilterComboBox.ItemsSource = years;
    }

    /// <summary>
    /// Обновление статуса
    /// </summary>
    private void UpdateStatus(string message)
    {
        StatusTextBlock.Text = message;
    }

    /// <summary>
    /// Обработка нажатия кнопки поиска
    /// </summary>
    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        ApplyFilters();
    }

    /// <summary>
    /// Сброс фильтров
    /// </summary>
    private void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
    {
        SearchTextBox.Text = string.Empty;
        BrandFilterComboBox.SelectedIndex = -1;
        YearFilterComboBox.SelectedIndex = -1;
        MinPriceTextBox.Text = string.Empty;
        MaxPriceTextBox.Text = string.Empty;

        _motorcycles = new List<Motorcycle>(_allMotorcycles);
        MotorcyclesDataGrid.ItemsSource = _motorcycles;
        UpdateStatus($"Фильтры сброшены. Показано {_motorcycles.Count} мотоциклов");
    }

    /// <summary>
    /// Применение фильтра по цене
    /// </summary>
    private void ApplyPriceFilterButton_Click(object sender, RoutedEventArgs e)
    {
        ApplyFilters();
    }

    /// <summary>
    /// Применение всех фильтров
    /// </summary>
    private void ApplyFilters()
    {
        var filtered = new List<Motorcycle>(_allMotorcycles);

        // Фильтр по поисковому запросу
        if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
        {
            string searchTerm = SearchTextBox.Text.ToLower();
            filtered = filtered.FindAll(m =>
                m.Brand.ToLower().Contains(searchTerm) ||
                m.Model.ToLower().Contains(searchTerm) ||
                m.Description?.ToLower().Contains(searchTerm) == true);
        }

        // Фильтр по марке
        if (BrandFilterComboBox.SelectedItem != null)
        {
            string selectedBrand = BrandFilterComboBox.SelectedItem.ToString();
            filtered = filtered.FindAll(m => m.Brand == selectedBrand);
        }

        // Фильтр по году
        if (YearFilterComboBox.SelectedItem != null)
        {
            if (int.TryParse(YearFilterComboBox.SelectedItem.ToString(), out int selectedYear))
            {
                filtered = filtered.FindAll(m => m.Year == selectedYear);
            }
        }

        // Фильтр по цене
        decimal minPrice = 0;
        decimal maxPrice = decimal.MaxValue;

        if (decimal.TryParse(MinPriceTextBox.Text, out decimal parsedMinPrice))
        {
            minPrice = parsedMinPrice;
        }

        if (decimal.TryParse(MaxPriceTextBox.Text, out decimal parsedMaxPrice))
        {
            maxPrice = parsedMaxPrice;
        }

        filtered = filtered.FindAll(m => m.Price >= minPrice && m.Price <= maxPrice);

        _motorcycles = filtered;
        MotorcyclesDataGrid.ItemsSource = _motorcycles;
        UpdateStatus($"Найдено {_motorcycles.Count} мотоциклов по фильтрам");
    }

    /// <summary>
    /// Обработка двойного клика по строке мотоцикла
    /// </summary>
    private void MotorcyclesDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var selectedMotorcycle = MotorcyclesDataGrid.SelectedItem as Motorcycle;
        if (selectedMotorcycle != null)
        {
            // Спрашиваем пользователя, хочет ли он посмотреть детали или добавить в корзину
            var result = MessageBox.Show($"Посмотреть детали мотоцикла {selectedMotorcycle.Brand} {selectedMotorcycle.Model} или добавить в корзину?",
                "Выбор действия",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question,
                MessageBoxResult.Yes);

            if (result == MessageBoxResult.Yes)
            {
                ShowMotorcycleDetails(selectedMotorcycle);
            }
            else if (result == MessageBoxResult.No)
            {
                AddToCart(selectedMotorcycle);
            }
            // Cancel - ничего не делаем
        }
    }
/// <summary>
    /// Обработка изменения выбора в DataGrid
    /// </summary>
    private void MotorcyclesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Здесь можно добавить логику при изменении выбора
    }

    /// <summary>
    /// Добавление мотоцикла в корзину
    /// </summary>
    private void AddToCart(Motorcycle selectedMotorcycle)
    {
        try
        {
            // Создаем контекст базы данных
            var options = DatabaseConfiguration.GetDbContextOptions();
            using var context = new MotorcycleShop.Data.SqlServer.MotorcycleShopDbContext(options);

            // Создаем репозитории
            var shoppingCartRepo = new MotorcycleShop.Data.SqlServer.ShoppingCartSqlServerRepository(context);
            var shoppingCartItemRepo = new MotorcycleShop.Data.SqlServer.ShoppingCartItemSqlServerRepository(context);

            // Получаем или создаем корзину для текущей сессии
            // В реальном приложении sessionId должен быть связан с аутентифицированным пользователем
            string sessionId = "DEFAULT_USER_SESSION"; // Для демонстрации используем фиксированную сессию

            var existingCart = shoppingCartRepo.GetAll()
                .FirstOrDefault(c => c.SessionId == sessionId);

            if (existingCart == null)
            {
                // Создаем новую корзину
                var newCart = new ShoppingCart(sessionId);
                var cartId = shoppingCartRepo.Add(newCart);
                existingCart = shoppingCartRepo.GetById(cartId);
            }

            // Проверяем, не добавлен ли уже этот мотоцикл в корзину
            var existingItem = shoppingCartItemRepo.GetAll()
                .FirstOrDefault(i => i.ShoppingCartId == existingCart.Id &&
                                    i.MotorcycleId == selectedMotorcycle.Id);

            if (existingItem != null)
            {
                // Если мотоцикл уже в корзине, увеличиваем количество
                existingItem.Quantity++;
                shoppingCartItemRepo.Update(existingItem);
            }
            else
            {
                // Добавляем новый элемент в корзину
                var cartItem = new ShoppingCartItem(existingCart.Id, selectedMotorcycle.Id, 1);
                shoppingCartItemRepo.Add(cartItem);
            }

            // Обновляем общую сумму корзины
            existingCart.LastModified = DateTime.Now;
            existingCart.TotalAmount = CalculateCartTotal(existingCart.Id, shoppingCartItemRepo, new MotorcycleShop.Data.SqlServer.MotorcycleSqlServerRepository(context));
            shoppingCartRepo.Update(existingCart);

            MessageBox.Show($"Мотоцикл {selectedMotorcycle.Brand} {selectedMotorcycle.Model} добавлен в корзину!",
                "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при добавлении в корзину: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Показ деталей мотоцикла
    /// </summary>
    private void ShowMotorcycleDetails(Motorcycle motorcycle)
    {
        var detailsWindow = new MotorcycleDetailsWindow(motorcycle);
        detailsWindow.ShowDialog();
    }

    /// <summary>
    /// Рассчитывает общую сумму корзины
    /// </summary>
    private decimal CalculateCartTotal(int cartId,
        MotorcycleShop.Data.SqlServer.ShoppingCartItemSqlServerRepository itemRepo,
        MotorcycleShop.Data.SqlServer.MotorcycleSqlServerRepository motoRepo)
    {
        var items = itemRepo.GetAll().Where(i => i.ShoppingCartId == cartId).ToList();
        decimal total = 0;

        foreach (var item in items)
        {
            var motorcycle = motoRepo.GetById(item.MotorcycleId);
            if (motorcycle != null)
            {
                total += motorcycle.Price * item.Quantity;
            }
        }

        return total;
    }

    /// <summary>
    /// Переход к корзине
    /// </summary>
    private void CartButton_Click(object sender, RoutedEventArgs e)
    {
        var cartWindow = new CartWindow();
        cartWindow.ShowDialog();
    }

    /// <summary>
    /// Обработка нажатия кнопки "Добавить мотоцикл"
    /// </summary>
    private void AddMotorcycleButton_Click(object sender, RoutedEventArgs e)
    {
        var options = DatabaseConfiguration.GetDbContextOptions();
        using var context = new MotorcycleShop.Data.SqlServer.MotorcycleShopDbContext(options);
        var repository = new MotorcycleShop.Data.SqlServer.MotorcycleSqlServerRepository(context);
        var editWindow = new MotorcycleEditWindow(repository);
        if (editWindow.ShowDialog() == true)
        {
            LoadMotorcycles(); // Обновляем список
            UpdateStatus("Мотоцикл добавлен");
        }
    }

    /// <summary>
    /// Обработка нажатия кнопки "Редактировать"
    /// </summary>
    private void EditMotorcycleButton_Click(object sender, RoutedEventArgs e)
    {
        var selectedMotorcycle = MotorcyclesDataGrid.SelectedItem as Motorcycle;
        if (selectedMotorcycle == null)
        {
            MessageBox.Show("Выберите мотоцикл для редактирования", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var options = DatabaseConfiguration.GetDbContextOptions();
        using var context = new MotorcycleShop.Data.SqlServer.MotorcycleShopDbContext(options);
        var repository = new MotorcycleShop.Data.SqlServer.MotorcycleSqlServerRepository(context);
        var editWindow = new MotorcycleEditWindow(repository, selectedMotorcycle);
        if (editWindow.ShowDialog() == true)
        {
            LoadMotorcycles(); // Обновляем список
            UpdateStatus("Мотоцикл обновлен");
        }
    }

    /// <summary>
    /// Обработка нажатия кнопки "Удалить"
    /// </summary>
    private void DeleteMotorcycleButton_Click(object sender, RoutedEventArgs e)
    {
        var selectedMotorcycle = MotorcyclesDataGrid.SelectedItem as Motorcycle;
        if (selectedMotorcycle == null)
        {
            MessageBox.Show("Выберите мотоцикл для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var result = MessageBox.Show($"Вы действительно хотите удалить мотоцикл {selectedMotorcycle.Brand} {selectedMotorcycle.Model}?",
            "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            var options = DatabaseConfiguration.GetDbContextOptions();
            using var context = new MotorcycleShop.Data.SqlServer.MotorcycleShopDbContext(options);
            var repository = new MotorcycleShop.Data.SqlServer.MotorcycleSqlServerRepository(context);
            if (repository.Delete(selectedMotorcycle.Id))
            {
                LoadMotorcycles(); // Обновляем список
                UpdateStatus("Мотоцикл удален");
            }
            else
            {
                MessageBox.Show("Не удалось удалить мотоцикл", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}