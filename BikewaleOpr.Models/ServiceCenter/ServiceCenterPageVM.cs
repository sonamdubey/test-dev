﻿using BikewaleOpr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Models.ServiceCenter
{ 
    
    /// <summary>		
    /// Written By : Snehal Dange on 27 July 2017		
    /// Summary : Class have all properties required for service centers page in Opr		
    /// </summary>		
    
    public class ServiceCenterPageVM
    {		
        public IEnumerable<Entities.BikeData.BikeMakeEntityBase> Makes { get; set; }		
        public IEnumerable<CityEntityBase> AllCityList { get; set; }		
		
    }
}
