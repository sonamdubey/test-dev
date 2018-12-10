using Carwale.Entity.PriceQuote;

namespace Carwale.Interfaces.PriceQuote
{
    public interface IModelSimilarPriceDetailsRepo
    {
        bool Create(ModelSimilarPriceDetail modelSimilarPriceDetail);
        bool Update(ModelSimilarPriceDetail modelSimilarPriceDetail);
        ModelSimilarPriceDetail Get(int modelId);
    }
}
