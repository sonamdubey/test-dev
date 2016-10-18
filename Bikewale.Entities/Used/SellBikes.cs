﻿using System;

namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 13 Oct 2016
    /// Summary: Entity to hold save inquery details
    /// </summary>
    public class SellBikeAd
    {
        public uint InquiryId { get; set; }
        public BikeData.BikeMakeEntityBase Make { get; set; }
        public BikeData.BikeModelEntityBase Model { get; set; }
        public BikeData.BikeVersionEntityBase Version { get; set; }
        public DateTime ManufacturingYear { get; set; }
        public uint KiloMeters { get; set; }
        public uint CityId { get; set; }
        public UInt64 Expectedprice { get; set; }
        public ushort Owner { get; set; }
        public string RegistrationPlace { get; set; }
        public string Color { get; set; }
        public uint ColorId { get; set; }
        public ushort SourceId { get; set; }
        public string ClientIp { get; set; }
        public string PageUrl { get; set; }
        public SellAdStatus Status { get; set; }
        public SellerEntity Seller { get; set; }
        public SellBikeAdOtherInformation OtherInfo { get; set; }
    }
    /// <summary>
    /// Created by  :   Sumit Kate on 14 Oct 2016
    /// Description :   Sell Bike Ad Other Information entity
    /// </summary>
    public class SellBikeAdOtherInformation
    {
        public string RegistrationNo { get; set; }
        public string InsuranceType { get; set; }
        public string AdDescription { get; set; }

    }

    /// <summary>
    /// Created by  :   Sumit Kate on 14 COt 2016
    /// Description :   Seller Entity
    /// </summary>
    public class SellerEntity : Customer.CustomerEntityBase
    {
        public SellerType SellerType { get; set; }
        public string Otp { get; set; }
    }
}
