<%@ Page  Language="C#"   %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<script runat="server">

    protected string BasicId = string.Empty, BTitle = string.Empty, Url = string.Empty, AuthorName = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        BindPage();	
    }  
    
    private void BindPage()
    {
        string connString = "server=192.168.1.10;uid=cwuser;pwd=cw@007;database=Carwale";
        SqlConnection con = new SqlConnection(connString);
        string sql = "select top 1 * from Con_EditCms_Basic where CategoryId=9";
        SqlCommand cmd = new SqlCommand(sql, con);
        SqlDataReader dr = null;
        try
        {

            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                BasicId = dr["id"].ToString();
                BTitle = dr["Title"].ToString();
                Url = dr["Url"].ToString();
                AuthorName = dr["AuthorName"].ToString();
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
        finally
        {
            dr.Close();
            // if (con.State == ConnectionState.Open)
            // con.Close();
        }


    }	
    
</script>
<!-- #include file="/autoexpo/includes/headNews.aspx" -->

<div id="content" class="left-grid">
<br />
	<span><%= BasicId %></span> <br />
    <span><%= BTitle %></span><br />
    <span><%= Url %></span><br />
    <span><%= AuthorName %></span><br />
</div>
<div style="clear:both;"></div>
<!-- #include file="/autoexpo/includes/footer.aspx" -->	  