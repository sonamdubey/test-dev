using AEPLCore.Cache.Interfaces;
using AutoMapper;
using Campaigns.DealerCampaignClient;
using Carwale.DTOs;
using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using Carwale.Entity.UsedCarsDealer;
using Carwale.Interfaces;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.Stock;
using Carwale.Notifications;
using Carwale.Utility;
using ProtoBufClass.Campaigns;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;

namespace Carwale.BL.Dealers
{
    public class Dealers : IDealers
    {
        static Random rnd = new Random();
        protected readonly IDealerCache _dealersCache;
        private readonly ICacheManager _cache;
        private readonly IGeoCitiesCacheRepository _geoCache;
        private static string _imgHostUrl = Carwale.Utility.CWConfiguration._imgHostUrl;
        private readonly IDealerRepository _dealerRepo;
        private readonly IStockBL _stockBL;

        public Dealers(IDealerCache dealersCache, ICacheManager cache, IGeoCitiesCacheRepository geoCache, IDealerRepository dealerRepo, IStockBL stockBL)
        {
            _dealersCache = dealersCache;
            _cache = cache;
            _geoCache = geoCache;
            _dealerRepo = dealerRepo;
            _stockBL = stockBL;
        }
        public DealerDetails GetDealerDetailsOnDealerId(int dealerId)
        {
            var dealerDetails = new DealerDetails();
            dealerDetails = _dealersCache.GetDealerDetailsOnDealerId(dealerId);
            return dealerDetails;
        }

        public List<CarModelSummary> GetDealerModelsOnMake(int makeId, int dealerId)
        {
            return _dealersCache.GetDealerModelsOnMake(makeId, dealerId);
        }

        public string GetDealerWorkingTime(string startWorkingTime, string endWorkingTime)
        {
            var validateTime = "([1-9]|0[1-9]|[1][0-2])[:]([0-9]|0[0-9]|[1-5][0-9])[ ][A|a|P|p][M|m]";
            string workingtime = "";

            if (startWorkingTime != null && endWorkingTime != null)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(startWorkingTime, validateTime) && System.Text.RegularExpressions.Regex.IsMatch(endWorkingTime, validateTime))
                {
                    startWorkingTime = (DateTime.Parse(startWorkingTime).ToString("HH:mm"));
                    endWorkingTime = (DateTime.Parse(endWorkingTime).ToString("HH:mm"));
                    workingtime = startWorkingTime + "-" + endWorkingTime;
                }
                else
                {
                    workingtime = "-";
                }
            }
            else
            {
                workingtime = "-";
            }

            return workingtime;
        }

        public List<AboutUsImageEntity> GetDealerImages(int dealerId)
        {
            WebClient client = new WebClient();
            string response = string.Empty;
            try
            {
                response = client.DownloadString(ConfigurationManager.AppSettings["AutobizHostURL"] + "/api/Images/Get?dealerId=" + dealerId);
                List<AboutUsImageEntity> dealerImageList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AboutUsImageEntity>>(response);
                if (dealerImageList != null && dealerImageList.Count > 0)
                {
                    for (int dealer = 0; dealer < dealerImageList.Count; dealer++)
                    {
                        dealerImageList[dealer].ImgThumbUrl = _imgHostUrl + ImageSizes._227X128 + dealerImageList[dealer].OriginalImgPath;
                        dealerImageList[dealer].ImgLargeUrl = _imgHostUrl + ImageSizes._600X337 + dealerImageList[dealer].OriginalImgPath;
                        dealerImageList[dealer].HostUrl = _imgHostUrl;
                    }
                }
                return dealerImageList;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Dealers.GetDealerImages()");
                objErr.LogException();
            }
            finally
            {
                client.Dispose();
            }
            return new List<AboutUsImageEntity>();
        }

        public List<DealerDetails> DealerDetailsOnMakeState(int makeId, int stateId, int count = -1)
        {
            throw new NotImplementedException();
        }
    }
}
