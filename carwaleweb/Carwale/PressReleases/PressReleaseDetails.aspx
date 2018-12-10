<%@ Page Language="C#" trace="false" Inherits="Carwale.UI.PressReleases.PressReleaseDetails" AutoEventWireUp="false" %>
<!doctype html>
<html>
<head>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 20;
	Title 			= title + " - Press Releases";
	Keywords		= "press release, press releases, PR";
	Description 	= "CarWale's press release on " + title;
	Revisit 		= "5";
	DocumentState 	= "Static";
    AdId            = "1398233965520";
    AdPath          = "/1017752/AboutUs_";
%>
<!-- #include file="/includes/global/head-script.aspx" -->
<style type="text/css">

	.abt ul li{border-top:1px solid #CCCCCC; border-left:1px solid #CCCCCC; border-right:1px solid #CCCCCC; padding:10px 0 0 20px; margin:0; height:21px; list-style-image:none; list-style:none;}
	.abt ul li.sel{background:url(https://img.carwale.com/images/common/menumidbg.gif); repeat:repeat-x;color:#CC0000; font-weight:bold;}
	.abt ul li.end{border-bottom:1px solid #CCCCCC;}
	.abt ul li a{font-weight:bold; color:#6C6C6C;}	
	.date{font-style:italic;  line-height:20px; }
	.description{line-height:17px; text-align:justify; }
    .abt ul li {height: auto; padding: 7px;

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
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/community/">Community</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/pressreleases/">Press Releases</a></li>
                             <li><span class="fa fa-angle-right margin-right10"></span><%=title%></li>
                        </ul>
                        <div class="clear"></div>
                    </div>                  
                    <h1 class="font30 text-black special-skin-text">
						<span style="float:right; margin-right:5px;">
							<% if (attachFile != "") { %>
								<asp:Button  id="btnDownload" runat="server" style="background-image:url(https://img.carwale.com/images/pdf_icon.jpg);width:25px;height:25px;cursor:pointer;" ></asp:Button>
							<% } %>
						</span>
					</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                <div class="grid-3">
                    <div class="content-box-shadow content-inner-block-10">
						<div class="abt">
                            <ul class="normal">
							<li>&raquo; <a href="/aboutus.aspx">About us</a></li>
							<li>&raquo; <a href="/carwalestory.aspx">The CarWale Story</a></li>
							<li>&raquo; <a href="/award/">Awards &amp; Recognitions</a></li>
							<li>&raquo; <a href="/media/">CarWale in News</a></li>
							<li class="sel">&raquo; <a href="/pressreleases/" style="color:#CC0000;">Press Releases</a></li>
							<li>&raquo; <a href="/career.aspx">Careers</a></li>
							<li>&raquo; <a href="/advertiseWithUs.aspx">Advertise With Us</a></li>
							<li class="end">&raquo; <a href="/contactus.aspx">Contact Us</a></li>
						</ul>
                        </div>
					</div>
				</div>
				<div class="grid-9">
                    <div class="content-box-shadow content-inner-block-10">
						<div style="text-align:justify; margin-left:20px;">
									
							<br />
							<div class="description">
								<p><%=detailSumm%></p>
								<p class="date" ><%=releaseDate.ToString("dd MMM yyyy hh:mm")%></p>
							</div>
					     </div>
					</div>
				</div>
                <div class="clear"></div>
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