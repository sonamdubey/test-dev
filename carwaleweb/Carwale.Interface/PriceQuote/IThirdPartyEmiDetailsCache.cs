using Carwale.Entity.PriceQuote;

namespace Carwale.Interfaces.PriceQuote
{
    public interface IThirdPartyEmiDetailsCache
    {
        ThirdPartyEmiDetails Get(int carVersionId, bool isMetallic);
    }
}
