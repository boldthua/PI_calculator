using System;
using System.Collections.Concurrent;
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
        ConcurrentQueue<PiMission> missions = new ConcurrentQueue<PiMission>();
        Object writeObj = new object();
        CancellationTokenSource cts = new CancellationTokenSource();
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(0, 4);
        public MainPresenter(IMainView view)
        {
            this.view = view;

        }
        public void SendMissionRequest(long sample)
        {
            lock (writeObj)
            {
                //雙重鎖定
                if (semaphore.CurrentCount <= 4)
                {
                    PiMission piMission = new PiMission(sample);
                    missions.Enqueue(piMission);
                    semaphore.Release();
                }
            }
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
                    semaphore.Wait();

                    if (cts.IsCancellationRequested)
                    {
                        cts = new CancellationTokenSource();
                        break;
                    }
                    if (missions.Count > 0)
                    {

                        Stopwatch stopwatch = Stopwatch.StartNew();
                        missions.TryDequeue(out PiMission piMission);
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
                            semaphore.Release();
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
