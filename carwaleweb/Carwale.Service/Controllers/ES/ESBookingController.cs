using AutoMapper;
using Carwale.DTOs.ES;
using Carwale.Entity.ES;
using Carwale.Interfaces.ES;
using Carwale.Notifications.Logs;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Carwale.Service.Controllers.ES
{
    public class ESBookingController : ApiController
    {
        private readonly IBookingCache _bookingCache;
        private readonly IBookingRepository _bookingRepo;
        public ESBookingController(IBookingCache bookingCache, IBookingRepository bookingRepo)
        {
            _bookingCache = bookingCache;
            _bookingRepo = bookingRepo;
        }

        [HttpGet, Route("api/booking/volvo/")]
        public IHttpActionResult GetModelColorsData(int modelId)
        {
            try
            {
                var modelVersionColors = _bookingCache.GetBookingModelData(modelId);
                var finalResponse = Mapper.Map<List<ESVersionColors>, List<ESVersionColorsDto>>(modelVersionColors);
                return Ok(finalResponse);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }            
        }

        [HttpGet, Route("api/check/availability/")]
        public IHttpActionResult GetCarAvailability(int versionId, int extColorId, int intColorId, bool isGetCarCount)
        {
            try
            {
                var carCount = _bookingRepo.GetSetCarCount(versionId, extColorId, intColorId, isGetCarCount);
                return Ok(carCount);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [HttpPost, Route("api/escustomer/")]
        public IHttpActionResult SaveCustomerData([FromBody] ESSurveyCustomerResponse customerResponse)
        {
            try
            {
                return Ok(_bookingRepo.SubmitEsCustomerData(customerResponse));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }
    }
}
