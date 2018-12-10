using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using Carwale.Entity.ViewModels;
using System;
using System.Collections.Generic;

namespace Carwale.Interfaces.Classified
{
    public interface ICommonOperationsRepository
    {
        IList<CarMakeEntityBase> GetLiveListingMakes();
        IList<DealerCityEntity> GetLiveListingCities();
        string GetMakeRootName(string makesList, string rootsList);
        UsedCarModel GetUsedCarListings();      
        void SendErrorMail(Exception ex, string methodName);
        CarModelMaskingResponse GetMakeDetailsByRootName(string rootName);
    }
}
