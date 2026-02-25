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
            void RenderDatas(List<PiModelDTO> list);
        }

        public interface IMainPresenter
        {
            void CreateMission(long sample);
            void Reflash();
        }
    }
}
