using Carwale.Entity.Classified.Leads;
using Carwale.Interfaces.Dealers.Used;
using Carwale.Utility;
using System.Text.RegularExpressions;

namespace Carwale.BL.Dealers.Used
{
    public class UsedDealerShowroomBL
    {
        public static string GetDealerShowroomUrl(string cityName, string sellerName, int sellerId)
        {
            return string.Format("/used/dealers-in-{0}/{1}-{2}/", Format.FormatURL(cityName), Format.FormatURL(sellerName), sellerId.ToString());
        }
    }
}
