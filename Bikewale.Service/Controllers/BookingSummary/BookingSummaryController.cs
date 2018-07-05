using Bikewale.DTO.BookingSummary;
using Bikewale.DTO.PriceQuote.BikeBooking;
using Bikewale.DTO.PriceQuote.CustomerDetails;
using Bikewale.DTO.PriceQuote.DetailedDealerQuotation;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Bikebooking;
using Bikewale.Service.AutoMappers.BookingSummary;
using Microsoft.Practices.Unity;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.BookingSummary
{
    /// <summary>
    /// Booking Summary Controller
    /// Author  :   Sumit Kate
    /// Created On  :   25 Aug 2015
    /// Modified By : Sushil Kumar on 7th Dec 2015
    /// Description : Added varients with min specs and price
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
            BookingPageDetailsEntity objBookingPageDetailsEntity = null;
            DDQDealerDetailBase dtoQuotation = null;
            PQCustomer dtoCustomer = null;
            BookingSummaryBase bookingSummary = null;
            BookingPageDetailsDTO objBookingPageDetailsDTO = null;

            try
            {

                #region Bike Versions available with dealer

                objBookingPageDetailsEntity = _objDealer.FetchBookingPageDetails(cityId, versionId, dealerId);
                objBookingPageDetailsDTO = BookingPageDetailsEntityMapper.Convert(objBookingPageDetailsEntity);

                #endregion

                #region Customer Details
                objCustomer = _objDealer.GetCustomerDetailsByPQId(pqId);

                if (objCustomer != null)
                {
                    dtoCustomer = PQCustomerMapper.Convert(objCustomer);
                }
                #endregion

                #region Detailed Dealer Quotation

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealers, Bikewale.DAL.AutoBiz.DealersRepository>();
                    Bikewale.Interfaces.AutoBiz.IDealers objDealer = container.Resolve<Bikewale.DAL.AutoBiz.DealersRepository>();
                    PQParameterEntity objParam = new PQParameterEntity();
                    objParam.CityId = cityId;
                    objParam.DealerId = dealerId;
                    objParam.VersionId = versionId;
                    dealerDetailEntity = objDealer.GetDealerDetailsPQ(objParam);
                }

                if (dealerDetailEntity != null)
                {
                    dtoQuotation = DDQDealerDetailBaseMapper.Convert(dealerDetailEntity);

                    uint insuranceAmount = 0;
                    bool isFreeInsurance = false;
                    foreach (var price in dtoQuotation.objQuotation.PriceList)
                    {
                        isFreeInsurance = Bikewale.Utility.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(), "", price.CategoryName, price.Price, ref insuranceAmount);
                        if (isFreeInsurance)
                            break;
                    }

                    dtoQuotation.IsInsuranceFree = isFreeInsurance;
                    dtoQuotation.InsuranceAmount = insuranceAmount;

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

                if (objBookingPageDetailsDTO != null && objBookingPageDetailsDTO.Varients != null)
                {
                    bookingSummary = new BookingSummaryBase();

                    if (dtoCustomer != null && dtoQuotation != null)
                    {
                        bookingSummary.Customer = dtoCustomer;
                        bookingSummary.DealerQuotation = dtoQuotation;
                    }

                    bookingSummary.Varients = objBookingPageDetailsDTO.Varients;
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.BookingSummary.BookingSummaryController.Get");
               
                return InternalServerError();
            }
        }
    }
}
