using AppWebApi.Common;
using Carwale.DAL.Forums;
using Carwale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace AppWebApi.Models
{
    public class ReviewsComments : IDisposable
    {
        private bool serverErrorOccured = false;
        [JsonIgnore]
        public bool ServerErrorOccured 
        {
            get { return serverErrorOccured; }
            set { serverErrorOccured = value; }
        }

        private string pageNo = "1";
        [JsonIgnore]
        public string PageNo
        {
            get { return pageNo; }
            set { pageNo = value; }
        }

        private string pageSize = "10";
        [JsonIgnore]
        public string PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        private string commentCount = "";
        [JsonIgnore]
        public string CommentCount
        {
            get { return commentCount; }
            set { commentCount = value; }
        }

        private string nextPageUrl = "";
        [JsonProperty("nextPageUrl")]
        public string NextPageUrl
        {
            get { return nextPageUrl; }
            set { nextPageUrl = value; }
        }

        [JsonIgnore]
        public string StartIndex
        {
            get { return Convert.ToString(((Convert.ToInt32(PageNo) - 1) * Convert.ToInt32(PageSize)) + 1); }
        }

        [JsonIgnore]
        public string EndIndex
        {
            get { return Convert.ToString(Convert.ToInt32(StartIndex) + Convert.ToInt32(PageSize) - 1); }
        }


        private string ReviewId { get; set; }
        private string ForumId { get; set; }

        public List<Comment> comments = new List<Comment>();
        DataSet ds = new DataSet();
  

        public ReviewsComments(string reviewId, string pageNo, string pageSize)
        {
            if (reviewId != null && reviewId != "")
            {
                ReviewId = reviewId;
            }

            PageNo = pageNo;
            PageSize = pageSize;

            GetComments();

            if ((Convert.ToInt32(PageNo) * Convert.ToInt32(PageSize)) >= Convert.ToInt32(CommentCount))
                NextPageUrl = "";
            else
                NextPageUrl = CommonOpn.ApiHostUrl + "ReviewsComments?reviewId=" + reviewId + "&pageNo=" + (Convert.ToInt32(PageNo) + 1) + "&pageSize=" + PageSize;
        }

        private void GetCommentCount()
        { 
        
        }

        private void GetComments()
        {
            DataSet ds = new DataSet();
            ForumsDAL forumRepo = new ForumsDAL();
            ds = forumRepo.GetForumReviewCount(Convert.ToInt32(ReviewId), Convert.ToInt32(StartIndex), Convert.ToInt32(EndIndex));           
            int i = 0;
            Comment comment;
            if (ds.Tables[1].Rows.Count > 0)
            {
                DataRow dRow=ds.Tables[1].Rows[0];
                CommentCount = dRow["CommentsCount"].ToString();
            }

            int commentsToBeFetched = 0;
            if (Convert.ToInt32(CommentCount) >= (Convert.ToInt32(PageSize) * Convert.ToInt32(PageNo)))
                commentsToBeFetched = Convert.ToInt32(PageSize);
            else
                commentsToBeFetched = Convert.ToInt32(CommentCount) - (Convert.ToInt32(PageSize) * (Convert.ToInt32(PageNo)-1));

            if (ds.Tables[0].Rows.Count > 0)
            {
                while (i < commentsToBeFetched)
                {
                    DataRow dRow = ds.Tables[0].Rows[i++];
                    comment = new Comment();
                    comment.Author = dRow["PostedBy"].ToString();
                    comment.Pubdate = CommonOpn.GetDate(dRow["MsgDateTime"].ToString());
                    comment.Content =  GetMessage(dRow["Message"].ToString());
                    comments.Add(comment);
                }
            }
        }

        /// Author: Rakesh Yadav
        /// Date Created: 30 Oct 2013
        /// Desc: modify post msg which contain msg of post to which it is replied
        
        public string GetMessage(string value)
        {
            string post = value;
          
            // Identify and replace quotes
            if (post.ToLower().IndexOf("[^^quote=") >= 0)
            {
                post = post.Replace("[^^quote=", "");
                post = post.Replace("[^^/quote^^]","");
                post = post.Replace("^^]", "");
            }

            post = post.Replace("<p>&nbsp;</p>", ""); // remove empty paragraphs.
            post = CommonOpn.RemoveAnchorTag(post);
            post = Format.RemoveHtmlTags(post);

            return post;
        }
        public void Dispose() {
            this.ds.Dispose();
        }
    }
}