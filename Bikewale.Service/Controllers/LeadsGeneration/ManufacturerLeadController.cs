using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Service.TCAPI;
using Bikewale.Service.Utilities;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.LeadsGeneration
{
    /// <summary>
    /// Craeted By  : Sushil Kumar
    /// Created On  : 23rd October 2015
    /// Summary     : To generate manufacturer Lead against the bikewale pricequote
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class ManufacturerLeadController : CompressionApiController//ApiController
    {
        private readonly IDealerPriceQuote _objIPQ = null;
        private readonly IDealer _objDealer = null;
        private readonly ILeadNofitication _objLead = null;
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="objIPQ"></param>
        /// <param name="dealer"></param>
        public ManufacturerLeadController(IDealerPriceQuote objIPQ, IDealer dealer, ILeadNofitication objLead)
        {
            _objIPQ = objIPQ;
            _objDealer = dealer;
            _objLead = objLead;
        }


        /// <summary>
        /// To genearate manufacturer Lead
        /// Modified By : Sadhana Upadhyay on 29 Dec 2015
        /// Summary : added utmz, utma, device Id etc
        /// Modified by :   Sumit Kate on 18 Aug 2016
        /// Description :   rearranged the data validation checks and return BadRequest if data is invalid
        /// </summary>
        /// <param name="objLead"></param>
        /// <returns></returns>
        /////uint cityId, uint versionId, string name, string email, string mobile, string pqId, UInt16? platformId, UInt16? leadSourceId, string deviceId = null
        [ResponseType(typeof(bool))]
        public IHttpActionResult Post([FromBody]ManufacturerLeadEntity objLead)
        {
            bool status = false;

            try
            {
                if (objLead != null)
                {
                    if (objLead.CityId > 0 && objLead.VersionId > 0 && objLead.PQId > 0 && !String.IsNullOrEmpty(objLead.Name) && !String.IsNullOrEmpty(objLead.Email) && !String.IsNullOrEmpty(objLead.Mobile) && objLead.DealerId > 0)
                    {
                        //save manufacturer lead
                        status = _objDealer.SaveManufacturerLead(objLead);

                        if (status)
                        {
                            //Push inquiry Id to AutoBiz
                            DPQ_SaveEntity entity = new DPQ_SaveEntity()
                            {
                                DealerId = objLead.DealerId,
                                PQId = objLead.PQId,
                                CustomerName = objLead.Name,
                                CustomerEmail = objLead.Email,
                                CustomerMobile = objLead.Mobile,
                                ColorId = null,
                                UTMA = Request.Headers.Contains("utma") ? Request.Headers.GetValues("utma").FirstOrDefault() : String.Empty,
                                UTMZ = Request.Headers.Contains("utmz") ? Request.Headers.GetValues("utmz").FirstOrDefault() : String.Empty,
                                DeviceId = objLead.DeviceId,
                                LeadSourceId = objLead.LeadSourceId
                            };
                            if (_objIPQ.SaveCustomerDetail(entity))
                            {
                                status = AutoBizAdaptor.PushInquiryInAB(Convert.ToString(objLead.DealerId), objLead.PQId, objLead.Name, objLead.Mobile, objLead.Email, Convert.ToString(objLead.VersionId), Convert.ToString(objLead.CityId));
                                _objLead.PushLeadToGaadi(objLead);
                            }
                        }
                        return Ok(status);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.ManufacturerLeadController.Post : " + Newtonsoft.Json.JsonConvert.SerializeObject(objLead));
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}