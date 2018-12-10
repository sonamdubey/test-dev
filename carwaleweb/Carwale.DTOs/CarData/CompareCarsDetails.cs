using Carwale.DTOs.PriceQuote;
using Carwale.Entity.CompareCars;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{

    public class CompareDetailsModel
    {
		public string Title = string.Empty;
		public string Description = string.Empty;
		public string TargetLabel = string.Empty;
		
		public CCarData CarData;
		public List<int> versionIds;
        public bool ShowCampaignSlab;
        public List<int> VersionsWithTyres;
        public List<ComparisonDataDto> ComparisonData;
        public List<EmiCalculatorModelData> EmiCalculatorModelData { get; set; }
        public Location Location { get; set; }
        private string summary;
        public string Summary{
            get { return summary; }
            set { summary = value; }
        }
        public ExperimentAdSlot ExperimentAdSlot { get; set; } 
        public CompareDetailsModel()
        {
            EmiCalculatorModelData = new List<EmiCalculatorModelData>();
        }
    }
	
}
