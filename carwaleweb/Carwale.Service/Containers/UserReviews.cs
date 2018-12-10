using Carwale.BL.CMS;
using Carwale.Cache.CMS.UserReviews;
using Carwale.DAL.CMS.UserReviews;
using Carwale.Interfaces.CMS.UserReviews;
using Carwale.Interfaces.NewCars;
using Microsoft.Practices.Unity;

namespace Carwale.Service.Containers
{
    public static class UserReviews
    {
        public static void RegisterTypes(UnityContainer container)
        {
            container
                .RegisterType<IServiceAdapterV2, UserReviewsListAdapter>("UserReviewsList", new ContainerControlledLifetimeManager())
                .RegisterType<IUserReviews, BL.CMS.UserReviews.UserReviews>(new ContainerControlledLifetimeManager())
                .RegisterType<IUserReviewsCache, UserReviewsCache>(new ContainerControlledLifetimeManager())
                .RegisterType<IUserReviewsRepository, UserReviewsRepository>(new ContainerControlledLifetimeManager());
        }
    }
}
