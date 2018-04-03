using Bikewale.Entities.BikeData;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.Entities.BikeSeries
{
    [Serializable]
    public class BikeSeriesCompareBikes
    {
        public string ModelName { get; set; }
        public string ModelMaskingName { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public uint Price { get; set; }
        public double Displacement { get; set; }
        public double Weight { get; set; }
        public double FuelCapacity { get; set; }
        public double Mileage { get; set; }
        public double SeatHeight { get; set; }
        public string BrakeType { get; set; }
        public uint Gears { get; set; }
        public double MaxPower { get; set; }
        public double MaxPowerRpm { get; set; }
        public IEnumerable<SpecsItem> MinSpecsList { get; set; }
        public int VersionId { get; set; }
    }


}
