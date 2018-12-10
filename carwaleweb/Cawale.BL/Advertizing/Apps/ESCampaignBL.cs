using Carwale.Entity.Advertizings.Apps;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.BL.Advertizing.Apps
{
    public class ESCampaignBL
    {
        public ESCampaign GetAd(int sourceId)
        {
            ESCampaign _es = new ESCampaign();
            if (sourceId.Equals(Convert.ToInt16(Platform.CarwaleAndroid)))
            {
                _es.HtmlUrl = "https://" + ConfigurationManager.AppSettings["HostUrl"].ToString() + "/es/creta/mobile-leaderboard.html";
            }
            if (sourceId.Equals(Convert.ToInt16(Platform.CarwaleiOS)))
            {
                _es.HtmlUrl = "https://" + ConfigurationManager.AppSettings["HostUrl"].ToString() + "/es/creta/mobile-leaderboard1.html";
            }

            _es.ShowAd = ConfigurationManager.AppSettings["ShowESCampaign"].ToString();
            _es.ModelDetailUrl = "https://" + ConfigurationManager.AppSettings["HostUrl"].ToString() + "/api/modeldetails/?Budget=-1,-1&FuelTypes=-1&BodyTypes=-1&Transmission=-1&SeatingCapacity=-1&EnginePower=-1&ImportantFeatures=-1&ModelId=862";
            _es.ModelId = 862;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["BannerImgHostUrl_App"].ToString()))
            {
                _es.Image = new CarImageBase() { HostUrl = ConfigurationManager.AppSettings["BannerImgHostUrl_App"].ToString(), ImagePath = ConfigurationManager.AppSettings["BannerImgOrigImgUrl_App"].ToString() };
            }
            return _es;
        }
    }
}
