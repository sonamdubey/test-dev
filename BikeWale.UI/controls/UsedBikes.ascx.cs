using Bikewale.BindViewModels.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Author : Vivek Gupta
    /// Created Date : 21st june 2016
    /// Desc : recently uploaded used bikes control
    /// </summary>
    public class UsedBikes : UserControl
    {
        protected Repeater rptUsedBikeNoCity, rptRecentUsedBikes;

        public uint MakeId { get; set; }
        public uint ModelId { get; set; }
        public uint TopCount { get; set; }
        public int? CityId { get; set; }
        public uint FetchedRecordsCount;
        public string makeName = string.Empty;
        public string modelName = string.Empty;
        public string cityName = string.Empty;
        public string makeMaskingName = string.Empty;
        public string modelMaskingName = string.Empty;
        public string cityMaskingName = string.Empty;
        public string pageHeading = string.Empty;
        public bool showWidget;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(Page_Load);
            showWidget = false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (isValidData())
                BindUsedBikes();
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
        /// </summary>
        protected void BindUsedBikes()
        {
            BindUsedBikesControl objUsed = new BindUsedBikesControl();
            objUsed.MakeId = MakeId;
            objUsed.ModelId = ModelId;
            objUsed.TopCount = TopCount;
            objUsed.CityId = CityId;
            FetchedRecordsCount = Convert.ToUInt32(objUsed.BindRecentUsedBikes(rptRecentUsedBikes, rptUsedBikeNoCity));
            makeName = objUsed.makeName;
            modelName = objUsed.modelName;
            cityName = objUsed.cityName;
            makeMaskingName = objUsed.makeMaskingName;
            modelMaskingName = objUsed.modelMaskingName;
            cityMaskingName = objUsed.cityMaskingName;
            pageHeading = objUsed.pageHeading.Trim();

        }
    }
}