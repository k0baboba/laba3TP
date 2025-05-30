using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Microsoft.VisualBasic;
using laba3;

namespace laba3TP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DataLoader dataLoader = new DataLoader();
        private DataProcessor dataProcessor = new DataProcessor();
        private ChartBuilder chartBuilder = new ChartBuilder();
        private ForecastCalculator forecastCalculator = new ForecastCalculator();

        // Переменные для хранения параметров прогноза
        private int forecastYears = 5;
        private int period = 5; // Период для скользящего среднего
        private bool parametersSet = false;

        public MainWindow()
        {
            InitializeComponent();
        }


        // Метод для получения параметров прогноза (запрашиваем только количество лет)
        private bool SetForecastParameters()
        {
            if (!parametersSet)
            {
                var (years, isValid) = SimpleInputHelper.GetForecastYears();

                if (isValid)
                {
                    forecastYears = years;
                    parametersSet = true;
                    return true;
                }
                return false;
            }
            return true;
        }

        private void SalaryButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем/устанавливаем параметры прогноза
            if (!SetForecastParameters()) return;

            OpenFileDialog openfileDialog = new OpenFileDialog();
            if (openfileDialog.ShowDialog() == true)
            {
                var data = dataLoader.LoadSalaryData(openfileDialog.FileName);
                DataTable.ItemsSource = data;

                var (maxGrowthMen, minGrowthMen, maxGrowthWomen, minGrowthWomen) = dataProcessor.CalculateSalaryGrowth(data);
                MessageBox.Show($"Мужчины: Макс рост {maxGrowthMen:F2}%, Мин рост {minGrowthMen:F2}%\nЖенщины: Макс рост {maxGrowthWomen:F2}%, Мин рост {minGrowthWomen:F2}%");

                var menForecast = forecastCalculator.MovingAverage(
                    data.Select(d => d.MenSalary).ToList(),
                    period,
                    forecastYears);

                var womenForecast = forecastCalculator.MovingAverage(
                    data.Select(d => d.WomenSalary).ToList(),
                    period,
                    forecastYears);

                chartBuilder.BuildSalaryChart(SalaryChart, data, menForecast, womenForecast);
            }
        }

        private void InflationButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем/устанавливаем параметры прогноза
            if (!SetForecastParameters()) return;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var data = dataLoader.LoadInflationData(openFileDialog.FileName);
                DataTable.ItemsSource = data;

                var forecast = forecastCalculator.MovingAverage(
                    data.Select(d => d.InflationRate).ToList(),
                    period,
                    forecastYears);

                chartBuilder.BuildInflationChart(SalaryChart, data, forecast);

                // Расчет стоимости товара с учетом прогноза инфляции
                double price = 1000; // Начальная стоимость товара
                for (int i = 0; i < forecastYears; i++)
                {
                    if (i < forecast.Count)
                    {
                        price *= (1 + forecast[i] / 100);
                    }
                }
                MessageBox.Show($"Стоимость товара через {forecastYears} лет: {price:F2} руб.");
            }
        }
    }

    public static class SimpleInputHelper
    {
        public static (int forecastYears, bool isValid) GetForecastYears()
        {
            try
            {
                // Запрашиваем только количество лет для прогноза
                string yearsInput = Interaction.InputBox(
                    "Введите количество лет для прогноза (1-20):",
                    "Параметры прогноза",
                    "5");

                if (string.IsNullOrEmpty(yearsInput))
                    return (0, false);

                if (!int.TryParse(yearsInput, out int years) || years <= 0 || years > 20)
                {
                    MessageBox.Show("Количество лет должно быть числом от 1 до 20", "Ошибка");
                    return (0, false);
                }

                return (years, true);
            }
            catch
            {
                return (0, false);
            }
        }
}
}
