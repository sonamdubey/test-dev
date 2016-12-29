
using BikewaleOpr.Entities;
using System.Collections.Generic;
namespace BikewaleOpr.Interface.ContractCampaign
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Dec 2016
    /// Description :   DealerCampaign Repository Interface
    /// </summary>
    public interface IDealerCampaignRepository
    {
        DealerCampaignEntity FetchBWDealerCampaign(uint campaignId);
        ICollection<CallToActionEntityBase> FetchDealerCallToActions();
        bool UpdateBWDealerCampaign(bool isActive, int campaignId, int userId, int dealerId, int contractId, int dealerLeadServingRadius, string maskingNumber, string dealerName, string dealerEmailId, int dailyleadlimit, ushort callToAction, bool isBookingAvailable = false);
        int InsertBWDealerCampaign(bool isActive, int userId, int dealerId, int contractId, int dealerLeadServingRadius, string maskingNumber, string dealerName, string dealerEmailId, int dailyleadlimit, ushort callToAction, bool isBookingAvailable = false);
    }
}
