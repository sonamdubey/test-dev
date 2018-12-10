using Carwale.Entity.Classified;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Carwale.Entity.Elastic
{
    [Serializable]
    public class SellCarEntityElastic
    {
        /// <summary>
        /// Added Class for Sell car entity for passing object of this class as parameter in API. || ELasticSearch
        /// Added By Jugal || 24 Nov 2014
        /// </summary>
        
        // Used Car Props

        public string ProfileId { get; set; }
        public string CarName { get; set; }
        public string MakeId { get; set; }
        public string MakeName { get; set; }
        public string MakeYear { get; set; }
        public string RootId { get; set; }
        public string RootName { get; set; }
        public string Comments { get; set; }
        public string FrontImagePath { get; set; }
        public int InquiryId { get; set; }
        public string Seller { get; set; }
        public string SellerType { get; set; }//Individual
        public int PhotoCount { get; set; }
        public string Price { get; set; }
        public string Km { get; set; }
        public string Url { get; set; }

        public string SourceId { get; set; }
        public string CustomerId { get; set; }
        public string ModelId { get; set; }
        public string ModelName { get; set; }
        public string VersionId { get; set; }
        public string VersionName { get; set; }

        public string MaskingName { get; set; }
        public string Color { get; set; }
        public string InteriorColor { get; set; }
        public string InteriorColorCode { get; set; }

        public string Fuel { get; set; }
        public string AdditionalFuel { get; set; }//Petrol
        public string GearBox { get; set; }
        public string SellerNote { get; set; }
        public int VideoCount { get; set; }
        public string OfferStartDate { get; set; }
        public string OfferEndDate { get; set; }
        public string LastUpdatedOn { get; set; }
        public string AreaId { get; set; }
        public string AreaName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public long CertificationId { get; set; }
        public string CertifiedLogoUrl { get; set; }
        public string OwnerTypeId { get; set; }
        public string NoOfOwners { get; set; }
        public bool IsPremium { get; set; }
        public bool IsHotDeal { get; set; }
        public string Emi { get; set; }
        public string EmiUrl { get; set; }
        public string ApiFlag { get; set; }
        public string MakeMapping { get; set; }
        public string RootMapping { get; set; }
        public string UsedCarMasterColorId { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public string PackageType { get; set; }
        public string ImageUrlMedium { get; set; }
        
        public string InsertionDate { get; set; }
        public string Score { get; set; }
        public string SortScore { get; set; }
        
        
        // Installed Features Info
        //This is main coloumn consists DriverAirBag, PassengerAirBag, AntiLockBrakes,Immobilizer, ChildSafetyLocks, TractionControl
        public string Features_SafetySecurity { get; set; }

        public string AirConditioning { get; set; }
        // Comfort And Convenience
        //This is main coloumn consists AirConditioning, PowerSteering, PowerWindows,
        //PowerSeats, PowerDoorLocks, Defogger, CentralLocking, SteeringAdjustment, RemoteBoot, FuelLid
        public string Features_Comfort { get; set; }
        

        // Other Features 
        // This is main coloumn consists AllowWheels, RearWashWiper, AudioSystem, CupHolder, TubelessTyres, FogLights, LeatherSeats, TechoMeter
        public string Features_Others { get; set; }

        // Car Conditions
        public string AcConditions { get; set; }
        public string BatteryCondition { get; set; }
        public string BrakesCondition { get; set; }
        public string CarElectricals { get; set; }
        public string CarEngine { get; set; }
        public string SeatCondition { get; set; }
        public string Suspension { get; set; }
        public string TyresCondition { get; set; }
        public string InteriorCondition { get; set; }
        public string ExteriorCondition { get; set; }
        public string OverAllCarCondition { get; set; }

        // Other Information
        public string RegistrationNo { get; set; }
        public string RegistrationPlace { get; set; }
        public string Mileage { get; set; }
        public string ReasonForSelling { get; set; }
        public string AvailableWarranties { get; set; }
        public string MajorModifications { get; set; }
        public string SpecialNote { get; set; }
        public string CarMileage { get; set; }
        public string PinCode { get; set; }
        public string Insurance { get; set; }
        public string InsuranceExpiry { get; set; }
        public string Tax { get; set; }
        public string YouTubeVideo { get; set; }
        public string IsYouTubeVideoApproved { get; set; }

        //Extra Attributes
        public string IsArchived { get; set; }
        public string IsApproved { get; set; }
        public string IsFake { get; set; }
        public string StatusId { get; set; }
        public string ListInClassifieds { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEMail { get; set; }
        public string CustomerMobile { get; set; }
        public string CarRegState { get; set; }
        public string IsVerified { get; set; }
        public string Accessories { get; set; }
        public string drivenIn { get; set; }

        public long CarWithPhoto { get; set; }
        public long CertifiedFlag { get; set; }
    }
}
