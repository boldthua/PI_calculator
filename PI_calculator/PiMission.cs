using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PI_calculator
{
    internal class PiMission
    {
        public long Sample { get; set; }

        public PiMission(long sample)
        {
            this.Sample = sample;
        }
        public double Calculate()
        {
            int sum = 0;
            List<double> list = new List<double>();
            Random random = new Random();
            for (int i = 0; i < Sample; i++)
            {
                Double a = (long)random.NextDouble();
                Double b = (long)random.NextDouble();
                if (a * a + b * b <= 1)
                    sum++;
            }
            return sum / (Sample - 1);
        }

    }
}
