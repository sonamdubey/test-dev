
using Bikewale.Entities.Used;
namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Sep 2016
    /// Description :   Used Bike Buyer Business Layer Interface
    /// </summary>
    public interface IUsedBikeBuyer
    {
        bool UploadPhotosRequest(PhotoRequest request);
    }
}
