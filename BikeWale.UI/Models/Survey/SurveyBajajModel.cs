using Bikewale.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models.Survey
{
    public class SurveyBajajModel
    {
        private readonly ISurvey _survey;
        private readonly BajajSurveyVM _response;

        public SurveyBajajModel(BajajSurveyVM Response, ISurvey Survey)
        {
            _survey = Survey;
            _response = Response;
        }

        public void GetData()
        {
            _survey.InsertBajajSurveyResponse(_response);
        }
    }
}