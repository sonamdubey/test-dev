
using BikewaleOpr.Entities.BikeColorImages;
namespace BikewaleOpr.Interface.BikeColorImages
{
    /// <summary>
    /// Created By :- Subodh Jain 09 jan 2017
    /// Summary :- Bikes Images Details 
    /// </summary>
    public interface IColorImagesBikeRepository
    {
        uint FetchPhotoId(ColorImagesBikeEntities objBikeColorDetails);
        bool DeleteBikeColorDetails(uint modelid);
    }
}
