using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using Carwale.UI.Common;
using Microsoft.Practices.Unity;
using Carwale.Interfaces;
using Carwale.DAL.CarData;
using AEPLCore.Cache;
using Carwale.Interfaces.CarData;
using Carwale.Cache.CarData;
using Carwale.Entity.CarData;
using System.Collections.Generic;
using Carwale.BL.CarData;
using Carwale.Interfaces.Classified;
using Carwale.DAL.Classified;
using Carwale.Cache.Classified;
using Carwale.Notifications;
using Carwale.Service;
using Carwale.Utility;

namespace Carwale.UI.Controls
{
    public class UCSimilarCars : UserControl
    {
        private string modelId = "-1";
        private int recordCount = 0;
        protected Repeater rptCars;

        public string ModelId
        {
            get { return modelId; }
            set { modelId = value; }
        }

        public int RecordCount
        {
            get { return recordCount; }
            set { recordCount = value; }
        }

        public string CityName
        {
            get { return cityName; }
            set { cityName = value; }
        }
        public List<SimilarCarModels> SimilarModels = new List<SimilarCarModels>();

        public string MakeName = "", ModelName = "", cityName = string.Empty;
        public string MaskingName = "";

        public int PQPageId { get; set; }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            //this.Load += new EventHandler( this.Page_Load );
        }

        /// <summary>
        /// Written By : Shalini on 25/08/14
        /// Populates the list of similar car models(model alternatives)
        /// </summary>
        /// <returns></returns>
        public int LoadSimilarCars()
        {
            try
            {
                Trace.Warn("similar2:" + SimilarModels.Count);
                if (SimilarModels.Count > 0) //for model details page
                {
                    rptCars.DataSource = SimilarModels;
                    rptCars.DataBind();
                }
                else
                {
                    int model = Convert.ToInt16(modelId);
                    List<SimilarCarModels> lstSimilarCars = new List<SimilarCarModels>();
                    IUnityContainer container = UnityBootstrapper.Resolver.GetContainer();
                    ICarModels similarcarContainer = container.Resolve<ICarModels>();
                    lstSimilarCars = similarcarContainer.GetSimilarCarsByModel(model, CurrentUser.CWC);
                    var modelTuple = new Tuple<int, string>(model, (Format.FormatSpecial(MakeName) + '-' + MaskingName));
                    lstSimilarCars.ForEach(x =>
                    {
                        var compareCarsTupleList = new List<Tuple<int, string>>() {
                        modelTuple,
                        new Tuple<int, string>(x.ModelId, (Format.FormatSpecial(x.MakeName)+'-'+ x.MaskingName))
                    };
                        var formatCompareUrl = Format.GetCompareUrl(compareCarsTupleList);
                        x.CompareCarUrl = string.Format("/comparecars/{0}", !string.IsNullOrWhiteSpace(formatCompareUrl) ? formatCompareUrl + "/" : string.Empty);
                    });
                    rptCars.DataSource = lstSimilarCars;
                    rptCars.DataBind();
                }
                RecordCount = rptCars.Items.Count;
            }
            catch (Exception err)
            {
                Trace.Warn("in load similarcars exceptn");
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            return RecordCount;
        }

        public string GetLandingUrl(string isFeatured, string make, string maskingName, string spotlightUrl)
        {
            if (!(isFeatured == "1" && spotlightUrl != "") && cityName != string.Empty)
                return "/" + UrlRewrite.FormatSpecial(make) + "-cars/" + maskingName + "/price-in-" + cityName.Replace(" ", "").ToLower() + "/";
            else if (!(isFeatured == "1" && spotlightUrl != ""))
                return "/" + UrlRewrite.FormatSpecial(make) + "-cars/" + maskingName + "/";
            else
                return spotlightUrl;
        }
    }
}