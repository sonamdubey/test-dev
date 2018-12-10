using Carwale.Entity.Classified.Leads;
using Carwale.Entity.Enum;

namespace Carwale.Interfaces.Classified.MyListings
{
    public interface IPremiumListingsLogic
    {
        bool ValidateUser(Seller seller);
        int GetPackageIdFromType(ClassifiedPackageType packageType);
    }
}
