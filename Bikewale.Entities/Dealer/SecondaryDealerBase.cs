using System;

namespace Bikewale.Entities.Dealer
{
	/// <summary>
	/// Created By : Prabhu Puredla
	/// Description : Entity which contains secondary dealers
	/// </summary>
	[Serializable]
	public class SecondaryDealerBase
	{
		public uint DealerId { set; get; }
		public string Name { set; get; }
		public string Area { set; get; }
		public double Distance { set; get; }
		public uint AreaId { set; get; }
        public uint MasterDealerId { set; get; }
	}
}
