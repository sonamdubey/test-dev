using System;

namespace BikewaleOpr.Entities.BikeData
{
	/// <summary>
	/// Created By : Ashish G. Kamble on 12 June 2018
	/// </summary>
	[Serializable]
	public class BikeVersionEntity : BikeVersionEntityBase
	{
		public bool IsImported { get; set; }
		public ushort BodyStyleId { get; set; }
		public ushort BikeFuelType { get; set; }		
		public double Displacement { get; set; }
		public int TopSpeed { get; set; }
		public double KerbWeight { get; set; }
	}
}
