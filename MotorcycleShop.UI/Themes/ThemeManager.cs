using System;
using System.Windows;

namespace MotorcycleShop.UI.Themes
{
    /// <summary>
    /// Класс для управления темами приложения
    /// </summary>
    public static class ThemeManager
    {
        private const string DarkThemeUri = "pack://application:,,,/Themes/DarkTheme.xaml";
        private const string LightThemeUri = "pack://application:,,,/Themes/LightTheme.xaml";

        public static bool IsDarkTheme { get; private set; } = false;

        /// <summary>
        /// Установка темной темы
        /// </summary>
        public static void SetDarkTheme()
        {
            SetTheme(DarkThemeUri);
            IsDarkTheme = true;
        }

        /// <summary>
        /// Установка светлой темы
        /// </summary>
        public static void SetLightTheme()
        {
            SetTheme(LightThemeUri);
            IsDarkTheme = false;
        }

        /// <summary>
        /// Переключение между темами
        /// </summary>
        public static void ToggleTheme()
        {
            if (IsDarkTheme)
            {
                SetLightTheme();
            }
            else
            {
                SetDarkTheme();
            }
        }

        /// <summary>
        /// Установка темы по URI
        /// </summary>
        /// <param name="themeUri">URI ресурса темы</param>
        private static void SetTheme(string themeUri)
        {
            try
            {
                var resourceDict = new ResourceDictionary
                {
                    Source = new Uri(themeUri)
                };

                // Удаляем старую тему
                var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
                for (int i = mergedDictionaries.Count - 1; i >= 0; i--)
                {
                    var dict = mergedDictionaries[i];
                    if (dict.Source != null &&
                        (dict.Source.OriginalString.Contains("/Themes/") ||
                         dict.Source.OriginalString.Contains("\\Themes\\")))
                    {
                        mergedDictionaries.RemoveAt(i);
                    }
                }

                // Добавляем новую тему
                mergedDictionaries.Add(resourceDict);
            }
            catch (Exception ex)
            {
                // В случае ошибки просто игнорируем или логируем
                System.Diagnostics.Debug.WriteLine($"Ошибка при установке темы: {ex.Message}");
            }
        }
    }
}