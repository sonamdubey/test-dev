using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Enum
{
    public class EditorialEnum
    {
        public enum PurchasedAs
        {
            New = 1,
            Used, 
            NotPurchased
        }

        public enum RatingQuestion
        {
            PurchasedAs =1,
            Familiarity
        }

        public enum ReviewQuestions
        {
            ExteriorAndStyle = 1,
            ComfortAndSpace,
            Performance,
            FuelEconomy,
            Value
        }
        public enum ReviewRating
        {
            Poor = 1,
            Fair = 2,
            Good = 3,
            VeryGood = 4,
            Excellent = 5
        }
    }
}
