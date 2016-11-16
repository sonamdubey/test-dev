
namespace Bikewale.Entities.AWS
{
    /// <summary>
    /// Created by  :   Sumit Kate on 15 Nov 2016
    /// Description :   AWS Image upload token
    /// </summary>
    public class Token
    {
        public string Key { get; set; }
        public string URI { get; set; }
        public string AccessKeyId { get; set; }
        public string Policy { get; set; }
        public string Signature { get; set; }
    }
}
