using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MotorcycleShop.Domain;

namespace MotorcycleShop.UI
{
    /// <summary>
    /// Логика взаимодействия для CartWindow.xaml
    /// Форма для просмотра и редактирования корзины покупок
    /// </summary>
    public partial class CartWindow : Window
    {
        private List<ShoppingCartItem> _cartItems;
        private readonly MotorcycleShop.Data.SqlServer.MotorcycleShopDbContext _context;
        private readonly MotorcycleShop.Data.SqlServer.ShoppingCartSqlServerRepository _shoppingCartRepository;
        private readonly MotorcycleShop.Data.SqlServer.ShoppingCartItemSqlServerRepository _shoppingCartItemRepository;
        private readonly MotorcycleShop.Data.SqlServer.MotorcycleSqlServerRepository _motorcycleRepository;

        public CartWindow()
        {
            InitializeComponent();

            // Инициализация контекста и репозиториев
            var options = DatabaseConfiguration.GetDbContextOptions();
            _context = new MotorcycleShop.Data.SqlServer.MotorcycleShopDbContext(options);
            _shoppingCartRepository = new MotorcycleShop.Data.SqlServer.ShoppingCartSqlServerRepository(_context);
            _shoppingCartItemRepository = new MotorcycleShop.Data.SqlServer.ShoppingCartItemSqlServerRepository(_context);
            _motorcycleRepository = new MotorcycleShop.Data.SqlServer.MotorcycleSqlServerRepository(_context);

            LoadCartItems();
        }

        /// <summary>
        /// Загрузка товаров из корзины
        /// </summary>
        private void LoadCartItems()
        {
            try
            {
                // В реальном приложении sessionId должен быть связан с аутентифицированным пользователем
                string sessionId = "DEFAULT_USER_SESSION"; // Для демонстрации используем фиксированную сессию

                // Получаем корзину пользователя
                var shoppingCartRepo = new MotorcycleShop.Data.SqlServer.ShoppingCartSqlServerRepository(_context);
                var shoppingCartItemRepo = new MotorcycleShop.Data.SqlServer.ShoppingCartItemSqlServerRepository(_context);

                var cart = shoppingCartRepo.GetAll()
                    .FirstOrDefault(c => c.SessionId == sessionId);

                if (cart != null)
                {
                    // Получаем элементы корзины
                    var cartItems = shoppingCartItemRepo.GetAll()
                        .Where(item => item.ShoppingCartId == cart.Id)
                        .ToList();

                    // Загружаем мотоциклы для каждого элемента корзины
                    _cartItems = new List<ShoppingCartItem>();
                    foreach (var item in cartItems)
                    {
                        var motorcycle = _motorcycleRepository.GetById(item.MotorcycleId);
                        if (motorcycle != null)
                        {
                            item.Motorcycle = motorcycle; // Устанавливаем мотоцикл для отображения
                            _cartItems.Add(item);
                        }
                    }
                }
                else
                {
                    // Если корзина не найдена, создаем пустой список
                    _cartItems = new List<ShoppingCartItem>();
                }

                CartItemsDataGrid.ItemsSource = _cartItems;
                UpdateTotalAmount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке товаров из корзины: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обновление общей суммы заказа
        /// </summary>
        private void UpdateTotalAmount()
        {
            decimal total = 0;
            foreach (var item in _cartItems)
            {
                if (item.Motorcycle != null)
                {
                    total += item.Motorcycle.Price * item.Quantity;
                }
            }
            TotalAmountTextBlock.Text = total.ToString("C");
        }

        /// <summary>
        /// Обработка нажатия кнопки "Оформить заказ"
        /// </summary>
        private void PlaceOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cartItems.Count == 0)
            {
                MessageBox.Show("Корзина пуста", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Открываем окно оформления заказа
            var orderWindow = new OrderWindow();
            orderWindow.ShowDialog();
        }

        /// <summary>
        /// Обработка нажатия кнопки "Удалить"
        /// </summary>
        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = CartItemsDataGrid.SelectedItem as ShoppingCartItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Выберите товар для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Вы действительно хотите удалить выбранный товар из корзины?", 
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Удаляем элемент из базы данных
                    _shoppingCartItemRepository.Delete(selectedItem.Id);

                    // Обновляем отображение
                    LoadCartItems();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении товара из корзины: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Обработка нажатия кнопки "Продолжить покупки"
        /// </summary>
        private void ContinueShoppingButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Обработка нажатия кнопки "Назад"
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}