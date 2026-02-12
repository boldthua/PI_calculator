using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PI_calculator
{
    internal class ViewModel
    {
        public long sample { get; set; }
        public ObservableCollection<PiModel> missions { get; set; } = new ObservableCollection<PiModel>();
        public ICommand command { get; set; }
        public void AddMission()
        {
            PiMission piMission = new PiMission(sample);
            Stopwatch stopwatch = Stopwatch.StartNew();
            double value = piMission.Calculate();
            string time = stopwatch.Elapsed.TotalSeconds.ToString();
            missions.Add(new PiModel(sample, time, value));
        }
        public ViewModel()
        {
            command = new RelayCommand(AddMission);
        }



        // 再問一次 get; set;
    }
}
