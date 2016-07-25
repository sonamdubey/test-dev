using Bikewale.DAL.Compare;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 12 May 2016
    /// Desc       : View Model to bind and pass repeater data to control
    /// </summary>
    public class BindSimilarCompareBikesControl
    {
        public uint FetchedRecordsCount { get; set; }
        public uint BindAlternativeBikes(Repeater rptSimlarCompareBikes, string versionList, uint count)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    IEnumerable<SimilarCompareBikeEntity> objSimilarBikes = new List<SimilarCompareBikeEntity>();
                    container.RegisterType<IBikeCompare, BikeCompareRepository>();
                    IBikeCompare objCompare = container.Resolve<IBikeCompare>();
                    objSimilarBikes = objCompare.GetSimilarCompareBikes(versionList, count);
                    if (objSimilarBikes != null)
                        FetchedRecordsCount = (uint)objSimilarBikes.Count();

                    if (FetchedRecordsCount > 0)
                    {
                        rptSimlarCompareBikes.DataSource = objSimilarBikes;
                        rptSimlarCompareBikes.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return FetchedRecordsCount;
        }

        /// <summary>
        /// Author : Vivek Gupta
        /// Date : 13-05-2016
        /// Desc : overload to get return of type IEnumerable<SimilarCompareBikeEntity>
        /// </summary>
        /// <param name="versionList"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<SimilarCompareBikeEntity> BindAlternativeBikes(string versionList, uint count)
        {
            IEnumerable<SimilarCompareBikeEntity> objSimilarBikes = new List<SimilarCompareBikeEntity>();

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeCompare, BikeCompareRepository>();
                    IBikeCompare objCompare = container.Resolve<IBikeCompare>();
                    objSimilarBikes = objCompare.GetSimilarCompareBikes(versionList, count);
                    if (objSimilarBikes != null)
                        FetchedRecordsCount = (uint)objSimilarBikes.Count();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objSimilarBikes;
        }
    }
}