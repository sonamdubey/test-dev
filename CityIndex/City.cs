using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityAutoSuggest
{
    public class CityList
    {
        public string Id { get; set; }                                              //  Object Id
        public CitySuggestion mm_suggest { get; set; }                              //  City Type
        public string name { get; set; }                                            //  Object Name
    }   

    public class CityTempList
    {
        public int CityId { get; set; }                                             //  CityId For Payload
        public string CityName { get; set; }                                        //  CityName For Payload
        public string MaskingName { get; set; }                                     //  Masking Name For Payload
        public int Wt { get; set; }                                                 //  For Weight
    }
}
