using Carwale.BL.Offers;
using Carwale.Interfaces.Offers;
using Carwale.Service.Adapters.Offers;
using Microsoft.Practices.Unity;

namespace Carwale.Service.Containers
{
    public static class Offers
    {
        public static void RegisterTypes(UnityContainer container)
        {
            container
                .RegisterType<IOfferBL, OfferBL>()
                .RegisterType<IOffersAdapter, OffersAdapter>();
        }
    }
}
