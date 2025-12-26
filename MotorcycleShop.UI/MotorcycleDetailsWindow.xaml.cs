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

                // Загружаем изображение мотоцикла
                LoadMotorcycleImage();

                UpdateStatus("Детали мотоцикла загружены");
            }
        }

        /// <summary>
        /// Загрузка изображения мотоцикла с обработкой ошибок
        /// </summary>
        private void LoadMotorcycleImage()
        {
            try
            {
                if (!string.IsNullOrEmpty(_motorcycle.ImageUrl))
                {
                    // Если URL изображения начинается с /, значит это локальный путь
                    if (_motorcycle.ImageUrl.StartsWith("/"))
                    {
                        // Заменяем на правильный путь к ресурсу
                        string resourcePath = "pack://application:,,,/" + _motorcycle.ImageUrl.TrimStart('/');
                        var bitmap = new System.Windows.Media.Imaging.BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(resourcePath);
                        bitmap.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad; // Загружаем полностью сразу
                        bitmap.EndInit();
                        bitmap.Freeze(); // Закрываем для изменений для предотвращения проблем с потоками
                        MotorcycleImage.Source = bitmap;
                    }
                    else
                    {
                        // Если это полный URL, используем его напрямую
                        var bitmap = new System.Windows.Media.Imaging.BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(_motorcycle.ImageUrl);
                        bitmap.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        bitmap.Freeze();
                        MotorcycleImage.Source = bitmap;
                    }
                }
                else
                {
                    // Если URL изображения пустой, используем плейсхолдер
                    LoadPlaceholderImage();
                }
            }
            catch
            {
                // Если не удалось загрузить изображение, используем плейсхолдер
                LoadPlaceholderImage();
            }
        }

        /// <summary>
        /// Загрузка плейсхолдера изображения
        /// </summary>
        private void LoadPlaceholderImage()
        {
            try
            {
                var bitmap = new System.Windows.Media.Imaging.BitmapImage();
                bitmap.BeginInit();
                // Используем одно из существующих изображений как плейсхолдер
                bitmap.UriSource = new Uri("pack://application:,,,/Images/yamaha-r1.png");
                bitmap.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                MotorcycleImage.Source = bitmap;
            }
            catch
            {
                // Если и плейсхолдер не загрузился, оставляем пустое изображение
                MotorcycleImage.Source = null;
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
        /// Обработка нажатия кнопки "Назад"
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}