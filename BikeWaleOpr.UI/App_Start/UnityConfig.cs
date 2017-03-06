using BikewaleOpr.BAL.Used;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.Used;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Unity.Mvc5;

namespace BikewaleOpr
{
    /// <summary>
    /// Created by : Ashish G. Kamble on 6 Jan 2017
    /// </summary>
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IBikeMakes, BikeMakesRepository>();
            container.RegisterType<IBikeModels, BikeModelsRepository>();
            container.RegisterType<IUsedBikes, UsedBikes>();
            

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}