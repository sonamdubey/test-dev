using System;

namespace Carwale.DTOs.Stock
{
    public class StockDTO
    {
        public int InquiryId { get; set; }
        public bool IsDealer { get; set; }

        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public string CityName { get; set; }
        
        public DateTime MfgDate { get; set; }
        public int Price { get; set; }
        public int Kilometers { get; set; }
        public string Owners { get; set; }
        
        public string Color { get; set; }
        public string Transmission { get; set; }
        public string Fuel { get; set; }
        public string AdditionalFuel { get; set; }
        public string RegistrationNumber { get; set; }
        
        public int DealerId { get; set; }
        public int StockId { get; set; }
        public int ImageCount { get; set; }

        public int PackageId { get; set; }
        public DateTime? PackageStartDate { get; set; }
    }
}
