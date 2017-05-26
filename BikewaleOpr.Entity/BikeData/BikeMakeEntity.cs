﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entities.BikeData
{
    /// <summary>
    /// Modified by : Aditi  Srivastava on 24 May 2017
    /// Summary     : Added oldmaskingname property for mails on masking name change
    /// </summary>
    public class BikeMakeEntity : BikeMakeEntityBase
    {
        public bool Futuristic { get; set; }
        public bool New { get; set; }
        public bool Used { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string OldMakeMasking { get; set; }
    }
}
