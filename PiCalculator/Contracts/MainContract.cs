using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PI_calculator.Models;

namespace PI_calculator
{
    internal class MainContract
    {
        public interface IMainView
        {
            void OnMissionResponse(List<PiModelDTO> list);
        }

        public interface IMainPresenter
        {
            /// <summary>
            /// 啟動執行緒任務，不斷接收來自SendMissionRequest的任務，並在背景計算PI
            /// </summary>
            void StartMission();

            /// <summary>
            /// 暫停執行緒任務，停止接收任何計算請求
            /// </summary>
            void StopMission();

            /// <summary>
            /// 發起計算請求，給予指定的sample size 計算PI
            /// </summary>
            /// <param name="sample"></param>
            void SendMissionRequest(long sample);

            /// <summary>
            /// 取得所有當前PI任務完成的計算結果
            /// </summary>
            void FetchCompletedMissions();

        }
    }
}
