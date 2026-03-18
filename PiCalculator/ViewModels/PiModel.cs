using PI_calculator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PI_calculator
{
    internal class PiModel : INotifyPropertyChanged
    {
        public long Sample { get; set; }
        public string Time { get; set; }
        private bool _isCompleted = false;
        private string _value;
        private double _progress;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                OnPropertyChanged();
            }
        }
        public double Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }
        private bool _isToBeCanceled = true;

        CancellationTokenSource cts = new CancellationTokenSource();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand cancelCommand { get; set; }
        public PiModel(PiModelDTO model)
        {
            this.Sample = model.Sample;
            this.Value = model.Value;
            if (model.Value.Contains("%"))
                Progress = double.Parse(model.Value.Trim('%'));
            this.Time = model.Time;
            cts = model.cts;
            IsToCancel = !model.IsCanceled;
            IsCompleted = model.IsCompleted;
            if (model.IsCompleted)
                IsToCancel = false;
            cancelCommand = new RelayCommand(CancelMission);
        }

        public bool IsToCancel
        {
            get { return _isToBeCanceled; }
            set
            {
                _isToBeCanceled = value;
                OnPropertyChanged(nameof(IsToCancel));
            }
        }
        public void CancelMission()
        {
            var result = MessageBox.Show("您確定要取消嗎？", "確認", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                IsToCancel = false;
                cts.Cancel();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
