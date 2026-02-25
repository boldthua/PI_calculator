using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PI_calculator.Models;
using PI_calculator.Presenters;
using static PI_calculator.MainContract;

namespace PI_calculator
{
    internal class ViewModel : IMainView
    {
        public long sample { get; set; }
        IMainPresenter presenter { get; set; }
        public ObservableCollection<PiModel> missions { get; set; } = new ObservableCollection<PiModel>();
        public Dictionary<long, PiModel> checkList { get; set; } = new Dictionary<long, PiModel>();
        public System.Threading.Timer timer { get; set; }

        public ICommand command { get; set; }
        public void AddMission()
        {
            if (CheckSample())
                presenter.CreateMission(sample);
        }
        public bool CheckSample()
        {
            //hash雜湊
            if (checkList.ContainsKey(sample))
            {
                MessageBox.Show("Sample Repeated!!");
                return false;
            }
            return true;
        }

        public void RenderDatas(List<PiModelDTO> list)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                missions.Clear();
                checkList.Clear();
                foreach (var model in list)
                {
                    PiModel mission = new PiModel(model.Sample, model.Time, model.Value);
                    missions.Add(mission);
                    checkList.Add(model.Sample, mission);
                }
            });
        }

        public ViewModel()
        {
            command = new RelayCommand(AddMission);
            presenter = new MainPresenter(this);

            timer = new System.Threading.Timer(x =>
            {
                presenter.Reflash();
            }, null, 0, 1000);
        }



        // 再問一次 get; set;
    }
}
