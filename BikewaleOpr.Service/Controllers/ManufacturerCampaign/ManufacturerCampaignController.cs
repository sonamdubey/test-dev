﻿using Bikewale.Notifications;
using BikewaleOpr.BAL.ContractCampaign;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Interface.ManufacturerCampaign;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers
{  /// <summary>
    /// Created by Subodh Jain 29 aug 2016
    /// Description :For Manufacturer Campaign
    /// </summary>
    /// <param name="dealerId"></param>
    /// <returns></returns>
    public class ManufacturerCampaignController : ApiController
    {
        private readonly IManufacturerCampaign _objManufacturerCampaign = null;
        private readonly IContractCampaign _objContractCampaign = null;
        private readonly IManufacturerReleaseMaskingNumber _objManufacturerReleaseMaskingNumber = null;
        public ManufacturerCampaignController(IManufacturerCampaign objManufacturerCampaign, IContractCampaign objContractCampaign, IManufacturerReleaseMaskingNumber objManufacturerReleaseMaskingNumber)
        {
            _objManufacturerCampaign = objManufacturerCampaign;
            _objContractCampaign = objContractCampaign;
            _objManufacturerReleaseMaskingNumber = objManufacturerReleaseMaskingNumber;
        }
        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description : return list of campaigns for selected dealer
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetManufactureCampaigns(uint dealerId)
        {
            IEnumerable<ManufactureDealerCampaign> _objMfgList = null;
            try
            {
                if (dealerId > 0)
                {
                    _objMfgList = _objManufacturerCampaign.SearchManufactureCampaigns(dealerId);
                    return Ok(_objMfgList);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "SearchManufactureCampaign");
                objErr.SendMail();
                return InternalServerError();
            }            
        }
        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description : Change status for Campaign
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetstatuschangeCampaigns(uint id, uint isactive)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _objManufacturerCampaign.statuschangeCampaigns(id, isactive);
            }
            catch (Exception ex)
            {
              ErrorClass objErr = new ErrorClass(ex, "statuschangeCampaigns");
                objErr.SendMail();
                return InternalServerError();
            }
            return Ok(isSuccess);
        }
        /// <summary>
        /// Created by :Sajal Gupta on 31/08/2016
        /// Description : Get dealer masking numbers from free pool
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetDealerMaskingNumbers(uint dealerId)
        {
            try
            {
                IEnumerable<MaskingNumber> numbersList = null;
                using (IUnityContainer container = new UnityContainer())
                {

                    container.RegisterType<IContractCampaign, ContractCampaign>();
                    IContractCampaign objCC = container.Resolve<IContractCampaign>();

                    numbersList = objCC.GetAllMaskingNumbers(dealerId);

                    if (numbersList != null && numbersList.Count() > 0)
                    {
                        return Ok(numbersList);
                        
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.AjaxCommon.GetDealerMaskingNumbers");
                objErr.SendMail();
            }
            return null;


        }

        /// <summary>
        /// Created by  :   Sajal Gupta on 31/08/2016
        /// Description :   Release Number
        /// </summary>
        /// <param name="maskingNumber"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ReleaseNumber(uint dealerId, int campaignId, string maskingNumber, int userId)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _objManufacturerReleaseMaskingNumber.ReleaseNumber(dealerId, campaignId, maskingNumber, userId);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.AjaxCommon.MapCampaign");
                objErr.SendMail();
            }
            return Ok(isSuccess);
        }
    }
}
