using Bikewale.Common;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    public class BikesInBudget : UserControl
    {
        protected DataList dlHighlights;
        private string profileNo = "";
        private int records = 10, price = 0;
        private string cityId = "1";
        private int cityDistance = 0;

        protected Label lblCities;

        public string ProfileNo
        {
            set { profileNo = value; }
        }

        public int Records
        {
            set { records = value; }
        }

        private int _recordCount = 0;
        public int RecordCount
        {
            get { return _recordCount; }
            set { _recordCount = value; }
        }

        public int Price
        {
            set { price = value; }
        }

        private string _headerText = "No Worries... There are many to choose from";
        public string HeaderText
        {
            get { return _headerText; }
            set { _headerText = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CommonOpn op = new CommonOpn();

                cityId = op.GetCityId();
                cityDistance = Convert.ToInt16(op.GetCityDistance());

                ShowHighlights();
            }
        } // Page_Load


        //function to show more Bikes(10) at the same budget
        void ShowHighlights()
        {

            string sql;

            double lattDiff = CommonOpn.GetLattitude(cityDistance);
            double longDiff = CommonOpn.GetLongitude(cityDistance);
            if (price != 0)
            {
                sql = string.Format(@"select  ll.ProfileId, 
                         concat( bmo.MakeName ,' ', bmo.Name ,' ' , VersionName ) BikeMake, 
                         bmo.MakeName, bmo.Name as ModelName, 
                         lc.Name as CityName,
                         lc.maskingname as CityMaskingName, 
                         MakeYear, ll.Price, ll.Color, ll.Kilometers, bmo.makemaskingname as MakeMaskingName, bmo.maskingname as ModelMaskingName 
                         from livelistings as ll 
                         inner join bikemodels bmo  on ll.modelid = bmo.id
                         inner join bwcities as lc on  lc.id = {0} 
							                        and  ll.lattitude between (lc.lattitude - {1}) and (lc.lattitude + {1}) 
                                                    and  ll.longitude between (lc.longitude - {2}) and (lc.longitude + {2}) 
                         where 
                         ll.profileid <> '{3}'
                         order by abs(ll.price - {4}) asc 
                         limit {5};", cityId, lattDiff, longDiff, profileNo, price, records);

                DataSet ds = null;
                try
                {
                    ds = MySqlDatabase.SelectAdapterQuery(sql, ConnectionType.ReadOnly);
                    _recordCount = ds.Tables[0].Rows.Count;

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        dlHighlights.DataSource = ds.Tables[0];
                        dlHighlights.DataBind();
                    }
                }
                catch (Exception err)
                {
                    Trace.Warn(err.Message);
                    ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                    
                }
            }

            lblCities.Text = ""; //initially blank
        } // ShowHighlights



    }
}