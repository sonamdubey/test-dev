using System.Collections.Generic;

namespace Bikewale.PinCodesAutosuggest
{
    /// <summary>
    /// Created By : Sushil Kumar on 9th March 2017
    /// Description : PinCode entity list
    /// </summary>
    public class PinCodeList
    {
        public string Id { get; set; }
        public PinCodeSuggestion mm_suggest { get; set; }
        public string name { get; set; }
    }

    /// <summary>
    /// Created By : Sushil Kumar on 9th March 2017
    /// Description : PinCodeSuggestion entity
    /// </summary>
    public class PinCodeSuggestion
    {
        public List<string> input { get; set; }
        public string output { get; set; }
        public PayLoad payload { get; set; }
        public int Weight { get; set; }
    }


    /// <summary>
    /// Created By : Sushil Kumar on 9th March 2017
    /// Description : PayLoad entity
    /// </summary>
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


