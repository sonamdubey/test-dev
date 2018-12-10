using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.SellCarUsed
{
    public class CustomerSellInquiryData
    {
        public int Id { get; set; }
        public string CarName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public int CarVersionId { get; set; }
    }
}
