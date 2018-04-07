using System.Collections.Generic;

namespace Bikewale.BAL.ApiGateway.Entities.BikeData
{
	/// <summary>
	/// Created By : Ashish G. kamble on 6 Apr 2018
	/// Summary : Entity is input parameter for VersionsDataByItemIdsAdapter
	/// </summary>
	public class VersionsDataByItemIds_Input
	{
		/// <summary>
		/// List of version ids for which data is required
		/// </summary>
		public IEnumerable<int> Versions { get; set; }
		
		/// <summary>
		/// List of specs or features whose data is required
		/// </summary>
		public IEnumerable<EnumSpecsFeaturesItems> Items { get; set; }
	}
}
