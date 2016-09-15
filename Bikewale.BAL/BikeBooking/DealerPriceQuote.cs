﻿using Bikewale.DAL.AutoBiz;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.AutoBiz;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Bikewale.BAL.BikeBooking
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 24 Oct 2014
    /// </summary>
    public class DealerPriceQuote : Bikewale.Interfaces.BikeBooking.IDealerPriceQuote
    {
        private readonly Bikewale.Interfaces.BikeBooking.IDealerPriceQuote dealerPQRepository = null;

        public DealerPriceQuote()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<Bikewale.Interfaces.BikeBooking.IDealerPriceQuote, Bikewale.DAL.BikeBooking.DealerPriceQuoteRepository>();
                dealerPQRepository = container.Resolve<Bikewale.Interfaces.BikeBooking.IDealerPriceQuote>();
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
        /// Created By : Sadhana Upadhyay on 10 Dec 2014
        /// Summary : To get customer selected bike color by pqid
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        //public VersionColor GetPQBikeColor(uint pqId)
        //{
        //    VersionColor objColor = null;

        //    objColor = dealerPQRepository.GetPQBikeColor(pqId);

        //    return objColor;
        //}   //End of GetPQBikeColor

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

            versionId = dealerPQRepository.GetDefaultPriceQuoteVersion(modelId, cityId);

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
                    PQParams.VersionId = dealerPQRepository.GetDefaultPriceQuoteVersion(PQParams.ModelId, PQParams.CityId);
                }

                if (PQParams.VersionId > 0 && PQParams.AreaId > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealer, Bikewale.BAL.AutoBiz.Dealers>();
                        container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, DealerPriceQuoteRepository>();
                        IDealer objDealer = container.Resolve<IDealer>();
                        objDealerDetail = objDealer.IsSubscribedDealerExistsV3(PQParams.VersionId, PQParams.AreaId);
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
                ErrorClass objErr = new ErrorClass(ex, "ProcessPQ ex : " + ex.Message);
                objErr.SendMail();
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
                    //Fails to register for requested version get the default version price quote
                    if (quoteId == 0)
                    {
                        PQParams.VersionId = dealerPQRepository.GetDefaultPriceQuoteVersion(PQParams.ModelId, PQParams.CityId);
                        using (IUnityContainer container = new UnityContainer())
                        {
                            container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                            IPriceQuote objIPQ = container.Resolve<IPriceQuote>();
                            quoteId = objIPQ.RegisterPriceQuote(PQParams);
                        }
                    }
                }
                objPQOutput = new PQOutputEntity() { DealerId = PQParams.DealerId, PQId = quoteId, VersionId = PQParams.VersionId, IsDealerAvailable = (objDealerDetail != null) ? objDealerDetail.IsDealerAvailable : false };
            }
            return objPQOutput;
        }   //End of ProcessPQ

        /// <summary>
        /// Created By : Lucky Rathore
        /// Description : To get dealer ID if primary dealer exist for mention Input.
        /// Modified By  : Sushil Kumar on 8th August 2016
        /// Description : Changed paramters order for IsSubscribedDealerExistsV3(versionId, areaId)
        /// Modified By  : Sushil Kumar on 9th August 2016
        /// Description : Added null checks for objDealerDetail
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public DealerInfo IsDealerExists(uint versionId, uint areaId)
        {
            DealerInfo objDealerDetail = null;
            BikeWale.Entities.AutoBiz.DealerInfo objDealerInfo = null;
            try
            {
                objDealerDetail = new DealerInfo();
                if (versionId > 0 && areaId > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealer, Bikewale.BAL.AutoBiz.Dealers>();
                        container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, DealerPriceQuoteRepository>();
                        IDealer objDealer = container.Resolve<IDealer>();
                        objDealerInfo = objDealer.IsSubscribedDealerExistsV3(versionId, areaId);

                        if (objDealerInfo != null)
                        {
                            objDealerDetail.DealerId = objDealerInfo.DealerId;
                            objDealerDetail.IsDealerAvailable = objDealerInfo.IsDealerAvailable;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "ProcessPQ ex : " + ex.Message);
                objErr.SendMail();
            }
            return objDealerDetail;
        }

        public BookingPageDetailsEntity FetchBookingPageDetails(uint cityId, uint versionId, uint dealerId)
        {
            BookingPageDetailsEntity pageDetail = null;
            try
            {
                pageDetail = dealerPQRepository.FetchBookingPageDetails(cityId, versionId, dealerId);
            }
            catch (Exception ex)
            {
                dealerId = 0;
                ErrorClass objErr = new ErrorClass(ex, "FetchBookingPageDetails ex : " + ex.Message);
                objErr.SendMail();
            }
            return pageDetail;
        }

    }   //End of Class
}   //End of namespace