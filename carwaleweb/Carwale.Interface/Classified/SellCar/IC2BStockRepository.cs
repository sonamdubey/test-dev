using Carwale.Entity.Classified.SellCarUsed;
using System;
using System.Collections.Generic;

namespace Carwale.DAL.Classified.SellCar
{
    public interface IC2BStockRepository
    {
        C2BStockDetails GetC2BStockDetails(int inquiryId);
        void SellCarHotLead(int inquiryId, int hotLeadPrice);
        void LogC2BApiErrors(int inquiryId,int tempId,List<ValidationErrors> errors);
        void RemoveC2BApiError(int inquiryId);
        void ResendFailedC2BStocks();
    }
}
