using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created by:-Subodh Jain on 12 sep 2016
    /// Description :- Compare popular similar bikes on model page
    /// </summary>
    public class PopularModelCompare : UserControl
    {
        public string versionId { get; set; }
        protected ICollection<SimilarCompareBikeEntity> objSimilarBikes = null;
        public string ModelName;
        private ushort _topCount = 0;
        public uint fetchedCount { get; set; }
        public int? cityid { get; set; }
        public ushort TopCount
        {
            get { return _topCount; }
            set { _topCount = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindPopularCompareBikes();
        }
        /// <summary>
        /// Created by : Subodh Jain 12 sep 2016
        /// Desc       : To bind popular compare  bikes for model page
        /// </summary>
        private void BindPopularCompareBikes()
        {
            BindSimilarCompareBikesControl objAlt = new BindSimilarCompareBikesControl();
            objAlt.cityid = cityid.HasValue && cityid > 0 ? cityid.Value : Convert.ToInt16(Bikewale.Utility.BWConfiguration.Instance.DefaultCity);
            objSimilarBikes = objAlt.BindPopularCompareBikes(versionId, TopCount);
            fetchedCount = objAlt.FetchedRecordsCount;
        }
    }
}
