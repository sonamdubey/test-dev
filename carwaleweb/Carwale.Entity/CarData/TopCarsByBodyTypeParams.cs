namespace Carwale.Entity.CarData
{
	public class TopCarsByBodyTypeParams
	{
		public int Count { get; set; }
		public short BodyType { get; set; }
		public int CityId { get; set; }
		public int ZoneId { get; set; }
		public short PageNo { get; set; }
        public bool IsMobile { get; set; }
	}
	public class TopCarsByBodyTypeAndBudgetParams : TopCarsByBodyTypeParams
	{
		public int LowerPriceLimit { get; set; }
		public int UpperPriceLimit { get; set; }
        public string CityName { get; set; }
		public string Budget { get; set; }
	}
}
