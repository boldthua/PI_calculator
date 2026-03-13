using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
        CancellationTokenSource cts = new CancellationTokenSource();
        AutoResetEvent autoEvent = new AutoResetEvent(false);
        public MainPresenter(IMainView view)
        {
            this.view = view;

        }
        public void SendMissionRequest(long sample)
        {
            PiMission piMission = new PiMission(sample);
            lock (writeObj)
                missions.Enqueue(piMission);
            autoEvent.Set();
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
                    autoEvent.WaitOne();


                    if (cts.IsCancellationRequested)
                    {
                        cts = new CancellationTokenSource();
                        break;
                    }
                    if (missions.Count > 0)
                    {

                        Stopwatch stopwatch = Stopwatch.StartNew();
                        PiMission piMission = new PiMission(0);
                        lock (readObj)
                            piMission = missions.Dequeue();
                        Task.Run(async () =>
                        {
                            PiModelDTO model = new PiModelDTO(piMission.Sample);
                            piRepository.AddModel(model);
                            double result = (double)await piMission.Calculate(model);
                            if (model.IsCanceled == false)
                            {
                                model.Value = result.ToString();
                            }
                            model.Time = stopwatch.Elapsed.TotalSeconds.ToString();
                            view.OnMissionResponse(piRepository.GetData());
                        });
                    }
                }
            }
            );
        }

        public void StopMission()
        {
            cts.Cancel();
        }
    }
}
