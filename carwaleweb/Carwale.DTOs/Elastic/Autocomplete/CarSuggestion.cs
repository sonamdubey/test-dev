using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Elastic
{
    [Serializable]
    public class CarSuggestion
    {
        public List<string> input { get; set; }
        public string output { get; set; }
        public CarPayLoad payload { get; set; }
        public Context contexts { get; set; }
        public int weight { get; set; }
        public string suggest { get; set; }
    }
}
