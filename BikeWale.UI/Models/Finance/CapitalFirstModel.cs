using Bikewale.Entities.Dealer;
using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Finance;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Bikewale.Models.Finance
{
    /// <summary>
    /// Created by : Snehal Dange on 25th May 2018
    /// </summary>
    public class CapitalFirstModel
    {
        private readonly IFinanceCacheRepository _financeCache;
        public bool IsMobile { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="financeCache"></param>
        public CapitalFirstModel(IFinanceCacheRepository financeCache)
        {
            _financeCache = financeCache;
        }


        public CapitalFirstVM GetData(NameValueCollection queryCollection)
        {
            ushort platformId = 0;
            CapitalFirstVM viewModel = new CapitalFirstVM();
            try
            {
                viewModel.ObjLead = new ManufacturerLeadEntity();
                viewModel.ObjLead.CampaignId = Convert.ToUInt16(queryCollection["campaingid"]);
                viewModel.ObjLead.DealerId = Convert.ToUInt16(queryCollection["dealerid"]);
                viewModel.ObjLead.LeadSourceId = Convert.ToUInt16(queryCollection["leadsourceid"]);
                viewModel.ObjLead.VersionId = Convert.ToUInt16(queryCollection["versionid"]);
                viewModel.ObjLead.PQId = Convert.ToUInt32(queryCollection["pqid"]);
                viewModel.PageUrl = queryCollection["url"];
                viewModel.BikeName = queryCollection["bike"];
                viewModel.LoanAmount = Convert.ToUInt32(queryCollection["loanamount"]);

                GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
                if (location != null)
                    viewModel.ObjLead.CityId = location.CityId;
                viewModel.objLeadJson = Newtonsoft.Json.JsonConvert.SerializeObject(viewModel.ObjLead);

                if (IsMobile)
                {
                    viewModel.PlatformId = ushort.TryParse(queryCollection["platformid"], out platformId) ? platformId : (ushort)DTO.PriceQuote.PQSources.Mobile;
                }
                else
                {
                    viewModel.PlatformId = (ushort)DTO.PriceQuote.PQSources.Desktop;
                }
                SetPanStatus(viewModel);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Finance.GetData");
            }
            return viewModel;
        }

        /// <summary>
        /// Created by : Snehal Dange on 25th May 2018
        /// Description: Method to set the pan status for the current city
        /// </summary>
        /// <param name="viewModel"></param>
        private void SetPanStatus(CapitalFirstVM viewModel)
        {
            try
            {
                if (viewModel != null && viewModel.ObjLead != null)
                {
                    CityPanMapping obj = null;
                    IEnumerable<CityPanMapping> panCityMapping = _financeCache.GetCapitalFirstPanCityMapping();
                    obj = panCityMapping.FirstOrDefault(m => m.CityId.Equals(viewModel.ObjLead.CityId));
                    if (obj != null)
                    {
                        viewModel.PanStatus = obj.PanStatus;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Finance.SetPanStatus");
            }
        }
    }
}