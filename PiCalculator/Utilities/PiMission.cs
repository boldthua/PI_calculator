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
        private readonly Random random = new Random();
        public PiMission(long sample)
        {
            this.Sample = sample;
        }
        public async Task<double> Calculate()
        {
            object locker = new object();
            long count = 2_500_000;
            var batchSize = Sample % count == 0 ? Sample / count : (Sample / count + 1);
            long sum = 0;

            await Parallel.ForAsync(0, batchSize, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, (index, token) =>
            {
                Debug.WriteLine($"第{index + 1}個batch開始執行");
                for (int i = 0; i < count; i++)
                {
                    Double a = random.NextDouble();
                    Double b = random.NextDouble();
                    if (a * a + b * b <= 1)
                    {
                        lock (locker)
                            sum++;
                    }
                }
                Debug.WriteLine($"第{index + 1}個batch執行完成");
                return ValueTask.CompletedTask;
            });

            return (4.0 * sum) / (Sample - 1);
        }



    }
}
