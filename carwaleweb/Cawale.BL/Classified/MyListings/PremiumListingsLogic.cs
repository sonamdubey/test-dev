using Carwale.Entity.Classified.Leads;
using Carwale.Entity.Enum;
using System;
using System.Configuration;
using Carwale.Interfaces.Classified.MyListings;
using Carwale.Interfaces.Classified.SellCar;

namespace Carwale.BL.Classified.MyListings
{
    public class PremiumListingsLogic : IPremiumListingsLogic
    {   
        public bool ValidateUser(Seller seller)
        {
            if (seller == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(seller.Name) || !Utility.RegExValidations.IsValidEmail(seller.Email) || !Utility.RegExValidations.IsValidMobile(seller.Mobile)
                || string.IsNullOrEmpty(seller.Address))
            {
                return false;
            }
            return true;
        }

        public int GetPackageIdFromType(ClassifiedPackageType packageType)
        {
            switch (packageType)
            {
                case ClassifiedPackageType.AssistedSales:
                    return 115;
                default:
                    return 0;
            }
        }
    }
}
