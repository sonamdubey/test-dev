using Carwale.Cache.NewCarFinder;
using Carwale.DAL.NewCarFinder;
using Carwale.Interfaces.NewCarFinder;
using Microsoft.Practices.Unity;

namespace Carwale.Service.Containers
{
    public static class NewCarFinder
    {
        public static void RegisterTypes(UnityContainer container)
        {
            container
                .RegisterType<INewCarFinderCacheRepository, NewCarFinderCacheRepository>()
                .RegisterType<INewCarFinderRepository, NewCarFinderRepository>();
        }
    }
}

