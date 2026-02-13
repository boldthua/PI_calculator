using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PI_calculator.Models
{
    internal class PiModelDTO
    {
        public long Sample { get; set; }
        public string Time { get; set; }
        public double Value { get; set; }

        public PiModelDTO(long sample, string time, double Value)
        {
            this.Sample = sample;
            this.Value = Value;
            this.Time = time;
        }
    }
}
