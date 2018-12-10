using Carwale.DTOs.Classified.MyListings;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified.Leads;
using Carwale.Entity.Classified.SellCarUsed;
using System.Collections.Generic;

namespace Carwale.Interfaces.Classified.MyListings
{
    public interface IMyListings
    {
        string GetMobileFromProfileId(int profileId);
        bool IsValidToken(string authToken, string mobileNumber, string ipAddress, string cwcCookie);
        bool ValidateSearchType(int searchByType);
        string GetMobile(int type, string value);
        MyListingsDTO GetDataForEditListing(int inquiryId);
        void SendMail(int inquiryId, SellCarInfo sellCarInfo);
        void RefreshCacheWithCriticalRead(int inquiryId);
        string GetDestinationUrl(int inquiryId, string page);
        void AddCookie(string name, string value, string path);
        PageMetaTags GetPageMetaTags();
        IList<ClassifiedRequest> GetClassifiedRequests(int inquiryId, int requestDate);
        int GetC2BLeadsCount(int inquiryId);
        int GetCarTradeLeadsCount(int inquiryId);
        bool IsImagesPendingToApprove(int inquiryId);
    }
}
