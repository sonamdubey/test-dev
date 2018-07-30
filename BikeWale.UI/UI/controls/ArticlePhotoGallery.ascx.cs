using Bikewale.Entities.CMS.Photos;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Ashwini Todkar on 1 Oct 2014
    /// Control to show gallry of article photos 
    /// </summary>
    public class ArticlePhotoGallery : UserControl
    {
        public int BasicId { get; set; }
        public IEnumerable<ModelImage> ModelImageList { get; set; }
        protected Repeater rptPhotos;


        public void BindPhotos()
        {
            rptPhotos.DataSource = ModelImageList;
            rptPhotos.DataBind();
        }
    }
}