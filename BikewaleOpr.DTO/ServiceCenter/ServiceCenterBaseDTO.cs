namespace BikewaleOpr.DTO.ServiceCenter
{
    /// <summary>		
    /// Created By : Snehal Dange		
    /// Created On  : 29 July 2017		
    /// Description : Service center Details Dto.		
    /// </summary>		

    public class ServiceCenterBaseDTO
    {
        public uint Id { get; set; }		
        public string Name { get; set; }		
        public string Address { get; set; }		
        public string Phone { get; set; }		
        public string Mobile { get; set; }		
        public string Lattitude { get; set; }		
        public string Longitude { get; set; }		
        public string CityMaskingName { get; set; }		
        public string MakeMaskingName { get; set; }		
		
        public sbyte ActiveStatus { get; set; }
    }
}
