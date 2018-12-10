<%@ Page Language="C#" Trace="false" %>
<!doctype html>
<html>
<head>   
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 2;
	Title 			= "Contact Us";
	Description 	= "Complete contact information, phone numbers, fax number of CarWale.";
	Keywords		= "";
	Revisit 		= "30";
	DocumentState 	= "Static";
    AdId            = "1398233965520";
    AdPath          = "/1017752/AboutUs_";
%>
 <!-- #include file="/includes/global/head-script.aspx" -->
<style type="text/css">
<!--
	.abt ul li{border-top:1px solid #CCCCCC; border-left:1px solid #CCCCCC; border-right:1px solid #CCCCCC; padding:7px; margin:0; height:auto; list-style-image:none; list-style:none;}
	.abt ul li.sel{background:url(https://img.carwale.com/images/common/menumidbg.gif); repeat:repeat-x;color:#CC0000; font-weight:bold;}
	.abt ul li.end {color:#CC0000; font-weight:bold; border-bottom:1px solid #CCCCCC; background:url(https://img.carwale.com/images/common/menumidbg.gif); repeat:repeat-x;}
	.abt ul li.endLast {font-weight:bold; border-bottom:1px solid #CCCCCC; }	
	.abt ul li a{font-weight:bold; color:#6C6C6C;}
	.readable a { text-decoration:none; }
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
                   
                    <h1 class="font30 text-black special-skin-text">Contact CarWale</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
            </section>
<div style="width:954px;">
	<div class="boxxl_rd_top">&nbsp;</div>
	<div class="boxxl_rd_mid" style="height:auto;">
		<div class="boxxl_rd_container" style="height:auto;">
			<div class="boxxl_rd_container" style="height:auto;">
		<div class="grid-3">
			<div class="content-box-shadow">
                        <div class="content-inner-block-10">
                            <div class="abt">
						<ul class="normal">
							<li>&raquo; <a href="/aboutus.aspx">About us</a></li>
							<li>&raquo; <a href="/carwalestory.aspx">The CarWale Story</a></li>
							<li>&raquo; <a href="/award/">Awards &amp; Recognitions</a></li>
							<%--<li>&raquo; <a href="/media">CarWale in News</a></li>--%>
							<li>&raquo; <a href="/PressReleases/">Press Releases</a></li>
							<li>&raquo; <a href="/career.aspx">Careers</a></li>
							<li>&raquo; <a href="/advertiseWithUs.aspx">Advertise With Us</a></li>
							<li class="end sel">&raquo; Contact Us</li>
						</ul>
					</div>
                      </div>
                    </div>
            </div>
            <div class="grid-9">
                <table>
                <tr>
						<div style="text-align:justify;" class="content-box-shadow content-inner-block-10 margin-bottom20">
							<div class="readable margin-bottom90 margin-top20">
							<p>
								We are located at<br>
								Automotive Exchange Pvt Ltd, <br>
								12th floor, Vishwaroop IT Park,<br>
								Sector 30A, Vashi,<br />
								Navi Mumbai - 400705<br />
						
								Phone: (022) 6739 8888<br>
								Fax: (022) 6645 9665, 6739 8877<br>
								Email: <a href="mailto:contact@carwale.com">contact@carwale.com</a>
							</p>
							<br />
							</div>
						</div>
					</td>
				</tr>
			</table>
		</div>
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