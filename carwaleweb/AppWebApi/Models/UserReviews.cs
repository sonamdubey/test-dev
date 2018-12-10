using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using System.Data;
using AppWebApi.Common;
using Carwale.Utility;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Interfaces.CMS.UserReviews;
using Carwale.Service;

namespace AppWebApi.Models
{
    public class UserReviews : IDisposable
    {
        /*
         Author:Rakesh Yadav
         Date Created: 16 july 2013
         Desc: define properties
         */
        private bool serverErrorOccured = false;
        [JsonIgnore]
        public bool ServerErrorOccured
        {
            get { return serverErrorOccured; }
            set { serverErrorOccured = value; }
        }

        private string makeName = "";
        [JsonProperty("makeName")]
        public string MakeName
        {
            get { return makeName; }
            set { makeName = value; }
        }

        private string makeId = "";
        [JsonProperty("makeId")]
        public string MakeId
        {
            get { return makeId; }
            set { makeId = value; }
        }

        private string modelName = "";
        [JsonProperty("modelName")]
        public string ModelName
        {
            get { return modelName; }
            set { modelName = value; }
        }

        private string modelId = "";
        [JsonProperty("modelId")]
        public string ModelId
        {
            get { return modelId; }
            set { modelId = value; }
        }
    
        private string versionId = "";
        [JsonProperty("versionId")]
        public string VersionId
        {
            get { return versionId; }
            set { versionId = value; }
        }

        private string reviewCount = "";
        [JsonProperty("reviewCount")]
        public string ReviewCount
        {
            get { return reviewCount; }
            set { reviewCount = value; }
        }

        private string smallPicUrl = "";
        [JsonProperty("smallPicUrl")]
        public string SmallPicUrl
        {
            get { return smallPicUrl; }
            set { smallPicUrl = value; }
        }

        private string largePicUrl = "";
        [JsonProperty("largePicUrl")]
        public string LargePicUrl
        {
            get { return largePicUrl; }
            set { largePicUrl = value; }
        }
        private string startPrice = "";
        [JsonProperty("startPrice")]
        public string StartPrice
        {
            get { return startPrice; }
            set { startPrice = value; }
        }

