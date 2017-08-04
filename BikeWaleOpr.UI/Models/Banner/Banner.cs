using BikewaleOpr.Interface.Banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Models.Banner
{
    public class Banner
    {
        private readonly IBannerRepository _objBannerRespository = null;       
        public Banner(IBannerRepository objBannerRespository)
        {
            _objBannerRespository = objBannerRespository;
        }
        public BannerVM GetData(uint bannerId)
        {
            BannerVM objBannerVM = null;
            objBannerVM = _objBannerRespository.GetBannerDetails(bannerId);           
            return objBannerVM;
        }
    }
}