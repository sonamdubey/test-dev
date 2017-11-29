using Bikewale.Notifications;
using BikewaleOpr.Interface.Banner;
using System;

namespace BikewaleOpr.Models.Banner
{
    public class BannersListPage
    {
        private readonly IBannerRepository _objBannerRespository = null;
        private uint _bannerStatus = 0;
        public BannersListPage(IBannerRepository objBannerRespository, uint? bannerStatus)
        {
            _objBannerRespository = objBannerRespository;
            if (bannerStatus.HasValue)
                _bannerStatus = bannerStatus.Value;
        }
        public BannerListVM GetData()
        {
            BannerListVM objPage = null;
            try
            {
                objPage = new BannerListVM();
                objPage.BannerList = _objBannerRespository.GetBanners(_bannerStatus);               
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.Models.Banner.BannersListPage"));
            }
            return objPage;
        }
    }
}