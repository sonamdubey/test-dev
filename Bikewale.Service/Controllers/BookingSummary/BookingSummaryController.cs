using Bikewale.DTO.BookingSummary;
using Bikewale.DTO.PriceQuote.CustomerDetails;
using Bikewale.DTO.PriceQuote.DetailedDealerQuotation;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entity.BikeBooking;
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
        /// <summary>
        /// Gets the Booking Summary information
        /// </summary>
        /// <param name="pqId">Price Quote Id</param>
        /// <param name="versionId">Bike Version Id</param>
        /// <param name="dealerId">Dealer Id</param>
        /// <param name="cityId">City Id</param>
        /// <returns></returns>
        [ResponseType(typeof(BookingSummaryBase))]
        public HttpResponseMessage Get(uint pqId, uint versionId, uint dealerId, uint cityId)
        {
            PQ_DealerDetailEntity dealerDetailEntity = null;
            PQCustomerDetail objCustomer = null;

            DDQDealerDetailBase dtoQuotation = null;
            PQCustomer dtoCustomer = null;
            BookingSummaryBase bookingSummary = null;
            try
            {
                string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";

                #region Customer Details
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                    objCustomer = objDealer.GetCustomerDetails(pqId);
                }
                if (objCustomer != null)
                {
                    dtoCustomer = BookingSummaryEntityToDTO.ConvertCustomer(objCustomer);
                }
                #endregion

                #region Detailed Dealer Quotation
                string _apiUrl = String.Format("/api/Dealers/GetDealerDetailsPQ/?versionId={0}&DealerId={1}&CityId={2}", versionId, dealerId, cityId);

                dealerDetailEntity = BWHttpClient.GetApiResponseSync<PQ_DealerDetailEntity>(_abHostUrl, _requestType, _apiUrl, dealerDetailEntity);
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                    objCustomer = objDealer.GetCustomerDetails(pqId);
                }

                if (dealerDetailEntity != null)
                {
                    dtoQuotation = BookingSummaryEntityToDTO.ConvertDetailEntity(dealerDetailEntity);
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
                    return Request.CreateResponse(HttpStatusCode.OK, bookingSummary);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Booking summary not found.");
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.BookingSummary.BookingSummaryController.Get");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
    }
}
