using BikewaleOpr.Entity.DealerCampaign;
using System.Collections.Generic;
using BikewaleOpr.Entities;

namespace BikewaleOpr.Models.DealerCampaign
{
    /// <summary>
    /// Written By : Ashis G. Kamble on 15 may 2017
    /// Summary : Model to hold the data for dealer campaign serving areas
    /// </summary>
    public class CampaignServingAreasVM
    {
        public uint DealerId { get; set; }
        public string DealerName { get; set; }
        public uint CampaignId { get; set; }
        public IEnumerable<CityArea> MappedAreas{ get; set; }        
        public IEnumerable<CityArea> AdditionallyMappedAreas { get; set; }
        public IEnumerable<City> Cities { get; set; }
        public string AdditionalAreaJson { get; set; }
        //public IEnumerable<City> AdditionalCities { get; set; }
        public IEnumerable<StateEntityBase> States { get; set; }

    }   // class
}   // namespace
