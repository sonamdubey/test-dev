namespace BikeWale.DTO.AutoBiz
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 6 Nov 2014
    /// </summary>
    public class DealerAreaDetailsDTO
    {
        public uint AreaId { get; set; }
        public string AreaName { get; set; }
        public uint CityId { get; set; }
        public string PinCode { get; set; }
        public uint DealerId { get; set; }
        public string DealerOrganization { get; set; }
        public uint DealerCount { get; set; }
        public uint DealerRank { get; set; }
        public string MakeName { get; set; }
    }
}
