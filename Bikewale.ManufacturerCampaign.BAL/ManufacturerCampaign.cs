using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Cache;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Bikewale.ManufacturerCampaign.BAL
{
    public class ManufacturerCampaign : IManufacturerCampaign
    {
        private readonly Interface.IManufacturerCampaignCache _cacheRepo = null;
        private readonly Interface.IManufacturerCampaignRepository _repo = null;

        private readonly Array _pages = Enum.GetValues(typeof(Entities.ManufacturerCampaignServingPages));
        public ManufacturerCampaign(Interface.IManufacturerCampaignCache cacheRepo, Interface.IManufacturerCampaignRepository repo)
        {
            _cacheRepo = cacheRepo;
            _repo = repo;
        }
        public ManufacturerCampaignEntity GetCampaigns(uint modelId, uint cityId, ManufacturerCampaignServingPages pageId)
        {
            return _cacheRepo.GetCampaigns(modelId, cityId, pageId);
        }

        /// <summary>
        /// Created By : Sushil Kumar on 11th Aug 2017
        /// Description : Save manufacturer id /dealer id against the pqid for manufacturer campaigns impressions
        /// </summary>
        /// <param name="pqId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public bool SaveManufacturerIdInPricequotes(uint pqId, uint dealerId)
        {
            bool isSuccess = false;
            try
            {
                if (pqId > 0 && dealerId > 0)
                {

                    NameValueCollection objNVC = new NameValueCollection();
                    objNVC.Add("par_pqid", pqId.ToString());
                    objNVC.Add("par_dealerid", dealerId.ToString());
                    SyncBWData.PushToQueue("updatepqidformanufacturerid", DataBaseName.BW, objNVC);

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.ManufacturerCampaign.BAL.GetManufactureCampaigns");
            }
            return isSuccess;
        }


        public bool ClearCampaignCache(uint campaignId)
        {
            try
            {
                string keyFormat = "BW_ES_Campaign_M_{0}_C_{1}_P_";
                var rules = _repo.GetManufacturerCampaignRules(campaignId);
                if (rules != null && rules.ManufacturerCampaignRules != null && rules.ManufacturerCampaignRules.Any())
                {
                    IEnumerable<string> keys = rules.ManufacturerCampaignRules.Select(m => String.Format(keyFormat, m.ModelId, m.CityId));

                    List<string> allKeys = new List<string>();
                    foreach (var page in _pages)
                    {
                        foreach (var key in keys)
                        {
                            allKeys.Add(string.Concat(key, Convert.ToUInt16(page)));
                        }
                    }
                    BwMemCache.ClearManufacturerCampaign(allKeys);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.ManufacturerCampaign.BAL.ClearCampaignCache");
                return false;
            }
            return true;
        }

        public bool ClearCampaignCache(uint campaignId, IEnumerable<string> modelIds, IEnumerable<string> cityIds)
        {
            ICollection<String> keys = new List<String>();

            foreach (var page in _pages)
            {
                foreach (var modelId in modelIds)
                {
                    foreach (var cityId in cityIds)
                    {
                        keys.Add(String.Format("BW_ES_Campaign_M_{0}_C_{1}_P_{2}", modelId, cityId, Convert.ToUInt16(page)));
                    }
                }
            }
            BwMemCache.ClearManufacturerCampaign(keys);
            return true;
        }
    }
}
