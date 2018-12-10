using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.Stock.Certification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.CarDetails
{
   
    public class CarDetailsModel
    {
        public CarDetailsWeb carDetailEntity;
        public StockCertification certification;
        public List<RecommendedCarsEntity> RecommendedCarsEntity;
        public Dictionary<string, string> AbsureMainTabs;
        public CustomerDetailsEntity CustomerDetails;
        public bool VideoCdtn { get; set; }
        public bool DealerProfileImgCdtn { get; set; }
        public bool IsCertified { get; set; }
        public string RankFromQueryString { get; set; }
        public string IsPremiumFromQueryString { get; set; } //Important to pass it from search page as premium cars are marked as non-premium (ProcessElasticJson.cs)
        public string ListingsTrackingPlatform { get; set; }
        public string DeliveryCity { get; set; }
        public int DeliveryCityId { get; set; }
        public string NoWarrantyDetails { get; set; }
        public string UrlToRedirect { get; set; }
        public bool HasShowroom { get; set; }
        public string viewAllTyreUrl { get; set; }
        public bool IsCarwaleReferrer { get; set; }
        public string PhotoGalleryUrl { get; set; }
    }
}
