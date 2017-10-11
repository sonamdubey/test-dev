/// <summary>
/// Created by : Vivek Singh Tomar on 19th Sep 2017
/// Summary : Entity to author details
/// </summary>
namespace Bikewale.Entities.Authors
{
    public class AuthorEntity: AuthorEntityBase
    {
        public string FullDescription { get; set; }
        public string EmailId { get; set; }
        public string FacebookProfile { get; set; }
        public string GooglePlusProfile { get; set; }
        public string LinkedInProfile { get; set; }
        public string TwitterProfile { get; set; }
    }
}
