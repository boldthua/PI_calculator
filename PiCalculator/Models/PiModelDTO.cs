using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PI_calculator.Models
{
    internal class PiModelDTO
    {
        public long Sample { get; set; }
        public string Time { get; set; }
        public string Value { get; set; }

        public bool IsCanceled = false;
        public bool IsCompleted = false;
        public CancellationTokenSource cts = new CancellationTokenSource();

        public event PropertyChangedEventHandler? PropertyChanged;

        public PiModelDTO(long sample)
        {
            this.Sample = sample;
            Time = "Computing...";
            Value = string.Empty;
        }

        public void CancelMission()
        {
            IsCanceled = true;
            Value = "任務已取消";
        }

    }
}
