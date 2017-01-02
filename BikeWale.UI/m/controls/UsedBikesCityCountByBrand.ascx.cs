using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using System;
using System.Linq;

namespace Bikewale.m.controls
{
    public partial class UsedBikesCityCountByBrand : System.Web.UI.UserControl
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

        private void BindCountList()
        {
            viewModel = new BindUsedBikesInCityCount(MakeId);

            fetchedCount = (uint)viewModel.bikesCountCityList.Count();

            var objMake = new MakeHelper().GetMakeNameByMakeId(MakeId);
            makeName = objMake.MakeName;
            makeMaskingName = objMake.MaskingName;
        }
    }
}