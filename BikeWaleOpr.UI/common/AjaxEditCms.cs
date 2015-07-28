using System;
using System.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BikeWaleOpr.Common;
using BikeWaleOpr.EditCms;
using AjaxPro;

/// <summary>
/// Summary description for AjaxTest
/// </summary>
/// 
namespace BikeWale
{
    public class AjaxEditCms
    {
        [AjaxPro.AjaxMethod()]
        public bool PublishArticle(string addToForum, string message, string bid, string path, string articleType, string customerId, string titleText, string isDealerFriendly)
        {
            bool retVal = false;

            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            Database db = new Database();

            string conStr = db.GetConString();
            con = new SqlConnection(conStr);

            try
            {
                cmd = new SqlCommand("Con_EditCms_PublishArticle", con);
                cmd.CommandType = CommandType.StoredProcedure;

                prm = cmd.Parameters.Add("@BasicId", SqlDbType.BigInt);
                prm.Value = bid;

                prm = cmd.Parameters.Add("@Path", SqlDbType.VarChar, 50);
                prm.Value = path;

                prm = cmd.Parameters.Add("@AddToForum", SqlDbType.Bit);
                if (addToForum == "1")
                    prm.Value = true;
                else
                    prm.Value = false;

                prm = cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt);
                prm.Value = customerId;

                prm = cmd.Parameters.Add("@Message", SqlDbType.VarChar, 500);
                prm.Value = message;

                prm = cmd.Parameters.Add("@ArticleType", SqlDbType.Int);
                prm.Value = articleType;

                prm = cmd.Parameters.Add("@TitleText", SqlDbType.VarChar, 100);
                prm.Value = titleText;

                prm = cmd.Parameters.Add("@IsDealerFriendly", SqlDbType.Bit);
                if (isDealerFriendly == "1")
                    prm.Value = true;
                else
                    prm.Value = false;

                con.Open();

                cmd.ExecuteNonQuery();

                retVal = true;

            }
            catch (Exception err)
            {
                retVal = false;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return retVal;
        }   // End of PublishArticle method

