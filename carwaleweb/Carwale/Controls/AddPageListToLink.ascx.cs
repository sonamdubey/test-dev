using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carwale.UI.Common;

namespace Carwale.UI.Controls
{
    public class AddPageListToLink : UserControl
    {
        protected string pageUrl = "";
        protected int prevPageNo = 0;
        protected int nextPageNo = 0;
        protected string prevUrl;
        protected string nextUrl;
        protected HyperLink prevSlot;
        protected HyperLink nextSlot;
        GeneratePageList pagelist = new GeneratePageList();
        public int PageSize { get; set; }

        public int CurrentPageNumber { get; set; }

        /// <summary>
        /// Number Of Pages Per Slot
        /// </summary>
        int _PagesPerSlot = 10;
        public int PagesPerSlot
        {
            get { return _PagesPerSlot; }
            set { _PagesPerSlot = value; }
        }

        /// <summary>
        /// Total no of records matching user's criteria
        /// </summary>
        public int TotalRecordCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BaseUrl { get; set; }

        //public List<int> PageList { get; set; }

        protected Repeater rptLinkPageList;

        void Page_Load(object sender, EventArgs e)
        {
            GenerateStrip();
        }


        public void GenerateStrip()
        {
            pageUrl = BaseUrl;

            pagelist.CurrentPageNumber = CurrentPageNumber;
            //pagelist.CurrentPageNumber = pageNo;
            pagelist.PageSize = PageSize;
            pagelist.PagesPerSlot = PagesPerSlot;
            pagelist.TotalRecordCount = TotalRecordCount;
            List<int> result = pagelist.GetPageList();
            GetPervNextLink();
            rptLinkPageList.DataSource = result;
            rptLinkPageList.DataBind();
        }

        private void GetPervNextLink()
        {
            GetNextPrevPage();
            if (pagelist.CurrentSlot == 1 && pagelist.SlotCount > 1)
            {
                prevSlot.Visible = false;

            }
            else if (pagelist.CurrentSlot == 1 && pagelist.SlotCount == 1)
            {
                prevSlot.Visible = false;
                nextSlot.Visible = false;
            }
            else if (pagelist.CurrentSlot == pagelist.SlotCount)
            {
                nextSlot.Visible = false;

            }
        }

        private void GetNextPrevPage()
        {
            prevPageNo = (((pagelist.CurrentSlot) - 1) * pagelist.PagesPerSlot);
            nextPageNo = ((pagelist.CurrentSlot) * pagelist.PagesPerSlot) + 1;

            prevUrl = pageUrl + "&page=" + prevPageNo;
            nextUrl = pageUrl + "&page=" + nextPageNo;
            nextSlot.Attributes.Add("href", nextUrl);
            prevSlot.Attributes.Add("href", prevUrl);
        }
    }
}