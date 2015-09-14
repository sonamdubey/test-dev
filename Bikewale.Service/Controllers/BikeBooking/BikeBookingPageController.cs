﻿using Bikewale.DTO.PriceQuote.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Bikebooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.BikeBooking
{
    /// <summary>
    /// Bike Booking Page Controller
    /// Author  :   Sumit Kate
    /// Created On  :   10 Sept 2015
    /// </summary>
    public class BikeBookingPageController : ApiController
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
                    objBookingPageOutput = new BookingPageOutput();
                    objBookingPageOutput.BikeModelColors = objBookingPageDetailsDTO.BikeModelColors;
                    objBookingPageOutput.Disclaimers = objBookingPageDetailsDTO.Disclaimers;
                    objBookingPageOutput.Offers = objBookingPageDetailsDTO.Offers;
                    objBookingPageOutput.Varients = objBookingPageDetailsDTO.Varients;
                    return Ok(objBookingPageOutput);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.BikeBookingPageController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
