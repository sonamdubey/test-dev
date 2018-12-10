using Carwale.Entity.ES;

namespace Carwale.Interfaces.ES
{
    public interface ISurveyCache
    {
        ESSurveyEnity GetSurveyQuestions(int campaignId);
    }
}
