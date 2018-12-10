using System;

namespace Carwale.Entity.PriceQuote
{
    [Serializable]
    public class PQItem
    {
        public int CategoryItemId { get; set; }
        public string Key { get; set; }
        public long Value { get; set; }
        public bool IsMetallic { get; set; }
    }
}
