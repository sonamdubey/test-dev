using BikewaleOpr.BAL;
using BikewaleOpr.BAL.Used;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.Used;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Unity.Mvc5;
using BikewaleOpr.Interface.UserReviews;
using BikewaleOpr.DALs.UserReviews;

namespace BikewaleOpr
{
    /// <summary>
    /// Created by : Ashish G. Kamble on 6 Jan 2017
    /// Modified by : Sajal Gupta on 09-03-2017
    /// Description : Added IBikeModels, IUsedBikes, IHomePage
    /// </summary>
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IBikeMakes, BikeMakesRepository>()
                .RegisterType<IBikeModelsRepository, BikeModelsRepository>()
                .RegisterType<IBikeVersions, BikeVersionsRepository>()
                .RegisterType<IBikeModels, BikeModels>()
                .RegisterType<IUsedBikes, UsedBikes>()
                .RegisterType<IHomePage, HomePage>()
                .RegisterType<IUserReviewsRepository, UserReviewsRepository>()
                .RegisterType<IBikeMakes, BikeMakesRepository>()
                .RegisterType<IBikeModelsRepository, BikeModelsRepository>();


            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}