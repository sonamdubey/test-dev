
using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeSeries;
using System.Collections.Generic;
namespace Bikewale.Models.BikeSeries
{
    public class BikeSeriesCompareVM
    {
        public IEnumerable<BikeSeriesCompareBikes> BikeSeriesCompareBikeWithSpecs { get; set; }

        public IEnumerable<string> BikeCompareSegments { get; set; }

        public BikeSpecs ObjBikeSpecs { get; set; }
		public BikeMakeBase BikeMake { get; set; }
		public BikeSeriesEntityBase SeriesBase { get; set; }


	}
    public class BikeSpecs
    {
        public ushort MaxPower { get; set; }
        public ushort Mileage { get; set; }
        public ushort Price { get; set; }
        public ushort Weight { get; set; }
        public ushort Displacement { get; set; }
        public ushort SeatHeight { get; set; }
        public ushort BrakeType { get; set; }
        public ushort FuelCapacity { get; set; }
        public ushort Gears { get; set; }

    }
}
