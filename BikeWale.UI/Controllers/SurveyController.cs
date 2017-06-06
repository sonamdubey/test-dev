using Bikewale.Models.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class SurveyController : Controller
    {
        // GET: Survey
        [Route("survey/bajaj/")]
        public ActionResult BajajSurvey_Index()
        {
            BajajSurveyModel model = new BajajSurveyModel();
            return View(model);
        }

        [HttpPost]
        [Route("survey/bajaj/SubmitReview/")]
        public ActionResult SubmitBajajReview(BajajSurveyModel model)
        {
            return View();
        }
    }
}