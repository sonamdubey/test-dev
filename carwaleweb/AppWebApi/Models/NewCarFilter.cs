using AppWebApi.Common;
using Carwale.Entity;
using Carwale.Interfaces;
using Carwale.Service;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Linq;

namespace AppWebApi.Models
{
    public class NewCarFilter
    {
        private readonly ICarMakesCacheRepository _carMakesCacheRepo;
        public NewCarFilter()
        {
             using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
             {
                 _carMakesCacheRepo = container.Resolve<ICarMakesCacheRepository>();
             }
             GetMakes();
            
        }

        private bool serverErrorOccurred = false;
        [JsonIgnore]
        public bool ServerErrorOccurred
        {
            get { return serverErrorOccurred; }
            set { serverErrorOccurred = value; }
        }

        public List<FilterItem> makes = new List<FilterItem>();
        /* Author: Rakesh Yadav
         * Date Created: 12 June 2013
         * Discription: create list of filter */
        


        /* Author: Rakesh Yadav
         * Date Created: 12 June 2013
         * Discription: Get list of makes
         * Date Modified: 23 Apr 2014
         * Discription: Removed Ids(56,61) from sql query
         */
        private void GetMakes()
        {
            try
            {
                List<MakeLogoEntity> carDetailsbyMake = new List<MakeLogoEntity>();
                carDetailsbyMake = _carMakesCacheRepo.GetCarMakesWithLogo("new");
                makes = carDetailsbyMake.Select(x => new FilterItem { Value = x.MakeId.ToString(), Label = x.MakeName ,Icon = x.HostURL + ImageSizes._0X0  + x.OriginalImgPath}).ToList();
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