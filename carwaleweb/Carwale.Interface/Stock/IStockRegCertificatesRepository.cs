
namespace Carwale.Interfaces.Stock
{
    public interface IStockRegCertificatesRepository
    {
        int AddStockRegCertificate(int inquiryId, string hostUrl, string originalImgPath);
        void DeleteStockRegCertificate(int inquiryId, int rcId);
        string GetStockRegCertificate(int inquiryId);
    }
}