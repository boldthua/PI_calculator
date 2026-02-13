using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PI_calculator.Models;

namespace PI_calculator.Presenters
{
    internal class PiRepository
    {
        public List<PiModelDTO> list = new List<PiModelDTO>();
        public PiRepository() { }

        public void AddModel(PiModelDTO model)
        {
            list.Add(model);
        }
        public List<PiModelDTO> GetData()
        {
            return list;
        }
    }
}
