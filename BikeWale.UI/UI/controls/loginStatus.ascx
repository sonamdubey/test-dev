<%@ Control Language="C#" %>
<%@ Import NameSpace="System" %>
<%@ Import NameSpace="System.Web" %>
<%@ Import NameSpace="System.Web.Security" %>
<%@ Import NameSpace="Bikewale.Common" %>
<%@ Import Namespace="System.Globalization" %>
<script language="c#" runat="server">
	string login, loginUrl;
    protected string userId;
	void Page_Load()
	{
		CommonOpn op = new CommonOpn();
		
		//get the user id of the current login account
        userId = CurrentUser.Id;
		
		//if it is -1 then no user is login, else the user with the id is login
		if(userId != "-1")
		{
			login = "<b>LOGOUT</b>";
			loginUrl = CommonOpn.AppPath + "users/login.aspx?logout=logout";
			hrefLogin.Text = login;
			hrefLogin.NavigateUrl = loginUrl;

            string name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CurrentUser.Name.ToLower());
            lblUser.Text = name;
		}
		else
		{
            login = "<b>LOGIN</b>";
			loginUrl = CommonOpn.AppPath + "users/login.aspx";
			hrefLogin.Text = login;
			hrefLogin.NavigateUrl = loginUrl;
			lblUser.Text = "Guest";
		}
	}
</script>
<div class="grid_3 omega">
    <ul class="right-align">
        <li>Welcome, <asp:Label ID="lblUser" runat="server" /> <asp:HyperLink ID="hrefLogin" runat="server"></asp:HyperLink></li>
        <%=CurrentUser.Id!= "-1" ? "": "<li>|</li><li><a href='/users/login.aspx'>REGISTER</a></li>" %>        
    </ul>
</div>
