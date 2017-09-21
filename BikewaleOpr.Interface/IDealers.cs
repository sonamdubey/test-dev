
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Entity.Dealers;
using System;
using System.Collections.Generic;
using System.Data;

namespace BikewaleOpr.Interface
{
    /// <summary>
    /// Modified by : Aditi Srivastava on 9 Feb 2017
    /// Summary : Added functions to get list of makes and dealers in a city
    /// </summary>
    public interface IDealers
    {
        IEnumerable<FacilityEntity> GetDealerFacilities(uint dealerId);
        IEnumerable<DealerMakeEntity> GetDealersByCity(UInt32 cityId);
        UInt16 SaveDealerFacility(FacilityEntity objData);
        bool UpdateDealerFacility(FacilityEntity objData);
        DataTable GetOfferTypes();
        EMI GetDealerLoanAmounts(uint dealerId);
        List<OfferEntity> GetDealerOffers(int dealerId);
        bool SaveDealerOffer(int dealerId, uint userId, int cityId, string modelId, int offercategoryId, string offerText, int? offerValue, DateTime offervalidTill, bool isPriceImpact, string terms);
        bool DeleteDealerOffer(string offerId);
        bool SaveBikeAvailability(uint dealerId, uint bikemodelId, uint? bikeversionId, ushort numOfDays);
        List<OfferEntity> GetBikeAvailability(uint dealerId);
        bool EditAvailabilityDays(int availabilityId, int days);
        uint GetAvailabilityDays(uint dealerId, uint versionId);
        void SaveDealerDisclaimer(uint dealerId, uint makeId, uint? modelId, uint? versionId, string disclaimer);
        bool DeleteDealerDisclaimer(uint disclaimerId);
        List<DealerDisclaimerEntity> GetDealerDisclaimer(uint dealerId);
        bool EditDisclaimer(uint disclaimerId, string newDisclaimerText);
        #region bike booking amount function declaration
        IEnumerable<BookingAmountEntity> GetBikeBookingAmount(uint dealerId);
        bool UpdateBookingAmount(BookingAmountEntityBase objbookingAmtBase);
        bool SaveBookingAmount(BookingAmountEntity objBookingAmt, UInt32 updatedById);
        #endregion
        bool DeleteBookingAmount(uint bookingId);
        bool UpdateDealerBikeOffers(DealerOffersEntity dealerOffers);
        bool SaveVersionAvailability(uint dealerId, string bikeVersionIds, string numberOfDays);
        bool DeleteVersionAvailability(uint dealerId, string bikeVersionId);
        bool CopyOffersToCities(uint dealerId, string lstOfferIds, string lstCityId);
        IEnumerable<DealerBenefitEntity> GetDealerBenefits(uint dealerId);
        bool DeleteDealerBenefits(string benefitIds);
        bool SaveDealerBenefit(uint dealerId, uint cityId, uint CatId, string BenefitText, uint UserId, uint BenefitId);
        bool SaveDealerEMI(uint dealerId, float? MinDownPayment, float? MaxDownPayment, UInt16? MinTenure, UInt16? MaxTenure, float? MinRateOfInterest, float? MaxRateOfInterest, float? MinLtv, float? MaxLtv, string loanProvider, float? ProcessingFee, uint? id, UInt32 UserID);
        bool DeleteDealerEMI(uint id);
        IEnumerable<BikewaleOpr.Entities.BikeData.BikeMakeEntityBase> GetDealerMakesByCity(int cityId);
        IEnumerable<DealerEntityBase> GetDealersByMake(uint makeId, uint cityId);
    }

    public interface IDealer
    {
        IEnumerable<uint> GetAllAvailableDealer(uint versionId, uint areaId);
        IEnumerable<DealerPriceQuoteDetailed> GetDealerPriceQuoteDetail(uint versionId, uint cityId, string dealerIds);

    }
}
