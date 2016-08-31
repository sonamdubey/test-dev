<%@ Page trace="false" AutoEventWireUp="false" Inherits="Bikewale.Newsletter.Unsubscribe" Language="C#" %>
<%@ Import Namespace="Bikewale.Common" %>
<%
	// Define all the necessary meta-tags info here.	
	Title 			= "Unsubscribe News-Letters";
    //Modified By :Sajal Gupta on 03 August 2016
    isAd300x250Shown = false;
    isAd300x250BtfShown = false;
%>
<!-- #include file="/includes/headmybikewale.aspx" -->
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li><a href="/">Home</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/mybikewale/">My BikeWale</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>Unsubsribe NewsLetters</strong></li>
        </ul><div class="clear"></div>
    </div>
    <div class="grid_12 margin-top15 min-height">
            <h1 class="FeaturedAdvise">Unsubscribe Newsletters</h1>
            <div class="margin-top10">
	            <div id="dReq" runat="server">
		            <p>Please provide the email address you want to unsubscribe from BikeWale's newsletter.</p>
                    <div class="margin-top10">
                        <asp:TextBox ID="txtEmail" Columns="30" MaxLength="50" runat="server" />
		                <asp:Button ID="butUnsubscribe" runat="server" Text="Unsubscribe Me" class="buttons" />
                    </div>		            
	            </div>
	            <div id="dMes" runat="server" class="error margin-top15" visible="false">
		            <p>You have been successfully removed from bikewale.com monthly newsletter mailing list.</p>
	            </div>
                <div id="errMsg" class="error margin-top15"></div>
            </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#butUnsubscribe").click(function () {
                var isValid = true;                
                var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
                var email = $("#txtEmail").val();
                var errMsg = $("#errMsg");
                
                errMsg.text("");

                if (email.trim() == "") {
                    errMsg.text("Emailid is required");                    
                    isValid = false;
                } else if (!reEmail.test($.trim(email).toLowerCase())) {                    
                    errMsg.text("Please enter valid EmailId.");
                    isValid = false;
                }                
                return isValid;
            });
        });
    </script>
</div>
<!-- Footer starts here -->
<!-- #include file="/includes/footerInner.aspx" -->
<!-- Footer ends here -->  