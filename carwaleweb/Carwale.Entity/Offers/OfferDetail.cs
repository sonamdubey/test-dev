using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Offers
{
    public class OfferDetail
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public string ExpiryDate { get; set; }
    }
}
