using System;
using Carwale.Entity.Common;

namespace Carwale.Entity.CarData
{
	[Serializable]
	public class TransmissionTypeBase: IdName
	{
		public string Icon { get; set; }
		public string AppIcon { get; set; }
	}
}
