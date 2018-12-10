using Carwale.Entity.Customers;
using Carwale.Entity.ES;
using System.Collections.Generic;

namespace Carwale.Interfaces.ES
{
    public interface IBookingRepository
    {
        List<ESVersionColors> GetBookingModelData(int modelId);
        int GetSetCarCount(int versionId, int extColorId, int intColorId, bool isGetCount);
        int SubmitEsCustomerData(ESSurveyCustomerResponse customerResponse);
        int SaveEsInquiry(EsInquiry customerInquiry);
        CustomerMinimal GetEsCustomer(int customerId);
        EsBookingSummary GetBookingSummary(int inquiryId);
    }
}
