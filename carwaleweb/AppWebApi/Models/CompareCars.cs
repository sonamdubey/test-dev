using AppWebApi.Common;
using Carwale.Cache.CompareCars;
using Carwale.Entity;
using Carwale.Entity.CompareCars;
using Carwale.Interfaces.CompareCars;
using Carwale.Service;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace AppWebApi.Models
{
    public class CompareCars
    {
        private bool serverErrorOccurred = false;
        [JsonIgnore]
        public bool ServerErrorOccurred
        {
            get { return serverErrorOccurred; }
            set { serverErrorOccurred = value; }
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

        private int carCount = 0;
        private int CarCount
        {
            get { return carCount; }
            set { carCount = value; }
        }

        private string moreCompCarUrl = "";
        [JsonProperty("moreCompCarUrl")]
        public string MoreCompCarUrl
        {
            get { return moreCompCarUrl; }
            set { moreCompCarUrl = value; }
        }

        public List<CompareCarItem> compareCars = new List<CompareCarItem>();
        IUnityContainer container = UnityBootstrapper.Resolver.GetContainer();
        public ICompareCarsCacheRepository compareCarCache() {return container.Resolve<CompareCarsCache>();}

        /*
        Author:Rakesh Yadav
        Date Created: 08 Apr 2014
        Desc: get cars for comparision, count of cars pairs to b compared and form next page url
        */
        public CompareCars(string pageNo, string pageSize)
        {
            PageNo = pageNo;
            PageSize = pageSize;
            GetCompareCars();
            GetCompareCarCount();

            if ((Convert.ToInt32(PageNo) * Convert.ToInt32(PageSize)) >= CarCount)
                MoreCompCarUrl = "";
            else
                MoreCompCarUrl = CommonOpn.ApiHostUrl + "CompareCarList?pageNo=" + (Convert.ToInt32(PageNo) + 1) + "&pageSize=" + PageSize;
        }

        /*
        Author:Rakesh Yadav
        Date Created: 08 Apr 2014
        Desc: Fetch cars to be comapred from database and populate it into list
        */
        private void GetCompareCars()
        {
            try
            {
                
                List<CompareCarOverview> compareCarList = new List<CompareCarOverview>();
                compareCarList= compareCarCache().GetCompareCarsDetails(new Pagination() { PageNo = Convert.ToUInt16(PageNo), PageSize = Convert.ToUInt16(PageSize) });
                foreach (var car in compareCarList)
                {
                    compareCars.Add(new CompareCarItem {
                        FirstCaName = car.Car1.CarName,
                        SecondCarName = car.Car2.CarName,
                        ImageUrl = car.HostUrl + ImageSizes._0X0 + car.OriginalImgPath,
                        DetailUrl = CommonOpn.ApiHostUrl + "CompareCarsDetails?version1=" + car.Car1.VersionId.ToString() + "&version2=" + car.Car2.VersionId.ToString(),
                        FirstCarVersionId = car.Car1.VersionId.ToString(),
                        SecondCarVersionId=car.Car2.VersionId.ToString(),
                        HostUrl=car.HostUrl,
                        OriginalImgPath=car.OriginalImgPath
                    });
                }
            }
            catch (Exception err)
            {
                ServerErrorOccurred = true;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /*
        Author:Rakesh Yadav
        Date Created: 08 Apr 2014
        Desc: get count of total records available for comaprision for pagination
        */
        private void GetCompareCarCount()
        {
            try
            {
                CarCount=compareCarCache().GetCompareCarCount();
            }
            catch (Exception err)
            {
                ServerErrorOccurred = true;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}