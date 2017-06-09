using Bikewale.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bikewale.Models;

namespace Bikewale.Models.Survey
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 09 Jun 2017
    /// Summary: BAL layer for surveys
    /// </summary>
    public class SurveyBajajModel
    {
        private readonly ISurvey _survey;
        private readonly BajajSurveyVM _response;

        public SurveyBajajModel(BajajSurveyVM Response, ISurvey Survey)
        {
            _survey = Survey;
            _response = Response;
        }

        public void SaveBajajResponse()
        {
            FormatOutPut(_response);
            _survey.InsertBajajSurveyResponse(_response);
        }

        private void FormatOutPut(BajajSurveyVM _response)
        {
            if (_response != null)
            {
                if (!string.IsNullOrEmpty(_response.CurrentBike))
                {
                    _response.CurrentBike = "No, I don't ride a two wheeler";
                }
                if (_response.MultipleModel != null && _response.MultipleModel.Count > 0)
                {
                    _response.BikeToPurchase = string.Join(",", _response.MultipleModel);
                }
                else
                {
                    _response.BikeToPurchase = "No, I don't wish to purchase a two wheeler";
                }
                if (_response.AdMedium != null && _response.AdMedium.Count > 0)
                    _response.AllMedium = string.Join(",", _response.AdMedium);
            }
        }
    }
}