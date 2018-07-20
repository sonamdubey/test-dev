using Bikewale.ManufacturerCampaign.DTO.SearchCampaign;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Entities.SearchCampaign;
using Bikewale.Notifications;
using BikewaleOpr.BAL.ContractCampaign;
using BikewaleOpr.Entities.ContractCampaign;
using BikewaleOpr.Interface.Amp;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Interface.ManufacturerCampaign;
using BikewaleOpr.Service.AutoMappers.BikeData;
using BikewaleOpr.Service.AutoMappers.Location;
using BikewaleOpr.Service.AutoMappers.ManufacturerCampaign;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace BikewaleOpr.Service.Controllers.ManufacturerCamapaigns
{
    /// <summary>
    /// Created by Subodh Jain 29 aug 2016
    /// Description :For Manufacturer Campaign
    /// </summary>    
    public class ManufacturerCampaignController : ApiController
    {
        private readonly Interface.ManufacturerCampaign.IManufacturerCampaignRepository _objManufacturerCampaign;
        private readonly IManufacturerReleaseMaskingNumber _objManufacturerReleaseMaskingNumber;
        private readonly Bikewale.ManufacturerCampaign.Interface.IManufacturerCampaignRepository _objMfgCampaign;
        private readonly Bikewale.ManufacturerCampaign.Interface.IManufacturerCampaign _manufacturerCampaign;  
        private readonly Bikewale.ManufacturerCampaign.Interface.IManufacturerCampaignRepository _repo;
        private readonly IBikeModelsRepository _bikeModelsRepository;
        private readonly IBikeMakesRepository _bikeMakesRepository;
        private readonly IAmpCache _ampCache;

        public ManufacturerCampaignController(Interface.ManufacturerCampaign.IManufacturerCampaignRepository objManufacturerCampaign, IManufacturerReleaseMaskingNumber objManufacturerReleaseMaskingNumber, Bikewale.ManufacturerCampaign.Interface.IManufacturerCampaignRepository objMfgCampaign, Bikewale.ManufacturerCampaign.Interface.IManufacturerCampaign manufacturerCampaign, Bikewale.ManufacturerCampaign.Interface.IManufacturerCampaignRepository repo, IBikeModelsRepository bikeModelsRepository, IBikeMakesRepository bikeMakesRepository, IAmpCache ampCache)
        {
            _objManufacturerCampaign = objManufacturerCampaign;
            _objManufacturerReleaseMaskingNumber = objManufacturerReleaseMaskingNumber;
            _objMfgCampaign = objMfgCampaign;
            _manufacturerCampaign = manufacturerCampaign;
            _repo = repo;
            _bikeModelsRepository = bikeModelsRepository;
            _bikeMakesRepository = bikeMakesRepository;
            _ampCache = ampCache;
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
                ErrorClass.LogError(ex, "ManufacturerCampaignController.GetCampaigns");

                return InternalServerError();
            }
        }
        /// <summary>
        /// Created by Subodh Jain 22 jun 2017
        /// Description : return list of campaigns for selected dealer
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<ManufacturerCampaignDetailsDTO>)), Route("api/v2/campaigns/manufacturer/search/dealerId/{dealerId}/allActiveCampaign/{allActiveCampaign}")]
        public IHttpActionResult GetCampaignsV2(uint dealerId, uint allActiveCampaign)
        {
            IEnumerable<ManufacturerCampaignDetailsList> _objMfgList = null;
            try
            {
                if (dealerId > 0)
                {
                    _objMfgList = _objManufacturerCampaign.GetManufactureCampaigns(dealerId, allActiveCampaign);
                    return Ok(SearchMapper.Convert(_objMfgList));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("ManufacturerCampaignController.GetCampaignsV2 dealerId:{0}", dealerId));

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
                _manufacturerCampaign.ClearCampaignCache(campaignId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ManufacturerCampaignController.UpdateCampaignStatus");

                return InternalServerError();
            }
            return Ok(isSuccess);
        }

        [HttpPost, Route("api/v2/campaigns/manufacturer/updatecampaignstatus/campaignId/{campaignId}/status/{status}")]
        public IHttpActionResult UpdateCampaignStatusV2(uint campaignId, uint status)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _objManufacturerCampaign.UpdateCampaignStatus(campaignId, status);
                _manufacturerCampaign.ClearCampaignCache(campaignId);
                ClearAmpCache(campaignId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("ManufacturerCampaignController.UpdateCampaignStatusV2 campaignid: {0} status : {1}", campaignId, status));
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

                    if (numbersList != null && numbersList.Any())
                    {
                        return Ok(numbersList);

                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.AjaxCommon.GetDealerMaskingNumbers");

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
        public IHttpActionResult ReleaseNumber(uint dealerId, uint campaignId, string maskingNumber, int userId)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _objManufacturerReleaseMaskingNumber.ReleaseNumber(dealerId, (int)campaignId, maskingNumber, userId);
                _manufacturerCampaign.ClearCampaignCache(campaignId);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                ErrorClass.LogError(ex, "BikewaleOpr.AjaxCommon.MapCampaign");

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
                    if (_models != null && _models.Any())
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
                ErrorClass.LogError(ex, string.Format("ManufacturerCampaignController.GetBikeModels. MakeId : {0}", makeId));
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

                    if (_cities != null && _cities.Any())
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
                ErrorClass.LogError(ex, string.Format("ManufacturerCampaignController.GetCitiesByState. StateId:{0}", stateId));
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
                {
                    isSuccess = _objMfgCampaign.DeleteManufacturerCampaignRules(ruleEntity.CampaignId, ruleEntity.ModelId, ruleEntity.StateId, ruleEntity.CityId, ruleEntity.UserId, ruleEntity.IsAllIndia);

                    _manufacturerCampaign.ClearCampaignCache(ruleEntity.CampaignId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ManufacturerCampaignController.DeleteRule");
                return InternalServerError();
            }
            return Ok(isSuccess);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 04 Aug 2017
        /// Description :   API to reset total lead delivered for a Campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost, Route("api/campaigns/manufacturer/{campaignId}/totalleads/reset/")]
        public IHttpActionResult ResetTotalLeadDeliveredCount(uint campaignId, uint userId)
        {
            bool isSuccess = false;
            try
            {
                if (campaignId > 0 && userId > 0)
                {
                    isSuccess = _objMfgCampaign.ResetTotalLeadDelivered(campaignId, userId);
                    _manufacturerCampaign.ClearCampaignCache(campaignId);
                }
                else
                {
                    return BadRequest("Invalid data.");
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("ManufacturerCampaignController.ResetTotalLeadDeliveredCount({0},{1})", campaignId, userId));
                return InternalServerError();
            }
            return Ok(isSuccess);
        }

        [ResponseType(typeof(String)), Route("api/campaigns/generateleadcaptureformlink/"), HttpPost]
        public IHttpActionResult GenerateLeadCaptureFormLink(LeadPopupRequest[] reqs)
        {
            if (true)
            {

                return Ok(GenerateLeadCaptureFormLinks(reqs));
            }
        }

        private IEnumerable<KeyValuePair<string, string>> GenerateLeadCaptureFormLinks(IEnumerable<LeadPopupRequest> reqs)
        {
            ICollection<KeyValuePair<string, string>> urls = new List<KeyValuePair<string, string>>();
            foreach (var req in reqs)
            {
                string url = String.Format("{0}/m/popup/leadcapture/?q={1}&platformId={2}", req.hostUrl, Utils.Utils.EncryptTripleDES(string.Format(@"modelid={0}&cityid={1}&areaid={2}&bikename={3}&location={4}&city={5}&area={6}&ismanufacturer={7}&dealerid={8}&dealername={9}&dealerarea={10}&versionid={11}&leadsourceid={12}&pqsourceid={13}&mfgcampid={14}&pqid={15}&pageurl={16}&clientip={17}&dealerheading={18}&dealermessage={19}&dealerdescription={20}&pincoderequired={21}&emailrequired={22}&dealersrequired={23}&url={24}",
                                                                req.modelId, req.cityId, string.Empty, req.bikeName, string.Empty, string.Empty, string.Empty,
                                                                req.isManufacturer, req.dealerId, req.dealerName, req.area, req.versionId, req.leadSourceId, req.pqSourceId, req.campaignId, req.pqId, string.Empty, "", req.popupHeading, req.popupSuccessMessage, req.popupDescription, req.pincodeRequired, req.emailRequired, req.dealerRequired,
                                                                req.returnPageUrl)), req.platformId > 0 ? req.platformId : 2);
                urls.Add(new KeyValuePair<string, string>(req.bikeName, url));
            }
            return urls;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 17 July 2018
        /// Description : Amp cache clear for model page
        /// </summary>
        /// <param name="campaignId"></param>
        private void ClearAmpCache(uint campaignId)
        {
            var rules = _repo.GetManufacturerCampaignRules(campaignId);
            if (rules != null && rules.ManufacturerCampaignRules != null && rules.ManufacturerCampaignRules.Any())
            {
                IEnumerable<uint> modelIds = rules.ManufacturerCampaignRules.Select(m => m.ModelId);
                _ampCache.UpdateModelAmpCache(modelIds);
            }
        }

        public class LeadPopupRequest
        {
            public string modelId;
            public string cityId;
            public string bikeName;
            public bool isManufacturer;
            public uint dealerId;
            public string dealerName;
            public string area;
            public uint versionId;
            public uint leadSourceId;
            public string pqSourceId;
            public string campaignId;
            public uint pqId;
            public string popupHeading;
            public string popupSuccessMessage;
            public string popupDescription;
            public bool pincodeRequired;
            public bool emailRequired;
            public bool dealerRequired;
            public string returnPageUrl;
            public string hostUrl;
            public ushort platformId;

        }

    }
}
