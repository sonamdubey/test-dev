using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created By :- Subodh Jain 3 feb 2017
    /// Summary :- Control For similar bike videos
    /// </summary>
    public class SimilarBikeVideos : System.Web.UI.UserControl
    {
        public uint ModelId { get; set; }
        public uint TotalCount { get; set; }
        public uint FetchCount { get; set; }
        protected IEnumerable<SimilarBikeWithVideo> SimilarBikeVideoList;
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            BindVideoDetails();
        }
        /// <summary>
        /// Created by :- Subodh Jain 3 feb 2017
        /// Summary :- Bind Video details for similar bikes
        /// </summary>
        private void BindVideoDetails()
        {
            try
            {
                BindSimilarBikeVideos objVideos = new BindSimilarBikeVideos();
                if (objVideos != null)
                {
                    SimilarBikeVideoList = objVideos.GetSimilarBikesVideos(ModelId, TotalCount);
                    if (SimilarBikeVideoList != null)
                    {
                        FetchCount = (uint)SimilarBikeVideoList.Count();
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Controls.BindVideoDetails modelid: {0}", ModelId));
            }
        }
    }
}