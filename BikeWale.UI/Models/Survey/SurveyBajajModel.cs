using Bikewale.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bikewale.Models;

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
            FormatOutPut(_response);
            _survey.InsertBajajSurveyResponse(_response);
        }

        private void FormatOutPut(BajajSurveyVM _response)
        {
            if (_response != null)
            {
                if (_response.MultipleModel != null && _response.MultipleModel.Count > 0)
                    _response.BikeToPurchase = string.Join(",", _response.MultipleModel);
            }
        }
    }
}