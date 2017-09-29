using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.DealerCampaign;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Interface.Location;
using Bikewale.Utility;

namespace BikewaleOpr.Models.DealerCampaign
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 15 May 2017
    /// Summary : Model for the page serving areas. Model have function to manipulate all the data on page
    /// </summary>
    public class CampaignServingAreasModel
    {
        private readonly IDealerCampaignRepository _campaignRepo = null;
        private readonly ICommuteDistance _distance = null;
        private readonly ILocation _location = null;

        /// <summary>
        /// Constructor to initialize the dependencies
        /// </summary>
        /// <param name="campaignRepo"></param>
        public CampaignServingAreasModel(IDealerCampaignRepository campaignRepo, ICommuteDistance distance, ILocation location)
        {
            _campaignRepo = campaignRepo;
            _distance = distance;
            _location = location;
        }

        #region GetPageData method
        /// <summary>
        /// Written By : Ashish G. Kamble on 15 may 2017
        /// Summary : Function to get the data for the page in viewmodel.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns>Returns viewmodel CampaignServingAreasVM</returns>
        public CampaignServingAreasVM GetPageData(uint dealerId, uint campaignid)
        {
            CampaignServingAreasVM objVM = new CampaignServingAreasVM();

            objVM.DealerId = dealerId;
            objVM.CampaignId = campaignid;            

            // Get areas mapped to the dealer's location from db
            DealerCampaignArea objAreas = _campaignRepo.GetMappedDealerCampaignAreas(dealerId);

            if (objAreas!=null)
            {
                // Get actually mapped areas to dealer's location (list contains both automatically mapped and additionally mapped areas). Grouping areas according to cities
                objVM.DealerName = objAreas.DealerName;
                if (objAreas.Areas!=null && objAreas.Areas.Any())
                {
                    IEnumerable<CityArea> cityAreas = objAreas.Areas.GroupBy(
                                p => new { p.CityId, p.CityName },
                                p => new Area() { Id = p.AreaId, Name = p.AreaName, IsAutoAssigned = p.IsAutoAssigned },
                                (key, g) => new CityArea() { City = new City() { Id = key.CityId, Name = key.CityName }, Areas = g }
                                );

                    if (cityAreas != null)
                    {
                        // Filter automatically mapped areas
                        if (cityAreas.Any(m => m.AutoAssignedAreas != null && m.AutoAssignedAreas.Any()))
                            objVM.MappedAreas = cityAreas.Where(m => m.AutoAssignedAreas != null && m.AutoAssignedAreas.Any()).Select(m => new CityArea() { City = m.City, Areas = m.AutoAssignedAreas });

                        // Filter additionally mapped areas
                        if (cityAreas.Any(m => m.AdditionalAreas != null && m.AdditionalAreas.Any()))
                            objVM.AdditionallyMappedAreas = cityAreas.Where(m => m.AdditionalAreas != null && m.AdditionalAreas.Any()).Select(m => new CityArea() { City = m.City, Areas = m.AdditionalAreas });

                        if (objVM.AdditionallyMappedAreas != null && objVM.AdditionallyMappedAreas.Any())
                        {
                            objVM.AdditionalAreaJson = Newtonsoft.Json.JsonConvert.SerializeObject(objVM.AdditionallyMappedAreas);
                        }
                    } 
                } 
            }

            // Get states for mapping to dropdownlist
            objVM.States = _location.GetStates();

            return objVM;
        } 
        #endregion


        /// <summary>
        /// Written By : Ashish G. Kamble on 12 May 2017
        /// Summary : Action to map areas with dealer location.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="campaignServingStatus">Status of the serving areas to the particular campaign.</param>
        /// <param name="servingRadius">Serving radius for the given dealer (campaign serving radius).</param>
        /// <param name="cityIdList">Comma separated city id list. e.g. cityid1, cityid2, cityid3</param>
        public void MapCampaignAreas(uint dealerId, uint campaignid, ushort campaignServingStatus, ushort servingRadius, string cityIdList, string[] stateIdList)
        {
            _distance.SaveCampaignAreas(dealerId, campaignid, campaignServingStatus, servingRadius, cityIdList, stateIdList.ToCSV());
        }


        /// <summary>
        /// Written By : Ashish G. Kamble on 12 May 2017
        /// Summary : Action to map addtional areas to the dealer's location
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="areaIdList">Comma separated area id list. e.g. areaid1, areaid2, areaid3</param>
        public void MapAdditionalAreas(uint dealerId, string areaIdList)
        {
            _distance.SaveAdditionalCampaignAreas(dealerId, areaIdList);
        }


        /// <summary>
        /// Written By : Ashish G. Kamble on 12 May 2017
        /// Summary : Action to remove multiple areas mapped with the dealers location
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="areaIdList">Comma separated area id list. e.g. areaid1, areaid2, areaid3</param>
        public void RemmoveAdditionallyMappedAreas(uint dealerId, string areaIdList)
        {
            _campaignRepo.DeleteAdditionalMappedAreas(dealerId, areaIdList);
        }

    }   // class
}   // namespace