/*
 Author: Rakesh Yadav
 Date Created : 18 Sep 2013
 */

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MobileWeb.Controls
{
    public class CarCompareItem : UserControl
    {
        private string imageUrl = "";
        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        private string compareText = "";
        public string CompareText
        {
            get { return compareText; }
            set { compareText = value; }
        }

        private string comapreUrl = "";
        public string ComapreUrl
        {
            get { return comapreUrl; }
            set { comapreUrl = value; }
        }

        private string sponsoredText = "";
        public string SponsoredText
        {
            get { return sponsoredText; }
            set { sponsoredText = value; }
        }
        public string versionId1 = "";
        public string VersionId1
        {
            get { return versionId1; }
            set { versionId1 = value; }
        }
        public string versionId2 = "";
        public string VersionId2
        {
            get { return versionId2; }
            set { versionId2 = value; }
        }
    }
}