using System;
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
//        #region ratingsStaticOld
//        static readonly string ZeroRating = @"<img src='http://img.aeplcdn.com/images/ratings/0.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/0.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/0.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/0.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/0.png'>";

//        static readonly string HalfRating = @"<img src='http://img.aeplcdn.com/images/ratings/half.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/0.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/0.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/0.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/0.png'>";

//        static readonly string OneRating = @"<img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                          <img src='http://img.aeplcdn.com/images/ratings/0.png'>
//                                          <img src='http://img.aeplcdn.com/images/ratings/0.png'>
//                                          <img src='http://img.aeplcdn.com/images/ratings/0.png'>
//                                          <img src='http://img.aeplcdn.com/images/ratings/0.png'>";

//        static readonly string OneHalfRating = @"<img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                              <img src='http://img.aeplcdn.com/images/ratings/half.png'>
//                                              <img src='http://img.aeplcdn.com/images/ratings/0.png'>
//                                              <img src='http://img.aeplcdn.com/images/ratings/0.png'>
//                                              <img src='http://img.aeplcdn.com/images/ratings/0.png'>";

//        static readonly string TwoRating = @"<img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                          <img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                          <img src='http://img.aeplcdn.com/images/ratings/0.png'>
//                                          <img src='http://img.aeplcdn.com/images/ratings/0.png'>
//                                          <img src='http://img.aeplcdn.com/images/ratings/0.png'>";

//        static readonly string TwoHalfRating = @"<img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                              <img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                              <img src='http://img.aeplcdn.com/images/ratings/half.png'>
//                                              <img src='http://img.aeplcdn.com/images/ratings/0.png'>
//                                              <img src='http://img.aeplcdn.com/images/ratings/0.png'>";


//        static readonly string ThreeRating = @"<img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/0.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/0.png'>";

//        static readonly string ThreeHalfRating = @"<img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                                <img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                                <img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                                <img src='http://img.aeplcdn.com/images/ratings/half.png'>
//                                                <img src='http://img.aeplcdn.com/images/ratings/0.png'>";

//        static readonly string FourRating = @"<img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/0.png'>";

//        static readonly string FourHalfRating = @"<img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                                <img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                                <img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                                <img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                                <img src='http://img.aeplcdn.com/images/ratings/half.png'>";

//        static readonly string FiveRating = @"<img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/1.png'>
//                                            <img src='http://img.aeplcdn.com/images/ratings/1.png'>"; 
//        #endregion

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
         /*   string oneImg = "<img src='http://img.aeplcdn.com/images/ratings/1.png'>";
            string zeroImg = "<img src='http://img.aeplcdn.com/images/ratings/0.png'>";
            string halfImg = "<img src='http://img.aeplcdn.com/images/ratings/half.png'>";

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
