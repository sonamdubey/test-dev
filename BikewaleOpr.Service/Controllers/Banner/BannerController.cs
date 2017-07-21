using BikewaleOpr.Entity;
using BikewaleOpr.Interface.Banner;
using BikewaleOpr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers
{
    
    public class BannerController : ApiController
    {
        private readonly IBannerRepository _objBannerRespository = null;

        public BannerController(IBannerRepository objBannerRespository)
        {
            _objBannerRespository = objBannerRespository;
        }
        [HttpPost, Route("api/banner/submit/{bannerDescription}/{startDate}/{endDate}/")]
        public IHttpActionResult SaveBannerBasicDetails( DateTime startDate, DateTime endDate, string bannerDescription,uint? id=null)
        {
            uint campaignid = 0;
            campaignid= _objBannerRespository.SaveBannerBasicDetails(startDate, endDate, bannerDescription,id??0);
            return Ok(campaignid);
        }
        [HttpPost, Route("api/desktop/submit/")]
        public IHttpActionResult SaveBanner([FromBody] BannerVM objBanner ,uint? platformId=1)
        {
            uint id = platformId ?? 0;
            BannerDetails objBannerDetails =( id == 1) ? objBanner.DesktopBannerDetails : objBanner.MobileBannerDetails; 
           bool success= _objBannerRespository.SaveBannerProperties(objBannerDetails, id,objBanner.CampaignId);
            return Ok(success);
        }
    }
}
