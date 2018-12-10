using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Elastic
{
    [Serializable]
    public class ESCarDocument
    {
        public string id { get; set; }                                 
        public CarSuggestion mm_suggest { get; set; }                  
        public CarPayLoad payload { get; set; }                            
        public string name { get; set; }                                
        public string output { get; set; }                              
    }
}
