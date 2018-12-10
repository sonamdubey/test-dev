using System;
namespace Carwale.Entity.CarData
{
    [Serializable]
    public class CarVersions
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public int ModelId { get; set; }
        public string SpecsSummary { get; set; } = string.Empty;
        public string MaskingName { get; set; }
        public bool New { get; set; }
        public bool Futuristic { get; set; }
        public bool IsSpecsExist { get; set; }
        public float ReviewRate { get; set; }
        public double MinPrice { get; set; }
        public int MinAvgPrice { get; set; }
        public int ReviewCount { get; set; }
        public int BodyStyleId { get; set; }
        public int TransmissionTypeId { get; set; }
        public string TransmissionType { get; set; }
        public int FuelTypeId { get; set; }
        public string CarFuelType { get; set; }
        public string Displacement { get; set; }
        public double Arai { get; set; }
        public string MileageUnit { get; set; }
        public DateTime LaunchDate { get; set; }
        public string VersionMaskingName { get; set; }
        public string Drivetrain { get; set; }
        public string SeatingCapacity { get; set; }
        public string MaxPower { get; set; }
    }

    public class CarVersionsV1
    {
        public int Id;
        public string Version;
        public int ModelId;
        public string SpecsSummary;
        public string MaskingName;
        public bool New;
        public bool Futuristic;
        public bool IsSpecsExist;
        public float ReviewRate;
        public double MinPrice;
        public int MinAvgPrice;
        public int ReviewCount;
        public int BodyStyleId;
        public int TransmissionTypeId;
        public string TransmissionType;
        public string CarFuelType;
        public int FuelTypeId;
        public DateTime LaunchDate;
        public string VersionMaskingName;
    }
}
