using Carwale.DTOs.Deals;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.PriceQuote;
using System;

namespace Carwale.DTOs.NewCars
{
    public class NewCarVersionsDTO
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public int ModelId { get; set; }
        public string SpecsSummary { get; set; }
        public string MaskingName { get; set; }
        public bool New { get; set; }
        public bool Futuristic { get; set; }
        public bool IsSpecsExist { get; set; }
        public float ReviewRate { get; set; }
        public double MinPrice { get; set; }
        public int ReviewCount { get; set; }
        public int BodyStyleId { get; set; }
        public string TransmissionType { get; set; }
        public int TransmissionTypeId { get; set; }
        public string CarFuelType { get; set; }
        public int FuelTypeId { get; set; }
        public DateTime LaunchDate { get; set; }
        public DiscountSummaryDTO DiscountSummary { get; set; }
        public PriceOverview CarPriceOverview { get; set; }
        public string VersionMaskingName { get; set; }
        public EmiCalculatorModelData EmiCalculatorModelData { get; set; }
        public string MaxPower { get; set; }
        public string Displacement { get; set; }
    }

    public class NewCarVersionsDTOV2
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public int ModelId { get; set; }
        public string SpecsSummary { get; set; }
        public string MaskingName { get; set; }
        public bool New { get; set; }
        public bool Futuristic { get; set; }
        public bool IsSpecsExist { get; set; }
        public float ReviewRate { get; set; }
        public double MinPrice { get; set; }
        public int ReviewCount { get; set; }
        public int BodyStyleId { get; set; }
        public string TransmissionType { get; set; }
        public int TransmissionTypeId { get; set; }
        public string CarFuelType { get; set; }
        public int FuelTypeId { get; set; }
        public DiscountSummaryDTO DiscountSummary { get; set; }
        public PriceOverviewDTOV2 CarPriceOverview { get; set; }
        public string VersionEmi { get; set; }
        public string VersionPriceQuote { get; set; }
        public string VersionMaskingName { get; set; }
        public EmiCalculatorModelData EmiCalculatorModelData { get; set; }
    }
}
