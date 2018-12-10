using Carwale.Entity.Classified.CarDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Classified.CarDetail
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 29 MAy 2015
    /// </summary>
    public interface IListingDetails
    {
        CarDetailsEntity GetIndividualListingDetails(uint inquiryId, bool isMasterConnection);
        CarDetailsEntity GetDealerListingDetails(uint inquiryId);
        bool PhotoRequestDone(int sellInquiryId, int buyerId, int consumerType);
        bool UploadPhotosRequest(int sellInquiryId, int buyerId, int consumerType, string buyerMessage);
        int ReportListing(int inquiryId, int inquiryType, int reasonId, string description, string email);
        List<ReportListingReasons> GetReportListingReasons(bool isDealer);
    }
}
