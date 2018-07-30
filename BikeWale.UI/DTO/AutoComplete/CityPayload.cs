
namespace Bikewale.DTO.AutoComplete
{
    /// <summary>
    /// Created by  :   Sumit Kate on 06 May 2016
    /// Description :   City Payload DTO
    /// </summary>
    public class CityPayload
    {
        public uint cityId { get; set; }
        public string cityMaskingName { get; set; }
        public bool isDuplicate { get; set; }
    }
}
