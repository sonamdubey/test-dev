using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityAutoSuggest
{
    public class CitySuggestion
    {
        public List<string> input { get; set; }                                     //  For Inputs
        public string output { get; set; }                                          //  For Output
        public Payload payload { get; set; }                                        //  Define Payload
        public int weight { get; set; }                                             //  Weight For Ordering
    }
    public class Payload
    {
        public int CityId { get; set; }                                             //  CityId for Url
        public string CityMaskingName { get; set; }                                 //  CityMaskingName for Url
    }
}
