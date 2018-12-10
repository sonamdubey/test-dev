<%@ Page trace="false" AutoEventWireUp="false" Inherits="Carwale.UI.Newsletter.Unsubscribe" Language="C#" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<!doctype html>
<html>
<head>
    
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 1;
	Title 			= "Unsubscribe News-Letters";
	Description 	= "Unsubscribe News-Letters";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Static";
    AdId            = "1336981731134";
    AdPath          = "/7590/CarWale_ReviewsNews/CarWale_ReviewsNews_";
%>
    <!-- #include file="/includes/global/head-script.aspx" -->
</head>
<body class="bg-light-grey header-fixed-inner">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <section>
              <div class="container margin-bottom20">
                <div class="grid-12">
                <div class="content-box-shadow content-inner-block-10">
                    <h1 class="font30 text-black border-solid-bottom padding-bottom10 special-skin-text">Unsubscribe Newsletters</h1>
                    <div class="margin-top10" style="min-height:200px;">
        	            <div id="dReq" runat="server">
                            <p class="margin-bottom15">Please provide the email address you want to unsubscribe from CarWale.com monthly newsletter.</p>
                            <div>
                                <asp:TextBox ID="txtEmail" Columns="30" CssClass="form-control leftfloat margin-right10" width="300px" MaxLength="50" runat="server" />
                                <asp:Button ID="butUnsubscribe" CssClass="btn btn-orange btn-xs leftfloat" runat="server" Text="Unsubscribe Me" />
                                <div class="clear"></div>
                            </div>
                        </div>
                        <div id="dMes" runat="server" class="error" visible="false">
                            <p>You have been successfully removed from carwale.com monthly newsletter mailing list.</p>
                        </div>
                    </div>
                </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
    </form>
</body>
</html>
 
