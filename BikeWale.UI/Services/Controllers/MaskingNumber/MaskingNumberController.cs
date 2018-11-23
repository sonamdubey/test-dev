using Bikewale.Common;
using Bikewale.Service.Utilities;
using System;
using System.Web.Http;
using System.Web.Http.Description;
using Bikewale.DTO.MaskingNumber;
using Bikewale.Entities.MaskingNumber;
using AutoMapper;
using Bikewale.Interfaces.Lead;
using log4net;
using Bikewale.Services.AutoMappers.MaskingNumber;

namespace Bikewale.Services.Controllers.MaskingNumber
{
    public class MaskingNumberController : CompressionApiController
    {
        private readonly ILead _objLead = null;
        static ILog _logger = LogManager.GetLogger("InvalidMaskingLeadLogger");

        public MaskingNumberController(ILead objLead)
        {
            _objLead = objLead;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 15 November 2018
        /// Description : api to submit masking number lead
        /// Modifier    : Kartin on 19 nov 2018
        /// Desc        : added logger
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ResponseType(typeof(uint)), Route("api/savemaskingnumberlead/"), HttpPost]
        public IHttpActionResult Post([FromBody]MaskingNumberLeadInputDto input)
        {
            try
            {
                if (input != null && !string.IsNullOrEmpty(input.CustomerMobile) && input.ModelId > 0 && input.VersionId > 0 && input.DealerId > 0)
                {
                    if(input.CampaignId == 0)
                    {
                        _logger.Error(string.Format("CampaignId is 0 for masking lead dealerid is {0}. lead data - {1}", input.DealerId, Newtonsoft.Json.JsonConvert.SerializeObject(input)), null);
                    }
                    MaskingNumberLeadEntity objLead = MaskingNumberMapper.Convert(input);
                    uint leadId = _objLead.ProcessMaskingLead(objLead);
                    return Ok(leadId);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Services.Controllers.MaskingNumber.Post({0})", Newtonsoft.Json.JsonConvert.SerializeObject(input)));
                return InternalServerError();
            }
        }
    }
}