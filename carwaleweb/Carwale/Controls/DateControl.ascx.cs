using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Carwale.UI.Controls
{
    public class DateControl : UserControl
    {
        protected DropDownList cmbDay, cmbMonth;
        protected TextBox txtYear;
        protected CheckBox chkDate;

        //string dateVspl="";
        DateTime dateValue = DateTime.Today;

        int futureTolerance = 0; // no Tolerance.

        // Need Checkbox?
        public bool Checkbox
        {
            get { return chkDate.Visible; }
            set { chkDate.Visible = value; }
        }

        // Future Date Tolerance! Default 0 i.e. No Tolerance.
        public int FutureTolerance
        {
            get { return futureTolerance; }
            set { futureTolerance = value; }
        }

        // Need Just Month and Year?
        public bool MonthYear
        {
            set
            {
                if (value)
                    cmbDay.Attributes["style"] = "display:none;";
            }
        }

        // Need Just Day and Month?
        public bool DayMonth
        {
            set
            {
                if (value)
                    txtYear.Attributes["style"] = "display:none;";
            }
        }

        /// <summary>
        /// MM/dd/yyyy
        /// </summary>
        public DateTime Value
        {
            //assign Day
            get
            {
                /*if( cmbMonth.SelectedIndex > -1 )
                    dateVspl= cmbMonth.SelectedValue.ToString();
                else
                    dateVspl = DateTime.Today.Month.ToString();
					
                if(cmbDay.SelectedIndex > -1 )
                    dateVspl=dateVspl + "/" + cmbDay.SelectedValue.ToString();
                else
                    dateVspl = dateVspl + "/" + DateTime.Today.Day;
									
                //assign Year	
                if( txtYear.Text != "" )
                    dateVspl = dateVspl + "/" + txtYear.Text.Trim() ;	
                else
                    dateVspl = dateVspl + "/" + DateTime.Today.Year;
					
                dateValue=Convert.ToDateTime( dateVspl );
                Trace.Warn("Returned Date" + dateValue );
				
                return dateValue ;*/


                string day = "", month = "", year = "";


                if (cmbMonth.SelectedIndex > -1)
                    month = cmbMonth.SelectedValue;
                else
                    month = DateTime.Today.Month.ToString();

                if (cmbDay.SelectedIndex > -1)
                    day = cmbDay.SelectedValue;
                else
                    day = DateTime.Today.Day.ToString();

                if (txtYear.Text.Trim() != "")
                    year = txtYear.Text.Trim();
                else
                    year = DateTime.Today.Year.ToString();

                dateValue = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));

                Trace.Warn("Returned Date" + dateValue);

                return dateValue;
            }
            set
            {
                dateValue = value;
                cmbDay.SelectedIndex = cmbDay.Items.IndexOf(cmbDay.Items.FindByValue(dateValue.Day.ToString()));

            }
        }

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            Trace.Warn("DateControl: Loading...");

            if (!IsPostBack)
            {
                fillDropDown();
                txtYear.Text = dateValue.Year.ToString();
            }
        }

        private void fillDropDown()
        {
            int i = 0;
            //fill days
            for (i = 1; i <= 31; i++)
            {
                cmbDay.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            cmbDay.SelectedIndex = cmbDay.Items.IndexOf(cmbDay.Items.FindByValue(dateValue.Day.ToString()));

            //fill month

            cmbMonth.Items.Add(new ListItem("Jan", "1"));
            cmbMonth.Items.Add(new ListItem("Feb", "2"));
            cmbMonth.Items.Add(new ListItem("Mar", "3"));
            cmbMonth.Items.Add(new ListItem("Apr", "4"));
            cmbMonth.Items.Add(new ListItem("May", "5"));
            cmbMonth.Items.Add(new ListItem("Jun", "6"));
            cmbMonth.Items.Add(new ListItem("Jul", "7"));
            cmbMonth.Items.Add(new ListItem("Aug", "8"));
            cmbMonth.Items.Add(new ListItem("Sep", "9"));
            cmbMonth.Items.Add(new ListItem("Oct", "10"));
            cmbMonth.Items.Add(new ListItem("Nov", "11"));
            cmbMonth.Items.Add(new ListItem("Dec", "12"));

            cmbMonth.SelectedIndex = cmbMonth.Items.IndexOf(cmbMonth.Items.FindByValue(dateValue.Month.ToString()));
        }



    } //class

}//namespace	