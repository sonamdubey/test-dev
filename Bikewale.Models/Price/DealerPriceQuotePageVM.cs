
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;
namespace Bikewale.Models.Price
{
    public class DealerPriceQuotePageVM : ModelBase
    {
        private string _leadBtnLongText = "Get offers from dealer";
        public BikeVersionEntity SelectedVersion { get; set; }
        public IEnumerable<BikeVersionsListEntity> VersionsList { get; set; }
        public DealerQuotationEntity PrimaryDealer { get; set; }
        public DealerPackageTypes DealerType { get; set; }
        public EMI _objEMI { get; set; }
        public BikeQuotationEntity Quotation { get; set; }
        public ManufacturerCampaign ManufacturerCampaign { get; set; }
        public string MinSpecsHtml { get; set; }
        public string Location { get; set; }
        public string BikeName { get; set; }
        public uint PQId { get; set; }
        public uint DealerId { get; set; }
        public uint CityId { get; set; }
        public uint AreaId { get; set; }
        public uint TotalPrice { get; set; }

        public uint ModelId { get { return (this.SelectedVersion != null && this.SelectedVersion.ModelBase != null) ? (uint)this.SelectedVersion.ModelBase.ModelId : 0; } }

        public uint VersionId { get { return (this.SelectedVersion != null) ? (uint)this.SelectedVersion.VersionId : 0; } }

        public bool IsPrimaryDealerAvailable { get { return (this.PrimaryDealer != null && this.PrimaryDealer.DealerDetails != null); } }

        public string LeadBtnLargeText { get { return (this.IsPrimaryDealerAvailable) ? this.PrimaryDealer.DealerDetails.DisplayTextLarge : _leadBtnLongText; } }






        public PQ_QuotationEntity objPrice = null;
        public PopularDealerServiceCenter cityDealers;
        public DealersEntity dealers { get; set; }
        public List<VersionColor> objColors = null;
        public IEnumerable<PQ_Price> primaryPriceList = null;


        public string bikeName = string.Empty, bikeVersionName = string.Empty, minspecs = string.Empty, pageUrl = string.Empty, dealerName, dealerArea, dealerAddress, makeName, modelName, mpqQueryString, pq_leadsource = "34", pq_sourcepage = "58", currentCity = string.Empty, currentArea = string.Empty;

        public uint offerCount = 0, bookingAmount;
        public bool IsBWPriceQuote,
            isUSPBenfits,
            isoffer,
            isEMIAvailable,
            IsDiscount,
            IsPremium,
            IsStandard,
            IsDeluxe;
        public Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity DetailedDealer { get; set; }
        public double latitude, longitude;

    }
}
