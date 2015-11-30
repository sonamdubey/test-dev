using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleAutoSuggest
{
    public class CityList
    {
        public string Id { get; set; }
        public CitySuggestion mm_suggest { get; set; }
        public string name { get; set; }
    }

    public class CityTempList
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string MaskingName { get; set; }
    }
}
