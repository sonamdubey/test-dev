using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Utility
{
	public static class PageMapping
	{
		private static Dictionary<int, string> dict = new Dictionary<int, string> {
			{1,"HomePage"},
			{2,"NewCarSearchPage" },
			{201,"BestCarsPage"}
		};
		public static string GetPageNameById(int pageId)
		{
			if (dict.ContainsKey(pageId))
			{
				return dict[pageId];
			}
			return "HomePage";
		}

	}
}
