using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleAutoSuggest
{
    public class BikeList
    {
        public string Id { get; set; }
        public BikeSuggestion mm_suggest { get; set; }
        public string name { get; set; }
    }

    public class BikeSuggestion
    {
        public List<string> input { get; set; }
        public string output { get; set; }
        public PayLoad payload { get; set; }
        public int Weight { get; set; }
    }

    public class PayLoad
    {
        public string MakeId { get; set; }
        public string ModelId { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public string Futuristic { get; set; }
    }

    public class TempList
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public bool New { get; set; }
        public bool Futuristic { get; set; }
    }
}
