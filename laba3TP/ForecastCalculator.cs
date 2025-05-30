using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba3
{
    public class ForecastCalculator
    {
        public List<double> MovingAverage(List<double> values, int period, int forecastYears)
        {
            List<double> forecast = new List<double>();

            // Копия исходных данных для расчетов
            List<double> workingData = new List<double>(values);

            // Расчёт прогноза на основе последних period значений
            for (int i = 0; i < forecastYears; i++)
            {
                // Берем последние period значений для расчета среднего
                double avg = workingData.Skip(workingData.Count - period).Take(period).Average();
                forecast.Add(avg);

                // Добавляем прогнозное значение в рабочие данные для следующего расчета
                workingData.Add(avg);
            }

            return forecast;
        }
    }
}
