using Bikewale.Notifications;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Interface.ContractCampaign;
using System.Collections.Generic;

namespace BikewaleOpr.BAL.ContractCampaign
{
    /// <summary>
    /// Created By  : Sushil Kumar on 14th July 2016 
    /// Description : Contract Campaign BL for contract campaign mapping
    /// </summary>
    public class ContractCampaign : IContractCampaign
    {
        //IContractCampaign _contractCampaign = null;

        //public ContractCampaign(IContractCampaign contractCampaign)
        //{
        //    _contractCampaign = contractCampaign;
        //}


        /// <summary>
        /// Created By  : Sushil Kumar on 14th July 2016 
        /// Description : To get list of all available masking numbers
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public IEnumerable<MaskingNumber> GetAllMaskingNumbers(uint dealerId)
        {
            IEnumerable<MaskingNumber> _maskingNumbers = null;
            try
            {
                string api = string.Format("/api/maskingnumbers/getall/?dealerId={0}", dealerId);

                using (Bikewale.Utility.BWHttpClient objClient = new Bikewale.Utility.BWHttpClient())
                {
                    _maskingNumbers = objClient.GetApiResponseSync<IEnumerable<MaskingNumber>>(Bikewale.Utility.APIHost.CWS, Bikewale.Utility.BWConfiguration.Instance.APIRequestTypeJSON, api, _maskingNumbers);
                }
            }
            catch (System.Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BAL.CampaignContract.GetAllMaskingNumbers");
                objErr.SendMail();
            }
            return _maskingNumbers;
        }


        /// <summary>
        /// Created By  : Sushil Kumar on 14th July 2016 
        /// Description : To map contract and campaign
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="contractId"></param>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public bool IsCCMapped(uint dealerId, uint contractId, uint campaignId)
        {
            bool _isCCMapped = false;
            try
            {
                string _apiUrl = string.Format("/api/contracts/mapcampaign/?dealerid={0}&contractid={1}&campaignid={2}&applicationId=2", dealerId, contractId, campaignId);

                using (Bikewale.Utility.BWHttpClient objClient = new Bikewale.Utility.BWHttpClient())
                {
                    _isCCMapped = objClient.PostSync<bool, bool>(Bikewale.Utility.APIHost.CWS, Bikewale.Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _isCCMapped);
                }
            }
            catch (System.Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BAL.CampaignContract.IsCCMapped");
                objErr.SendMail();
            }
            return _isCCMapped;
        }

    }
}
