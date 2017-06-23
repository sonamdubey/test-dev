using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using BikewaleOpr.BAL.ContractCampaign;
using BikewaleOpr.Entities.ContractCampaign;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Interface.ManufacturerCampaign;
using BikewaleOpr.Service.AutoMappers.BikeData;
using BikewaleOpr.Service.AutoMappers.Location;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers.ManufacturerCamapaigns
{
    /// <summary>
    /// Created by Subodh Jain 29 aug 2016
    /// Description :For Manufacturer Campaign
    /// </summary>    
    public class ManufacturerCampaignController : ApiController
    {
        private readonly IManufacturerCampaignRepository _objManufacturerCampaign = null;
        private readonly IManufacturerReleaseMaskingNumber _objManufacturerReleaseMaskingNumber = null;
        private readonly IManufacturerCampaign _objMfgCampaign = null;
        public ManufacturerCampaignController(IManufacturerCampaignRepository objManufacturerCampaign, IManufacturerReleaseMaskingNumber objManufacturerReleaseMaskingNumber, IManufacturerCampaign objMfgCampaign)
        {
            _objManufacturerCampaign = objManufacturerCampaign;
            _objManufacturerReleaseMaskingNumber = objManufacturerReleaseMaskingNumber;
            _objMfgCampaign = objMfgCampaign;
        }

        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description : return list of campaigns for selected dealer
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpGet, Route("api/campaigns/manufacturer/search/dealerId/{dealerId}")]
        public IHttpActionResult GetCampaigns(uint dealerId)
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
                ErrorClass objErr = new ErrorClass(ex, "ManufacturerCampaignController.GetCampaigns");
                objErr.SendMail();
                return InternalServerError();
            }
        }
        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description : API to make manufacturer campaign active or inactive
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpPost, Route("api/campaigns/manufacturer/updatecampaignstatus/campaignId/{campaignId}/status/{isactive}")]
        public IHttpActionResult UpdateCampaignStatus(uint campaignId, bool isactive)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _objManufacturerCampaign.UpdateCampaignStatus(campaignId, isactive);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManufacturerCampaignController.UpdateCampaignStatus");
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

        /// <summary>
        /// Created by : Aditi Srivastava on 22 June 2017
        /// Summary    : Get models of a make
        /// </summary>
        [HttpGet, Route("api/campaigns/manufacturer/models/makeId/{makeId}")]
        public IHttpActionResult GetBikeModels(uint makeId)
        {
            IEnumerable<BikeModelEntity> _models = null;
            try
            {
                if (makeId > 0)
                {
                    _models = _objMfgCampaign.GetBikeModels(makeId);
                    if (_models != null && _models.Count() > 0)
                    {
                        IEnumerable<BikeModelDTO> objModelsDTO = BikeModelsMapper.ConvertV2(_models);
                        return Ok(objModelsDTO);
                    }
                    else
                    {
                        return Ok();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("ManufacturerCampaignController.GetBikeModels. MakeId : {0}", makeId));
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 22 June 2017
        /// Summary    : Get cities of a state
        /// </summary>
        [HttpGet, Route("api/campaigns/manufacturer/cities/stateId/{stateId}")]
        public IHttpActionResult GetCitiesByState(uint stateId)
        {
            IEnumerable<CityEntity> _cities = null;
            try
            {
                if (stateId > 0)
                {
                    _cities = _objMfgCampaign.GetCitiesByState(stateId);

                    if (_cities != null && _cities.Count() > 0)
                    {
                        IEnumerable<CityDTO> objCitiesDTO = CityMapper.Convert(_cities);
                        return Ok(objCitiesDTO);
                    }
                    else
                    {
                        return Ok();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("ManufacturerCampaignController.GetCitiesByState. StateId:{0}", stateId));
                return InternalServerError();
            }
        }

        [HttpPost, Route("api/campaigns/manufacturer/deleterule/")]
        public IHttpActionResult DeleteRule([FromBody]ManufacturerRuleEntityDTO ruleEntity)
        {
            bool isSuccess = false;
            try
            {
                if (ruleEntity != null)
                    isSuccess = _objMfgCampaign.DeleteManufacturerCampaignRules(ruleEntity.CampaignId, ruleEntity.ModelId, ruleEntity.StateId, ruleEntity.CityId, ruleEntity.UserId, ruleEntity.IsAllIndia);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManufacturerCampaignController.DeleteRule");
                return InternalServerError();
            }
            return Ok(isSuccess);
        }

    }
}
