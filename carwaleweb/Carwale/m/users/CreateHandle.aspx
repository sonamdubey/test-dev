<%@ Page Language="C#" ContentType="text/html" Inherits="MobileWeb.Users.CreateHandle" AutoEventWireup="false" trace="false" %>
<%@ Import Namespace="Carwale.BL.Experiments" %>
<%
    bool showExperimentalColor = ProductExperiments.IsShowExperimentalColor(CookiesCustomers.AbTest);
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<!-- #include file="/m/includes/global/head-script.aspx" -->
<style>
    .error{color:red;}
</style>
</head>

<body class="<%= (showExperimentalColor ? "btn-abtest" : "")%>">
    <!-- #include file="/m/includes/header.aspx" -->
	<!--Outer div starts here-->
	<div data-role="page" >
        <section class="container">
    	<!--Main container starts here-->
    	<div id="main-container">
            <div class="grid-12"> 
            <h1 class="pghead margin-top10 margin-bottom10">Create community user name</h1>
            <form runat="server">
            <div class="box content-inner-block-10 rounded-corner2 content-box-shadow margin-bottom10">
                CarWale Community requires you to complete this one-time registration process. Participation in the community is possible only if you complete it.<br/><br/>
                User name should be 3-20 characters in length. Use A-Z, 0-9 and dot (.) to form a name.<br/><br/> 
                <div>Community user name &nbsp;&nbsp; <span id="spnUserName" class="error" style="display:none;"></span></div>
                <div class="margin-top5"><asp:TextBox CssClass="form-control" id="txtHandle" runat="server" data-role="none" /></div>
                <div class="margin-top15"><asp:LinkButton id="btnGetUserName" runat="server" class="linkButtonBig btn btn-xs btn-orange btn-full-width" style="border-style:none;" Text="Get me this user name" /></div>
                <%if(userNameAvailable){%>
                <div class="margin-top5 error" id="divErrMsg" style="margin-bottom:10px;">&nbsp;</div>
                <%}else{%>
                <div class="margin-top5 error" id="divErrMsg" style="margin-bottom:10px;">User name not available. Please try with another user name.</div>
                <%}%>
                <div class="margin-top5"><asp:Label id="lblMessage" runat="server" Visible="false" class="error" Text="Enter user name in valid format" /></div>
            </div>
            </form>
            
            </div>
            <div class="clear"></div>
        </div>
        <!--Main container ends here-->
        </section>
        <div class="clear"></div>
    </div>
    <!--Outer div ends here-->
    <!-- #include file="/m/includes/footer.aspx" -->
	<!-- #include file="/m/includes/global/footer-script.aspx" -->
    <script language="javascript" type="text/javascript">
        function InputValid() {
            var val = $("#txtHandle").val().toLowerCase();
            $("#divErrMsg").html("&nbsp;");
            var chk = /^[a-z_0-9_.]*$/;
            if (val == "") {
                $("#divErrMsg").html("Enter user name");
                return false;
            }
            else if (val.length < 3 || val.length > 20) {
                $("#divErrMsg").html("Length should be 3-20 characters");
                return false;
            }
            else if (chk.test(val) == false) {
                $("#divErrMsg").html("Only A-Z, 0-9 and dot (.) allowed");
                return false;
            }
            else if (val.substr(0, 1) == '.') {
                $("#divErrMsg").html("First character can only be a number or alphabet");
                return false;
            }
            return true;
        }
            </script>
</body>
</html>