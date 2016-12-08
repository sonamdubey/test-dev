﻿using Bikewale.BAL.PriceQuote;
using Bikewale.BAL.Used.Search;
using Bikewale.DAL.Used.Search;
using Bikewale.Entities;
using Bikewale.Entities.Used.Search;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.Used.Search;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Bikewale.BindViewModels.Webforms
{
    /// <summary>
    /// Created by  : Sangram Nandkhile on 16 mar 2016
    /// Summary     : A ViewModel to bind data with Model page view
    /// </summary>
    public class ModelPageVM
    {
        public bool IsPremiumDealer { get; set; }
        public string Organization { get; set; }
        public string AreaName { get; set; }
        public short SecondaryDealerCount { get; set; }
        public string MaskingNumber { get; set; }
        public IEnumerable<OfferEntityBase> Offers { get; set; }
        public ushort OfferCount { get; set; }
        public IEnumerable<Bikewale.Entities.PriceQuote.v2.NewBikeDealerBase> SecondaryDealersV2 { get; set; }
        public IEnumerable<Bikewale.Entities.PriceQuote.NewBikeDealerBase> SecondaryDealers { get; set; }
        public string MobileNo { get; set; }
        public Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity DealerCampaignV2 { get; set; }
        public string primaryDealerDistance { get; set; }

        public ModelPageVM(uint cityId, uint versionId, uint dealerId, uint areaId)
        {
            try
            {
                // DealerCampaign = GetDetailedDealer(cityId, versionId, dealerId);

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuoteDetail, DealerPriceQuoteDetail>();
                    IDealerPriceQuoteDetail objIPQ = container.Resolve<IDealerPriceQuoteDetail>();
                    DealerCampaignV2 = objIPQ.GetDealerQuotationV2(cityId, versionId, dealerId, Convert.ToUInt32(areaId));
                    if (DealerCampaignV2 != null)
                    {
                        if (DealerCampaignV2.PrimaryDealer != null && DealerCampaignV2.PrimaryDealer.DealerDetails != null)
                        {
                            if (DealerCampaignV2.PrimaryDealer.DealerDetails.DealerPackageType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium)
                            {
                                IsPremiumDealer = true;
                                Organization = DealerCampaignV2.PrimaryDealer.DealerDetails.Organization;
                                if (DealerCampaignV2.PrimaryDealer.DealerDetails.objArea != null)
                                    AreaName = DealerCampaignV2.PrimaryDealer.DealerDetails.objArea.AreaName;
                                SecondaryDealerCount = Convert.ToInt16(DealerCampaignV2.SecondaryDealerCount);
                                MaskingNumber = DealerCampaignV2.PrimaryDealer.DealerDetails.MaskingNumber;
                                Offers = DealerCampaignV2.PrimaryDealer.OfferList;
                                MobileNo = DealerCampaignV2.PrimaryDealer.DealerDetails.MobileNo;
                                // SecondaryDealers = DealerCampaign.SecondaryDealers;
                                if (DealerCampaignV2.PrimaryDealer.OfferList != null)
                                    OfferCount = Convert.ToUInt16(DealerCampaignV2.PrimaryDealer.OfferList.Count());
                            }
                            else
                                primaryDealerDistance = DealerCampaignV2.PrimaryDealer.DealerDetails.Distance;
                        }

                        //DealerCampaign.SecondaryDealersV2 = DealerCampaignV2.SecondaryDealers;
                        if (DealerCampaignV2.SecondaryDealers != null)
                        {
                            SecondaryDealersV2 = DealerCampaignV2.SecondaryDealers;
                            SecondaryDealerCount = Convert.ToInt16(DealerCampaignV2.SecondaryDealerCount);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Webforms.ModelPageVM constructor");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By :-Subodh Jain 07 oct 2016
        /// Desc:- To get total number of used bikes
        /// </summary>
        public uint TotalUsedBikes(uint modelId, uint cityId)
        {
            SearchResult UsedBikes = null;
            uint totalUsedBikes = 0;
            try
            {
                ISearch objSearch = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ISearchFilters, ProcessSearchFilters>()
                        .RegisterType<ISearchQuery, SearchQuery>()
                        .RegisterType<ISearchRepository, SearchRepository>()
                        .RegisterType<ISearch, SearchBikes>();
                    objSearch = container.Resolve<ISearch>();
                    InputFilters objFilters = new InputFilters();
                    // If inputs are set by hash, hash overrides the query string parameters
                    if (cityId > 0)
                        objFilters.City = cityId;
                    if (modelId > 0)
                        objFilters.Model = Convert.ToString(modelId);
                    UsedBikes = objSearch.GetUsedBikesList(objFilters);
                    if (UsedBikes != null)
                        totalUsedBikes = (uint)UsedBikes.TotalCount;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ModelPageVM.TotalUsedBikes()");
                objErr.SendMail();
            }
            return totalUsedBikes;
        }

    }
}