using Bikewale.DTO.PriceQuote;
using Bikewale.DTO.PriceQuote.BikeQuotation;
using Bikewale.DTO.PriceQuote.DealerPriceQuote;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote;
using Bikewale.Service.TCAPI;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.LeadsGeneration
{
    /// <summary>
    /// Craeted By  : Sushil Kumar
    /// Created On  : 23rd October 2015
    /// Summary     : To generate manufacturer Lead against the bikewale pricequote
    /// </summary>
    public class ManufacturerLeadController : ApiController
    {
        private readonly IDealerPriceQuote _objIPQ = null;
        private readonly IDealer _objDealer = null;
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="objIPQ"></param>
        /// <param name="dealer"></param>
        public ManufacturerLeadController(IDealerPriceQuote objIPQ, IDealer dealer)
        {
            _objIPQ = objIPQ;
            _objDealer = dealer;
        }

        /// <summary>
        /// To genearate manufacturer Lead
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="mobile"></param>
        /// <param name="pqId"></param>
        /// <param name="cityId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        [ResponseType(typeof(bool))]
        public IHttpActionResult Get(uint cityId, uint versionId, string name, string email, string mobile, string pqId)
        {
            string abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
            ManufacturerLeadEntity objLead = null;
            bool status = false;

            try
            {

                if (cityId > 0 && versionId > 0 && !String.IsNullOrEmpty(pqId) && !String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(mobile) )
                {
                    objLead = new ManufacturerLeadEntity();
                    objLead.Name = name;
                    objLead.Mobile = mobile;
                    objLead.Email = email;
                    objLead.CityId = cityId;
                    objLead.VersionId = versionId;
                    objLead.PQId = Convert.ToUInt32(pqId);

                    //dealer Id for TVS manufacturer 
                    objLead.DealerId = Convert.ToUInt32(ConfigurationManager.AppSettings["TVSManufacturerId"]);

                    if (objLead != null && objLead.PQId > 0 && objLead.DealerId > 0)
                    {
                        //save manufacturer lead
                        status = _objDealer.SaveManufacturerLead(objLead);

                        if (status)
                        {
                            //Push inquiry Id to AutoBiz
                            if (_objIPQ.SaveCustomerDetail(objLead.DealerId, objLead.PQId, objLead.Name, objLead.Mobile, objLead.Email,null))
                            {
                                status = AutoBizAdaptor.PushInquiryInAB(Convert.ToString(objLead.DealerId), objLead.PQId, objLead.Name, objLead.Mobile, objLead.Email, Convert.ToString(objLead.VersionId), Convert.ToString(objLead.CityId));
                            }
                        }
                    }                    
                }
                return Ok(status); 
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.PriceQuoteController.Post");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
