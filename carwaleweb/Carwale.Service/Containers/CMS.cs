using Carwale.BL.CMS;
using Carwale.BL.CMS.Photos;
using Carwale.BL.Editorial;
using Carwale.BL.Videos;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.NewCars;
using Microsoft.Practices.Unity;
using Carwale.BL.EditCMS;
using Carwale.DAL.CMS.ThreeSixty;
using Carwale.Cache.CMS;

namespace Carwale.Service.Containers
{
    public static class CMS
    {
        public static void RegisterTypes(UnityContainer container)
        {
            container
                .RegisterType<IServiceAdapterV2, ExpertReviewAdapter>("ExpertReviewDetails")
                .RegisterType<IServiceAdapterV2, ExpertReviewAdapterMobile>("ExpertReviewDetailsMobile")
                .RegisterType<IServiceAdapterV2, NewsDetailAdapter>("NewsDetails")
                .RegisterType<IPhotos, CMSPhotosBL>()
                .RegisterType<IVideosBL, VideosBL>()
                .RegisterType<ICMSContent, CMSContentBL>()
                .RegisterType<IServiceAdapterV2, ThreeSixtyViewAdapter>("ThreeSixtyAdapter")
                .RegisterType<IThreeSixtyView, ThreeSixtyView>()
                .RegisterType<IServiceAdapterV2, MobileGalleryAdapter>("MobileGalleryAdapter")
                .RegisterType<IServiceAdapterV2, DesktopGalleryAdapter>("DesktopGalleryAdapter")
                .RegisterType<IThreeSixtyDal, ThreeSixtyDal>()
                .RegisterType<IThreeSixtyCache, ThreeSixtyCache>();
        }
    }
}
