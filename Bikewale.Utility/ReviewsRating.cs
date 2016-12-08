﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public static class ReviewsRating
    {
        static readonly string fullStar = "star-one-icon";
        static readonly string halfStar = "star-half-icon";
        static readonly string noStar = "star-zero-icon";

        static readonly string ZeroRating = String.Format("<span class='{0}'></span><span class='{0}'></span><span class='{0}'></span><span class='{0}'></span><span class='{0}'></span>",noStar);
        static readonly string HalfRating = String.Format("<span class='{1}'></span><span class='{0}'></span><span class='{0}'></span><span class='{0}'></span><span class='{0}'></span>", noStar,halfStar);
        static readonly string OneHalfRating = String.Format("<span class='{1}'></span><span class='{2}'></span><span class='{0}'></span><span class='{0}'></span><span class='{0}'></span>", noStar, fullStar, halfStar);
        static readonly string OneRating = String.Format("<span class='{1}'></span><span class='{0}'></span><span class='{0}'></span><span class='{0}'></span><span class='{0}'></span>", noStar, fullStar);
        static readonly string TwoRating = String.Format("<span class='{1}'></span><span class='{0}'></span><span class='{0}'></span><span class='{0}'></span><span class='{0}'></span>", noStar, fullStar);
        static readonly string TwoHalfRating = String.Format("<span class='{1}'></span><span class='{1}'></span><span class='{2}'></span><span class='{0}'></span><span class='{0}'></span>", noStar, fullStar, halfStar);
        static readonly string ThreeRating = String.Format("<span class='{1}'></span><span class='{1}'></span><span class='{1}'></span><span class='{0}'></span><span class='{0}'></span>", noStar, fullStar);
        static readonly string ThreeHalfRating = String.Format("<span class='{1}'></span><span class='{1}'></span><span class='{1}'></span><span class='{2}'></span><span class='{0}'></span>", noStar, fullStar, halfStar);
        static readonly string FourRating = String.Format("<span class='{1}'></span><span class='{1}'></span><span class='{1}'></span><span class='{1}'></span><span class='{0}'></span>", noStar, fullStar);
        static readonly string FourHalfRating = String.Format("<span class='{1}'></span><span class='{1}'></span><span class='{1}'></span><span class='{1}'></span><span class='{2}'></span>", noStar, fullStar, halfStar);
        static readonly string FiveRating = String.Format("<span class='{1}'></span><span class='{1}'></span><span class='{1}'></span><span class='{1}'></span><span class='{1}'></span>", noStar, fullStar);



        public static string GetRateImage(double value)
        {
         /*   string oneImg = "<img src='http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/d/1.png'>";
            string zeroImg = "<img src='http://imgd2.aeplcdn.com/0x0/bw/static/design15/old-images/d/0.png'>";
            string halfImg = "<img src='http://imgd3.aeplcdn.com/0x0/bw/static/design15/old-images/d/half.png'>";

            StringBuilder sb = new StringBuilder();
            int absVal = (int)Math.Floor(value);

            if (value == 0.0d)
            {
                return "Not rated yet";
            }

            int i;
            for (i = 0; i < absVal; i++)
                sb.Append(oneImg);

            if (value > absVal)
                sb.Append(halfImg);
            else
                i--;

            for (int j = 5; j > i + 1; j--)
                sb.Append(zeroImg);

            return sb.ToString();*/

          int absVal = (int)Math.Floor(value);

          switch(absVal)
          {

            case 0:
              if (value > absVal)
                return HalfRating;
              else
                return ZeroRating;

            case 1:
              if (value > absVal)
                return OneHalfRating;
              else
                return OneRating;
              
            case 2:
              if (value > absVal)
                return TwoHalfRating;
              else
                return TwoRating;
              

            case 3:
              if (value > absVal)
                return ThreeHalfRating;
              else
                return ThreeRating;
              

            case 4:
              if (value > absVal)
                return FourHalfRating;
              else
                return FourRating;
              
            case 5:
              return FiveRating;

            default:
              return "Not rated yet";
          }


        }

    }
}
