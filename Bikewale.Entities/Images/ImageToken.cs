
namespace Bikewale.Entities.Images
{
    /// <summary>
    /// Created by  :   Sumit Kate on 15 Nov 2016
    /// Description :   Image Upload Token Entity
    /// </summary>
    public class ImageToken : AWS.Token
    {
        public uint Id { get; set; }
        public bool Status { get; set; }
        public bool ServerError { get; set; }
        public uint PhotoId { get; set; }
        public string OriginalImagePath { get; set; }
    }
}
