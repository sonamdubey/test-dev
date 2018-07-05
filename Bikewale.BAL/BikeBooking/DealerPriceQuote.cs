using Bikewale.Cache.Core;
using Bikewale.Cache.PriceQuote;
using Bikewale.DAL.AutoBiz;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.AutoBiz;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.DAL;
using Bikewale.ManufacturerCampaign.Cache;

namespace Bikewale.BAL.BikeBooking
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 24 Oct 2014
    /// </summary>
    public class DealerPriceQuote : Bikewale.Interfaces.BikeBooking.IDealerPriceQuote
    {
        private readonly Bikewale.Interfaces.BikeBooking.IDealerPriceQuote dealerPQRepository = null;
        private readonly IPriceQuoteCache _pqCache = null;
        private readonly IApiGatewayCaller _apiGatewayCaller;
        private readonly IManufacturerCampaign _objManufacturerCampaign;
        public DealerPriceQuote()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<Bikewale.Interfaces.BikeBooking.IDealerPriceQuote, Bikewale.DAL.BikeBooking.DealerPriceQuoteRepository>();
                container.RegisterType<ICacheManager, MemcacheManager>();
                container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                container.RegisterType<IPriceQuoteCache, PriceQuoteCache>();
                container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, Bikewale.DAL.AutoBiz.DealerPriceQuoteRepository>();
                container.RegisterType<IManufacturerCampaignCache, ManufacturerCampaignCache>();
                container.RegisterType<IManufacturerCampaignRepository, ManufacturerCampaignRepository>();
                container.RegisterType<IManufacturerCampaign, ManufacturerCampaign.BAL.ManufacturerCampaign>();
                dealerPQRepository = container.Resolve<Bikewale.Interfaces.BikeBooking.IDealerPriceQuote>();
                container.RegisterType<IApiGatewayCaller, ApiGatewayCaller>();
                _pqCache = container.Resolve<IPriceQuoteCache>();
                _apiGatewayCaller = container.Resolve<IApiGatewayCaller>();
                _objManufacturerCampaign = container.Resolve<IManufacturerCampaign>();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 29 Oct 2014
        /// Summary :  to save customer detail in newbikedealerpricequote table
        /// Modified By : Sadhana Upadhyay on 29 Dec 2015
        /// Summary : replace parameters with entity
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="pqId"></param>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="customerEmail"></param>
        /// <returns></returns>
        public uint SaveCustomerDetailByPQId(DPQ_SaveEntity entity)
        {
            uint leadId = 0;
            leadId = dealerPQRepository.SaveCustomerDetailByPQId(entity);
            return leadId;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 26 June 2018
        /// Description : passes leadId in input and return leadId
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public uint SaveCustomerDetailByLeadId(Bikewale.Entities.BikeBooking.v2.DPQ_SaveEntity entity)
        {
            return dealerPQRepository.SaveCustomerDetailByLeadId(entity);
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 29 Oct 2014
        /// Summary : To update isverified flag in newbikedealerpricequote table
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public bool UpdateIsMobileVerified(uint pqId)
        {
            bool isSuccess = false;
            isSuccess = dealerPQRepository.UpdateIsMobileVerified(pqId);
            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 29 Oct 2014
        /// Summary :  to update mobile no in newbikedealerpricequote table
        /// </summary>
        /// <param name="pqId"></param>
        /// <param name="mobileNo"></param>
        /// <returns></returns>
        public bool UpdateMobileNumber(uint pqId, string mobileNo)
        {
            bool isSuccess = false;
            isSuccess = dealerPQRepository.UpdateMobileNumber(pqId, mobileNo);
            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 3 Nov 2014
        /// Summary : to update ispushedab flag in pq_newbikedealerpricequote
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public bool PushedToAB(uint pqId, uint abInquiryId)
        {
            bool isSuccess = false;
            isSuccess = dealerPQRepository.PushedToAB(pqId, abInquiryId);
            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 10 Nov 2014
        /// Summary : To get customer details
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public PQCustomerDetail GetCustomerDetailsByPQId(uint pqId)
        {
            PQCustomerDetail objCustomer = null;
            objCustomer = dealerPQRepository.GetCustomerDetailsByPQId(pqId);
            return objCustomer;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 26 JUne 2018
        /// Description : get customer details based on leadId
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public PQCustomerDetail GetCustomerDetailsByLeadId(uint leadId)
        {
            PQCustomerDetail objCustomer = null;
            objCustomer = dealerPQRepository.GetCustomerDetailsByLeadId(leadId);
            return objCustomer;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 2nd Dec
        /// Summary : to check whether customer is verified for given pq id or not
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public bool IsNewBikePQExists(uint pqId)
        {
            bool isVerified = false;
            isVerified = dealerPQRepository.IsNewBikePQExists(pqId);
            return isVerified;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 2 Dec 2014
        /// Summary : To get Version list
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<BikeVersionEntityBase> GetVersionList(uint versionId, uint dealerId, uint cityId)
        {
            List<BikeVersionEntityBase> objVersions = null;
            objVersions = dealerPQRepository.GetVersionList(versionId, dealerId, cityId);
            return objVersions;
        }

        public bool SaveRSAOfferClaim(RSAOfferClaimEntity objOffer, string bikeName)
        {
            bool isSuccess = false;
            bool isOfferClaimEmailAlertEnabled = false;
            string helmet = string.Empty;
            string[] emailAddress = null;
            isSuccess = dealerPQRepository.SaveRSAOfferClaim(objOffer, bikeName);
            isOfferClaimEmailAlertEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["isRSAOfferClaimEmailAlertEnabled"]);
            if (isSuccess && isOfferClaimEmailAlertEnabled)
            {
                switch (objOffer.HelmetId)
                {
                    case 1:
                        helmet = "Vega Cruiser Open Face Helmet (Size: M)";
                        break;
                    case 2:
                        helmet = "Replay Dream Plain Flip-up Helmet (Size: M)";
                        break;
                    case 3:
                        helmet = "Vega Cliff Full Face Helmet (Size: M)";
                        break;
                    default:
                        break;
                }
                emailAddress = System.Configuration.ConfigurationManager.AppSettings["OfferClaimAlertEmail"].Split(',');
                Bikewale.Notifications.ComposeEmailBase objOfferClaim = new Bikewale.Notifications.MailTemplates.OfferClaimAlertNotification(objOffer, bikeName, helmet);
                objOfferClaim.Send(emailAddress[0], string.Format("BW Offer Claim Alert : {0}", objOffer.DealerName), "", emailAddress, null);
            }
            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 16 Dec 2014
        /// Summary : To update Color in dealer price quote table
        /// </summary>
        /// <param name="colorId"></param>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public bool UpdatePQBikeColor(uint colorId, uint pqId)
        {
            bool isSuccess = false;

            isSuccess = dealerPQRepository.UpdatePQBikeColor(colorId, pqId);

            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 17 Dec 2014
        /// Summary : To update transactional details in dealer price quote table
        /// </summary>
        /// <param name="pqId"></param>
        /// <param name="transId"></param>
        /// <param name="isTransComplete"></param>
        /// <returns></returns>
        public bool UpdatePQTransactionalDetail(uint pqId, uint transId, bool isTransComplete, string bookingReferenceNo)
        {
            bool isUpdated = false;

            isUpdated = dealerPQRepository.UpdatePQTransactionalDetail(pqId, transId, isTransComplete, bookingReferenceNo);

            return isUpdated;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Dec 2014
        /// Summary : to get status of dealer notification
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="date"></param>
        /// <param name="customerMobile"></param>
        /// <param name="customerEmail"></param>
        /// <returns></returns>
        public bool IsDealerNotified(uint dealerId, string customerMobile, ulong customerId)
        {
            bool isNotified = false;

            isNotified = dealerPQRepository.IsDealerNotified(dealerId, customerMobile, customerId);

            return isNotified;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 15 Jan 2015
        /// Summary : To get whether Dealer Prices Available or not
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public bool IsDealerPriceAvailable(uint versionId, uint cityId)
        {
            bool isDealerAreaAvailable = false;

            isDealerAreaAvailable = dealerPQRepository.IsDealerPriceAvailable(versionId, cityId);

            return isDealerAreaAvailable;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 20 July 2015
        /// Summary : to get default version id for price quote
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public uint GetDefaultPriceQuoteVersion(uint modelId, uint cityId)
        {
            uint versionId = 0;

            versionId = _pqCache.GetDefaultPriceQuoteVersion(modelId, cityId);

            return versionId;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 20 July 2015
        /// Summary : To get AreaList if Dealer Prices Available
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="versionId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<Bikewale.Entities.Location.AreaEntityBase> GetAreaList(uint modelId, uint cityId)
        {
            List<Bikewale.Entities.Location.AreaEntityBase> objArea = null;

            objArea = dealerPQRepository.GetAreaList(modelId, cityId);

            return objArea;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 20 July 2015
        /// Summary : To process price Quote
        /// Modified By :   Sumit Kate on 21 Mar 2016
        /// Description :   Consume the newer Subscription Model AB API version
        /// Modified By : Vivek Gupta on 29-04-2016
        /// Desc : In case of dealerId=0 and isDealerAvailable = true , while redirecting to pricequotes ,don't redirect to BW PQ redirect to dpq
        /// Modified By : Sajal Gupta on 13-01-2017
        /// Desc : Removed code for selecting different version if pqid is 0;
        /// Modified by : Ashutosh Sharma on 29 Jun 2018
        /// Description : Fetching manufacturer campaign when Subsciption dealer is not available and isManufacturerCampaignRequired. Using Dealer Id of manufacturer when registering PQ.
        /// </summary>
        /// <param name="PQParams"></param>
        /// <returns></returns>
        public PQOutputEntity ProcessPQ(PriceQuoteParametersEntity PQParams, bool isManufacturerCampaignRequired = false)
        {
            PQOutputEntity objPQOutput = null;
            //uint dealerId = 0;
            ulong quoteId = 0;
            BikeWale.Entities.AutoBiz.DealerInfo objDealerDetail = new BikeWale.Entities.AutoBiz.DealerInfo();
            ManufacturerCampaignEntity campaigns = null;
            try
            {
                if (PQParams.VersionId <= 0)
                {
                    if (PQParams.AreaId > 0)
                        PQParams.VersionId = dealerPQRepository.GetDefaultPriceQuoteVersion(PQParams.ModelId, PQParams.CityId, PQParams.AreaId);
                    else
                        PQParams.VersionId = _pqCache.GetDefaultPriceQuoteVersion(PQParams.ModelId, PQParams.CityId);
                }

                if (PQParams.VersionId > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealer, Bikewale.BAL.AutoBiz.Dealers>();
                        container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, DealerPriceQuoteRepository>();
                        IDealer objDealer = container.Resolve<IDealer>();
                        objDealerDetail = objDealer.GetSubscriptionDealer(
                            PQParams.ModelId,
                            PQParams.CityId,
                            PQParams.AreaId);
                    }
                }
                else
                {
                    objDealerDetail = new BikeWale.Entities.AutoBiz.DealerInfo();
                }
            }
            catch (Exception ex)
            {
                objDealerDetail.DealerId = 0;
                objDealerDetail.IsDealerAvailable = false;
                ErrorClass.LogError(ex, "ProcessPQ ex : " + ex.Message);
            }
            finally
            {
                bool isManufacturerDealer = false;
                if (PQParams.VersionId > 0)
                {
                    if (objDealerDetail != null)
                        PQParams.DealerId = objDealerDetail.DealerId;
                    if(isManufacturerCampaignRequired && PQParams.DealerId == 0 && PQParams.ManufacturerCampaignPageId > 0)
                    {
                        campaigns = _objManufacturerCampaign.GetCampaigns(PQParams.ModelId, PQParams.CityId, PQParams.ManufacturerCampaignPageId);
                        PQParams.DealerId = campaigns != null && campaigns.LeadCampaign != null ? campaigns.LeadCampaign.DealerId : campaigns.EMICampaign != null ? campaigns.EMICampaign.DealerId : 0;
                        isManufacturerDealer = true;
                    }
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                        IPriceQuote objIPQ = container.Resolve<IPriceQuote>();
                        quoteId = objIPQ.RegisterPriceQuote(PQParams);
                    }
                }
                objPQOutput = new PQOutputEntity()
                {
                    DealerId = !isManufacturerDealer ? PQParams.DealerId : 0,
                    PQId = quoteId,
                    VersionId = PQParams.VersionId,
                    IsDealerAvailable = objDealerDetail != null && objDealerDetail.IsDealerAvailable,
                    ManufacturerCampaign = campaigns
                };
            }
            return objPQOutput;
        }   //End of ProcessPQ


        

        /// <summary>
        /// Created by: Sangram Nandkhile on 14 Feb 2017
        /// Summary: Fetch dealer properties for default version, Set priceQuote by specific version Id
        /// Modified by : Ashutosh Sharma on 29 Jun 2018
        /// Description : Fetching manufacturer campaign when Subsciption dealer is not available. Using Dealer Id of manufacturer when registering PQ.
        /// </summary>
        public Bikewale.Entities.BikeBooking.v2.PQOutputEntity ProcessPQV2(Entities.PriceQuote.v2.PriceQuoteParametersEntity PQParams,bool isDealerSubscriptionRequired = false)
        {
            Bikewale.Entities.BikeBooking.v2.PQOutputEntity objPQOutput = null;
            if (PQParams != null)
            {
                string quoteId = string.Empty;
                BikeWale.Entities.AutoBiz.DealerInfo objDealerDetail = new BikeWale.Entities.AutoBiz.DealerInfo();
                try
                {
                    uint defaultVersionId = 0;
                    objDealerDetail = GetDefaultVersionAndSubscriptionDealer(PQParams.ModelId, PQParams.CityId, PQParams.AreaId, PQParams.VersionId, isDealerSubscriptionRequired, out defaultVersionId);
                    PQParams.VersionId = PQParams.VersionId != 0 ? PQParams.VersionId : defaultVersionId;
                }
                catch (Exception ex)
                {
                    objDealerDetail.DealerId = 0;
                    objDealerDetail.IsDealerAvailable = false;
                    ErrorClass.LogError(ex, string.Format("ProcessPQV2 => ModelId {0} VersionId {1}", PQParams.ModelId, PQParams.VersionId));
                }
                finally
                {
                    objPQOutput = RegisterPQAndGetPQ(PQParams, objDealerDetail, true);
                }
            }
            return objPQOutput;
        }   //End of ProcessPQV2

        /// <summary>
        /// Created by  : Pratibha Verma on 19 June 2018
        /// Description : remove PQId dependency
        /// </summary>
        public Bikewale.Entities.BikeBooking.v2.PQOutputEntity ProcessPQV3(Entities.PriceQuote.v2.PriceQuoteParametersEntity PQParams)
        {
            Bikewale.Entities.BikeBooking.v2.PQOutputEntity objPQOutput = null;
            //uint dealerId = 0;
            string quoteId = string.Empty;
            BikeWale.Entities.AutoBiz.DealerInfo objDealerDetail = new BikeWale.Entities.AutoBiz.DealerInfo();
            try
            {
                uint defaultVersionId = 0;
                objDealerDetail = GetDefaultVersionAndSubscriptionDealer(PQParams.ModelId, PQParams.CityId, PQParams.AreaId, PQParams.VersionId, true, out defaultVersionId);
                PQParams.VersionId = PQParams.VersionId != 0 ? PQParams.VersionId : defaultVersionId;
            }
            catch (Exception ex)
            {
                objDealerDetail.DealerId = 0;
                objDealerDetail.IsDealerAvailable = false;
                ErrorClass.LogError(ex, "ProcessPQV3 ex : " + ex.Message);

            }
            finally
            {
                objPQOutput = RegisterPQAndGetPQ(PQParams, objDealerDetail, false);
            }
            return objPQOutput;
        }   //End of ProcessPQ

        /// <summary>
        /// Created by : Ashutosh Sharma on 29 Jun 2018
        /// Description : Method to get dealer subscription and default version based on dealer.
        /// </summary>
        private BikeWale.Entities.AutoBiz.DealerInfo GetDefaultVersionAndSubscriptionDealer(uint modelId, uint cityId, uint areaId, uint versionId, bool isDealerSubscriptionRequired, out uint defaultVersionId)
        {
            BikeWale.Entities.AutoBiz.DealerInfo objDealerDetail = null;
            defaultVersionId = 0;
            if (cityId > 0)
            {
                if (versionId == 0)
                {
                    if (areaId > 0)
                        defaultVersionId = dealerPQRepository.GetDefaultPriceQuoteVersion(modelId, cityId, areaId);
                    else
                        defaultVersionId = _pqCache.GetDefaultPriceQuoteVersion(modelId, cityId);
                }

                if(isDealerSubscriptionRequired)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealer, Bikewale.BAL.AutoBiz.Dealers>();
                        container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, DealerPriceQuoteRepository>();
                        IDealer objDealer = container.Resolve<IDealer>();

                        objDealerDetail = objDealer.GetSubscriptionDealer(modelId, cityId, areaId);
                    }
                }
                else
                {
                    objDealerDetail = new BikeWale.Entities.AutoBiz.DealerInfo();
                }
                
            }

            return objDealerDetail;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 29 Jun 2018
        /// Description : Method to get manufacturer campaign and register pq with dealerid of subscribed dealer OR manufacturer dealer if subscribed dealer is not available.
        /// </summary>
        /// <param name="PQParams"></param>
        /// <param name="objDealerDetail"></param>
        /// <param name="isManufacturerCampaignRequired"></param>
        /// <returns></returns>
        private Entities.BikeBooking.v2.PQOutputEntity RegisterPQAndGetPQ(Entities.PriceQuote.v2.PriceQuoteParametersEntity PQParams, BikeWale.Entities.AutoBiz.DealerInfo objDealerDetail, bool isManufacturerCampaignRequired)
        {
            string quoteId = string.Empty;
            ManufacturerCampaignEntity campaigns = null;
            Entities.BikeBooking.v2.PQOutputEntity objPQOutput;
            bool isManufacturerDealer = false;
            if (PQParams.VersionId > 0 && objDealerDetail != null)
            {
                PQParams.DealerId = objDealerDetail.DealerId;
            }
            if (isManufacturerCampaignRequired && PQParams.DealerId == 0 && PQParams.ManufacturerCampaignPageId > 0)
            {
                campaigns = _objManufacturerCampaign.GetCampaigns(PQParams.ModelId, PQParams.CityId, PQParams.ManufacturerCampaignPageId);
                PQParams.DealerId = campaigns != null && campaigns.LeadCampaign != null ? campaigns.LeadCampaign.DealerId : campaigns.EMICampaign != null ? campaigns.EMICampaign.DealerId : 0;
                isManufacturerDealer = true;
            }

            if (PQParams.VersionId > 0 && PQParams.CityId > 0)
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                    IPriceQuote objIPQ = container.Resolve<IPriceQuote>();
                    quoteId = objIPQ.RegisterPriceQuoteV2(PQParams);
                }
            }
            objPQOutput = new Bikewale.Entities.BikeBooking.v2.PQOutputEntity()
            {
                DealerId = !isManufacturerDealer ? PQParams.DealerId : 0,
                PQId = quoteId,
                VersionId = PQParams.VersionId,
                IsDealerAvailable = (objDealerDetail != null) ? objDealerDetail.IsDealerAvailable : false,
                ManufacturerCampaign = campaigns
            };
            return objPQOutput;
        }

        /// <summary>
        /// Modified By  : Rajan Chauhan on 26 Mar 2018
        /// Description  : Added MinSpec to pageDetail.Varients
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="versionId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public BookingPageDetailsEntity FetchBookingPageDetails(uint cityId, uint versionId, uint dealerId)
        {
            BookingPageDetailsEntity pageDetail = null;
            try
            {
                int iVersionId = (int)versionId;
                pageDetail = dealerPQRepository.FetchBookingPageDetails(cityId, versionId, dealerId);
                if (pageDetail != null && pageDetail.Varients != null && iVersionId > 0)
                {
                    GetVersionSpecsSummaryByItemIdAdapter adapt = new GetVersionSpecsSummaryByItemIdAdapter();
                    VersionsDataByItemIds_Input specItemInput = new VersionsDataByItemIds_Input
                    {
                        Versions = new List<int> { iVersionId },
                        Items = new List<EnumSpecsFeaturesItems> {
                            EnumSpecsFeaturesItems.RearBrakeType,
                            EnumSpecsFeaturesItems.WheelType
                        }
                    };
                    adapt.AddApiGatewayCall(_apiGatewayCaller, specItemInput);
                    _apiGatewayCaller.Call();
                    IEnumerable<VersionMinSpecsEntity> versionMinSpecsEntityList = adapt.Output;
                    if (versionMinSpecsEntityList != null)
                    {
                        BikeDealerPriceDetail objVersion = pageDetail.Varients.FirstOrDefault(varient => varient.MinSpec.VersionId == iVersionId);
                        if (objVersion != null)
                        {
                            VersionMinSpecsEntity objVersionMinSpec = versionMinSpecsEntityList.FirstOrDefault(versionSpecEntity => versionSpecEntity.VersionId.Equals(iVersionId));
                            if (objVersionMinSpec != null)
                            {
                                objVersion.MinSpec.MinSpecsList = objVersionMinSpec.MinSpecsList;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dealerId = 0;
                ErrorClass.LogError(ex, "FetchBookingPageDetails ex : " + ex.Message);

            }
            return pageDetail;
        }


        /// <summary>
        /// Created By : Sushil Kumar on 29th Nov 2016
        /// Description : To update dealer daily limit count   
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="abInquiryId"></param>
        /// <returns></returns>
        public bool UpdateDealerDailyLeadCount(uint campaignId, uint abInquiryId)
        {
            return dealerPQRepository.UpdateDealerDailyLeadCount(campaignId, abInquiryId);
        }

        /// <summary>
        /// Created By : Sushil Kumar on 29th Nov 2016
        /// Description : To check dealer daily limit count exceeds or not for campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public bool IsDealerDailyLeadLimitExceeds(uint campaignId)
        {
            return dealerPQRepository.IsDealerDailyLeadLimitExceeds(campaignId);
        }


        /// <summary>
        /// Created by  :   Sumit Kate on 16 Dec 2016
        /// Description :   Call DAL GetDefaultPriceQuoteVersion
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public uint GetDefaultPriceQuoteVersion(uint modelId, uint cityId, uint areaId)
        {
            return dealerPQRepository.GetDefaultPriceQuoteVersion(modelId, cityId, areaId);
        }
    }   //End of Class
}   //End of namespace