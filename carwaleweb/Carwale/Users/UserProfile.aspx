<%@ Page Language="C#" %>
<script language="c#" runat="server">
	void Page_Load( object sender, EventArgs e )
	{
		Response.Redirect( "/community/members/profile.aspx?" + Request.ServerVariables["QUERY_STRING"] );
	}
</script>