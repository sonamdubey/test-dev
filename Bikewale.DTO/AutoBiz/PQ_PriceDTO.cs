using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.AutoBiz
{
    public class PQ_PriceDTO
    {
        public UInt32 CategoryId { get; set; }
        public string CategoryName { get; set; }
        public UInt64 Price { get; set; }
        public UInt32 DealerId { get; set; }
    }
}
