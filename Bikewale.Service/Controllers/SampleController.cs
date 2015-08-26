using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers
{
    /// <summary>
    /// This is a sample controller
    /// </summary>
    public class SampleController : ApiController
    {
        /// <summary>
        /// This is GET for Sample Controller
        /// </summary>
        /// <returns>List of Strings</returns>
        [ResponseType(typeof(IEnumerable<string>))]
        public IHttpActionResult Get()
        {
            List<string> lstSample = new List<string>();
            lstSample.Add("Sample 1");
            lstSample.Add("Sample 2");
            lstSample.Add("Sample 3");
            return Ok(lstSample);

        }
    }
}
