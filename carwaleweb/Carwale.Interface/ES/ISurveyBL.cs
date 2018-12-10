using Carwale.Entity.ES;

namespace Carwale.Interfaces.ES
{
    public interface ISurveyBL
    {
        int SaveSurveyData(ESSurveyCustomerResponse Customer, string cwcCookie);
        bool ElectricCars(string cwcCookie);
    }
}
