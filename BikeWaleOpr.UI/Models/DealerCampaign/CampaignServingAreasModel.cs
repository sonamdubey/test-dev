﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.DealerCampaign;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.ContractCampaign;

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

        /// <summary>
        /// Constructor to initialize the dependencies
        /// </summary>
        /// <param name="campaignRepo"></param>
        public CampaignServingAreasModel(IDealerCampaignRepository campaignRepo, ICommuteDistance distance)
        {
            _campaignRepo = campaignRepo;
            _distance = distance;
        }

        #region GetPageData method
        /// <summary>
        /// Written By : Ashish G. Kamble on 15 may 2017
        /// Summary : Function to get the data for the page in viewmodel.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns>Returns viewmodel CampaignServingAreasVM</returns>
        public CampaignServingAreasVM GetPageData(uint dealerId)
        {
            CampaignServingAreasVM objVM = new CampaignServingAreasVM();

            objVM.DealerId = dealerId;
            objVM.DealerName = "";

            // Get areas mapped to the dealer's location from db
            IEnumerable<CampaignAreas> objAreas = _campaignRepo.GetMappedDealerCampaignAreas(dealerId);

            // Get actually mapped areas to dealer's location (list contains both automatically mapped and additionally mapped areas). Grouping areas according to cities
            IEnumerable<CityArea> cityAreas = objAreas.GroupBy(
                p => new { p.CityId, p.CityName },
                p => new Area() { Id = p.AreaId, Name = p.AreaName, IsAutoAssigned = p.IsAutoAssigned },
                (key, g) => new CityArea() { City = new City() { Id = key.CityId, Name = key.CityName }, Areas = g }
                );

            if (cityAreas != null)
            {
                // Filter automatically mapped areas
                if (cityAreas.Any(m => m.AutoAssignedAreas != null && m.AutoAssignedAreas.Count() > 0))
                    objVM.MappedAreas = cityAreas.Where(m => m.AutoAssignedAreas != null && m.AutoAssignedAreas.Count() > 0).Select(m => new CityArea() { City = m.City, Areas = m.AutoAssignedAreas });

                // Filter additionally mapped areas
                if (cityAreas.Any(m => m.AdditionalAreas != null && m.AdditionalAreas.Count() > 0))
                    objVM.AdditionallyMappedAreas = cityAreas.Where(m => m.AdditionalAreas != null && m.AdditionalAreas.Count() > 0).Select(m => new CityArea() { City = m.City, Areas = m.AdditionalAreas });

                //if (objVM.AdditionallyMappedAreas != null && objVM.AdditionallyMappedAreas.Count() > 0)
                //{
                //    objVM.AdditionalCities = objVM.AdditionallyMappedAreas.Select(s => s.City);
                //}
            }

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
        public void MapCampaignAreas(uint dealerId, ushort campaignServingStatus, ushort servingRadius, string cityIdList)
        {
            _distance.SaveCampaignAreas(dealerId, campaignServingStatus, servingRadius, cityIdList);
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