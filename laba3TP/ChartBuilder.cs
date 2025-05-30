using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;

namespace laba3
{
    public class ChartBuilder
    {
        public void BuildSalaryChart(CartesianChart chart, List<SalaryData> data, List<double> menForecast, List<double> womenForecast)
        {
            // Очищаем существующие оси
            chart.AxisX.Clear();
            chart.AxisY.Clear();

            // Создаем полный список лет (исторические + прогнозные)
            var allYears = new List<string>();
            allYears.AddRange(data.Select(d => d.Year.ToString()));

            // Добавляем годы для прогноза
            int lastYear = data.Last().Year;
            for (int i = 1; i <= menForecast.Count; i++)
            {
                allYears.Add((lastYear + i).ToString());
            }

            // Подготавливаем данные для прогноза (с пустыми значениями в начале)
            var menForecastValues = new ChartValues<double>();
            var womenForecastValues = new ChartValues<double>();

            // Добавляем null значения для исторических данных
            for (int i = 0; i < data.Count; i++)
            {
                menForecastValues.Add(double.NaN);
                womenForecastValues.Add(double.NaN);
            }

            // Добавляем прогнозные значения
            menForecastValues.AddRange(menForecast);
            womenForecastValues.AddRange(womenForecast);

            chart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Мужчины",
                    Values = new ChartValues<double>(data.Select(d => d.MenSalary)),
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 8
                },
                new LineSeries
                {
                    Title = "Женщины",
                    Values = new ChartValues<double>(data.Select(d => d.WomenSalary)),
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 8
                },
                new LineSeries
                {
                    Title = "Прогноз Мужчины",
                    Values = menForecastValues,
                    Stroke = System.Windows.Media.Brushes.Blue,
                    StrokeDashArray = new System.Windows.Media.DoubleCollection(new double[] { 5, 5 }),
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 6
                },
                new LineSeries
                {
                    Title = "Прогноз Женщины",
                    Values = womenForecastValues,
                    Stroke = System.Windows.Media.Brushes.Red,
                    StrokeDashArray = new System.Windows.Media.DoubleCollection(new double[] { 5, 5 }),
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 6
                }
            };

            // Настраиваем оси
            chart.AxisX.Add(new Axis
            {
                Title = "Год",
                Labels = allYears.ToArray(),
                Separator = new Separator { Step = 1 }
            });

            chart.AxisY.Add(new Axis
            {
                Title = "Зарплата",
                LabelFormatter = value => value.ToString("N0")
            });
        }

        // public void BuildInflationChart(CartesianChart chart, List<InflationData> data, List<double> forecast)
        // Код Гомера
    }
}