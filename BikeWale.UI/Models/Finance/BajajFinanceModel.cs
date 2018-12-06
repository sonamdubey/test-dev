using Bikewale.Entities.Dealer;
using Bikewale.Entities.Location;
using Bikewale.Notifications;
using Bikewale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;

namespace Bikewale.Models.Finance
{
    /// <summary>
    /// Author  : Kartik Rathod on 19 july 2019
    /// </summary>
    public class BajajFinanceModel
    {

        public bool IsMobile { get; set; }

        public BajajFinanceVM GetData(NameValueCollection queryCollection)
        {
            BajajFinanceVM viewModel = null;
            ushort platformId = 0;

            try
            {
                if (queryCollection != null)
                {
                    bool sendSMStoCustomer = false;
                    uint pqId = 0, cityId = 0;
                    viewModel = new BajajFinanceVM();
                    viewModel.ObjLead = new ManufacturerLeadEntity();
                    viewModel.ObjLead.CampaignId = Convert.ToUInt16(queryCollection["campaignid"]);
                    viewModel.ObjLead.DealerId = Convert.ToUInt16(queryCollection["dealerid"]);
                    viewModel.ObjLead.LeadSourceId = Convert.ToUInt16(queryCollection["leadsourceid"]);
                    viewModel.ObjLead.VersionId = Convert.ToUInt16(queryCollection["versionid"]);
                    UInt32.TryParse(queryCollection["pqid"], out pqId);
                    viewModel.ObjLead.PQId = pqId;
                    viewModel.PageUrl = queryCollection["url"];
                    viewModel.BikeName = queryCollection["bike"];
                    viewModel.ObjLead.BikeName = queryCollection["bike"];
                    viewModel.ObjLead.DealerName = queryCollection["dealerName"];
                    Boolean.TryParse(queryCollection["sendLeadSMSCustomer"], out sendSMStoCustomer);
                    viewModel.ObjLead.SendLeadSMSCustomer = sendSMStoCustomer;
                    viewModel.ObjLead.PQGUId = queryCollection["pqguid"];
                    UInt32.TryParse(queryCollection["cityid"], out cityId);
                    viewModel.ObjLead.CityId = cityId;

                    if (viewModel.ObjLead.CityId == 0)
                    {
                        GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
                        if (location != null)
                            viewModel.ObjLead.CityId = location.CityId;
                    }

                    if (IsMobile)
                    {
                        viewModel.PlatformId = ushort.TryParse(queryCollection["platformid"], out platformId) ? platformId : (ushort)DTO.PriceQuote.PQSources.Mobile;
                    }
                    else
                    {
                        viewModel.PlatformId = (ushort)DTO.PriceQuote.PQSources.Desktop;
                    }

                    viewModel.objLeadJson = JsonConvert.SerializeObject(viewModel.ObjLead, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.BajajFinanceModel.GetData");
            }
            return viewModel;
        }
    }
}