using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.DAL.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Microsoft.Practices.Unity;
using Bikewale.Entities.Customer;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using System.Configuration;
using Bikewale.Utility;
using Bikewale.Notifications;
using Bikewale.Interfaces.PriceQuote;

namespace Bikewale.BAL.BikeBooking
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 24 Oct 2014
    /// </summary>
    public class DealerPriceQuote : IDealerPriceQuote
    {
        private readonly IDealerPriceQuote dealerPQRepository = null;

        public DealerPriceQuote()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerPriceQuote, Bikewale.DAL.BikeBooking.DealerPriceQuoteRepository>();
                dealerPQRepository = container.Resolve<IDealerPriceQuote>();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 29 Oct 2014
        /// Summary :  to save customer detail in newbikedealerpricequote table
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="pqId"></param>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="customerEmail"></param>
        /// <returns></returns>
        public bool SaveCustomerDetail(uint dealerId, uint pqId, string customerName, string customerMobile, string customerEmail)
        {
            bool isSuccess = false;

            isSuccess = dealerPQRepository.SaveCustomerDetail(dealerId, pqId, customerName, customerMobile, customerEmail);

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
        public bool UpdateAppointmentDate(uint pqId, DateTime date)
        {
            bool isSuccess = false;

            isSuccess = dealerPQRepository.UpdateAppointmentDate(pqId, date);

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
        /// </summary>
        /// <param name="PQParams"></param>
        /// <returns></returns>
        public PQOutputEntity ProcessPQ(PriceQuoteParametersEntity PQParams)
        {
            PQOutputEntity objPQOutput = null;
            uint dealerId = 0;
            ulong quoteId = 0;

            try
            {
                if(PQParams.VersionId <= 0)
                {
                    PQParams.VersionId = dealerPQRepository.GetDefaultPriceQuoteVersion(PQParams.ModelId, PQParams.CityId);                    
                }

                if (PQParams.VersionId > 0 && PQParams.AreaId > 0)
                {
                    string api = "/api/DealerPriceQuote/IsDealerExists/?areaid=" + PQParams.AreaId + "&versionid=" + PQParams.VersionId;

                    using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                    {
                        //dealerId = objClient.GetApiResponseSync<uint>(Utility.BWConfiguration.Instance.ABApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, api, dealerId);
                        dealerId = objClient.GetApiResponseSync<uint>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, api, dealerId);
                    }
                }
            }
            catch(Exception ex)
            {
                dealerId = 0;
                ErrorClass objErr = new ErrorClass(ex, "ProcessPQ ex : " + ex.Message);
                objErr.SendMail();
            }
            finally
            {
                if (PQParams.VersionId > 0)
                {
                    PQParams.DealerId = dealerId;

                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                        IPriceQuote objIPQ = container.Resolve<IPriceQuote>();

                        quoteId = objIPQ.RegisterPriceQuote(PQParams);
                    }
                }

                objPQOutput = new PQOutputEntity() { DealerId = PQParams.DealerId, PQId = quoteId, VersionId = PQParams.VersionId };
            }
            return objPQOutput;
        }   //End of ProcessPQ

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
