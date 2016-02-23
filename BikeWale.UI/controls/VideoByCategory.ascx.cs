using Bikewale.BindViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Entities.Videos;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 18 Feb 2016
    /// Description : For Expert Review Video Controles.
    /// Modified By : Sushil Kumar K
    /// Modified On : 19th February 2016
    /// Description : Bind VideosByCategories Repeater for differnt categories  
    /// </summary>
    /// </summary>
    public class VideoByCategory : System.Web.UI.UserControl
    {
        protected Repeater rptVideosByCat;
        public ushort TotalRecords { get; set; }
        public EnumVideosCategory CategoryId { get; set; }
        public string viewMoreURL { get; set; }
        public ushort FetchedRecordsCount { get; set; }
        public string SectionTitle { get; set; }
        public string SectionBackgroundClass { get; set; }
        public BikeVideoEntity FirstVideoRecord { get; set; }
        public string CategoryIdList { get; set; }  //comma separated category ids e.g. 48,47,49

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(CategoryIdList))
            {
                BindVideosSectionCatwise objVideo = new BindVideosSectionCatwise();
                objVideo.TotalRecords = this.TotalRecords;
                objVideo.CategoryId = this.CategoryId;
                objVideo.FetchVideos();
                this.FetchedRecordsCount = objVideo.FetchedRecordsCount;
                this.FirstVideoRecord = objVideo.FirstVideoRecord;
                objVideo.BindVideos(rptVideosByCat);
            }
            else{
                BindVideosSectionSubCatwise objVideo = new BindVideosSectionSubCatwise();
                objVideo.TotalRecords = this.TotalRecords;
                objVideo.CategoryIdList = this.CategoryIdList;
                objVideo.FetchVideos();
                this.FetchedRecordsCount = objVideo.FetchedRecordsCount;
                this.FirstVideoRecord = objVideo.FirstVideoRecord;
                objVideo.BindVideos(rptVideosByCat);
            }            
            
            
        }

        public override void Dispose()
        {
            rptVideosByCat.DataSource = null;
            rptVideosByCat.Dispose();
            base.Dispose();
        }
    }
}