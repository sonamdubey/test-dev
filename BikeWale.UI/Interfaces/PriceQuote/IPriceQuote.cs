using Bikewale.DTO.PriceQuote.Version;
using Bikewale.Entities;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using System;
using System.Collections.Generic;

namespace Bikewale.Interfaces.PriceQuote
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Interface for the price quote.
    /// Modified By :   Sumit Kate
    /// Date        :   16 Oct 2015
    /// Description :   Added new method UpdatePriceQuote to update the price quote details
    /// Modified By :   Vivek Gupta on 20-05-2016
    /// Description :   Added new reference of method FetchPriceQuoteOfTopCities to fetch top city prices
	/// Modified By :   Rajan Chauhan on 27 June 2018
	/// Description :   Added UpdatePriceQuoteDetailsByLeadId for updating PQ details 
    /// </summary>
    public interface IPriceQuote
    {
        ulong RegisterPriceQuote(PriceQuoteParametersEntity pqParams);
        BikeQuotationEntity GetPriceQuoteById(ulong pqId);
        Bikewale.Entities.PriceQuote.v2.BikeQuotationEntity GetPriceQuote(uint cityId, uint versionId);
        BikeQuotationEntity GetPriceQuote(PriceQuoteParametersEntity pqParams);
        List<OtherVersionInfoEntity> GetOtherVersionsPrices(ulong pqId);
        IEnumerable<OtherVersionInfoEntity> GetOtherVersionsPrices(uint modelId, uint cityId);
        bool UpdatePriceQuote(UInt32 pqId, PriceQuoteParametersEntity pqParams);
        bool UpdatePriceQuoteDetailsByLeadId(UInt32 leadId, PriceQuoteParametersEntity pqParams);
        bool SaveBookingState(UInt32 pqId, PriceQuoteStates state);
        PriceQuoteParametersEntity FetchPriceQuoteDetailsById(UInt64 pqId);
        IEnumerable<PriceQuoteOfTopCities> FetchPriceQuoteOfTopCities(uint modelId, uint topCount);
        IEnumerable<PriceQuoteOfTopCities> GetModelPriceInNearestCities(uint modelId, uint cityId, ushort topCount);
        IEnumerable<BikeQuotationEntity> GetVersionPricesByModelId(uint modelId, uint cityId, out bool HasArea);
        IEnumerable<BikeQuotationEntity> GetVersionPricesByModelId(uint modelId, uint cityId);
        BikeQuotationEntity GetPriceQuoteById(ulong p, Entities.BikeBooking.LeadSourceEnum leadSourceEnum);
        IDictionary<uint, List<ManufacturerDealer>> GetManufacturerDealers(uint dealerId);
        string RegisterPriceQuoteV2(Bikewale.Entities.PriceQuote.v2.PriceQuoteParametersEntity pqParams);
        void GetDealerVersionsPriceByModelCity(IEnumerable<BikeVersionMinSpecs> versionList, uint cityId, uint modelId, uint dealerId = 0);
        Bikewale.Entities.PriceQuote.v2.BikeQuotationEntity GetPriceQuote(uint cityId, uint versionId, LeadSourceEnum page);
		bool GetMLAStatus(int makeId, uint cityId);
        VersionPrice GetVersionPriceByCityId(uint versionId, uint cityId);
        IList<PriceCategory> GetVersionPriceListByCityId(uint versionId, uint cityId);
    }
}
