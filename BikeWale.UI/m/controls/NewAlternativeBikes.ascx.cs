using Bikewale.BindViewModels.Controls;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 9 Sept 2015
    /// Summary : Control to show the alternative bikes of the the specific version
    /// </summary>
    public class NewAlternativeBikes : System.Web.UI.UserControl
    {
        public Repeater rptAlternateBikes;
        public uint VersionId { get; set; }
        public string modelName { get; set; }
        public bool IsPriceInCity { get; set; }
        public string CityName { get; set; }
        public uint CityId { get; set; }
        public string CustomHeading { get; set; }
        private ushort _topCount = 6;
        public ushort TopCount
        {
            get { return _topCount; }
            set { _topCount = value; }
        }

        public int? Deviation { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string WidgetTitle { get; set; }

        public int PQSourceId { get; set; }
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindAlternativeBikes();
        }

        private void BindAlternativeBikes()
        {
            BindAlternativeBikesControl objAlt = new BindAlternativeBikesControl();
            objAlt.VersionId = VersionId;
            objAlt.TopCount = TopCount;
            objAlt.Deviation = Deviation;
            objAlt.cityId = CityId != 0 ? CityId : Convert.ToUInt32(Bikewale.Utility.BWConfiguration.Instance.DefaultCity);
            objAlt.BindAlternativeBikes(rptAlternateBikes);

            FetchedRecordsCount = objAlt.FetchedRecordsCount;
        }
    }
}