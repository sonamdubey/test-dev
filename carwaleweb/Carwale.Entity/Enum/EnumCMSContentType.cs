using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity
{
    public enum OrderBy
    {
        Date = 1,
        Views = 2
    }

    public enum EnumMobileVerificationSourceId
    {
        Carwale_Seller_Verification = 1,
        Carwale_Mobile_Seller_Verification = 2,
        Carwale_Mobile_Buyer_Verification = 3,
        Carwale_Buyer_Verification = 4,
        Carwale_Android_Buyer_Verification = 5,
        Carwale_Android_Seller_Verification = 6,
        Carwale_PQ_Car_Loan_Verification = 7,
        Carwale_PQ_Auto_Dealer_Verification = 8
    }


    public enum EnumGenericContentType
    {
        news = 1,
        videos = 2,
        expertreviews = 3,
        userreviews = 4,
        features = 5,
        galleries = 6
    }
    public enum EditCMSCacheCategoryEnum
    {
        News = 0,
        ExpertReviews = 1,
        Features = 2,
        Videos = 3,
        All = 4
    }

}
