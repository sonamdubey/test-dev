using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    public class SimilarBikes : UserControl
    {
        protected Repeater rptSimilarBikes;
        protected BikeVersionEntity bikeVersionEntity;
        protected IEnumerable<SimilarBikeEntity> objSimilarBikes;

        protected int recordCount = 0;
        public int cityid { get; set; }
        private string _topCount = "3";
        public string TopCount
        {
            get { return _topCount; }
            set { _topCount = value; }
        }

        private string _versionId = string.Empty;
        public string VersionId
        {
            get { return _versionId; }
            set { _versionId = value; }
        }

        protected uint _percentDeviation = 15;
        public uint PercentDeviation
        {
            get { return _percentDeviation; }
            set { _percentDeviation = value; }
        }

        //Added By : Suresh Prajapati on 19 Aug 2014
        //Summary : flag to check discontinued bike
        protected bool _isNew = true;
        public bool IsNew
        {
            get { return _isNew; }
            set { _isNew = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(VersionId))
            {
                GetVersionDetail();
                BindSimilarBikes();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 5th Aug 2014
        /// Summary : To retrieve similar bikes w.r.t. versionId
        /// </summary>
        private void BindSimilarBikes()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeVersions<BikeVersionEntity, int>, BikeVersions<BikeVersionEntity, int>>();
                    IBikeVersions<BikeVersionEntity, int> objVersion = container.Resolve<IBikeVersions<BikeVersionEntity, int>>();

                    objSimilarBikes = objVersion.GetSimilarBikesList(Convert.ToInt32(VersionId), Convert.ToUInt32(TopCount), Convert.ToUInt32(cityid), false);

                    if (objSimilarBikes.Any())
                    {
                        rptSimilarBikes.DataSource = objSimilarBikes;
                        rptSimilarBikes.DataBind();
                    }
                }
            }
            catch (SqlException exSql)
            {
                ErrorClass.LogError(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }   //End of BindComparison

        /// <summary>
        /// Created By : Sadhana Upadhyay on 5th Aug 2014
        /// Summary : To get bike detail by version id
        /// </summary>
        protected void GetVersionDetail()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeVersionCacheRepository<BikeVersionEntity, uint>, BikeVersionsCacheRepository<BikeVersionEntity, uint>>()
                        .RegisterType<IBikeVersions<BikeVersionEntity, uint>, BikeVersions<BikeVersionEntity, uint>>()
                              .RegisterType<ICacheManager, MemcacheManager>()
                             ;
                    var objCache = container.Resolve<IBikeVersionCacheRepository<BikeVersionEntity, uint>>();


                    bikeVersionEntity = objCache.GetById(Convert.ToUInt32(VersionId));
                }
            }
            catch (SqlException exSql)
            {
                Trace.Warn("GetVersionDetail", exSql.LineNumber.ToString());
                ErrorClass.LogError(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                Trace.Warn("GetVersionDetail", ex.Message.ToString());
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }   // End of GetVersionDetail
    }   // End of Class
}   // End of namespace