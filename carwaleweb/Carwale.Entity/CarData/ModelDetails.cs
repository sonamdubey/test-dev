﻿using Newtonsoft.Json;

namespace Carwale.Entity.CarData
{
    public class ModelDetails : CarMakeModelEntityBase
    {
        public string SubSegment { get; set; }
        public string BodyStyle { get; set; }
        public bool IsUpcoming { get; set; }
        public bool IsNew { get; set; }
        public int RootId { get; set; }
        public string RootName { get; set; }
        public double FuelEconomy { get; set; }
        public double ValueForMoney { get; set; }
        public double Comfort { get; set; }
        public double Performance { get; set; }
        public double Looks { get; set; }
        public int ReviewCount { get; set; }
        public double ReviewRate { get; set; }
        public int EstimatedPrice { get; set; }
    }
}
