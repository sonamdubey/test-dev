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
        class Specifications
        {

            public float Displacement { get; set; }
            public float MaxPower { get; set; }
            public float MaximumTorque { get; set; }
            public ushort NoOfGears { get; set; }
            public float FuelTankCapacity { get; set; }
            public uint TopSpeed { get; set; }
        }

        class Features
        {

            public string Speedometer { get; set; }
            public bool FuelGauge { get; set; }
            public string TachometerType { get; set; }
            public bool DigitalFuelGauge { get; set; }
            public bool ElectricStart { get; set; }
            public bool Tripmeter { get; set; }
        }

    }

}
