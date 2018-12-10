using Carwale.Entity.PriceQuote;

namespace Carwale.Interfaces.PriceQuote
{
    public interface IThirdPartyEmiDetailsRepository
    {
        ThirdPartyEmiDetails Get(int carVersionId, bool isMetallic);
    }
}
