using Carwale.Entity.Accessories.Tyres;

namespace Carwale.Interfaces.Accessories.Tyres
{
    public interface IAccessoryRepo
    {
        ItemData GetAccessoryDataByItemId(int itemId);
    }
}
