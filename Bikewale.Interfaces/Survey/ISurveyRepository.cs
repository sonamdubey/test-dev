using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Models.Survey;

namespace Bikewale.Interfaces
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 07 Jun 2017
    /// </summary>
    public interface ISurveyRepository
    {
        void InsertBajajSurveyResponse(BajajSurveyVM surveryResponse);
    }

    public interface ISurvey
    {
        void InsertBajajSurveyResponse(BajajSurveyVM surveryResponse);
    }
}