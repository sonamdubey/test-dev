using Bikewale.Cache.Core;
using Bikewale.Cache.Videos;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Videos;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Webforms
{
    public class VideoDescriptionModel
    {
        public uint id { get; set; }
        public string VideoUrl { get; set; }
        public string Description  { get; set; }
        public string Views { get; set; }
        public string Title { get; set; }
        public string Likes { get; set; }

        public VideoDescriptionModel(uint videoId)
        {
            id= videoId;
            // Call api and get details
            VideoUrl = "https://www.youtube.com/embed/S_1Nj3T0j1w?rel=0&showinfo=0" + "&autoplay=0";
            Description = "The Street Triple story is indeed that of Dr. Jekyll and Mr. Hyde. On one hand, it meekly seems like being an under powered expensive street fighter. And then on another, it fiercely shows you the joys of motorcycling. So if you chose the Triple, or suggested the Triple to your friend, remember to SHARE the video ahead tagging them! If they re-share it quoting you; You and your friend could get some exclusive PowerDrift merchandise!</p>";
            Likes = "12,655";
            Views = "6,54,5464";
            Title = "KTM Duke stunts";
        }
    }
}