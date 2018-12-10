using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Elastic
{
    public class ESCityDocument
    {
        public string id { get; set; }
        public CitySuggestion city_suggest { get; set; }
        public string name { get; set; }
    }
}
