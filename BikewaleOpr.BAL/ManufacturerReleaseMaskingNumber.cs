﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikewaleOpr.Interface.ManufacturerCampaign;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.BAL.ContractCampaign;

namespace BikewaleOpr.BAL
{
  public  class ManufacturerReleaseMaskingNumber : IManufacturerReleaseMaskingNumber
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
                    if(objMfgCampaign.ReleaseCampaignMaskingNumber(campaignId))
                    {
                        _objContractCampaign.RelaseMaskingNumbers(dealerId, userId, maskingNumber);
                        isSuccess = true;
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
