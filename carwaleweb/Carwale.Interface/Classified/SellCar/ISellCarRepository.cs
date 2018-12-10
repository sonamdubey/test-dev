using System;
using System.Collections.Generic;
using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Entity;
using Carwale.Entity.Classified.SellCar;

namespace Carwale.Interfaces.Classified.SellCar
{
    public interface ISellCarRepository
    {
        int SaveSellCarBasicInfo(SellCarBasicInfo sellCarBasicInfo);
        List<Tuple<int, int>> GetListingCount(int inquiryId);
        int GetSellCarStepsCompleted(int inquiryId);
        bool UpdateSellCarCurrentStep(int carId, int currentStep);
        SellInquiriesOtherDetails GetSellCarOtherDetails(int inquiryId);
        int SaveTempSellCarInquiryDetails(SellCarInfo inquiry, SellCarCustomer customer = null);
        bool SaveSellCarOtherDetails(SellInquiriesOtherDetails details);
        SellCarConditions GetSellCarCondition(int inquiryId);
        bool SaveSellCarCondition(SellCarConditions sellCarCondition);
        CustomerSellInquiryData GetCustomerSellInquiryData(int inquiryId);
        CustomerSellInquiryVehicleData GetCustomerSellInquiryVehicleDetails(int inquiryId);
        List<CustomerSellInquiry> GetCustomerSellInquiries(string customerEmail, int defaultPackageId);
        bool UpdateCarIsArchived(int inquiryId);
        bool IsCustomerAuthorizedToManageCar(int customerId, int inquiryId);
        void GetSellCarExpiry(int customerId, int inquiryId, out DateTime? expiryDate, out string carName);
        bool RenewSellCarListing(int inquiryId, int customerId);
        int GetFreeListingCount(string mobile);
        TempCustomerSellInquiry GetTempCustomerSellInquiry(int tempInquiryId);
        int SaveSellCarDetails(SellCarInfo sellCarInfo, SellCarCustomer customer);
        C2BStockDetailsV2 GetTempSellCarDetails(int TempInquiryId);
        void InsertSellCarC2BLead(int? tempInquiryId, int? inquiryId, int? ctTempId, int? lastAction, string status);
        bool SaveOtherDetails(SellCarInfo sellCarInfo, int inquiryId);
        IEnumerable<int> C2BCities();
        bool InsertVerifiedMobileEmailPair(string mobile, string email,int sourceId);
        int SaveSellCarDetailsV1(SellCarInfo sellCarInfo, SellCarCustomer customer);
    }
}
