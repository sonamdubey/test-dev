using System;

/// <summary>
/// Added By : Suresh Prajapati on 31st Dec 2014
/// </summary>
namespace BikeWale.DTO.AutoBiz
{
    public class BookingAmountEntityDTO
    {
        public BookingAmountEntityBaseDTO objBookingAmountEntityBase { get; set; }
        public NewBikeDealersDTO objDealer { get; set; }
        public MakeEntityBaseDTO objMake { get; set; }
        public ModelEntityBaseDTO objModel { get; set; }
        public VersionEntityBaseDTO objVersion { get; set; }
    }

    public class MakeEntityBaseDTO
    {
        public UInt32 MakeId { get; set; }
        public string MakeName { get; set; }
        public string MaskingName { get; set; }
    }

    public class ModelEntityBaseDTO
    {
        public UInt32 ModelId { get; set; }
        public string ModelName { get; set; }
        public string MaskingName { get; set; }
    }

    public class VersionEntityBaseDTO
    {
        public UInt32 VersionId { get; set; }
        public string VersionName { get; set; }
    }


}