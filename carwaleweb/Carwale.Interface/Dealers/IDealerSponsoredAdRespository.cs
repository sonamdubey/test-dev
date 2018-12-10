using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using Carwale.Entity.Dealers;
using Carwale.Entity.ThirdParty.Leads;
using System.Collections.Generic;

namespace Carwale.Interfaces.Dealers
{
    public interface IDealerSponsoredAdRespository
    {
        ulong SaveDealerSponserdInquiry(DealerInquiryDetails inquiryDetails);
        void UpdateDealerSponserdInquiry(DealerInquiryDetails inquiryDetails);
        void UpdateDealerSponserdInquiryEmail(DealerInquiryDetails inquiryDetails);
        SponsoredDealer GetDealerDetailsByDealerId(int dealerId);
        List<NewCarDealersList> GetNewCarDealersByMakeAndCityId(int makeId, int cityId);
        void SaveFailedLeads(DealerInquiryDetails leadInfo, string errMessage);
        int LogThirdPartyInquiryDetails(ThirdPartyInquiryDetails InquiryDetails);
        void UpdateThirdPartyInquiryPushResponse(int LeadId, int pqDealerAdLeadId);
        IEnumerable<DealerInquiryDetails> GetLeadDetailsByLeadId(int LeadId);
        void SaveBlockedLeads(ulong pqDealerLeadId, int reasonId);
        int GetLeadCountForCurrentDayOnMobile(string mobile);
        int GetLeadCountForCurrentDayOnCwCookie(int platformId);
        void SaveLeadSponsoredBanner(ulong pqDealerLeadId, int targetModel, int targetVersion, int featuredVersion);
    }
}
