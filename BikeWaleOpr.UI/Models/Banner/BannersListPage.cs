using Bikewale.Notifications;
using BikewaleOpr.Interface.Banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Models.Banner
{
    public class BannersListPage
    {
        private readonly IBannerRepository _objBannerRespository = null;
        public BannersListPage(IBannerRepository objBannerRespository)
        {
            _objBannerRespository = objBannerRespository;
        }
        public BannerListVM GetData()
        {
            BannerListVM objPage = null;
            try
            {
                objPage = new BannerListVM();
                objPage.BannerList = _objBannerRespository.GetBanners();               
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.Models.Banner.BannersListPage"));
            }
            return objPage;
        }
    }
}