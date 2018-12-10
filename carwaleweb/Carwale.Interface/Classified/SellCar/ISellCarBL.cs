using Carwale.Entity.CarData;
using Carwale.Entity.Classified.Leads;
using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Entity.Enum;
using System.Collections.Generic;

namespace Carwale.Interfaces.Classified.SellCar
{
    public interface ISellCarBL
    {
        void UpdateSellCarCurrentStep(int inquiryId, int currentStep, bool updateForceFully);
        bool CheckFreeListingAvailability(string mobile);
        bool CreateCustomer(SellCarCustomer sellCustomer);
        int ProcessContactDetails(SellCarCustomer sellCustomer);
        int CreateSellCarInquiry(TempCustomerSellInquiry tempInquiry);
        PageMetaTags GetPageMetaTags();
        IEnumerable<int> GetMakeYears();
        IEnumerable<int> GetInsuranceYears();
        bool IsC2bCity(int cityId);
        bool InsertVerifiedMobile(string mobile,int sourceId);
        int CreateSellCarInquiryV1(TempCustomerSellInquiry tempInquiry);
    }
}
