using AppWebApi.Common;
using Carwale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using Carwale.BL.NewCars;
using Carwale.Service;
using Carwale.Entity.CarData;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS.Articles;
using HtmlAgilityPack;

namespace AppWebApi.Models
{
    public class UpComingCars
    {
        /*
         Author:Rakesh Yadav
         Date Created: 30 July 2013
         Desc: Properties
         */
        private bool serverErrorOccured = false;
        [JsonIgnore]
        public bool ServerErrorOccured
        {
            get { return serverErrorOccured; }
            set { serverErrorOccured = value; }
        }

        private int pageNo = 1;
        [JsonIgnore]
        public int PageNo
        {
            get { return pageNo; }
            set { pageNo = value; }
        }

        private int pageSize = 10;
        [JsonIgnore]
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        [JsonIgnore]
        public int StartIndex
        {
            get { return (PageNo - 1) * PageSize + 1; }
        }

        [JsonIgnore]
        public int LastIndex
        {
            get { return StartIndex + PageSize - 1; }
        }

        private string nextPageUrl = "";
        [JsonProperty("nextPageUrl")]
        public string NextPageUrl
        {
            get { return nextPageUrl; }
            set { nextPageUrl = value; }
        }

        private int CarCount { get; set; }

        private string makeId = "-1";
        private string MakeId
        {
            get { return makeId; }
            set { makeId = value; }
        }

        public List<UpComingCarItem> upCommingCars = new List<UpComingCarItem>();
        public List<Item> carMakes = new List<Item>();
        /*
         Author:Rakesh Yadav
         Date Created: 30 July 2013
         Desc: Set properties and populate upcomming cars of all makes
         */
        public UpComingCars(string pageNo, string pageSize)
        {
            PageNo = Convert.ToInt32(pageNo);
            PageSize = Convert.ToInt32(pageSize);

            GetUpcomingCarDetails();

            if ((Convert.ToInt32(PageNo) * Convert.ToInt32(PageSize)) >= CarCount)
                NextPageUrl = "";
            else
                NextPageUrl = CommonOpn.ApiHostUrl + "UpComingCars?pageNo=" + (PageNo + 1) + "&pageSize=" + PageSize;
        }

        /*
         Author:Rakesh Yadav
         Date Created: 8 Nov 2013
         Desc: Set properties and populate upcomming cars of perticular make 
         */
        public UpComingCars(string makeId, string pageNo, string pageSize)
        {
            PageNo = Convert.ToInt32(pageNo);
            PageSize = Convert.ToInt32(pageSize);

            if (makeId != null && makeId != "")
                MakeId = makeId;

            GetUpcomingCarDetails();

            if ((Convert.ToInt32(PageNo) * Convert.ToInt32(PageSize)) >= CarCount)
                NextPageUrl = "";
            else
                NextPageUrl = CommonOpn.ApiHostUrl + "UpComingCars?makeId=" + MakeId + "&pageNo=" + (PageNo + 1) + "&pageSize=" + PageSize;
        }

        /*
         Author: Rakesh Yadav
         Date Created: 8 Nov 2013         
         Desc: Get car makes list, upcoming cars and upcoming car count
         */
        private void GetUpcomingCarDetails()
        {
            GetCarMakes();
            GetUpComingCars();          
        }

        /*
         Author:Rakesh Yadav
         Date Created: 30 July 2013
         Desc: Populate upcomming cars
         */
        public void GetUpComingCars()
        {
            string detailUrl = "";
            ICarModels carModelrepo = UnityBootstrapper.Resolve<ICarModels>();
            ICMSContent _cacheSynopsis = UnityBootstrapper.Resolve<ICMSContent>();
            var allCars = carModelrepo.GetUpcomingCarModels(new Carwale.Entity.Pagination() { PageNo = (ushort)pageNo , PageSize = (ushort)pageSize} ,Convert.ToInt32(MakeId));
            try
            {
                foreach (var upcomingModel in allCars)
                {
                    HtmlDocument doc = new HtmlDocument();
                    CarCount = upcomingModel.RecordCount;
                    var synopsis = _cacheSynopsis.GetCarSynopsis(upcomingModel.ModelId, (int)Carwale.Entity.Enum.Application.CarWale);
                    if (MakeId != "-1")
                        detailUrl = CommonOpn.ApiHostUrl + "UpComingCarDetail?id=" + upcomingModel.ModelId.ToString() + "&makeId=" + MakeId;
                    else
                        detailUrl = CommonOpn.ApiHostUrl + "UpComingCarDetail?id=" + upcomingModel.ModelId.ToString();
                    doc.LoadHtml(synopsis.Description);
                    upCommingCars.Add(new UpComingCarItem
                    {
                        CarName = upcomingModel.MakeName.ToString() + " " + upcomingModel.ModelName.ToString(),
                        ExpectedLaunchDate = upcomingModel.ExpectedLaunch,
                        EstimatedPrice = Format.PriceLacCr(upcomingModel.Price.MinPrice.ToString("0.")) + " - " + Format.PriceLacCr(upcomingModel.Price.MaxPrice.ToString("0.")),
                        LargePicUrl = ImageSizes.CreateImageUrl(upcomingModel.Image.HostUrl.ToString(), ImageSizes._210X118, upcomingModel.Image.ImagePath),
                        SmallpicUrl = ImageSizes.CreateImageUrl(upcomingModel.Image.HostUrl.ToString(), ImageSizes._110X61, upcomingModel.Image.ImagePath),
                        HostUrl = upcomingModel.Image.HostUrl,
                        OriginalImgPath = upcomingModel.Image.ImagePath,
                        DetailUrl = detailUrl,
                        Description = doc.DocumentNode.InnerText
                    });

                }
            }
            catch (Exception err)
            {
                ServerErrorOccured = true;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /*
         Author: Rakesh Yadav
         Date Created : 8 Nov 2013
         Desc : get car makes list
         */
        private void GetCarMakes()
        {
            Item allMakes = new Item();
            allMakes.Id = "-1";
            allMakes.Name = "All brands";
            allMakes.Url = CommonOpn.ApiHostUrl + "UpComingCars?pageNo=1&pageSize=" + PageSize;
            carMakes.Add(allMakes);

            ICarMakesCacheRepository makeCacheRepo = UnityBootstrapper.Resolve<ICarMakesCacheRepository>();
            List<CarMakeEntityBase> makeList = new List<CarMakeEntityBase>();
            makeList = makeCacheRepo.GetCarMakesByType("upcoming");
            foreach (var make in makeList)
            {
                Item upcomingmake = new Item();
                upcomingmake.Id = make.MakeId.ToString();
                upcomingmake.Name = make.MakeName;
                upcomingmake.Url = CommonOpn.ApiHostUrl + "UpComingCars?makeId=" + make.MakeId.ToString() + "&pageNo=1&pageSize=" + PageSize;
                carMakes.Add(upcomingmake);
            }
        }
    }
}