using Carwale.BL.Prices;
using Carwale.Cache.PriceQuote;
using Carwale.DAL.PriceQuote;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.Prices;
using Carwale.Service.Adapters.NewCars;
using Microsoft.Practices.Unity;

namespace Carwale.Service.Containers
{
    public static class Prices
    {
        public static void RegisterTypes(UnityContainer container)
        {
            container
                .RegisterType<IEmiCalculatorBl, EmiCalculatorBl>()
                .RegisterType<IEmiCalculatorAdapter, EmiCalculatorAdapter>()
                .RegisterType<IThirdPartyEmiDetailsRepository, ThirdPartyEmiDetailsRepository>()
                .RegisterType<IThirdPartyEmiDetailsCache, ThirdPartyEmiDetailsCache>();

        }
    }
}
