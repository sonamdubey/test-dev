using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.BikeBooking
{
    public class DealerVersionPriceItemDTO
    {
        public string ItemName { get; set; }
        public uint Price { get; set; }
        public uint DealerId { get; set; }
        public uint ItemId { get; set; }
    }
}
