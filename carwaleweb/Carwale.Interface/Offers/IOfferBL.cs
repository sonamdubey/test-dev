using Carwale.Entity.OffersV1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Offers
{
    public interface IOfferBL
    {
        Entity.OffersV1.Offer GetOffer(OfferInput offerInput);
        bool ValidateOfferInput(OfferInput offerInput);
    }
}
