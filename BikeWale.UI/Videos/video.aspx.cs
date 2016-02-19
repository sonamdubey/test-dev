﻿using Bikewale.BindViewModels.Webforms;
using Bikewale.controls;
using Bikewale.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Videos
{
    public class video : System.Web.UI.Page
    {
        protected VideoDescriptionModel videoModel;
        protected SimilarVideos ctrlSimilarVideos;
        protected int videoId = 0;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // Read id from query string
            videoId = 18838;
            videoModel = new VideoDescriptionModel(videoId);
            
        }
        private void BindSimilarVideoControl()
        {
            ctrlSimilarVideos.TopCount = 6;
            ctrlSimilarVideos.BasicId = 20156;
        }
    }
}