﻿
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.ConfigurePageMetas;
using System.Collections.Generic;

namespace BikewaleOpr.Models.ConfigurePageMetas
{
    public class ConfigurePageMetasVM
    {
        public IEnumerable<BikeMakeEntityBase> Makes { get; set; }
        public string DesktopPages { get; set; }
        public string MobilePages { get; set; }
    }

   
}
