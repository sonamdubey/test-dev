using Carwale.Interfaces.Prices;
using Carwale.Notifications.Logs;
using System;
using System.Web.Http;
using Carwale.Utility;


namespace Carwale.Service.Controllers.PriceQuote
{
    public class EmiCalculatorController : ApiController
    {
        private readonly IEmiCalculatorAdapter _emiCalculatorAdapter;

        public EmiCalculatorController(IEmiCalculatorAdapter emiCalculatorAdapter)
        {
            _emiCalculatorAdapter = emiCalculatorAdapter;
        }

        [HttpGet]
        [Route("api/prices/emicalculator/")]
        public IHttpActionResult GetEmiCalculatorData(int versionId, int cityId)
        {
            try
            {
                if (versionId < 1 || cityId < 1)
                {
                    return BadRequest();
                }

                var emiCalculator = _emiCalculatorAdapter.GetEmiCalculatorData(versionId, cityId);
                if (emiCalculator == null || emiCalculator.EmiWidgetCommonData == null || !emiCalculator.EmiCalculatorData.IsNotNullOrEmpty())
                {
                    System.Net.Http.HttpResponseMessage responseMsg = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.NoContent);
                    return ResponseMessage(responseMsg);
                }
                return Ok(emiCalculator);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }
    }
}
