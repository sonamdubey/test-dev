
namespace BikewaleOpr.DTO.BikeData
{
    /// <summary>
    /// Created by sajal Gupta on 22-11-2017
    /// Desc : Added dto to add make footer
    /// </summary>
    public class MakeFooterDto
    {
        public uint MakeId { get; set; }
        public uint CategoryId { get; set; }
        public string CategoryDescription { get; set; }
        public string UserId { get; set; }
    }
}
