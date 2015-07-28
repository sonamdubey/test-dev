using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Bikewale.Common;
using Microsoft.Practices.Unity;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.BAL.BikeData;
using System.Collections.Generic;


namespace Bikewale.Controls
{
    public class SimilarBikes : UserControl
    {
        protected Repeater rptSimilarBikes;
        protected BikeVersionEntity bikeVersionEntity;
        protected List<SimilarBikeEntity> objSimilarBikes;

        protected int recordCount = 0;

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

                    objSimilarBikes = objVersion.GetSimilarBikesList(Convert.ToInt32(VersionId), Convert.ToUInt32(TopCount), PercentDeviation);

                    recordCount = objSimilarBikes.Count;

                    if (objSimilarBikes.Count > 0)
                    {
                        rptSimilarBikes.DataSource = objSimilarBikes;
                        rptSimilarBikes.DataBind();
                    }
                }
            }
            catch (SqlException exSql)
            {
                Trace.Warn("SimilarBikes",exSql.LineNumber.ToString());
                ErrorClass objErr = new ErrorClass(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                Trace.Warn("SimilarBikes", ex.Message.ToString());
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                    container.RegisterType<IBikeVersions<BikeVersionEntity, int>, BikeVersions<BikeVersionEntity, int>>();
                    IBikeVersions<BikeVersionEntity, int> objVersion = container.Resolve<IBikeVersions<BikeVersionEntity, int>>();

                    bikeVersionEntity = objVersion.GetById(Convert.ToInt32(VersionId));
                }
            }
            catch (SqlException exSql)
            {
                Trace.Warn("GetVersionDetail", exSql.LineNumber.ToString());
                ErrorClass objErr = new ErrorClass(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                Trace.Warn("GetVersionDetail", ex.Message.ToString());
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   // End of GetVersionDetail
    }   // End of Class
}   // End of namespace