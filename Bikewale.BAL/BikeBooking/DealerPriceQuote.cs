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
        public DealerPriceQuote()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<Bikewale.Interfaces.BikeBooking.IDealerPriceQuote, Bikewale.DAL.BikeBooking.DealerPriceQuoteRepository>();
                container.RegisterType<ICacheManager, MemcacheManager>();
                container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                container.RegisterType<IPriceQuoteCache, PriceQuoteCache>();
                container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, Bikewale.DAL.AutoBiz.DealerPriceQuoteRepository>();
                dealerPQRepository = container.Resolve<Bikewale.Interfaces.BikeBooking.IDealerPriceQuote>();
                container.RegisterType<IApiGatewayCaller, ApiGatewayCaller>();
                _pqCache = container.Resolve<IPriceQuoteCache>();
                _apiGatewayCaller = container.Resolve<IApiGatewayCaller>();
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
        public bool SaveCustomerDetail(DPQ_SaveEntity entity)
        {
            bool isSuccess = false;
            isSuccess = dealerPQRepository.SaveCustomerDetail(entity);
            return isSuccess;
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
        public PQCustomerDetail GetCustomerDetails(uint pqId)
        {
            PQCustomerDetail objCustomer = null;
            objCustomer = dealerPQRepository.GetCustomerDetails(pqId);
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
        /// </summary>
        /// <param name="PQParams"></param>
        /// <returns></returns>
        public PQOutputEntity ProcessPQ(PriceQuoteParametersEntity PQParams)
        {
            PQOutputEntity objPQOutput = null;
            //uint dealerId = 0;
            ulong quoteId = 0;
            BikeWale.Entities.AutoBiz.DealerInfo objDealerDetail = new BikeWale.Entities.AutoBiz.DealerInfo();
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
                if (PQParams.VersionId > 0)
                {
                    if (objDealerDetail != null)
                        PQParams.DealerId = objDealerDetail.DealerId;
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                        IPriceQuote objIPQ = container.Resolve<IPriceQuote>();
                        quoteId = objIPQ.RegisterPriceQuote(PQParams);
                    }
                }
                objPQOutput = new PQOutputEntity() { DealerId = PQParams.DealerId, PQId = quoteId, VersionId = PQParams.VersionId, IsDealerAvailable = (objDealerDetail != null) ? objDealerDetail.IsDealerAvailable : false };
            }
            return objPQOutput;
        }   //End of ProcessPQ
        /// <summary>
        /// Created by: Sangram Nandkhile on 14 Feb 2017
        /// Summary: Fetch dealer properties for default version, Set priceQuote by specific version Id
        /// </summary>
        /// <param name="PQParams"></param>
        /// <returns></returns>
        public PQOutputEntity ProcessPQV2(PriceQuoteParametersEntity PQParams)
        {

            PQOutputEntity objPQOutput = null;
            if (PQParams != null)
            {
                uint defaultVersionId = 0;
                bool isVersionPresent = PQParams.VersionId > 0 ? true : false;
                ulong quoteId = 0;
                BikeWale.Entities.AutoBiz.DealerInfo objDealerDetail = new BikeWale.Entities.AutoBiz.DealerInfo();
                try
                {
                    if (PQParams.AreaId > 0)
                        defaultVersionId = dealerPQRepository.GetDefaultPriceQuoteVersion(PQParams.ModelId, PQParams.CityId, PQParams.AreaId);
                    else
                        defaultVersionId = _pqCache.GetDefaultPriceQuoteVersion(PQParams.ModelId, PQParams.CityId);

                    if (PQParams.CityId > 0)
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
                    ErrorClass.LogError(ex, string.Format("ProcessPQV2 => ModelId {0} VersionId {1}", PQParams.ModelId, PQParams.VersionId));
                }
                finally
                {
                    if (PQParams.VersionId == 0)
                    {
                        PQParams.VersionId = defaultVersionId;
                    }
                    if (PQParams.VersionId > 0)
                    {
                        if (objDealerDetail != null)
                            PQParams.DealerId = objDealerDetail.DealerId;
                        using (IUnityContainer container = new UnityContainer())
                        {
                            container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                            IPriceQuote objIPQ = container.Resolve<IPriceQuote>();
                            quoteId = objIPQ.RegisterPriceQuote(PQParams);
                        }
                    }
                    objPQOutput = new PQOutputEntity()
                    {
                        DealerId = PQParams.DealerId,
                        PQId = quoteId,
                        VersionId = PQParams.VersionId,
                        DefaultVersionId = defaultVersionId,
                        IsDealerAvailable = (objDealerDetail != null) ? objDealerDetail.IsDealerAvailable : false
                    };
                }
            }
            return objPQOutput;
        }   //End of ProcessPQV2

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
                pageDetail = dealerPQRepository.FetchBookingPageDetails(cityId, versionId, dealerId);
                if (pageDetail != null && pageDetail.Varients != null)
                {
                    GetVersionSpecsByItemIdAdapter adapt1 = new GetVersionSpecsByItemIdAdapter();
                    VersionsDataByItemIds_Input specItemInput = new VersionsDataByItemIds_Input {
                        Versions = new List<int> { (int)versionId },
                        Items = new List<EnumSpecsFeaturesItems> {
                            EnumSpecsFeaturesItems.BrakeType,
                            EnumSpecsFeaturesItems.AlloyWheels
                        }
                    };
                    adapt1.AddApiGatewayCall(_apiGatewayCaller, specItemInput);
                    _apiGatewayCaller.Call();
                    IEnumerable<VersionMinSpecsEntity> versionMinSpecsEntityList = adapt1.Output;
                    if (versionMinSpecsEntityList != null)
                    {
                        VersionMinSpecsEntity objVersionMinSpec = null;
                        foreach (BikeDealerPriceDetail objVersion in pageDetail.Varients)
                        {
                            if (objVersion.MinSpec.VersionId == versionId)
                            {
                                objVersionMinSpec = versionMinSpecsEntityList.FirstOrDefault(versionSpecEntity => versionSpecEntity.VersionId.Equals(objVersion.MinSpec.VersionId));
                                if (objVersionMinSpec != null)
                                {
                                    objVersion.MinSpec.MinSpecsList = objVersionMinSpec.MinSpecsList;
                                }
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