using System;

namespace PriceQuoteConsumer.Entities
{
	public class PriceQuoteParameters
	{
		public string GUID { get; set; }
		public uint VersionId { get; set; }
		public uint CityId { get; set; }
		public uint AreaId { get; set; }
		public UInt16 BuyingPreference { get; set; }
		public ulong CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string CustomerEmail { get; set; }
		public string CustomerMobile { get; set; }
		public string ClientIP { get; set; }
		public UInt16 SourceId { get; set; }
		public uint DealerId { get; set; }
		public string DeviceId { get; set; }
		public string UTMA { get; set; }
		public string UTMZ { get; set; }
		/// <summary>
		/// Source Id for widget on which onRoadPrice is shown
		/// </summary>
		public ushort? PQSourceId { get; set; }
		public string RefGUID { get; set; }
	}
}
