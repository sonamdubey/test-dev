using Bikewale.DTO.BookingSummary;
using Bikewale.DTO.PriceQuote.CustomerDetails;
using Bikewale.DTO.PriceQuote.DetailedDealerQuotation;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.BookingSummary;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.BookingSummary
{
    /// <summary>
    /// Booking Summary Controller
    /// Author  :   Sumit Kate
    /// Created On  :   25 Aug 2015
    /// </summary>
    public class BookingSummaryController : ApiController
    {
        private readonly IDealerPriceQuote _objDealer = null;

        public BookingSummaryController(IDealerPriceQuote objDealer)
        {
            _objDealer = objDealer;
        }

        /// <summary>
        /// Gets the Booking Summary information
        /// </summary>
        /// <param name="pqId">Price Quote Id</param>
        /// <param name="versionId">Bike Version Id</param>
        /// <param name="dealerId">Dealer Id</param>
        /// <param name="cityId">City Id</param>
        /// <returns></returns>
        [ResponseType(typeof(BookingSummaryBase))]
        public IHttpActionResult Get(uint pqId, uint versionId, uint dealerId, uint cityId)
        {
            PQ_DealerDetailEntity dealerDetailEntity = null;
            PQCustomerDetail objCustomer = null;

            DDQDealerDetailBase dtoQuotation = null;
            PQCustomer dtoCustomer = null;
            BookingSummaryBase bookingSummary = null;
            try
            {
                #region Customer Details
                objCustomer = _objDealer.GetCustomerDetails(pqId);

                if (objCustomer != null)
                {
                    dtoCustomer = PQCustomerMapper.Convert(objCustomer);                    
                }
                #endregion

                #region Detailed Dealer Quotation
                string _apiUrl = String.Format("/api/Dealers/GetDealerDetailsPQ/?versionId={0}&DealerId={1}&CityId={2}", versionId, dealerId, cityId);

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    //dealerDetailEntity = objClient.GetApiResponseSync<PQ_DealerDetailEntity>(Utility.BWConfiguration.Instance.ABApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, dealerDetailEntity);
                    dealerDetailEntity = objClient.GetApiResponseSync<PQ_DealerDetailEntity>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, dealerDetailEntity);
                }

                if (dealerDetailEntity != null)
                {
                    dtoQuotation = DDQDealerDetailBaseMapper.Convert(dealerDetailEntity);

                    if (dealerDetailEntity.objOffers != null)
                    {
                        dealerDetailEntity.objOffers.Clear();
                        dealerDetailEntity.objOffers = null;
                    }

                    if (dealerDetailEntity.objFacilities != null)
                    {
                        dealerDetailEntity.objFacilities.Clear();
                        dealerDetailEntity.objFacilities = null;
                    }
                }
                #endregion

                if (dtoCustomer != null && dtoQuotation != null)
                {
                    bookingSummary = new BookingSummaryBase();
                    bookingSummary.Customer = dtoCustomer;
                    bookingSummary.DealerQuotation = dtoQuotation;
                }

                if (bookingSummary != null)
                {
                    return Ok(bookingSummary);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.BookingSummary.BookingSummaryController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }        
    }
}
