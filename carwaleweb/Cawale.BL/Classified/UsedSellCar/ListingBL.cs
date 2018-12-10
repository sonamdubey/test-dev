using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Notifications.Logs;
using Newtonsoft.Json;
using RabbitMqPublishing;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Carwale.BL.Classified.UsedSellCar
{
    public class ListingsBL : IListingsBL
    {
        private readonly IListingRepository _listingRepo;

        public ListingsBL(IListingRepository listingRepo)
        {
            _listingRepo = listingRepo;
        }

        public void PushToQueue(Listing listings)
        {
            const string c2bDeleteListingKey = "c2bdeletelisting";
            RabbitMqPublish rabbitMqPublish = new RabbitMqPublish();
            string c2bDeleteListingQueue = ConfigurationManager.AppSettings["C2BDeleteListingQueue"];
            foreach (var listingDetail in listings.ListingDetails)
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add(c2bDeleteListingKey, JsonConvert.SerializeObject(listingDetail));
                rabbitMqPublish.PublishToQueue(c2bDeleteListingQueue, nvc);
            }
        }

        public bool UpdateExpiredListings()
        {
            bool isSuccess = true;
            var listings = _listingRepo.GetExpiredListings();
            foreach (var listing in listings)
            {
                bool result = false;
                if (listing.PackageType == (int)ClassifiedPackageType.FreePlan)
                {
                    result = _listingRepo.ListingDelete(listing.InquiryId, status:9);
                }
                else if (listing.PackageType == (int)ClassifiedPackageType.AssistedSales)
                {
                    result = _listingRepo.PatchListingsV1(listing.InquiryId, new SellCarInfo { IsPremium = false });
                }
                if (!result)
                {
                    isSuccess = false;
                    Logger.LogInfo("Failed to Update expired listing :" + listing.InquiryId);
                }
            }
            return isSuccess;
        }
    }
}
