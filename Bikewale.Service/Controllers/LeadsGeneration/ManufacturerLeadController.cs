using Bikewale.Entities.Customer;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Lead;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Specialized;
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
        private readonly IMobileVerificationCache _mobileVerCacheRepo = null;
        private readonly IManufacturerCampaignRepository _manufacturerCampaignRepo = null;
        private readonly ICustomerAuthentication<CustomerEntity, UInt32> _objAuthCustomer = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        private readonly ILead _leadProcess;
        /// <summary>
        /// Type Initializer
        /// </summary>
        /// <param name="objIPQ"></param>
        /// <param name="mobileVerCacheRepo"></param>
        /// <param name="manufacturerCampaignRepo"></param>
        public ManufacturerLeadController(IDealerPriceQuote objIPQ, IMobileVerificationCache mobileVerCacheRepo, IManufacturerCampaignRepository manufacturerCampaignRepo, ICustomerAuthentication<CustomerEntity, UInt32> objAuthCustomer, ICustomer<CustomerEntity, UInt32> objCustomer, ILead leadProcess)
        {
            _objIPQ = objIPQ;
            _mobileVerCacheRepo = mobileVerCacheRepo;
            _manufacturerCampaignRepo = manufacturerCampaignRepo;
            _objAuthCustomer = objAuthCustomer;
            _objCustomer = objCustomer;
            _leadProcess = leadProcess;
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
            if (objLead != null)
            {
                NameValueCollection headers = new NameValueCollection();
                headers["UTMA"] = Request.Headers.Contains("utma") ? Request.Headers.GetValues("utma").FirstOrDefault() : String.Empty;
                headers["UTMZ"] = Request.Headers.Contains("utmz") ? Request.Headers.GetValues("utmz").FirstOrDefault() : String.Empty;
                //headers["PlatformId"] = Request.Headers.Contains("platformid") ? Request.Headers.GetValues("platformid").FirstOrDefault() : String.Empty;

                uint leadId = _leadProcess.ProcessESLead(objLead, headers);

                if (leadId > 0)
                {
                    return Ok(leadId);
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
    }
}