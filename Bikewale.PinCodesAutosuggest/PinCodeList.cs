using System.Collections.Generic;

namespace Bikewale.PinCodesAutosuggest
{
    public class PinCodeList
    {
        public string Id { get; set; }
        public PinCodeSuggestion mm_suggest { get; set; }
        public string name { get; set; }
    }

    public class PinCodeSuggestion
    {
        public List<string> input { get; set; }
        public string output { get; set; }
        public PayLoad payload { get; set; }
        public int Weight { get; set; }
    }

    public class PayLoad
    {
        public uint PinCodeId { get; set; }
        public string PinCode { get; set; }
        public string Circle { get; set; }
        public string Area { get; set; }
        public string Taluka { get; set; }
        public string Division { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Region { get; set; }
    }
}


