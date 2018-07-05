using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.v2
{
	/// <summary>
	/// Created By  : Rajan Chauhan on 27 June 2018
	/// Description : PQ Update input DTO
	/// </summary>
	public class PQUpdateInput
	{
		[JsonProperty("pqguid")]
		public string PQGUId { get; set; }
		[JsonProperty("leadId")]
		public UInt32 LeadId { get; set; }
		[JsonProperty("versionId")]
		public UInt32 VersionId { get; set; }
		[JsonProperty("colorId")]
		public UInt32 ColorId { get; set; }
	}
}
