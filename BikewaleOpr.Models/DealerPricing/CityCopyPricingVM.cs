﻿using BikewaleOpr.Entities;
using BikewaleOpr.Entity;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerPricing
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
    /// Description :   View Model for Copy pricing to cities collapsable in dealer pricing management page
    /// </summary>
    public class CityCopyPricingVM
    {
        public IEnumerable<StateEntityBase> States { get; set; }
        public IEnumerable<CityNameEntity> Cities { get; set; }
    }
}