        private string nextPageReviewUrl = "";
        [JsonProperty("nextPageReviewUrl")]
        public string NextPageReviewUrl
        {
            get { return nextPageReviewUrl; }
            set { nextPageReviewUrl = value; }
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

        private string sortCriteria = "1";
        [JsonIgnore]
        public string SortCriteria
        {
            get { return sortCriteria; }
            set { sortCriteria = value; }
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
        [JsonProperty("hostUrl")]
        public string HostURL { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }

        private DataSet ds = new DataSet();
        public List<Rating> ratings = new List<Rating>();
        public List<Review> reviews = new List<Review>();
        public List<Sort> sorts = new List<Sort>();
        public List<Item> versions = new List<Item>();
        [JsonIgnore]
        public List<UserReviewEntity> userlReviewList = new List<UserReviewEntity>();
        /*
         Author:Rakesh Yadav
         Date Created: 16 july 2013
         Desc:Populate user reviews,next page url, url to see reviews for versions of maodel
         */
        public UserReviews(string modelId, string versionId, string pageNo, string pageSize, string sortCriteria)
        {
            ModelId = modelId;
            VersionId = versionId;
            PageNo = pageNo;
            PageSize = pageSize;
            SortCriteria = sortCriteria;       

            GetSortCriteria();
            GetUserReviews();
            if (!ServerErrorOccured)
            {
                GetModelDetails();         
                GetReviews();
                GetVersionUrls();
            }

            if ((Convert.ToInt32(PageNo) * Convert.ToInt32(PageSize)) >= Convert.ToInt32(ReviewCount))
                NextPageReviewUrl = "";
            else
                NextPageReviewUrl = CommonOpn.ApiHostUrl + "UserReviews?modelId=" + ModelId + "&versionId=" + VersionId + "&pageNo=" + (Convert.ToInt32(PageNo) + 1) + "&pageSize=" + PageSize + "&sortCriteria=" + SortCriteria;
        }

        /*
         Author:Rakesh Yadav
         Date Created: 16 july 2013
         Desc: Get sort criteria and generate url to see reviews with sort criteria
         */
        private void GetSortCriteria()
        {
            string SortUrl = CommonOpn.ApiHostUrl + "UserReviews?modelId=" + ModelId + "&versionId=" + VersionId + "&pageNo=1" + "&pageSize=" + PageSize + "&sortCriteria=";
            sorts.Add(new Sort { Label = "Most Helpful", SortUrl = SortUrl + 1 });
            sorts.Add(new Sort { Label = "Most Read", SortUrl = SortUrl + 2 });
            sorts.Add(new Sort { Label = "Most Recent", SortUrl = SortUrl + 3 });
            sorts.Add(new Sort { Label = "Most Rated", SortUrl = SortUrl + 4 });
        }

        /*
         Author:Rakesh Yadav
         Date Created: 16 july 2013
         Desc: Execute Stored procedure
         */
        private void GetUserReviews()
        {         
            try
            {
                IUserReviewsCache userRepo = UnityBootstrapper.Resolve<IUserReviewsCache>();         
                int modelId = CustomParser.parseIntObject(ModelId);
                int versionId = CustomParser.parseIntObject(VersionId);
                int startIndex = CustomParser.parseIntObject(StartIndex);
                int endIndex = CustomParser.parseIntObject(EndIndex);
                int sortCriteria = CustomParser.parseIntObject(SortCriteria);
                userlReviewList = userRepo.GetUserReviewsList(CustomParser.parseIntObject(makeId),modelId,versionId,startIndex,endIndex,sortCriteria);
                userlReviewList.ForEach(c => c.Description = string.IsNullOrEmpty(c.Description) ? string.Empty : Format.LimitTextWithoutDotAtTheEnd(c.Description, (int)(c.Description.Length * 0.15)));
            }
            catch (Exception err)
            {
                ServerErrorOccured = true;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /*
         Author:Rakesh Yadav
         Date Created: 16 july 2013
         Desc: Get model details
         */
        private void GetModelDetails()
        {         
            using (DbCommand cmd = DbFactory.GetDBCommand("GetCarsReviewData_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int32, ModelId));
                cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId", DbType.Int32, Convert.ToInt32(VersionId) > 0 ? Convert.ToInt32(VersionId) : Convert.DBNull));
                ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dRow = ds.Tables[0].Rows[0];
                    MakeId = dRow["MakeId"].ToString();
                    MakeName = dRow["MakeName"].ToString();
                    ModelId = dRow["ModelId"].ToString();
                    ModelName = dRow["ModelName"].ToString();
                    SmallPicUrl = ImageSizes.CreateImageUrl(dRow["HostURL"].ToString(), ImageSizes._110X61, dRow["OriginalImgPath"].ToString());
                    LargePicUrl = ImageSizes.CreateImageUrl(dRow["HostURL"].ToString(), ImageSizes._210X118, dRow["OriginalImgPath"].ToString());
                    HostURL = dRow["HostURL"].ToString();
                    OriginalImgPath = dRow["OriginalImgPath"].ToString();
                    StartPrice = CommonOpn.GetPrice(dRow["MinPrice"].ToString());
                    ReviewCount = dRow["TotalReviews"].ToString();

                    ratings.Add(new Rating { Label = "Overall", Value = CommonOpn.GetAbsReviewRate(Convert.ToDouble(dRow["ReviewRate"].ToString())) });
                    ratings.Add(new Rating { Label = "Looks", Value = CommonOpn.GetAbsReviewRate(Convert.ToDouble(dRow["Looks"].ToString())) });
                    ratings.Add(new Rating { Label = "Performance", Value = CommonOpn.GetAbsReviewRate(Convert.ToDouble(dRow["Performance"].ToString())) });
                    ratings.Add(new Rating { Label = "Space/Comfort", Value = CommonOpn.GetAbsReviewRate(Convert.ToDouble(dRow["Comfort"].ToString())) });
                    ratings.Add(new Rating { Label = "Fuel Economy", Value = CommonOpn.GetAbsReviewRate(Convert.ToDouble(dRow["FuelEconomy"].ToString())) });
                    ratings.Add(new Rating { Label = "Value For Money", Value = CommonOpn.GetAbsReviewRate(Convert.ToDouble(dRow["ValueForMoney"].ToString())) });
                }
            }
        }

 
        /*
         Author:Rakesh Yadav
         Date Created: 16 july 2013
         Desc: Get User Reviews
         */
        private void GetReviews()
        {
            Review r; 
            foreach(var userReview in userlReviewList)
            {
                r = new Review();
                r.Title = !string.IsNullOrEmpty(userReview.Title)?userReview.Title:string.Empty;
                if (!string.IsNullOrEmpty(userReview.HandleName))
                    r.Author = userReview.HandleName;
                else
                    if (!string.IsNullOrEmpty(userReview.CustomerName))
                        r.Author = userReview.CustomerName;
                    else
                        r.Author = string.Empty;
                r.HandleName = !string.IsNullOrEmpty(userReview.HandleName)?userReview.HandleName:string.Empty;
                int comment = 0,threadId = -1;
                int.TryParse(userReview.Comments.ToString(), out comment);
                int.TryParse(userReview.ThreadId.ToString(), out threadId);
                 r.Comments = comment;
                 r.ThreadId = threadId;
                 string date = userReview.ReviewDate.ToString();
                 r.ReviewDate = CommonOpn.GetDate(date);
                 r.ReviewRate = CommonOpn.GetAbsReviewRate(Convert.ToDouble(userReview.ReviewRate.ToString()));
                 r.Goods = userReview.Goods;
                 r.Bads = userReview.Bads;
                r.ReviewId = userReview.ReviewId;
                 r.Description = userReview.Description;
                 r.ReviewUrl = CommonOpn.ApiHostUrl + "UserReviewDetail?reviewId=" + userReview.ReviewId.ToString();
                 reviews.Add(r);
            }       
        }
        /*
         Author:Rakesh Yadav
         Date Created: 16 july 2013
         Desc: Generate urls to get version specific user reviews
        */
        private void GetVersionUrls()
        {
            string url = CommonOpn.ApiHostUrl + "UserReviews?modelId=" + ModelId + "&versionId=-1&pageNo=1&pageSize=" + PageSize + "&sortCriteria=1";
            versions.Add(new Item { Id = "-1", Name = "--All Versions--", Url = url });
            foreach (DataRow dRow in ds.Tables[1].Rows)
            {
                url = CommonOpn.ApiHostUrl + "UserReviews?modelId=" + ModelId + "&versionId=" + dRow["versionId"].ToString() + "&pageNo=1" + "&pageSize=" + PageSize + "&sortCriteria=1";
                versions.Add(new Item { Id = dRow["versionId"].ToString(), Name = dRow["versionName"].ToString(), Url = url });
            }

        }
        public void Dispose() {
            this.ds.Dispose();
        }
    }
}