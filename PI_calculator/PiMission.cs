using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PI_calculator
{
    internal class PiMission
    {
        public readonly long SampleSize;
        public double Calculate()
        {
            int sum = 0;
            List<double> list = new List<double>();
            Random random = new Random();

            for (int i = 0; i < SampleSize; i++)
            {
                Double a = (long)random.NextDouble();
                Double b = (long)random.NextDouble();
                if (a * a + b * b <= 1)
                    sum++;
            }
            return sum / SampleSize - 1;

        }
        public PiMission(long SampleSize)
        {
            this.SampleSize = SampleSize;
        }
    }
}
