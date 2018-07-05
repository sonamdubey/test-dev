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


		/// <summary>
		/// Modifier : Kartik Rathod on 16 may 2018, added dealerName,sendLeadSMSCustomer
		/// </summary>
		/// <param name="queryCollection"></param>
		/// <returns></returns>
        public CapitalFirstVM GetData(NameValueCollection queryCollection)
        {
            ushort platformId = 0;
            CapitalFirstVM viewModel = null;
            ManufacturerLeadEntity leadEntity = null;
            try
            {
                if (queryCollection != null)
                {
					bool sendSMStoCustomer = false;
                    uint pqId = 0;
                    UInt32.TryParse(queryCollection["pqid"], out pqId);
                    leadEntity = new ManufacturerLeadEntity();

                    leadEntity.CampaignId = Convert.ToUInt16(String.IsNullOrEmpty(queryCollection["campaingid"]) ? "0" : queryCollection["campaingid"]);
                    leadEntity.DealerId = Convert.ToUInt16(queryCollection["dealerid"]);
                    leadEntity.LeadSourceId = Convert.ToUInt16(String.IsNullOrEmpty(queryCollection["leadsourceid"]) ? "0" : queryCollection["leadsourceid"]);
                    leadEntity.VersionId = Convert.ToUInt16(queryCollection["versionid"]);
                    leadEntity.PQId = pqId;
                    leadEntity.CityId = Convert.ToUInt32(queryCollection["cityid"]);
					leadEntity.BikeName = queryCollection["bike"];
					leadEntity.DealerName = queryCollection["dealerName"];
					leadEntity.SendLeadSMSCustomer = Boolean.TryParse(queryCollection["sendLeadSMSCustomer"], out sendSMStoCustomer) ? sendSMStoCustomer : false;
                    leadEntity.PQGUId = queryCollection["pqguid"];

                    if (leadEntity.CityId == 0)
                    {
                        GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
                        if (location != null)
                            leadEntity.CityId = location.CityId;
                    }

                    viewModel = new CapitalFirstVM();
                    viewModel.ObjLead = leadEntity;
                    viewModel.PageUrl = queryCollection["url"];
                    viewModel.BikeName = queryCollection["bike"];
                    viewModel.LoanAmount = Convert.ToUInt32(String.IsNullOrEmpty(queryCollection["loanamount"]) ? "0" : queryCollection["loanamount"]);

                    if (viewModel.ObjLead != null)
                    {
                        viewModel.objLeadJson = Newtonsoft.Json.JsonConvert.SerializeObject(viewModel.ObjLead);
                    }

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
                if (viewModel != null)
                {
                    CityPanMapping obj = null;
                    if (_financeCache != null)
                    {
                        IEnumerable<CityPanMapping> panCityMapping = _financeCache.GetCapitalFirstPanCityMapping();
                        if (panCityMapping != null && viewModel.ObjLead != null)
                        {
                            obj = panCityMapping.FirstOrDefault(m => m.CityId.Equals(viewModel.ObjLead.CityId));
                            if (obj != null)
                            {
                                viewModel.PanStatus = obj.PanStatus;
                            }
                        }

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