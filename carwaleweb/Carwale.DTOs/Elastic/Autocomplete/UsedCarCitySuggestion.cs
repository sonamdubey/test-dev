using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Elastic
{
    class UsedCarCitySuggestion
    {
        public List<string> input { get; set; }
        public string output { get; set; }
        public UsedCarCityPayLoad payload { get; set; }
        public int weight { get; set; }
    }
}
