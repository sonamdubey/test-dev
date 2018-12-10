using Carwale.Entity.ES;

namespace Carwale.Interfaces.ES
{
    public interface ISurveyRepository
    {
        ESSurveyEnity GetSurveyQuestionAnswers(int campaignId);
        int SubmitSurvey(ESSurveyCustomerResponse objCustomer);
        int SubmitSurveyWithFreeText(ESSurveyCustomerResponse objCustomer);
    }
}
