
using BikewaleOpr.BAL.ContractCampaign;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Interface.ContractCampaign;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
namespace BikewaleOpr.common.ContractCampaignAPI
{
    /// <summary>
    /// 
    /// </summary>
    public class CwWebserviceAPI
    {

        IContractCampaign _cwWebserviceBL = null;

        /// <summary>
        /// 
        /// </summary>
        public CwWebserviceAPI()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IContractCampaign, ContractCampaign>();
                _cwWebserviceBL = container.Resolve<IContractCampaign>();
            }
        }

        /// <summary>
        /// Created by: Sushil Kumar on 15th July 2016
        /// Description: call API to get all masking numbers
        /// </summary>
        /// <param name="dealerId"></param>
        public IEnumerable<MaskingNumber> GetAllMaskingNumbers(uint dealerId)
        {
            return _cwWebserviceBL.GetAllMaskingNumbers(dealerId);
        }

        /// <summary>
        /// Created by: Sushil Kumar on 15th July 2016
        /// Description: Call CWS to map contract and campaign 
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="contractId"></param>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public bool IsCCMapped(uint dealerId, uint contractId, uint campaignId)
        {
            return _cwWebserviceBL.IsCCMapped(dealerId, contractId, campaignId);
        }


        /// <summary>
        /// Created by: Sushil Kumar on 15th July 2016
        /// Description: Release masking number carwale web service api call
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="userId"></param>
        /// <param name="maskingNumbers"></param>
        /// <returns></returns>
        public bool ReleaseMaskingNumber(uint dealerId, int userId, string maskingNumbers)
        {
            return _cwWebserviceBL.RelaseMaskingNumbers(dealerId, userId, maskingNumbers);
        }

        /// <summary>
        /// Created by: Sushil Kumar on 15th July 2016
        /// Description: Release masking number carwale web service api call
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="dealerNumber"></param>
        /// <param name="maskingNumber"></param>
        public bool AddCampaignContractData(ContractCampaignInputEntity _ccInputs)
        {
            return _cwWebserviceBL.AddCampaignContractData(_ccInputs);
        }


    }
}