using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Interfaces.PriceQuote;
using Carwale.Entity.PriceQuote;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.Dealers;
namespace Carwale.Interfaces.PriceQuote
{
    public interface IPQAdapter
    {
        List<T> GetPQByIds<T>(List<ulong> pqIdList) where T : new();
        List<T> GetPQByIds<T>(List<string> pqIdList, string userIdentifier) where T : new();
		List<T> GetPQ<T>(PQInput pqInputes, string userIdentifier);
		List<PQOnRoadPrice> GetPQ(int cityId, int versionId);
    }
}
