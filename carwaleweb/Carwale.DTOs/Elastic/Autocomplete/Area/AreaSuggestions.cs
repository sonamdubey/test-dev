using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Elastic.Autocomplete.Area
{

    public class AreaDocument
    {
        public string id;
        public AreaSuggestions areaSuggests;
        public string output { get; set; }
        public AreaPayLoad payload { get; set; }
    }

    public class AreaSuggestions
    {
        public List<string> input { get; set; }
        public int weight { get; set; }
    }
}
