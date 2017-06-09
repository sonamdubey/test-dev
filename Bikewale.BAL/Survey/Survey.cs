using Bikewale.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Models.Survey;
using Bikewale.Notifications;

namespace Bikewale.BAL
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 06 Jun  2017
    /// Summary: BAL layer for surveys
    /// </summary>
    public class Survey : ISurvey
    {
        public readonly ISurveyRepository _Isurvey;

        public Survey(ISurveyRepository ISurvey)
        {
            _Isurvey = ISurvey;
        }

        public void InsertBajajSurveyResponse(BajajSurveyVM surveryResponse)
        {
            try
            {
                _Isurvey.InsertBajajSurveyResponse(surveryResponse);
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "Survey.InsertBajajSurveyResponse()");
            }
        }
    }
}