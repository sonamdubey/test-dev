<!doctype html>
<html>
<head>
<%
	
	PageId 			= 72;
	Title 			= "Confirm Message";
	Description 	= "";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Static";
%>
<script runat="server">
	// Move to the new location.
	public string inquiryId = "";
	
	private void Page_Load(object sender, System.EventArgs e)
	{
		if( Request.QueryString["t"] != null && Request.QueryString["t"].ToString() != "")
		{
			if(Request.QueryString["t"] == "p")
			{
				pd.Visible = true;
			}
			else
			{
				cd.Visible = true;
			}
		}
		else
		{
			Response.Redirect( CommonOpn.AppPath + "pageNotFound.aspx" );
		}
		
		if( Request.QueryString["car"] != null && Request.QueryString["car"].ToString() != "" )
		{
			inquiryId = Request.QueryString["car"];
		}
	}
</script>
<!-- #include file="/includes/global/head-script.aspx" -->
<style>
    .alert{background:url(https://img.carwale.com/cw-common/bg_alert.gif) repeat-x; padding:7px; border:1px solid #F7EC64;}
    .mid-box {margin-top:10px;}
</style>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <input type="hidden" id="hdnIsPageFromCache" runat="server" />
        <section class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
        </section>
        <div class="clear"></div>
        <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul class="special-skin-text">
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/MyCarwale/default.aspx">My CarWale</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="MySellInquiry.aspx">My Inquiries</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Confirmation</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black special-skin-text">Congratulations!</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
				
                <div class="grid-12">
					<div class="content-box-shadow content-inner-block-10">						
                        <div class="white-shadow content-inner-block"  style="min-height:140px;">
                            <div class="alert">
		                        <p id="cd" align="justify" runat="server" visible="false">Your car details have been successfully updated. See your car details <a href="/used/cardetails.aspx?car=s<%= inquiryId %>">live</a> here.</p>
		                        <p id="pd" runat="server" visible="false">Your car photos are uploaded and queued for verification. It may take up to 24 working hours before they start showing up on the website.</p>	
	                        </div>
	                        <div class="mid-box"><a href="MySellInquiry.aspx" class="btn btn-orange">Continue</a></div>
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
