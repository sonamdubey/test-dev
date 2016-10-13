using System;

namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 13 Oct 2016
    /// Summary: Entity to hold save inquery details
    /// </summary>
    public class SellBikeAd
    {
        public uint InquiryId { get; set; }
        public ushort VersionId { get; set; }
        public DateTime ManufacturingYear { get; set; }
        public uint KiloMeters { get; set; }
        public uint CityId { get; set; }
        public UInt64 Expectedprice { get; set; }
        public ushort Owner { get; set; }
        public string RegistrationPlace { get; set; }
        public string Color { get; set; }
        public bool SellerType { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string Mobile { get; set; }


        public ushort SourceId { get; set; }
        public string ClientIp { get; set; }
        public string CustomerId { get; set; }
        public ushort StatusId { get; set; }

        public SellBikeAdOtherInformation OtherInfo { get; set; }
    }

    public class SellBikeAdOtherInformation
    {
        public string RegistrationNo { get; set; }
        public string InsuranceType { get; set; }
        public string AdDescription { get; set; }

    }

}
