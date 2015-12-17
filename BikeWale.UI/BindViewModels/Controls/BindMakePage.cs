using Bikewale.DTO.Make;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Bikewale.DTO.Widgets;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.DAL.BikeData;

namespace Bikewale.BindViewModels.Controls
{
    public class BindMakePage
    {
        public int totalCount { get; set; }
        public int makeId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeDescriptionEntity BikeDesc { get; set; }
        public Int64 MinPrice { get; set; }
        public Int64 MaxPrice { get; set; }

        public void BindMostPopularBikes(Repeater rptr)
        {            
            FetchedRecordsCount = 0;
            
            BikeDescriptionEntity description = null;
            IEnumerable<MostPopularBikesBase> objModelList = null;

            try
            {
                using(IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                        .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();

                    var makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
                    var modelRepository = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();

                    objModelList = modelRepository.GetMostPopularBikesByMake(makeId);
                    description = makesRepository.GetMakeDescription(makeId);                    
                }

                if (objModelList != null && objModelList.Count() > 0)
                {
                    FetchedRecordsCount = objModelList.Count();
                    Make = objModelList.FirstOrDefault().objMake;
                    BikeDesc = description;
                    MinPrice = objModelList.Min(bike => bike.VersionPrice);
                    MaxPrice = objModelList.Max(bike => bike.VersionPrice);

                    rptr.DataSource = objModelList;
                    rptr.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}