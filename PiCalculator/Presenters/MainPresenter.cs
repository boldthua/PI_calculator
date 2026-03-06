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
        Queue<PiMission> missions = new Queue<PiMission>();
        Object writeObj = new object();
        Object readObj = new object();

        public MainPresenter(IMainView view)
        {
            this.view = view;

        }
        public void SendMissionRequest(long sample)
        {
            PiMission piMission = new PiMission(sample);
            lock (writeObj)
                missions.Enqueue(piMission);

        }
        public void FetchCompletedMissions()
        {
            view.OnMissionResponse(piRepository.GetData());
        }

        public async void StartMission()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (missions.Count > 0)
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        PiMission piMission = null;
                        lock (readObj)
                            piMission = missions.Dequeue();
                        Task.Run(async () =>
                        {
                            double value = (double)await piMission.Calculate();
                            string time = stopwatch.Elapsed.TotalSeconds.ToString();
                            PiModelDTO model = new PiModelDTO(piMission.Sample, time, value);
                            piRepository.AddModel(model);
                            view.OnMissionResponse(piRepository.GetData());
                        });
                    }
                }
            }
            );
        }

        public void StopMission()
        {
            throw new NotImplementedException();
        }
    }
}
