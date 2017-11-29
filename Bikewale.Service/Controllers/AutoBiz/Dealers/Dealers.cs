using Bikewale.DAL.AutoBiz;
using Bikewale.Interfaces.AutoBiz;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Data;
using System.Web;
using System.Web.Http;

namespace Bikewale.Service.Controllers.AutoBiz
{
    /// <summary>
    /// Created By : Ashwini Todkar on  28th Oct 2014
    /// </summary>
    public class DealersController : ApiController
    {
        /// <summary>
        /// Created By : Suresh Prajapati on 29th Oct 2014
        /// Summary : To Get Dealer Cities for which Bike Dealer exists
        /// </summary>
        /// <returns>Dealer's Cities</returns>

        [HttpGet]
        public IHttpActionResult GetDealerCities()
        {
            DataTable objCities = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objCity = container.Resolve<DealersRepository>();
                    objCities = objCity.GetDealerCities();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealerCities ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
               
            }
            if (objCities != null)
                return Ok(objCities);
            else
                return NotFound();
        }
    }
}