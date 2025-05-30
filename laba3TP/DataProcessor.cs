using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace laba3
{
    public class DataProcessor
    {
        public (double maxGrowthMen, double minGrowthMen, double maxGrowthWomen, double minGrowthWomen) CalculateSalaryGrowth(List<SalaryData> data)
        {
            double maxGrowthMen = 0, minGrowthMen = 0, maxGrowthWomen = 0, minGrowthWomen = 0;
            for (int i = 1; i < data.Count; i++)
            {
                double growthMen = (data[i].MenSalary - data[i - 1].MenSalary) / data[i - 1].MenSalary * 100;
                double growthWomen = (data[i].WomenSalary - data[i - 1].WomenSalary) / data[i - 1].WomenSalary * 100;
                if (i == 1)
                {
                    maxGrowthMen = minGrowthMen = growthMen;
                    maxGrowthWomen = minGrowthWomen = growthWomen;
                }
                else
                {
                    maxGrowthMen = System.Math.Max(maxGrowthMen, growthMen);
                    minGrowthMen = System.Math.Min(minGrowthMen, growthMen);
                    maxGrowthWomen = System.Math.Max(maxGrowthWomen, growthWomen);
                    minGrowthWomen = System.Math.Min(minGrowthWomen, growthWomen);
                }
            }
            return (maxGrowthMen, minGrowthMen, maxGrowthWomen, minGrowthWomen);
        }
    }
}
