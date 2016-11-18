
namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by  :   Sumit Kate on 16 Sep 2016
    /// Description :   Make Masking Response entity
    /// </summary>
    public class MakeMaskingResponse
    {
        public uint MakeId { get; set; }
        public string MaskingName { get; set; }
        public ushort StatusCode { get; set; }
    }
}
