namespace Bikewale.Entities.Authors
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 20-Sep-2017
    /// Description : Entity Base for Author.
    /// </summary>
    public class AuthorEntityBase
    {
        public uint AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorFirstName { get; set; }
        public string ProfileImage { get; set; }
        public string Designation { get; set; }
        public string ShortDescription { get; set; }
        public string MaskingName { get; set; }
        public string HostUrl { get; set; }
        public string ImageName { get; set; }
    }
}