using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityAutoSuggest
{
    public class CitySuggestion
    {
        public IList<string> input { get; set; }                                     //  For Inputs
                                           //  Define Payload
        public int weight { get; set; } //  Weight For Ordering
        public Context contexts { get; set; } 
    }
    public class Context
    {
        public IList<string> types { get; set; }                         //  Context For new pricequote and user review
    }

    public class Payload
    {
        public int CityId { get; set; }                                             //  CityId for Url
        public string CityMaskingName { get; set; }                                 //  CityMaskingName for Url
        //public string StateName { get; set; }                                     //  StateName For append after city
        public bool IsDuplicate { get; set; }                                       //  Check IsDuplicate City  obj.mm_suggest.payload.IsUpcoming = true;
    }
}
