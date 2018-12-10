using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.OffersV1
{
    public class Offer
    {
        public OfferDetails OfferDetails { get; set; }
        public List<OfferCategoryDetails> CategoryDetails { get; private set; }

        public Offer()
        {
            this.CategoryDetails = new List<OfferCategoryDetails>();
        }
    }
}
