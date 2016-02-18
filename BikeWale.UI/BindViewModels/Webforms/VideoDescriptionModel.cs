using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.BindViewModels.Webforms
{
    public class VideoDescriptionModel
    {
        public string VideoUrl { get; set; }
        public string Description  { get; set; }
        public string Tags { get; set; }
        public string Views { get; set; }
        public string Title { get; set; }

        public VideoDescriptionModel(int videoId)
        {
            // Call api and get details
            VideoUrl = "https://www.youtube.com/embed/S_1Nj3T0j1w?rel=0&showinfo=0" + "&autoplay=1";
            Description = "<p>After a successful spell of the most popular Super bike, KTM has upped their game with their newest generation of the Duke.<br /><br />Termed as the New Age XUV 500, we find out what this new macho car has to offer!</p>";
            Tags = "KTM, Super Bike, Rush";
            Views = "6545464";
            Title = "KTM Duke stunts";
        }
    }
}