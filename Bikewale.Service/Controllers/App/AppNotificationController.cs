using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.App
{
    public class AppNotificationController : ApiController
    {

        /// <summary>
        ///  To get make Details based on MakeId  for DropDown
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns>Make Details </returns>
        [ResponseType(typeof(int))]
        public IHttpActionResult Get(string makeId)
        {
            BikeMakeEntityBase objMake = null;
            MakeBase objDTOMakeBase = null;
            try
            {
                objMake = _bikeMakes.GetMakeDetails(makeId);

                if (objMake != null)
                {
                    objDTOMakeBase = new MakeBase();
                    objDTOMakeBase = MakeListMapper.Convert(objMake);
                    return Ok(objDTOMakeBase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.App.AppNotificationController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }//get make details
        
    }
}
