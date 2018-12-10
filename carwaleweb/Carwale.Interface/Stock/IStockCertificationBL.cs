using Carwale.Entity.Stock.Certification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Stock
{
    public interface IStockCertificationBL
    {
        StockCertification GetCarCertification(int inquiryId, bool isDealer);
        void UploadCarExteriorImage(int certificationId, string imageUrl, string originalImgPath);
        string GetExteriorImagePath(string profileId, string originalImageUrl);
    }
}
