using Carwale.Entity.Classified.CarDetails;
using System;
using System.Collections.Generic;

namespace Carwale.Interfaces.Stock
{
    public interface IStockBL
    {
        CarDetailsEntity GetStock(string profileId);
        CarDetailsEntity GetStock(int inquiryId, bool isDealer);
        Tuple<List<Features>, List<Specification>> GetSpecificationsAndFeatures(int versionId);
        string GetDeliveryText(int deliveryCityId);
        void RefreshESStockOfDealer(int dealerId);
        void RefreshESStocksOfCertProg(int certificationId);
        void RefreshESStock(string profileId);
        void PublishToESQueue(IEnumerable<string> profileIds);
        string GetDetailsPageUrlFromRegistrationNumber(string regNo);
    }
}
