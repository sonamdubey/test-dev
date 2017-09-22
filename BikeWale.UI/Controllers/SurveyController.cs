using Bikewale.Interfaces;
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
        private readonly ISurvey _survey;

        public SurveyController(ISurvey Survey)
        {
            _survey = Survey;
        }


      

        // GET: Survey
        [Route("survey/bajaj/")]
        [Bikewale.Filters.DeviceDetection]
        public ActionResult BajajSurvey_Index(bool? isFormSubmitted)
        {
            BajajSurveyVM model = new BajajSurveyVM();
            model.IsSubmitted = isFormSubmitted.HasValue ? isFormSubmitted.Value : false;
            return View(model);
        }

        [HttpPost]
        [Route("survey/bajaj/SubmitReview/")]
        public ActionResult SubmitBajajReview(BajajSurveyVM model)
        {

            SurveyBajajModel objModel = new SurveyBajajModel(model, _survey);
            objModel.SaveBajajResponse();
            model.IsSubmitted = true;
            if (model.Source == "Mobile")
            {
                return RedirectToAction("BajajSurvey_Index_Mobile", new { isFormSubmitted = true });

            }
            else
            {
                return RedirectToAction("BajajSurvey_Index", new { isFormSubmitted = true });
            }
        }

        [Route("m/survey/bajaj/")]
        public ActionResult BajajSurvey_Index_Mobile(bool? isFormSubmitted)
        {
            BajajSurveyVM model = new BajajSurveyVM();
            model.IsSubmitted = isFormSubmitted.HasValue ? isFormSubmitted.Value : false;
            return View(model);
        }
    }
}