using Bikewale.Notifications;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Interface.ManufacturerCampaign;
using Microsoft.Practices.Unity;
using System;

namespace BikewaleOpr.BAL
{
    public class ManufacturerReleaseMaskingNumber : IManufacturerReleaseMaskingNumber
    {
        public bool ReleaseNumber(uint dealerId, int campaignId, string maskingNumber, int userId)
        {
            bool isSuccess = false;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IManufacturerCampaignRepository, BikewaleOpr.DALs.ManufactureCampaign.ManufacturerCampaign>();
                    container.RegisterType<IContractCampaign, BikewaleOpr.BAL.ContractCampaign.ContractCampaign>();
                    IManufacturerCampaignRepository objMfgCampaign = container.Resolve<IManufacturerCampaignRepository>();
                    IContractCampaign _objContractCampaign = container.Resolve<IContractCampaign>();

                    isSuccess = _objContractCampaign.RelaseMaskingNumbers(dealerId, userId, maskingNumber);

                    if (isSuccess)
                    {
                        isSuccess = objMfgCampaign.ReleaseCampaignMaskingNumber(campaignId);
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.BAL.ManufacturerReleaseMaskingNumber.ReleaseNumber");
                objErr.SendMail();
            }
            return isSuccess;
        }
    }
}
