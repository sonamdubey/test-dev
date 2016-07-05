using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using System;

/// <summary>
/// Added By : Suresh Prajapati on 31st Dec 2014
/// </summary>
namespace BikeWale.Entities.AutoBiz
{
    public class BookingAmountEntity
    {
        public BookingAmountEntityBase objBookingAmountEntityBase { get; set; }
        public NewBikeDealers objDealer { get; set; }
        public BikeMakeEntityBase objMake { get; set; }
        public BikeModelEntityBase objModel { get; set; }
        public BikeVersionEntityBase objVersion { get; set; }
    }

    public class MakeEntityBase
    {
        public UInt32 MakeId { get; set; }
        public string MakeName { get; set; }
        public string MaskingName { get; set; }
    }

    public class ModelEntityBase
    {
        public UInt32 ModelId { get; set; }
        public string ModelName { get; set; }
        public string MaskingName { get; set; }
    }

    public class VersionEntityBase
    {
        public UInt32 VersionId { get; set; }
        public string VersionName { get; set; }
    }
}