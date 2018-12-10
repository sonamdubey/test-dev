<%@ Page Language="C#" ContentType="text/html" AutoEventWireup="false" trace="false" Inherits="MobileWeb.Forums.RedirectToThank" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" itemscope itemtype="http://schema.org/WebPage">
<head runat="server">
    <title></title>
</head>
<body>
<script language="javascript" type="text/javascript">
    Common.showCityPopup = false;
    <%
    if (ch == "1" &&  handleExists == "0")
    {
    %>
    location.href = "/m/users/CreateHandle.aspx?returnUrl=<%=HttpUtility.UrlEncode("/m/forums/RedirectToThank.aspx?params=").ToString() + postId + ",0"%>";
    <%
    }
    %>
    <%
    else
    {
    %>	
	    var postPath = getCookie("postThankPath");	
	    document.cookie = "postThankPath=Thanked::<%=postId%>::<%=isSaved%>";
	    location.href = postPath + "#post<%=postId%>";
    <%
    }
    %>

    function getCookie(c_name)
	{
		if (document.cookie.length>0)
	    {
		 	c_start=document.cookie.indexOf(c_name + "=");
		  	if (c_start!=-1)
			{
				c_start=c_start + c_name.length+1;
				c_end=document.cookie.indexOf(";",c_start);
				if (c_end==-1) c_end=document.cookie.length;
				return unescape(document.cookie.substring(c_start,c_end));
			}
		}
		return "";
  	}    
</script>
</body>
</html>
