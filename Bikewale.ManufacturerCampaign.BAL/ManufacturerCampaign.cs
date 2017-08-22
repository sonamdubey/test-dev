using System;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using System.Collections.Specialized;
using Bikewale.Utility;

namespace Bikewale.ManufacturerCampaign.BAL
{
    public class ManufacturerCampaign : IManufacturerCampaign
    {
        private readonly Interface.IManufacturerCampaignRepository _repo = null;
        public ManufacturerCampaign(Interface.IManufacturerCampaignRepository repo)
        {
            _repo = repo;
        }
        public ManufacturerCampaignEntity GetCampaigns(uint modelId, uint cityId, ManufacturerCampaignServingPages pageId)
        {
            return _repo.GetCampaigns(modelId, cityId, pageId);
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.ManufacturerCampaign.BAL.GetManufactureCampaigns");
            }
            return isSuccess;
        }
    }
}
