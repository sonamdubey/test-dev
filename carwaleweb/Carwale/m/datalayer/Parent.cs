using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Security.Principal;
using System.Net.Mail;
using System.IO;
using System.Xml;
using MobileWeb.Common;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace MobileWeb.DataLayer 
{
	public class Parent : IDisposable
	{
		protected SqlDataReader _dr = null;
        protected IDataReader _drMysql = null;
		protected DataSet _ds = new DataSet();
		protected bool _getReader = false, _getDataSet = false, _getRepeater = false, _getDropDownList = false;
        protected string _sql, _sp;
		protected string _textField = "", _valueField = "";
		public SqlParameter [] Param = {};
		public Repeater Rpt;
		public DropDownList Ddl;		

        public void Dispose()
        {
            this._ds.Dispose();
        }

		public SqlDataReader dr 
		{ 
			get {return _dr;} 
			set {_dr=value; } 
		}
		
		public DataSet ds 
		{ 
			get {return _ds;} 
			set {_ds=value; } 
		}
		
		public bool GetReader 
		{ 
			get {return _getReader;} 
			set {_getReader=value; } 
		}
		
		public bool GetDataSet 
		{ 
			get {return _getDataSet;} 
			set {_getDataSet=value; } 
		}
		
		public bool GetRepeater 
		{ 
			get {return _getRepeater;} 
			set {_getRepeater=value; } 
		}
		
		public bool GetDropDownList 
		{ 
			get {return _getDropDownList;} 
			set {_getDropDownList=value; } 
		}
		
		public string TextField 
		{ 
			get {return _textField;} 
			set {_textField=value; } 
		}
		
		public string ValueField 
		{ 
			get {return _valueField;} 
			set {_valueField=value; } 
		}
		
		public string Sql 
		{ 
			get {return _sql;} 
			set {_sql=value; } 
		}

        public string Sp
        {
            get { return _sp; }
            set { _sp = value; }
        }

        public IDataReader drReader
        {
            get { return _drMysql; }
            set { _drMysql = value; }
        }

        public void LoadDataMySql(DbCommand cmd,string dbname)
        {
            if (GetReader)
                LoadReaderMySql(cmd, dbname);
            else if (GetDataSet)
                LoadDataSetMySql(cmd, dbname);
            else if (GetRepeater)
                LoadRepeaterMySql(cmd, dbname);
            else if (GetDropDownList)
                LoadDropDownListMySql(cmd, dbname);
        }    				

        private void LoadReaderMySql(DbCommand cmd,string dbname)
        {
            try
            {
                    drReader = MySqlDatabase.SelectQuery(cmd, dbname);

            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private void LoadDataSetMySql(DbCommand cmd, string dbname)
        {
            try
            {             
                using (DbCommand cmdMysql = cmd)
                {         
                    ds = MySqlDatabase.SelectAdapterQuery(cmdMysql, dbname);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private void LoadRepeaterMySql(DbCommand cmd, string dbname)
        {
            try
            {
                LoadReaderMySql(cmd, dbname);
                Rpt.DataSource = drReader;
                Rpt.DataBind();
            }
            catch (Exception err)
            {

                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                drReader.Close();
            }
        }

        private void LoadDropDownListMySql(DbCommand cmd, string dbname)
        {
            try
            {
                LoadReaderMySql(cmd, dbname);
                Ddl.DataSource = drReader;
                Ddl.DataTextField = TextField;
                Ddl.DataValueField = ValueField;
                Ddl.DataBind();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                drReader.Close();
            }
        }

    }
}		