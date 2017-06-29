﻿
using Bikewale.Entities.BikeData;
using Bikewale.Entities.manufacturecampaign;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.Price
{
    /// <summary>
    /// 
    /// </summary>
    public class DealerPriceQuotePageVM : ModelBase
    {
        public BikeVersionEntity SelectedVersion { get; set; }
        public IEnumerable<BikeVersionsListEntity> VersionsList { get; set; }
        public Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity DetailedDealer { get; set; }
        public BikeQuotationEntity Quotation { get; set; }
        public LeadCaptureEntity LeadCapture { get; set; }
        public DealerPackageTypes DealerType { get; set; }
        public string MinSpecsHtml { get; set; }
        public string Location { get; set; }
        public string BikeName { get; set; }
        public string MPQQueryString { get; set; }
        public uint PQId { get; set; }
        public uint DealerId { get; set; }
        public uint CityId { get; set; }
        public string CiyName { get; set; }
        public uint AreaId { get; set; }
        public uint TotalPrice { get; set; }
        public DealerCardVM OtherDealers { get; set; }
        public uint ModelId { get { return (this.SelectedVersion != null && this.SelectedVersion.ModelBase != null) ? (uint)this.SelectedVersion.ModelBase.ModelId : 0; } }
        public uint VersionId { get { return (this.SelectedVersion != null) ? (uint)this.SelectedVersion.VersionId : 0; } }

        public bool IsPrimaryDealerAvailable { get { return (this.DetailedDealer != null && this.DetailedDealer.PrimaryDealer != null && this.DetailedDealer.PrimaryDealer.DealerDetails != null); } }

        public bool IsDealerPriceQuote { get { return (this.DetailedDealer != null && this.DetailedDealer.PrimaryDealer != null && this.DetailedDealer.PrimaryDealer.PriceList != null && this.DetailedDealer.PrimaryDealer.PriceList.Count() > 0); } }

        public bool ShowOtherDealers { get { return (this.DealerId == 0 && this.DetailedDealer != null && this.DetailedDealer.SecondaryDealerCount == 0 && this.Quotation != null && string.IsNullOrEmpty(this.Quotation.ManufacturerAd)); } }

        public bool AreOtherDealersAvailable { get { return (this.OtherDealers != null && this.OtherDealers.Dealers != null && this.OtherDealers.Dealers.Count() > 0); } }

        public string LeadBtnLongText { get { return "Get offers from dealer"; } }
        public string LeadBtnShortText { get { return "Get offers"; } }

        public string ClientIP { get; set; }
        public string PageUrl { get; set; }
        public int PQSourcePage { get { return (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_DPQ_Quotation; } }
        public int PQLeadSource { get { return 34; } }

        public SimilarBikesWidgetVM SimilarBikesVM { get; set; }
        public ManufactureCampaignLeadEntity LeadCampaign { get; set; }
        public bool IsManufacturerLeadAdShown { get; set; }
        public ManufactureCampaignEMIEntity EMICampaign { get; set; }
        public bool IsManufacturerEMIAdShown { get; set; }
    }
}
