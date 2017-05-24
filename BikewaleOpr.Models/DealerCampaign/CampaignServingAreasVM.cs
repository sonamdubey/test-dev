﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikewaleOpr.Entity.DealerCampaign;

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
        public IEnumerable<CityArea> MappedAreas{ get; set; }        
        public IEnumerable<CityArea> AdditionallyMappedAreas { get; set; }
        public IEnumerable<City> Cities { get; set; }
        //public IEnumerable<City> AdditionalCities { get; set; }

    }   // class
}   // namespace
