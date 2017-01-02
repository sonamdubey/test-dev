using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bikewale.Notifications;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.Interface.BikeData;
using Microsoft.Practices.Unity;

namespace BikewaleOpr.Service.Controllers.Content
{    
    public class MakesController : ApiController
    {
        [Route("api/makes/{makeid}/synopsis/"), HttpGet]
        //[HttpGet]
        public IHttpActionResult GetSynopsis(int makeId)
        {
            if (makeId > 0)
            {
                string synopsis = string.Empty;

                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMakes, BikeMakesRepository>();
                        IBikeMakes objRepo = container.Resolve<IBikeMakes>();

                        synopsis = objRepo.Getsynopsis(makeId);
                    }
                }
                catch (Exception ex)
                {                    
                    ErrorClass objErr = new ErrorClass(ex, "GetSynopsis");
                    objErr.SendMail();
                }

                if (!String.IsNullOrEmpty(synopsis))
                    return Ok(synopsis);
                else
                    return NotFound();
            }
            else
                return BadRequest();        
        }
    }
}
