<%@ Page Language="C#" trace="false" Inherits="Carwale.UI.PressReleases.Default" AutoEventWireUp="false" %>
<%@ Register TagPrefix="Vspl" TagName="RepeaterPager" src="/Controls/RepeaterPagerAdvanced.ascx" %>
<!doctype html>
<html>
<head>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 20;
	Title 			= "CarWale Press Releases";
	Keywords		= "press releases, press release, PR";
	Description 		= "CarWale's Official Press Releases Section.";
	Revisit 		= "5";
	DocumentState 	= "Static";
    AdId            = "1398233965520";
    AdPath          = "/1017752/AboutUs_";
%>
<!-- #include file="/includes/global/head-script.aspx" -->
<style type="text/css">
<!--

	.abt ul li{border-top:1px solid #CCCCCC; border-left:1px solid #CCCCCC; border-right:1px solid #CCCCCC; padding:7px; margin:0; height:auto; list-style-image:none; list-style:none;}
	.abt ul li.sel{background:url(https://img.carwale.com/images/common/menumidbg.gif); repeat:repeat-x;color:#CC0000; font-weight:bold;}
	.abt ul li.end{border-bottom:1px solid #CCCCCC;}
	.abt ul li a{font-weight:bold; color:#6C6C6C;}
	.readable a { text-decoration:none; }
	.date{font-style:italic;  line-height:20px; }
	.description{line-height:17px; text-align:justify; }
-->
</style>
    </head>
    <body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
        <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
             <section class="container">
        <!-- #include file="/includes/header.aspx" -->
                 <div class="grid-12">
                        <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
                  <section class="container">
                <div class="grid-12">
                   
                    <h1 class="font30 text-black special-skin-text">Press Releases by CarWale</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
            </section>
            <div class="clear"></div>
<div style="width:954px;">
	<div class="boxxl_rd_top">&nbsp;</div>
	<div class="boxxl_rd_mid" style="height:auto;">
	<form runat="server">
		<div class="boxxl_rd_container" style="height:auto;">
		<div class="grid-3">
			<div class="content-box-shadow">
                        <div class="content-inner-block-10">
                            <div class="abt">
						<ul class="normal">
							<li>&raquo; <a href="/aboutus.aspx">About us</a></li>
							<li>&raquo; <a href="/carwalestory.aspx">The CarWale Story</a></li>
							<li>&raquo; <a href="/award/">Awards &amp; Recognitions</a></li>
							<%--<li>&raquo; <a href="/media/">CarWale in News</a></li>--%>
							<li class="sel">&raquo; Press Releases</li>
							<li>&raquo; <a href="/career.aspx">Careers</a></li>
							<li>&raquo; <a href="/advertiseWithUs.aspx">Advertise With Us</a></li>
							<li class="end">&raquo; <a href="/contactus.aspx">Contact Us</a></li>
						</ul>
					</div>
                      </div>
                    </div>
            </div>
            <div class="grid-9">
            <table>
                <tr>
					<td valign="top">
						<div style="text-align:justify;" class="content-box-shadow content-inner-block-10">
							<Vspl:RepeaterPager 
								id="rpgDetails" 
								PageSize="10" ResultName="News" 
								ShowHeadersVisible="false" 
								PagerPosition="TopBottom" 
								runat="server">
								<asp:Repeater ID="rptDetails" runat="server"  >
								<headertemplate>
									<table width="100%" border="0" cellpadding="5">
								</headertemplate>
								<itemtemplate>
									<tr>
										<td valign="top" class="readable" style="text-align:justify;">
											<div style="float:left;font-weight:bold;"><a href="pressreleasedetails.aspx?prid=<%# DataBinder.Eval( Container.DataItem, "Id" ) %>"> <%# DataBinder.Eval( Container.DataItem, "Title" ) %></a></div>
											<div style="float:right;text-align:right;"><asp:Button  id="btnDownload" runat="server" style="background-image:url(https://img.carwale.com/images/pdf_icon.jpg);width:25px;height:25px;cursor:pointer;"  CommandName="Update" ></asp:Button></div>
											<div style="clear:both;"></div>
											Release Date: <span class="date"><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "ReleaseDate")).ToString("dd-MMM-yyyy") %>
											</span><br>
											<span class="description"><%# DataBinder.Eval( Container.DataItem, "Summary" ) %>
											</span>
											<br>
											<a href="pressreleasedetails.aspx?prid=<%# DataBinder.Eval( Container.DataItem, "Id" ) %>"> 
												<img style="background:#FFFFFF;border:#FFFFFF;" src="<%=ImagingFunctions.GetRootImagePath()%>/images/home/more.gif" border="0"/>		
											</a>
											<input type="hidden" id="hdnPdfFile" runat="server" value='<%# DataBinder.Eval( Container.DataItem, "AttachedFile" ) %>' />
										</td>
									</tr>
									<tr><td colspan="2"><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/content_seperator_line.jpg" alt="" style="width:100%; height:2px;"></td></tr>
								</itemtemplate>
								<footertemplate>
									</table>
								</footertemplate>
								</asp:Repeater>	
							</Vspl:RepeaterPager>
					  </div>
					</td>
				</tr>
			</table>
		</div>
	</form>
	</div>	
	<div class="boxxl_rd_btm">&nbsp;</div>
</div>
</section>
            <div class="clear"></div>
            </section>
    <div class="clear"></div>
    <!-- #include file="/includes/footer.aspx" -->
    <!-- #include file="/includes/global/footer-script.aspx" -->
</body>
</html>