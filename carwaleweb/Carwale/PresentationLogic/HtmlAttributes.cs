using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carwale.UI.PresentationLogic
{
    public class HtmlAttributes
    {
        public static string GetTopBodyTypeLinkText(int carCount, CarBodyStyle bodyStyle) {
            return string.Format("Top{0} {1}s in India",carCount < 10 ? string.Empty : " " + carCount,bodyStyle.ToFriendlyString());
        }

        public static string GetTopBodyTypeLinkTitle(int carCount, CarBodyStyle bodyStyle)
        {
            return string.Format("Best {0}s in India - {2} | Top{1} {0}s", bodyStyle.ToFriendlyString(), carCount < 10 ? string.Empty : " "+carCount, DateTime.Now.ToString("Y"));
        }
		public static string GetTopBodyTypeAndBudgetLinkTitle(string budgetText, CarBodyStyle bodyStyle)
		{
			return string.Format("Best {0}s under {1}", bodyStyle.ToFriendlyString(), budgetText);
		}
	}
}