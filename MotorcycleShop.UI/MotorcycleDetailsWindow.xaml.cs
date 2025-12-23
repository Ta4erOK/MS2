using System;
using System.Windows;
using System.Windows.Controls;
using MotorcycleShop.Domain;

namespace MotorcycleShop.UI
{
    /// <summary>
    /// Логика взаимодействия для MotorcycleDetailsWindow.xaml
    /// Просмотр подробной информации о мотоцикле
    /// </summary>
    public partial class MotorcycleDetailsWindow : Window
    {
        private Motorcycle _motorcycle;

        public MotorcycleDetailsWindow(Motorcycle motorcycle)
        {
            InitializeComponent();
            _motorcycle = motorcycle;
            LoadMotorcycleDetails();
        }

        /// <summary>
        /// Загрузка деталей мотоцикла
        /// </summary>
        private void LoadMotorcycleDetails()
        {
            if (_motorcycle != null)
            {
                Title = $"Детали мотоцикла - {_motorcycle.Brand} {_motorcycle.Model}";
                
                BrandModelTextBlock.Text = $"{_motorcycle.Brand} {_motorcycle.Model}";
                YearTextBlock.Text = _motorcycle.Year.ToString();
                EngineVolumeTextBlock.Text = $"{_motorcycle.EngineVolume} см³";
                MileageTextBlock.Text = $"{_motorcycle.Mileage} км";
                ColorTextBlock.Text = _motorcycle.Color;
                PriceTextBlock.Text = _motorcycle.Price.ToString("C");
                DescriptionTextBlock.Text = _motorcycle.Description ?? "Описание отсутствует";
                
                // Здесь в дальнейшем можно загрузить изображение по URL
                // MotorcycleImage.Source = new BitmapImage(new Uri(_motorcycle.ImageUrl));
                
                UpdateStatus("Детали мотоцикла загружены");
            }
        }

        /// <summary>
        /// Обновление статуса
        /// </summary>
        private void UpdateStatus(string message)
        {
            StatusTextBlock.Text = message;
        }

        /// <summary>
        /// Обработка нажатия кнопки "Добавить в корзину"
        /// </summary>
        private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            // В реальном приложении здесь будет добавление в корзину
            MessageBox.Show($"Мотоцикл {_motorcycle.Brand} {_motorcycle.Model} добавлен в корзину!", 
                "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
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