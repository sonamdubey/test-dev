﻿
using BikewaleOpr.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace BikewaleOpr.Interface
{
    public interface IDealers
    {
        PQ_DealerDetailEntity GetDealerDetailsPQ(PQParameterEntity objParams);
        List<FacilityEntity> GetDealerFacilities(uint dealerId);
        DataTable GetAllDealers(UInt32 cityId);
        void SaveDealerFacility(uint dealerId, string facility, bool isActive);
        DataTable GetDealerCities();
        void UpdateDealerFacility(uint facilityId, string facility, bool isActive);
        DataTable GetOfferTypes();
        EMI GetDealerLoanAmounts(uint dealerId);
        List<OfferEntity> GetDealerOffers(int dealerId);
        void SaveDealerLoanAmounts(uint dealerId, ushort tenure, float rateOfInterest, ushort ltv, string loanProvider);
        bool SaveDealerOffer(int dealerId, uint userId, int cityId, string modelId, int offercategoryId, string offerText, int? offerValue, DateTime offervalidTill, bool isPriceImpact,string terms);
        void UpdateDealerLoanAmounts(uint dealerId, ushort tenure, float rateOfInterest, ushort ltv, string loanProvider);
        bool DeleteDealerOffer(string offerId);
        bool SaveBikeAvailability(uint dealerId, uint bikemodelId, uint? bikeversionId, ushort numOfDays);
        List<OfferEntity> GetBikeAvailability(uint dealerId);
        bool EditAvailabilityDays(int availabilityId, int days);
        uint GetAvailabilityDays(uint dealerId, uint versionId);
        void SaveDealerDisclaimer(uint dealerId, uint makeId, uint? modelId, uint? versionId, string disclaimer);
        void UpdateDealerDisclaimer(uint dealerId, uint versionId, string disclaimer);
        bool DeleteDealerDisclaimer(uint disclaimerId);
        List<DealerDisclaimerEntity> GetDealerDisclaimer(uint dealerId);
        bool EditDisclaimer(uint disclaimerId, string newDisclaimerText);
        #region bike booking amount function declaration
        List<BookingAmountEntity> GetBikeBookingAmount(uint dealerId);
        bool UpdateBookingAmount(BookingAmountEntityBase objbookingAmtBase);
        bool SaveBookingAmount(BookingAmountEntity objBookingAmt);
        BookingAmountEntity GetDealerBookingAmount(uint versionId, uint dealerId);
        #endregion
        bool DeleteBookingAmount(uint bookingId);
        bool UpdateDealerBikeOffers(uint offerId, uint userId, uint offerCategoryId, string offerText, uint? offerValue, DateTime offerValidTill, bool isPriceImpact, string terms);
        bool SaveBikeAvailability(DataTable dtValue);
        bool DeleteBikeAvailabilityDays(DataTable dtValue);
        bool CopyOffersToCities(uint dealerId, string lstOfferIds, string lstCityId);
        IEnumerable<DealerBenefitEntity> GetDealerBenefits(uint dealerId);
        bool DeleteDealerBenefits(string benefitIds);
        bool SaveDealerBenefit(uint dealerId, uint cityId, uint CatId, string BenefitText, uint UserId, uint BenefitId);
        bool SaveDealerEMI(uint dealerId, float? MinDownPayment, float? MaxDownPayment, UInt16? MinTenure, UInt16? MaxTenure, float? MinRateOfInterest, float? MaxRateOfInterest, float? MinLtv, float? MaxLtv, string loanProvider, float? ProcessingFee, uint? id, UInt32 UserID);
        bool DeleteDealerEMI(uint id);
    }

    public interface IDealer
    {
        IEnumerable<uint> GetAllAvailableDealer(uint versionId, uint areaId);
        IEnumerable<DealerPriceQuoteDetailed> GetDealerPriceQuoteDetail(uint versionId, uint cityId, string dealerIds);

    }
}
