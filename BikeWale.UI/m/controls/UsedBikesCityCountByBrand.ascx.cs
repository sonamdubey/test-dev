using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using System;
using System.Linq;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created by : Sajal Gupta on 03-01-2017
    /// Desc : Class to bind brand india city wise bike count.
    /// </summary>
    public class UsedBikesCityCountByBrand : System.Web.UI.UserControl
    {
        public BindUsedBikesInCityCount viewModel = null;
        public uint MakeId { get; set; }
        public uint fetchedCount;
        public string makeName = String.Empty, makeMaskingName = String.Empty;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (MakeId > 0)
                BindCountList();
        }

        /// <summary>
        /// Created by : Sajal Gupta on 03-01-2017
        /// Desc : Function to bind brand india city wise bike count.
        /// </summary>
        private void BindCountList()
        {
            try
            {
                viewModel = new BindUsedBikesInCityCount(MakeId);

                fetchedCount = (uint)viewModel.bikesCountCityList.Count();

                var objMake = new MakeHelper().GetMakeNameByMakeId(MakeId);
                makeName = objMake.MakeName;
                makeMaskingName = objMake.MaskingName;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.m.controls.UsedBikesCityCountByBrand.BindCountList()");
            }
        }
    }
}