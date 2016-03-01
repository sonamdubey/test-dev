﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.BindViewModels.Controls;

namespace Bikewale.Controls
{    
    /// <summary>
    /// Modified By : Lucky Rathore on 01 March 2016
    /// Description : functionality for view more videos URL added.
    /// </summary>
    public class VideosControl : System.Web.UI.UserControl
    {
        protected Repeater rptVideos;
        
        public int TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        protected string MoreVideoUrl = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindVideos();
            if (String.IsNullOrEmpty(ModelMaskingName)) 
            {
                MoreVideoUrl = string.Format("/bike-videos/{0}-{1}/", MakeMaskingName, MakeId);
            }
            else
            {
                MoreVideoUrl = string.Format("/bike-videos/{0}-{1}-{2}/", MakeMaskingName, ModelMaskingName, ModelId); ;
            }
        }

        protected void BindVideos()
        {
            BindVideosControl objVideo = new BindVideosControl();
            objVideo.TotalRecords = this.TotalRecords;
            objVideo.MakeId = this.MakeId;
            objVideo.ModelId = this.ModelId;

            objVideo.BindVideos(rptVideos);

            this.FetchedRecordsCount = objVideo.FetchedRecordsCount;
        }

        public override void Dispose()
        {
            rptVideos.DataSource = null;
            rptVideos.Dispose();

            base.Dispose();
        }
    }
}