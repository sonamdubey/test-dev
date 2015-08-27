using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Microsoft.Practices.Unity;

namespace Bikewale.Service
{
    public static class UnityBootstrapper
    {
        public static IUnityContainer Initialize()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();

            return container;
        }
    }
}