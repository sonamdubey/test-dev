<%@ Control Language="C#" %>
<%@ Import NameSpace="System" %>
<%@ Import NameSpace="System.Web" %>
<%@ Import NameSpace="System.Web.Security" %>
<%@ Import NameSpace="Carwale.UI.Common" %>
<script language="c#" runat="server">
    string login, loginUrl;
    void Page_Load()
    {
        CommonOpn op = new CommonOpn();

        //get the user id of the current login account
        string userId = CurrentUser.Id;

        //if it is -1 then no user is login, else the user with the id is login
        if(userId != "-1")
        {
            login = "Logout";
            hrefLogin.Visible = false;
            string name = CurrentUser.Name;
            spnReg.Visible = false;
            lblUser.Text = "Welcome " + name + "&nbsp; &nbsp;";
        }
        else
        {
            login = "";
            loginUrl = CommonOpn.AppPath + "users/login.aspx";
            hrefLogin.Text = login;
            hrefLogin.NavigateUrl = loginUrl;
            lblUser.Text = "";
            spnReg.Visible = true;
        }
    }
</script>
<asp:Label ID="lblUser" runat="server"></asp:Label> <asp:HyperLink ID="hrefLogin" runat="server"></asp:HyperLink>
<div id="spnReg" runat="server" style="float:left;">
    <ul id="login_ul" class="leftfloat">
        <li><a href="/users/login.aspx" rel="nofollow">Login</a></li>       
        <li><a href="/users/register.aspx" rel="nofollow">Register</a></li>
    </ul>	
</div>