using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using MotorcycleShop.Services;
using MotorcycleShop.Data.InMemory;
using MotorcycleShop.Data.Interfaces;

namespace MotorcycleShop.UI
{
    /// <summary>
    /// Логика взаимодействия для StatisticsWindow.xaml
    /// Форма для отображения статистики продаж
    /// </summary>
    public partial class StatisticsWindow : Window, INotifyPropertyChanged
    {
        private readonly StatisticsService _statisticsService;
        private DateOnly? _startDate;
        private DateOnly? _endDate;

        // Backing fields для свойств PlotModel
        private PlotModel? _statusPlotModel;
        private PlotModel? _monthPlotModel;
        private PlotModel? _popularMotorcyclesPlotModel;

        public StatisticsWindow()
        {
            InitializeComponent();
            
            // Инициализация сервиса статистики с in-memory репозиториями
            var motorcycleRepo = new MotorcycleRepository();
            var orderRepo = new OrderRepository();
            var orderItemRepo = new OrderItemRepository();
            _statisticsService = new StatisticsService(motorcycleRepo, orderRepo, orderItemRepo);
            
            DataContext = this;
            
            // Загружаем статистику
            LoadAllCharts();
        }

        // Свойства для фильтрации
        public DateOnly? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        public DateOnly? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }

        // Свойства для диаграмм
        public PlotModel? StatusPlotModel
        {
            get => _statusPlotModel;
            set
            {
                _statusPlotModel = value;
                OnPropertyChanged();
            }
        }

        public PlotModel? MonthPlotModel
        {
            get => _monthPlotModel;
            set
            {
                _monthPlotModel = value;
                OnPropertyChanged();
            }
        }

        public PlotModel? PopularMotorcyclesPlotModel
        {
            get => _popularMotorcyclesPlotModel;
            set
            {
                _popularMotorcyclesPlotModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Загрузка всех диаграмм
        /// </summary>
        private void LoadAllCharts()
        {
            LoadStatusChart();
            LoadMonthChart();
            LoadPopularMotorcyclesChart();
        }

        /// <summary>
        /// Загрузка диаграммы по статусам заказов (круговая диаграмма)
        /// </summary>
        private void LoadStatusChart()
        {
            try
            {
                var data = _statisticsService.GetOrdersByStatus();

                var plotModel = new PlotModel { Title = "Распределение заказов по статусам" };

                var pieSeries = new PieSeries
                {
                    StrokeThickness = 2.0,
                    InsideLabelPosition = 0.5,
                    AngleSpan = 360,
                    StartAngle = 0
                };

                foreach (var item in data)
                {
                    pieSeries.Slices.Add(new PieSlice(item.Status, item.Count)
                    {
                        IsExploded = false
                    });
                }

                plotModel.Series.Add(pieSeries);
                StatusPlotModel = plotModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке диаграммы статусов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Загрузка диаграммы по месяцам (линейная диаграмма)
        /// </summary>
        private void LoadMonthChart()
        {
            try
            {
                var data = _statisticsService.GetOrdersByMonth();

                var plotModel = new PlotModel { Title = "Динамика заказов по месяцам" };

                var categoryAxis = new CategoryAxis
                {
                    Position = AxisPosition.Bottom,
                    Angle = -45, // Поворот меток для лучшей читаемости
                    Title = "Месяцы"
                };

                foreach (var item in data)
                {
                    categoryAxis.Labels.Add(item.GetMonthName()); // "Янв 2025", "Фев 2025"...
                }

                plotModel.Axes.Add(categoryAxis);

                plotModel.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = "Количество заказов",
                    MinimumPadding = 0.1,
                    MaximumPadding = 0.1
                });

                var lineSeries = new LineSeries
                {
                    Title = "Количество заказов",
                    Color = OxyColors.Blue,
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 4,
                    MarkerFill = OxyColors.Blue
                };

                for (int i = 0; i < data.Count; i++)
                {
                    lineSeries.Points.Add(new DataPoint(i, data[i].Count));
                }

                plotModel.Series.Add(lineSeries);
                MonthPlotModel = plotModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке диаграммы по месяцам: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Загрузка диаграммы популярных мотоциклов (столбчатая диаграмма)
        /// </summary>
        private void LoadPopularMotorcyclesChart()
        {
            try
            {
                var data = _statisticsService.GetPopularMotorcycles();

                // Ограничиваем количество элементов для лучшей визуализации
                if (data.Count > 10)
                {
                    data = data.Take(10).ToList();
                }

                var plotModel = new PlotModel { Title = "Топ-10 популярных мотоциклов" };

                var categoryAxis = new CategoryAxis
                {
                    Position = AxisPosition.Left, // Слева для горизонтальных столбцов
                    Title = "Мотоциклы",
                    Angle = -20 // Наклон меток для экономии места
                };

                foreach (var item in data)
                {
                    categoryAxis.Labels.Add(item.MotorcycleName);
                }

                plotModel.Axes.Add(categoryAxis);

                plotModel.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = "Количество продаж",
                    MinimumPadding = 0.1,
                    MaximumPadding = 0.1
                });

                var barSeries = new BarSeries
                {
                    Title = "Количество продаж",
                    FillColor = OxyColors.Coral
                };

                foreach (var item in data)
                {
                    barSeries.Items.Add(new BarItem { Value = item.Count });
                }

                plotModel.Series.Add(barSeries);
                PopularMotorcyclesPlotModel = plotModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке диаграммы популярных мотоциклов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработка нажатия кнопки "Применить фильтр"
        /// </summary>
        private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            LoadAllCharts();
        }

        /// <summary>
        /// Обработка нажатия кнопки "Сбросить"
        /// </summary>
        private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            StartDate = null;
            EndDate = null;
            LoadAllCharts();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == DataContextProperty)
            {
                OnPropertyChanged(nameof(DataContext));
            }
        }
    }
}