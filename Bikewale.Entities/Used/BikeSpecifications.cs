using System;

namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created By : Sushil Kumar on 27th August 2016
    /// Description : Bike specifications entity for used bikes
    /// </summary>
    [Serializable]
    public class BikeSpecifications
    {

        #region Specifications
        public float Displacement { get; set; }
        public float MaxPower { get; set; }
        public float MaxPowerRPM { get; set; }
        public float MaximumTorque { get; set; }
        public float MaximumTorqueRPM { get; set; }
        public ushort FuelEfficiencyOverall { get; set; }
        public ushort NoOfGears { get; set; }
        public string BrakeType { get; set; }
        #endregion

        #region Features
        public string Speedometer { get; set; }
        public bool FuelGauge { get; set; }
        public string TachometerType { get; set; }
        public bool DigitalFuelGauge { get; set; }
        public bool ElectricStart { get; set; }
        public bool Tripmeter { get; set; }
        #endregion

    }

}
