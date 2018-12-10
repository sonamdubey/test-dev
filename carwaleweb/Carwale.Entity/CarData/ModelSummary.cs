using Carwale.Entity.Deals;
using Carwale.Entity.PriceQuote;
using System;

namespace Carwale.Entity.CarData
{
	[Serializable]
	public class ModelSummary
	{
		public int ModelId { get; set; }
		public string ModelName { get; set; }
		public string MaskingName { get; set; }
		public int MakeId { get; set; }
		public double ModelRating { get; set; }
		public int ReviewCount { get; set; }
		public string OriginalImage { get; set; }
		public string HostUrl { get; set; }
		public bool New { get; set; }
		public bool Futuristic { get; set; }
		public double MinPrice { get; set; }
		public PriceOverview CarPriceOverview { get; set; }
		public DateTime LaunchDate { get; set; }
		public int CWConfidence { get; set; }
		public bool ShowDate { get; set; }
		public DealsStock DiscountSummary { get; set; }
	}
}
