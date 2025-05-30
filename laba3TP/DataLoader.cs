using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace laba3
{
    public class DataLoader
    {
        public List<SalaryData> LoadSalaryData(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            List<SalaryData> data = new List<SalaryData>();

            // Пропускаем заголовок (первую строку)
            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split(';'); // Используем точку с запятой
                data.Add(new SalaryData
                {
                    Year = int.Parse(parts[0]),
                    MenSalary = double.Parse(parts[1]),    // Male
                    WomenSalary = double.Parse(parts[2])   // Female
                });
            }
            return data;
        }

        public List<InflationData> LoadInflationData(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            List<InflationData> data = new List<InflationData>();
            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split(';'); // Используем точку с запятой
                data.Add(new InflationData
                {
                    Year = int.Parse(parts[0]),
                    InflationRate = double.Parse(parts[1])
                });
            }
            return data;
        }
    }
}
