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
        public ActionResult BajajSurvey_Index()
        {
            BajajSurveyVM model = new BajajSurveyVM();
            return View(model);
        }

        [HttpPost]
        [Route("survey/bajaj/SubmitReview/")]
        public ActionResult SubmitBajajReview(BajajSurveyVM model)
        {
            model.IsSubmitted = true;
            SurveyBajajModel objModel = new SurveyBajajModel(model, _survey);
            objModel.GetData();

            return View("~/views/Survey/BajajSurvey_Index.cshtml", model);
        }

        [Route("m/survey/bajaj/")]
        public ActionResult BajajSurvey_Index_Mobile()
        {
            BajajSurveyVM model = new BajajSurveyVM();
            return View(model);
        }
    }
}