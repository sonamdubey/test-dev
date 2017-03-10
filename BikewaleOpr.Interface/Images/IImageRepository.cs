using BikewaleOpr.Entities.Images;

namespace BikewaleOpr.Interface.Images
{
    public interface IImageRepository
    {
        ulong Add(Image image);
    }
}
