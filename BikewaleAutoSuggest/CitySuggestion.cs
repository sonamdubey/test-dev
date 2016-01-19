using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikewaleAutoSuggest
{
    public class CitySuggestion
    {
        public List<string> input { get; set; }
        public string output { get; set; }
        public Payload payload { get; set; }
        public int Weight { get; set; }
    }
    public class Payload
    {
        public int CityId { get; set; }
        public string CityMaskingName { get; set; }
    }
}
