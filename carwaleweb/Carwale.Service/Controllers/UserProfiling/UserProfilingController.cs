using Carwale.Interfaces.UserProfiling;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Carwale.Service.Controllers.UserProfiling
{
    public class UserProfilingController: ApiController
    {
        private readonly IUserProfilingBL _userProf;

        public UserProfilingController(IUserProfilingBL userProf)
        {
            _userProf = userProf;
        }
        [HttpGet, Route("api/userprofiles/models/")]
        public IHttpActionResult GetModelVersionSpecs()
        {
            try
            {
                var modelVersionData = _userProf.GetVersionDetails();
                return Ok(modelVersionData);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "UserProfilingController.GetModelVersionSpecs()");
                objErr.LogException();
            }
            return InternalServerError();
        }
    }
}