        [AjaxPro.AjaxMethod()]
        public string GetSubCategory(string categoryId)
        {
            string sql = string.Empty;
            StringBuilder sb = new StringBuilder();
            CommonEditCms objCommon = new CommonEditCms();
            Database db = new Database();

            sql = "Select Id, Name from Con_EditCms_SubCategories Where CategoryId = @CategoryId And IsActive = 1 Order By Name ";

            SqlParameter[] param = { new SqlParameter("@CategoryId", categoryId) };
            try
            {
                sb.Append(objCommon.GetJSONString(db.SelectAdaptQry(sql, param).Tables[0]));
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return sb.ToString();
        }   // End if GetSubCategory

        [AjaxPro.AjaxMethod()]
        public bool AllowBikeSelection(string categoryId)
        {
            bool returnVal = true;
            string sql = "SELECT AllowBikeSelection FROM Con_EditCms_Category WHERE ID = @CategoryId";
            SqlDataReader dr = null;
            Database db = new Database();
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CategoryId", SqlDbType.BigInt).Value = categoryId;

                dr = db.SelectQry(cmd);
                if (dr.Read())
                {
                    returnVal = Convert.ToBoolean(dr[0]);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (dr != null)
                    dr.Close();
                db.CloseConnection();
            }
            return returnVal;
        }

        [AjaxPro.AjaxMethod()]
        public string CallTestMethod()
        {
            return "test method called";
        }

        [AjaxPro.AjaxMethod()]
        public bool UpdateBasicInfo(string id, string title, string authorName, string authorId, string description, string displayDate, string subCatId, string cfId, string valType, string value, string extdInfoId, string isFeatured)
        {
            bool retVal = false;
            string strRetVal = string.Empty;

            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            Database db = new Database();

            string conStr = db.GetConString();
            con = new SqlConnection(conStr);

            string[] datetimeVal = displayDate.Split('-');

            try
            {
                cmd = new SqlCommand("Con_EditCms_Basic_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;

                prm = cmd.Parameters.Add("@Title", SqlDbType.VarChar, 250);
                prm.Value = title;

                prm = cmd.Parameters.Add("@DisplayDate", SqlDbType.DateTime);
                prm.Value = new DateTime(int.Parse(datetimeVal[0]), int.Parse(datetimeVal[1]), int.Parse(datetimeVal[2]), int.Parse(datetimeVal[3]), int.Parse(datetimeVal[4]), 00);

                prm = cmd.Parameters.Add("@AuthorName", SqlDbType.VarChar, 100);
                prm.Value = authorName;

                prm = cmd.Parameters.Add("@AuthorId", SqlDbType.BigInt);
                prm.Value = authorId;

                prm = cmd.Parameters.Add("@Description", SqlDbType.VarChar, 8000);
                prm.Value = description;

                prm = cmd.Parameters.Add("@LastUpdatedBy", SqlDbType.BigInt);
                prm.Value = CurrentUser.Id;

                prm = cmd.Parameters.Add("@LastUpdatedTime", SqlDbType.DateTime);
                prm.Value = DateTime.Now;

                prm = cmd.Parameters.Add("@SubCatId", SqlDbType.VarChar, 2000);
                prm.Value = subCatId;

                prm = cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                prm.Value = id;

                prm = cmd.Parameters.Add("@CFId", SqlDbType.VarChar, 200);
                prm.Value = cfId;

                prm = cmd.Parameters.Add("@ValType", SqlDbType.VarChar, 200);
                prm.Value = valType;

                prm = cmd.Parameters.Add("@Value", SqlDbType.VarChar, 2000);
                prm.Value = value;

                prm = cmd.Parameters.Add("@ExtdInfoId", SqlDbType.VarChar, 200);
                prm.Value = extdInfoId;

                prm = cmd.Parameters.Add("@IsFeatured", SqlDbType.Bit);
                prm.Value = isFeatured;                

                //Trace.Warn("SubCatId: " + subCatId);
                //Trace.Warn("id: " + id);
                con.Open();
                cmd.ExecuteNonQuery();
                strRetVal = "Done";
                //if (divDynamicControl.Controls.Count > 0)
                //{
                //    saveOtherInfo((HtmlTable)divDynamicControl.FindControl("tblOtherInfo"), Request.QueryString["bid"]);
                //}
                //SaveOtherInfo(id);

                retVal = true;
            }
            catch (SqlException ex)
            {
                retVal = false;
                strRetVal = ex.Message;
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                retVal = false;
                strRetVal = err.Message;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return retVal;
        }

        [AjaxPro.AjaxMethod()]
        public int SaveBasicInfo(string categoryId, string title, string url, string authorName, string authorId, string description, string displayDate, string subCatId, string cfId, string valType, string value, string isFeatured)
        {
            int retVal = 0;
            string strRetVal = string.Empty;

            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            Database db = new Database();

            string conStr = db.GetConString();
            con = new SqlConnection(conStr);

            string[] datetimeVal = displayDate.Split('-');

            try
            {
                cmd = new SqlCommand("Con_EditCms_Basic_Save", con);
                cmd.CommandType = CommandType.StoredProcedure;

                prm = cmd.Parameters.Add("@CategoryId", SqlDbType.BigInt);
                prm.Value = categoryId;

                prm = cmd.Parameters.Add("@Title", SqlDbType.VarChar, 250);
                prm.Value = title;

                prm = cmd.Parameters.Add("@DisplayDate", SqlDbType.DateTime);
                prm.Value = new DateTime(int.Parse(datetimeVal[0]), int.Parse(datetimeVal[1]), int.Parse(datetimeVal[2]), int.Parse(datetimeVal[3]), int.Parse(datetimeVal[4]), 00);

                prm = cmd.Parameters.Add("@AuthorName", SqlDbType.VarChar, 100);
                prm.Value = authorName;

                prm = cmd.Parameters.Add("@AuthorId", SqlDbType.BigInt);
                prm.Value = authorId;

                prm = cmd.Parameters.Add("@Description", SqlDbType.VarChar, 8000);
                prm.Value = description;

                prm = cmd.Parameters.Add("@LastUpdatedBy", SqlDbType.BigInt);
                prm.Value = CurrentUser.Id;

                prm = cmd.Parameters.Add("@LastUpdatedTime", SqlDbType.DateTime);
                prm.Value = DateTime.Now;

                prm = cmd.Parameters.Add("@Url", SqlDbType.VarChar, 200);
                prm.Value = url;

                prm = cmd.Parameters.Add("@EnteredBy", SqlDbType.BigInt);
                prm.Value = CurrentUser.Id;

                prm = cmd.Parameters.Add("@EntryDate", SqlDbType.DateTime);
                prm.Value = DateTime.Now;

                prm = cmd.Parameters.Add("@SubCatId", SqlDbType.VarChar, 2000);
                prm.Value = subCatId;

                prm = cmd.Parameters.Add("@CFId", SqlDbType.VarChar, 2000);
                prm.Value = cfId;

                prm = cmd.Parameters.Add("@ValType", SqlDbType.VarChar, 2000);
                prm.Value = valType;

                prm = cmd.Parameters.Add("@Value", SqlDbType.VarChar, 2000);
                prm.Value = value;

                prm = cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@IsFeatured", SqlDbType.Bit);
                prm.Value = isFeatured;                

                //Trace.Warn("SubCatId: " + subCatId);
                //Trace.Warn("id: " + id);
                con.Open();
                cmd.ExecuteNonQuery();
                if (cmd.Parameters["@ID"].Value.ToString() != "")
                    retVal = int.Parse(cmd.Parameters["@ID"].Value.ToString());
            }
            catch (SqlException ex)
            {
                retVal = 0;
                strRetVal = "SQL:   " + ex.Message;
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                retVal = 0;
                strRetVal = err.Message;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return retVal;
            //return strRetVal;
        }

        [AjaxPro.AjaxMethod()]
        public bool CheckExtdInfoExist(string categoryId)
        {
            bool retVal = false;
            string sql = string.Empty;
            SqlCommand cmd = new SqlCommand();
            Database db = new Database();
            SqlDataReader dr = null;
            string strRetVal = string.Empty;

            sql = "Select ID From Con_EditCms_CategoryFields Where CategoryId = @CategoryId And IsActive = 1";
            try
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = categoryId;

                dr = db.SelectQry(cmd);
                while (dr.Read())
                {
                    strRetVal = dr["ID"].ToString() + ",";
                }
                if (strRetVal != string.Empty)
                {
                    retVal = true;
                }
                else
                {
                    retVal = false;
                }
            }
            catch (SqlException ex)
            {
                retVal = false;
                strRetVal = ex.Message;
                //strRetVal = string.Empty;
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                retVal = false;
                strRetVal = err.Message;
                //strRetVal = string.Empty;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
                db.CloseConnection();
            }
            return retVal;
            //return strRetVal;
        }

        [AjaxPro.AjaxMethod()]
        public string FetchExtdInfo(string categoryId)
        {
            string sql = string.Empty;

            SqlCommand cmd = new SqlCommand();
            Database db = new Database();
            DataSet ds = new DataSet();
            StringBuilder sbExtdInfo = new StringBuilder();

            sql = "Select ID, FieldName, ValueType From Con_EditCms_CategoryFields Where CategoryId = @CategoryId And IsActive = 1";
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = categoryId;

            try
            {
                ds = db.SelectAdaptQry(cmd);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; ++i)
                    {
                        sbExtdInfo.Append(dt.Rows[i]["ID"].ToString()).Append(",");
                        sbExtdInfo.Append(dt.Rows[i]["FieldName"].ToString()).Append(",");
                        switch (dt.Rows[i]["ValueType"].ToString())
                        {
                            case "1": sbExtdInfo.Append("checkbox_").Append(dt.Rows[i]["ValueType"].ToString());
                                break;
                            case "2": sbExtdInfo.Append("textbox_").Append(dt.Rows[i]["ValueType"].ToString());
                                break;
                            case "3": sbExtdInfo.Append("textbox_").Append(dt.Rows[i]["ValueType"].ToString());
                                break;
                            case "4": sbExtdInfo.Append("textbox_").Append(dt.Rows[i]["ValueType"].ToString());
                                break;
                            case "5": sbExtdInfo.Append("date_").Append(dt.Rows[i]["ValueType"].ToString());
                                break;
                        }
                        sbExtdInfo.Append("|");
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return sbExtdInfo.ToString();
        }

        [AjaxPro.AjaxMethod()]
        public string FetchExtdInfoData(string basicId, string valType, string cfId)
        {
            string sql = string.Empty;
            string valField = string.Empty;
            string valElem = string.Empty;
            SqlCommand cmd = new SqlCommand();
            Database db = new Database();
            DataSet ds = new DataSet();
            StringBuilder sbExtdInfo = new StringBuilder();

            switch (valType)
            {
                case "1":
                    valField = "BooleanValue";
                    valElem = "checkbox";
                    break;
                case "2":
                    valField = "NumericValue";
                    valElem = "textbox";
                    break;
                case "3":
                    valField = "DecimalValue";
                    valElem = "textbox";
                    break;
                case "4":
                    valField = "TextValue";
                    valElem = "textbox";
                    break;
                case "5":
                    valField = "DateTimeValue";
                    valElem = "textbox";
                    break;
            }

            sql = "Select ID, " + valField + " From Con_EditCms_OtherInfo Where BasicId = @BasicId And CategoryFieldId = @CFId";


            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@BasicId", SqlDbType.BigInt).Value = basicId;
            cmd.Parameters.Add("@CFId", SqlDbType.BigInt).Value = cfId;

            try
            {
                ds = db.SelectAdaptQry(cmd);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; ++i)
                    {
                        sbExtdInfo.Append(dt.Rows[i]["ID"].ToString()).Append("|");
                        sbExtdInfo.Append(dt.Rows[i][valField].ToString()).Append("|").Append(valElem);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return sbExtdInfo.ToString();
        }

        [AjaxPro.AjaxMethod()]
        public string SaveBasicInfo_NEW(string categoryId, string title, string url, string authorName, string authorId, string description, string displayDate, string subCatId, string cfId, string valType, string value)
        {
            int retVal = 0;
            string strRetVal = string.Empty;

            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            Database db = new Database();

            string conStr = db.GetConString();
            con = new SqlConnection(conStr);

            string[] datetimeVal = displayDate.Split('-');

            try
            {
                cmd = new SqlCommand("Con_EditCms_Basic_Save", con);
                cmd.CommandType = CommandType.StoredProcedure;

                prm = cmd.Parameters.Add("@CategoryId", SqlDbType.BigInt);
                prm.Value = categoryId;

                prm = cmd.Parameters.Add("@Title", SqlDbType.VarChar, 250);
                prm.Value = title;

                prm = cmd.Parameters.Add("@DisplayDate", SqlDbType.DateTime);
                prm.Value = new DateTime(int.Parse(datetimeVal[0]), int.Parse(datetimeVal[1]), int.Parse(datetimeVal[2]), int.Parse(datetimeVal[3]), int.Parse(datetimeVal[4]), 00);

                prm = cmd.Parameters.Add("@AuthorName", SqlDbType.VarChar, 100);
                prm.Value = authorName;

                prm = cmd.Parameters.Add("@AuthorId", SqlDbType.BigInt);
                prm.Value = authorId;

                prm = cmd.Parameters.Add("@Description", SqlDbType.VarChar, 8000);
                prm.Value = description;

                prm = cmd.Parameters.Add("@LastUpdatedBy", SqlDbType.BigInt);
                prm.Value = CurrentUser.Id;

                prm = cmd.Parameters.Add("@LastUpdatedTime", SqlDbType.DateTime);
                prm.Value = DateTime.Now;

                prm = cmd.Parameters.Add("@Url", SqlDbType.VarChar, 200);
                prm.Value = url;

                prm = cmd.Parameters.Add("@EnteredBy", SqlDbType.BigInt);
                prm.Value = CurrentUser.Id;

                prm = cmd.Parameters.Add("@EntryDate", SqlDbType.DateTime);
                prm.Value = DateTime.Now;

                prm = cmd.Parameters.Add("@SubCatId", SqlDbType.VarChar, 2000);
                prm.Value = subCatId;

                prm = cmd.Parameters.Add("@CFId", SqlDbType.BigInt);
                prm.Value = cfId == string.Empty ? 0 : int.Parse(cfId);

                prm = cmd.Parameters.Add("@ValType", SqlDbType.BigInt);
                prm.Value = valType == string.Empty ? 0 : int.Parse(valType);

                prm = cmd.Parameters.Add("@Value", SqlDbType.VarChar, 250);
                prm.Value = value;

                prm = cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                prm.Direction = ParameterDirection.Output;

                //Trace.Warn("SubCatId: " + subCatId);
                //Trace.Warn("id: " + id);
                con.Open();
                cmd.ExecuteNonQuery();
                if (cmd.Parameters["@ID"].Value.ToString() != "")
                    retVal = int.Parse(cmd.Parameters["@ID"].Value.ToString());
                //if (divDynamicControl.Controls.Count > 0)
                //{
                //    saveOtherInfo((HtmlTable)divDynamicControl.FindControl("tblOtherInfo"), Request.QueryString["bid"]);
                //}
                //SaveOtherInfo(id);

            }
            catch (SqlException ex)
            {
                retVal = 0;
                strRetVal = ex.Message;
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                retVal = 0;
                strRetVal = err.Message;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            //return retVal;
            return strRetVal;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay
        /// Summary : To unpublish articles
        /// </summary>
        /// <param name="bid">Basic Id</param>
        /// <param name="customerId">Current Customer Id</param>
        /// <param name="reasonToUnpublish"> Reason to Unpublish</param>
        /// <param name="cid">Category Id</param>
        /// <returns></returns>
    [AjaxPro.AjaxMethod()]
        public bool UnPublishArticle(string bid, string customerId, string reasonToUnpublish, string cid)
        {
            bool retVal = false;
            Database db = new Database();

            try
            {
                using(SqlConnection con=new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Con_EditCMS_UnpublishArticle";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@BasicId", SqlDbType.Int).Value = bid;
                        cmd.Parameters.Add("@ReasonToUnpublish", SqlDbType.VarChar, 250).Value = reasonToUnpublish;
                        cmd.Parameters.Add("@UnpublishBy", SqlDbType.Int).Value = customerId;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                retVal = true;
            }
            catch (SqlException err)
            {
                retVal = false;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return retVal;
        }   // End of UnPublishArticle
    }
}