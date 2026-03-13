using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PI_calculator.Models;
using PI_calculator.Presenters;
using static PI_calculator.MainContract;

namespace PI_calculator
{
    internal class ViewModel : IMainView, INotifyPropertyChanged
    {
        public long sample { get; set; }
        IMainPresenter presenter { get; set; }
        public ObservableCollection<PiModel> missions { get; set; } = new ObservableCollection<PiModel>();
        public Dictionary<long, PiModel> checkList { get; set; } = new Dictionary<long, PiModel>();

        public System.Threading.Timer timer { get; set; }
        Object checkListObj = new object();
        private bool _isToStop = true;
        public string StopOrNot => _isToStop ? "Stop" : "Resume";

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsToStop
        {
            get { return _isToStop; }
            set
            {
                _isToStop = value;
                OnPropertyChanged(nameof(StopOrNot));
                OnPropertyChanged(nameof(IsToStop));
            }
        }
        public ICommand command { get; set; }
        public ICommand stopCommand { get; set; }
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public void AddMission()
        {
            lock (checkListObj)
            {
                if (CheckSample())
                {
                    checkList.Add(sample, null);
                    presenter.SendMissionRequest(sample);
                }
            }
        }
        public void StopMission()
        {
            if (StopOrNot == "Stop")
            {
                var result = MessageBox.Show("您確定要取消嗎？", "確認", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                    return;
            }
            if (_isToStop == true)
            {
                IsToStop = false;
                presenter.StopMission();
                return;
            }
            IsToStop = true;
            presenter.StartMission();

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

        public void OnMissionResponse(List<PiModelDTO> list)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                missions.Clear();
                foreach (var model in list)
                {
                    PiModel mission = new PiModel(model);
                    missions.Add(mission);
                }
            });
        }

        public ViewModel()
        {
            command = new RelayCommand(AddMission);
            stopCommand = new RelayCommand(StopMission);
            presenter = new MainPresenter(this);

            presenter.StartMission();



            timer = new System.Threading.Timer(x =>
            {
                presenter.FetchCompletedMissions();
            }, null, 0, 1000);
        }
    }
}
