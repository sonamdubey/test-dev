using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Carwale.Entity.Classified
{
    [Serializable, JsonObject]
    public class StockParamCount
    {
        [JsonProperty]
        public FilterBy objFilterBy { get; set; }

        [JsonProperty]
        public FuelType objFuel { get; set; }

        [JsonProperty]
        public BodyType objBodyType { get; set; }

        [JsonProperty]
        public SellerType objSeller { get; set; }

        [JsonProperty]
        public Owners objOwners { get; set; }

        [JsonProperty]
        public City objCity { get; set; }

        [JsonProperty]
        public int TotalStock { get; set; }
    }

    [Serializable, JsonObject]
    public class City
    {
        [JsonProperty]
        public int CityId { get; set; }

        [JsonProperty]
        public int CityCount { get; set; }

        [JsonProperty]
        public string CityName { get; set; }
    }

    [Serializable, JsonObject]
    public class FilterBy
    {
        [JsonProperty]
        public int CertifiedCars { get; set; }      // All Certified Cars

        [JsonProperty]
        public int CarsWithPhotos { get; set; }

        [DataMember]
        public int FranchiseCars { get; set; }
    }

    /// <summary>
    /// Created By : Supriya Bhide on 1st April 2015
    /// Description : New entity added for additional filters
    /// </summary>
    [Serializable, JsonObject]
    public class FilterByAdditional : FilterBy
    {
        [JsonProperty]
        public int OtherCertifiedCars { get; set; }     // Cars excluding Absure Certified cars

        [JsonProperty]
        public int CarWaleCertifiedCars { get; set; }   // Only Absure Certified Cars

        [JsonProperty]
        public int CarWaleGuaranteeCars { get; set; }   // Only Carwale Guarantee Cars

        [JsonProperty]
        public int CarWaleInspectedCars { get; set; }

        [JsonProperty]
        public int CarTradeCertifiedCars { get; set; }
    }
    
    [Serializable, JsonObject]
    public class FuelType
    {
        [JsonProperty]
        public int Petrol { get; set; }

        [JsonProperty]
        public int Diesel { get; set; }

        [JsonProperty]
        public int CNG { get; set; }

        [JsonProperty]
        public int Electric { get; set; }

        [JsonProperty]
        public int LPG { get; set; }

        [JsonProperty]
        public int Hybrid { get; set; }
    }

    [Serializable, JsonObject]
    public class BodyType
    {
        [JsonProperty]
        public int Hatchback { get; set; }

        [JsonProperty]
        public int Coupe { get; set; }

        [JsonProperty]
        public int Suv { get; set; }

        [JsonProperty]
        public int Minivan { get; set; }

        [JsonProperty]
        public int Truck { get; set; }

        [JsonProperty]
        public int StationWagon { get; set; }

        [JsonProperty]
        public int Sedan { get; set; }

        [JsonProperty]
        public int Convertible { get; set; }
    }

    [Serializable, JsonObject]
    public class SellerType
    {
        [JsonProperty]
        public int Individual { get; set; }

        [JsonProperty]
        public int Dealer { get; set; }
    }

   [Serializable, JsonObject]
    public class Owners
    {
        [JsonProperty]
        public int First { get; set; }

        [JsonProperty]
        public int Second { get; set; }

        [JsonProperty]
        public int ThirdAndAbove { get; set; }

        [JsonProperty]
        public int Unregistered { get; set; }
    }

    [Serializable, JsonObject]
    public class StockCount
    {
        [JsonProperty]
        public int TotalStockCount { get; set; }
    }

    [Serializable, JsonObject]
    public class Transmission
    {
        [JsonProperty]
        public int Manual { get; set; }

        [JsonProperty]
        public int Automatic { get; set; }
    }

    [Serializable, JsonObject]
    public class AvailableColors
    {
        [JsonProperty]
        public int Beige { get; set; }

        [JsonProperty]
        public int Black { get; set; }

        [JsonProperty]
        public int Blue { get; set; }

        [JsonProperty]
        public int Brown { get; set; }

        [JsonProperty]
        public int GoldNYellow { get; set; }

        [JsonProperty]
        public int Green { get; set; }

        [JsonProperty]
        public int Grey { get; set; }

        [JsonProperty]
        public int Maroon { get; set; }

        [JsonProperty]
        public int Purple { get; set; }

        [JsonProperty]
        public int Red { get; set; }

        [JsonProperty]
        public int Silver { get; set; }

        [JsonProperty]
        public int White { get; set; }

        [JsonProperty]
        public int Others { get; set; }
    }

    public class Area
    {
        public int AreaId { get; set; }
        public int AreaCount { get; set; }
    }
} //namespace
