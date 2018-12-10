using Carwale.Entity.Common;
using System;

namespace Carwale.Entity.CarData
{
	[Serializable]
	public class FuelTypeBase: IdName
	{

		public string Icon { get; set; }
		public string AppIcon { get; set; }
	}
}
