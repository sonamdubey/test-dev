using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public static class ReviewsRating
    {

        public static string GetRateImage(double value)
        {
            string oneImg = "<img src='/images/ratings/1.png'>";
            string zeroImg = "<img src='/images/ratings/0.png'>";
            string halfImg = "<img src='/images/ratings/half.png'>";

            StringBuilder sb = new StringBuilder();
            int absVal = (int)Math.Floor(value);

            int i;
            for (i = 0; i < absVal; i++)
                sb.Append(oneImg);

            if (value > absVal)
                sb.Append(halfImg);
            else
                i--;

            for (int j = 5; j > i + 1; j--)
                sb.Append(zeroImg);

            return sb.ToString();
        }

    }
}
