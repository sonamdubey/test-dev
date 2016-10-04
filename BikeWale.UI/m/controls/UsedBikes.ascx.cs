using Bikewale.BindViewModels.Controls;
using System;
using System.Web.UI.WebControls;
namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created by Subodh Jain on 15 sep 2016
    /// Dec:- Used bike widget for model and make page mobile
    /// </summary>
    public class UsedBikes : System.Web.UI.UserControl
    {
        protected Repeater rptUsedBikeNoCity, rptRecentUsedBikes;
        public uint MakeId { get; set; }
        public uint ModelId { get; set; }
        public uint TopCount { get; set; }
        public int? CityId { get; set; }
        public string header { get; set; }
        public int fetchedCount;
        public string makeName = string.Empty;
        public string modelName = string.Empty;
        public string cityName = string.Empty;
        public string makeMaskingName = string.Empty;
        public string modelMaskingName = string.Empty;
        public string cityMaskingName = string.Empty;
        public string pageHeading = string.Empty;



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
        }

        /// <summary>
        ///  Created by Subodh Jain on 15 sep 2016
        /// Desc:-Function to validate the data passed to the widget
        /// </summary>
        /// <returns></returns>
        private bool isValidData()
        {

            return MakeId > 0 ? true : false;
        }

        /// <summary>
        /// Created By :subodh jain on 14 sep 2016
        /// Description : Function to bind used bikes for the makes          
        /// </summary>
        protected void BindUsedBikes()
        {
            BindUsedBikesControl objUsed = new BindUsedBikesControl();
            objUsed.MakeId = MakeId;
            objUsed.ModelId = ModelId;
            objUsed.TopCount = TopCount;
            objUsed.CityId = CityId;
            fetchedCount = objUsed.BindRecentUsedBikes(rptRecentUsedBikes, rptUsedBikeNoCity);
            makeName = objUsed.makeName;
            modelName = objUsed.modelName;
            cityName = objUsed.cityName;
            makeMaskingName = objUsed.makeMaskingName;
            modelMaskingName = objUsed.modelMaskingName;
            cityMaskingName = objUsed.cityMaskingName;
            pageHeading = objUsed.pageHeading;

        }
    }
}