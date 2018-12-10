using Carwale.BL.Stock;
using Carwale.Cache.Classified.Slots;
using Carwale.DAL.Classified.Slots;
using Carwale.Interfaces.Classified.Slots;
using Carwale.Interfaces.Stock;
using Microsoft.Practices.Unity;

namespace Carwale.Service.Containers
{
    public static class ClassifiedSlots
    {
        public static void RegisterTypes(IUnityContainer unityContainer)
        {
            unityContainer
                .RegisterType<ISlotsRepository, SlotsRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<ISlotsCacheRepository, SlotsCacheRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<IStocksBySlot, StocksBySlot>(new ContainerControlledLifetimeManager());
        }
    }
}
