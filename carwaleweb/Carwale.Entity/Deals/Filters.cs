using System;

namespace Carwale.Entity.Deals
{
    [Serializable]
    public class Filters
    {
        public int CityId { get; set; }
        public string Makes { get; set; }
        public string Fuels { get; set; }
        public string Transmissions { get; set; }
        public string BodyTypes { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int PN { get; set; } //PageNo.
        public int PS { get; set; } //PageSize
        public int SO { get; set; } // SortOrder
        public int SC { get; set; } // SortCriteria
        public int StartBudget { get; set; }
        public int EndBudget { get; set; }
        public string budget { get; set; }


    }
}
