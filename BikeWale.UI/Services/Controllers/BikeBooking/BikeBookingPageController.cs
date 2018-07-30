using Bikewale.DTO.PriceQuote.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Bikebooking;
using Bikewale.Service.Utilities;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.BikeBooking
{
    /// <summary>
    /// Bike Booking Page Controller
    /// Author  :   Sumit Kate
    /// Created On  :   10 Sept 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class BikeBookingPageController : CompressionApiController//ApiController
    {
        private readonly IDealerPriceQuote _objDealerPriceQuote = null;
        private readonly IMobileVerificationRepository _mobileVerRespo = null;
        /// <summary>
        /// Constructor
        /// </summary>
        public BikeBookingPageController(IDealerPriceQuote objDealerPriceQuote, IMobileVerificationRepository mobileVerRespo)
        {
            _objDealerPriceQuote = objDealerPriceQuote;
            _mobileVerRespo = mobileVerRespo;
        }

        /// <summary>
        /// Gets the dealer price quote details of bike version
        /// </summary>
        /// <param name="cityId">city id</param>
        /// <param name="versionId">version id</param>
        /// <param name="dealerId">dealer id</param>
        /// <returns></returns>
        [ResponseType(typeof(BookingPageOutput))]
        public IHttpActionResult Get(uint cityId, uint versionId, uint dealerId)
        {
            BookingPageDetailsEntity objBookingPageDetailsEntity = null;
            BookingPageDetailsDTO objBookingPageDetailsDTO = null;
            BookingPageOutput objBookingPageOutput = null;
            try
            {
                objBookingPageDetailsEntity = _objDealerPriceQuote.FetchBookingPageDetails(cityId, versionId, dealerId);
                if (objBookingPageDetailsEntity != null)
                {
                    objBookingPageDetailsDTO = BookingPageDetailsEntityMapper.Convert(objBookingPageDetailsEntity);

                    if (objBookingPageDetailsEntity != null)
                    {
                        if (objBookingPageDetailsEntity.Offers != null)
                        {
                            objBookingPageDetailsEntity.Offers.Clear();
                            objBookingPageDetailsEntity.Offers = null;
                        }


                        if (objBookingPageDetailsEntity.Varients != null)
                        {
                            objBookingPageDetailsEntity.Varients.Clear();
                            objBookingPageDetailsEntity.Varients = null;
                        }

                        if (objBookingPageDetailsEntity.Disclaimers != null)
                        {
                            objBookingPageDetailsEntity.Disclaimers.Clear();
                            objBookingPageDetailsEntity.Disclaimers = null;
                        }

                    }

                    objBookingPageOutput = new BookingPageOutput();
                    objBookingPageOutput.Disclaimers = objBookingPageDetailsDTO.Disclaimers;
                    objBookingPageOutput.Offers = objBookingPageDetailsDTO.Offers;
                    objBookingPageOutput.Varients = objBookingPageDetailsDTO.Varients;
                    uint insuranceAmount = 0;
                    foreach (var varient in objBookingPageOutput.Varients)
                    {
                        foreach (var price in varient.PriceList)
                        {
                            if (Bikewale.Utility.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(), "", price.ItemName, Convert.ToUInt32(price.Price), ref insuranceAmount))
                            {
                                price.Price = -price.Price;
                                break;
                            }
                        }
                        if (insuranceAmount > 0)
                        {
                            varient.OnRoadPrice = varient.OnRoadPrice - insuranceAmount;
                        }
                    }
                    return Ok(objBookingPageOutput);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.BikeBookingPageController.Get");
               
                return InternalServerError();
            }
        }
    }
}
