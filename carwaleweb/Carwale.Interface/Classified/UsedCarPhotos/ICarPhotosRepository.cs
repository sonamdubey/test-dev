using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Classified;

namespace Carwale.Interfaces.Classified.UsedCarPhotos
{
    public interface ICarPhotosRepository
    {
        List<CarPhoto> GetCarPhotos(int inquiryId, bool isDealer);
        int SaveCarPhotos(CarPhoto carPhoto);
        bool UpdateMainImage(int inquiryId, int photoId, bool isDealer);
        bool RemoveCarPhoto(int inquiryId, int photoId);
    }
}
