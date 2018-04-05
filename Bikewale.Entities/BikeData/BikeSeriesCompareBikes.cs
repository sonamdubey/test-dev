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
        public float Displacement { get; set; }
        public float Weight { get; set; }
        public float FuelCapacity { get; set; }
        public float Mileage { get; set; }
        public float SeatHeight { get; set; }
        public string BrakeType { get; set; }
        public ushort Gears { get; set; }
        public float MaxPower { get; set; }
        public float MaxPowerRpm { get; set; }
        public int VersionId { get; set; }
    }


}
