using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public class ReviewRating
    {
        public static string GetRateImage(double value)
        {
            string oneImg = "<img src=\"http://img.aeplcdn.com/images/ratings/1.gif\">";
            string zeroImg = "<img src=\"http://img.aeplcdn.com/images/ratings/0.gif\">";
            string halfImg = "<img src=\"http://img.aeplcdn.com/images/ratings/half.gif\">";

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
