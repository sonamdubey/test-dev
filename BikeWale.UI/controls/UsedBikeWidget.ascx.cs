using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.UsedBikes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Author : Sangram Nandkhile
    /// Created Date : 22nd Sep 2016
    /// Desc : recently uploaded used bikes with provision to show Ad and total counts
    /// </summary>
    public class UsedBikeWidget : UserControl
    {
        protected Repeater rptUsedBikeNoCity, rptRecentUsedBikes;

        public uint MakeId { get; set; }
        public uint ModelId { get; set; }
        public uint TopCount { get; set; }
        public int? CityId { get; set; }
        public uint FetchedRecordsCount;
        public string makeName = string.Empty;
        public string modelName = string.Empty;

        public string modelMaskingName = string.Empty;
        public string cityMaskingName = string.Empty;
        public string pageHeading { get; set; }

        public bool isAd { get; set; }
        protected short masterGrid = 12;
        protected short childGrid = 4;
        protected IEnumerable<MostRecentBikes> objUsedBikes = null;
        public string cityName { get; set; }
        public string makeMaskingName { get; set; }
        public string AdId { get; set; }
        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (isValidData())
                BindUsedBikes();
            if (isAd)
            {
                masterGrid = 8;
                childGrid = 6;
            }
        }

        /// <summary>
        /// Function to validate the data passed to the widget
        /// </summary>
        /// <returns></returns>
        private bool isValidData()
        {
            bool isValid = true;
            if (MakeId < 0 || ModelId < 0)
            {
                isValid = false;
            }
            return isValid;
        }

        /// <summary>
        /// Created By : Sajal Gupta on 15/09/2016
        /// Description : Function to bind used bikes for the makes/models.          
        /// Modified By :-Subodh Jain on 1 Dec 2016
        /// Summary :- Added page heading null check if null add default heading
        /// </summary>
        /// <returns></returns>
        protected void BindUsedBikes()
        {
            BindUsedBikesControl objUsed = new BindUsedBikesControl();
            objUsed.MakeId = MakeId;
            objUsed.ModelId = ModelId;
            objUsed.TopCount = TopCount;
            objUsed.CityId = CityId;
            objUsedBikes = objUsed.FetchUsedBikes(TopCount, MakeId, ModelId, CityId);
            if (objUsedBikes != null)
            {
                MostRecentBikes firstBike = objUsedBikes.FirstOrDefault();
                if (firstBike != null)
                {
                    makeName = firstBike.MakeName;
                    makeMaskingName = firstBike.MakeMaskingName;
                }
                FetchedRecordsCount = Convert.ToUInt16(objUsedBikes.Count());
            }
            if (string.IsNullOrEmpty(pageHeading))
                pageHeading = string.Format("Used {0} bikes in {1}", makeName, cityName);
        }
    }
}