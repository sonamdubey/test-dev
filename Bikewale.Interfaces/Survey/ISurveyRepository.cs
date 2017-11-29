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