using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PI_calculator.Models;
using PI_calculator.Presenters;
using static PI_calculator.MainContract;

namespace PI_calculator.Presenters
{
    internal class MainPresenter : IMainPresenter
    {
        PiRepository piRepository = new PiRepository();
        IMainView view;
        public MainPresenter(IMainView view)
        {
            this.view = view;
        }
        public async void CreateMission(long sample)
        {
            PiMission piMission = new PiMission(sample);
            Stopwatch stopwatch = Stopwatch.StartNew();
            double value = (double)await piMission.Calculate();
            string time = stopwatch.Elapsed.TotalSeconds.ToString();
            PiModelDTO model = new PiModelDTO(sample, time, value);
            piRepository.AddModel(model);
            view.RenderDatas(piRepository.GetData());
        }
    }
}
