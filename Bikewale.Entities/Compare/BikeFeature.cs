using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare
{
    public class BikeFeature
    {
        public uint VersionId { get; set; }
        public string Speedometer { get; set; }
        public bool? Tachometer { get; set; }
        public string TachometerType { get; set; }
        public bool? ShiftLight { get; set; }
        public bool? ElectricStart { get; set; }
        public bool? Tripmeter { get; set; }
        public string NoOfTripmeters { get; set; }
        public string TripmeterType { get; set; }
        public bool? LowFuelIndicator { get; set; }
        public bool? LowOilIndicator { get; set; }
        public bool? LowBatteryIndicator { get; set; }
        public bool? FuelGauge { get; set; }
        public bool? DigitalFuelGauge { get; set; }
        public bool? PillionSeat { get; set; }
        public bool? PillionFootrest { get; set; }
        public bool? PillionBackrest { get; set; }
        public bool? PillionGrabrail { get; set; }
        public bool? StandAlarm { get; set; }
        public bool? SteppedSeat { get; set; }
        public bool? AntilockBrakingSystem { get; set; }
        public bool? Killswitch { get; set; }
        public bool? Clock { get; set; }
        public string Colors { get; set; }
    }
}
