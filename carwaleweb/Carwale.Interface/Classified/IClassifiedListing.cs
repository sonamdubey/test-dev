using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Classified;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace Carwale.Interfaces.Classified
{
    public interface IClassifiedListing
    {
        List<StockSummary> GetSimilarUsedModels(int modelId);
        List<StockSummary> GetLuxuryCarRecommendations(int carId, int dealerId, int pageId);
    }//class
}//namespace
