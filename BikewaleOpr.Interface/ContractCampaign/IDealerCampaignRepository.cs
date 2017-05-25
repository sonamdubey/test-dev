﻿
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ContractCampaign;
using System.Collections.Generic;
using BikewaleOpr.Entity.DealerCampaign;
namespace BikewaleOpr.Interface.ContractCampaign
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Dec 2016
    /// Description :   DealerCampaign Repository Interface
    /// Modified by :   Sumit Kate on 18 Jan 2017
    /// Description :   Added methods MakesByDealerCity, DealersByMakeCity and DealerCampaigns
    /// Modified by :   Sumit Kate on 12 May 2017
    /// Description :   Removed lead serving radius from UpdateBWDealerCampaign and InsertBWDealerCampaign method parameter list
    /// </summary>
    public interface IDealerCampaignRepository
    {
        DealerCampaignEntity FetchBWDealerCampaign(uint campaignId);
        ICollection<CallToActionEntityBase> FetchDealerCallToActions();
        bool UpdateBWDealerCampaign(bool isActive, int campaignId, int userId, int dealerId, int contractId, string maskingNumber, string dealerName, string dealerEmailId, int dailyleadlimit, ushort callToAction, bool isBookingAvailable = false);
        int InsertBWDealerCampaign(bool isActive, int userId, int dealerId, int contractId, string maskingNumber, string dealerName, string dealerEmailId, int dailyleadlimit, ushort callToAction, bool isBookingAvailable = false);
        ICollection<BikeMakeEntityBase> MakesByDealerCity(uint cityId);
        ICollection<DealerEntityBase> DealersByMakeCity(uint cityId, uint makeId, bool activecontract);
        ICollection<DealerCampaignDetailsEntity> DealerCampaigns(uint dealerId, bool activecontract);

        IEnumerable<CampaignAreas> GetMappedDealerCampaignAreas(uint dealerId);
        void SaveDealerCampaignAreaMapping(uint dealerId, uint campaignid, ushort campaignServingStatus, ushort servingRadius, string cityIdList);
        DealerAreaDistance GetDealerToAreasDistance(uint dealerId, ushort campaignServingStatus, ushort servingRadius, string cityIdList);
        DealerAreaDistance GetDealerAreasWithLatLong(uint dealerId, string areasList);
        void SaveAdditionalAreasMapping(uint dealerId, string areasList);        
        void DeleteAdditionalMappedAreas(uint dealerId, string areadIdList);
    }
}
