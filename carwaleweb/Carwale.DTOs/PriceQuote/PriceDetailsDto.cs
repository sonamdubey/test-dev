using Carwale.Entity.Common;
using System.Collections.Generic;

namespace Carwale.DTOs.PriceQuote
{
    public class PriceDetailsDto
    {
        public Entity.Price.PriceQuote PriceQuote { get; set; }
        public Dictionary<int, IdName> CampaignTemplates { get; set; }
    }
}
