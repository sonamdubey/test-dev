﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikeWale.Entities.AutoBiz
{
    public class BikeColorAvailability
    {
        public uint ColorId { get; set; }
        public string ColorName { get; set; }
        public uint DealerId { get; set; }
        public short NoOfDays { get; set; }
        public bool isActive { get; set; }
        public uint VersionId { get; set; }
        public string HexCode { get; set; }
    }

    /// <summary>
    /// Created By : Sadhana Upadhyay on 11 Jan 2016
    /// Summary : To return list of bike color
    /// </summary>
    public class BikeAvailabilityByColor
    {
        public uint ColorId { get; set; }
        public string ColorName { get; set; }
        public uint DealerId { get; set; }
        public short NoOfDays { get; set; }
        public bool isActive { get; set; }
        public uint VersionId { get; set; }
        public IEnumerable<string> HexCode { get; set; }
    }
}
