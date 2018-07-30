using Bikewale.DTO.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Version
{
	/// <summary>
	/// Created by : Ashutosh Sharma on 26 Dec 2017.
	/// Description : DTO for specs and features of a version.
	/// </summary>
	public class VersionSpecs
	{
		[JsonProperty("specs")]
		public Specifications objSpecs { get; set; }
		[JsonProperty("features")]
		public Features objFeatures { get; set; }
	}
}
