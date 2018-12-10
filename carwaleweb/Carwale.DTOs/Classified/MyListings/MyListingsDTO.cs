using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;

namespace Carwale.DTOs.Classified.MyListings
{
    public class MyListingsDTO
    {
        public int Id { get; set; }
        public string CarName { get; set; }
        public int MakeYear { get; set; }
        public int MakeMonth { get; set; }
        public string MakeMonthYearFormatted { get; set; }
        public string Price { get; set; }
        public string Kilometers { get; set; }
        public string KilometersFormatted { get; set; }
        public string ImageURL { get; set; }
        public int PhotoCount { get; set; }
        public string ClassifiedExpiryDate { get; set; }
        public string Status { get; set; }
        public bool IsListingCompleted { get; set; }
        public int TotalInq { get; set; }
        public List<Carwale.DTOs.CarData.Color> CarColors { get; set; }
        public List<CarVersionEntity> OtherCarVersions { get; set; }
        public Dictionary<int, string> Owners { get; set; }
        public short? SelectedOwner { get; set; }
        public Dictionary<int, string> Insurance { get; set; }
        public string SelectedInsurance { get; set; }
        public List<int> InsuranceYears { get; set; }
        public Dictionary<int, string> InsuranceMonths { get; set; }
        public string PriceFormatted { get; set; }
        public string RegistrationNumber { get; set; }
        public string SelectedRegType { get; set; }
        public Dictionary<int, string> RegType { get; set; }
        public bool IsImagePendingToApprove { get; set; }
        public Platform Source { get; set; }
        public ClassifiedPackageType PackageType { get; set; }
    }
}
