using System;
using System.Windows;
using MotorcycleShop.Domain;

namespace MotorcycleShop.UI
{
    /// <summary>
    /// Логика взаимодействия для MotorcycleEditWindow.xaml
    /// Форма для добавления и редактирования мотоциклов
    /// </summary>
    public partial class MotorcycleEditWindow : Window
    {
        private Motorcycle _motorcycle;
        private bool _isEditMode;
        private readonly MotorcycleShop.Data.SqlServer.MotorcycleSqlServerRepository _repository;

        public MotorcycleEditWindow(MotorcycleShop.Data.SqlServer.MotorcycleSqlServerRepository repository, Motorcycle motorcycle = null)
        {
            InitializeComponent();
            _repository = repository;
            _motorcycle = motorcycle;
            _isEditMode = motorcycle != null;
            
            if (_isEditMode)
            {
                LoadMotorcycleData();
                Title = "Редактирование мотоцикла";
                TitleTextBlock.Text = "Редактирование мотоцикла";
            }
            else
            {
                Title = "Добавление нового мотоцикла";
                // Для нового мотоцикла устанавливаем значения по умолчанию
                InStockCheckBox.IsChecked = true;
            }
        }

        /// <summary>
        /// Загрузка данных мотоцикла в форму
        /// </summary>
        private void LoadMotorcycleData()
        {
            if (_motorcycle != null)
            {
                BrandTextBox.Text = _motorcycle.Brand;
                ModelTextBox.Text = _motorcycle.Model;
                YearTextBox.Text = _motorcycle.Year.ToString();
                ColorTextBox.Text = _motorcycle.Color;
                EngineVolumeTextBox.Text = _motorcycle.EngineVolume.ToString();
                MileageTextBox.Text = _motorcycle.Mileage.ToString();
                PriceTextBox.Text = _motorcycle.Price.ToString();
                DescriptionTextBox.Text = _motorcycle.Description ?? string.Empty;
                InStockCheckBox.IsChecked = _motorcycle.InStock;
            }
        }

        /// <summary>
        /// Получение данных из формы
        /// </summary>
        private bool GetFormData(out Motorcycle motorcycle)
        {
            motorcycle = null;

            // Проверка обязательных полей
            if (string.IsNullOrWhiteSpace(BrandTextBox.Text))
            {
                MessageBox.Show("Введите марку мотоцикла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ModelTextBox.Text))
            {
                MessageBox.Show("Введите модель мотоцикла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(YearTextBox.Text, out int year) || year < 1900 || year > DateTime.Now.Year + 1)
            {
                MessageBox.Show("Введите корректный год выпуска", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(EngineVolumeTextBox.Text, out decimal engineVolume) || engineVolume <= 0)
            {
                MessageBox.Show("Введите корректный объем двигателя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(MileageTextBox.Text, out int mileage) || mileage < 0)
            {
                MessageBox.Show("Введите корректный пробег", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Создание объекта мотоцикла
            motorcycle = new Motorcycle(
                BrandTextBox.Text.Trim(),
                ModelTextBox.Text.Trim(),
                year,
                ColorTextBox.Text.Trim(),
                engineVolume,
                mileage,
                price,
                string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ? null : DescriptionTextBox.Text.Trim(),
                null, // ImageUrl - в реальном приложении можно добавить поле для URL изображения
                InStockCheckBox.IsChecked == true
            );

            // Устанавливаем ID, если редактируем существующий мотоцикл
            if (_isEditMode && _motorcycle != null)
            {
                motorcycle.Id = _motorcycle.Id;
            }

            return true;
        }

        /// <summary>
        /// Обработка нажатия кнопки "Сохранить"
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (GetFormData(out Motorcycle motorcycle))
            {
                try
                {
                    if (_isEditMode)
                    {
                        // Обновление существующего мотоцикла
                        if (_repository.Update(motorcycle))
                        {
                            MessageBox.Show("Мотоцикл успешно обновлен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                            DialogResult = true;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить мотоцикл", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        // Добавление нового мотоцикла
                        int id = _repository.Add(motorcycle);
                        if (id > 0)
                        {
                            MessageBox.Show("Мотоцикл успешно добавлен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                            DialogResult = true;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось добавить мотоцикл", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении мотоцикла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Обработка нажатия кнопки "Отмена"
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}