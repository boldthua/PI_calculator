using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public ICommand command { get; set; }
        public void AddMission()
        {
            presenter.CreateMission(sample);
        }

        public void RenderDatas(List<PiModelDTO> list)
        {
            missions.Clear();
            foreach (var model in list)
            {
                PiModel mission = new PiModel(model.Sample, model.Time, model.Value);
                missions.Add(mission);
            }
        }

        public ViewModel()
        {
            command = new RelayCommand(AddMission);
            presenter = new MainPresenter(this);
        }



        // 再問一次 get; set;
    }
}
