<%@ Page Language="C#" Inherits="Carwale.UI.Community.Default" AutoEventWireup="false" trace="false" %>
<%@ Register TagPrefix="ur" TagName="UserReviews" Src="/Controls/CarwaleReviews.ascx"%>
<%@ import namespace="Carwale.UI.Common" %>
<%@ import namespace="Carwale.Utility" %>

<!doctype html>
<html>
<head>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 8;
	Title 			= "CarWale Community";
	Description 	= "CarWale ";
	Keywords		= "forums, user reviews, photos, user community, car community, indian car community";
	Revisit 		= "15";
	DocumentState 	= "Static";
    AdId            = "1397027524795";
    AdPath          = "/1017752/Carwale_Community_";
%>
<!-- #include file="/includes/global/head-script.aspx" -->
<style type="text/css">

	.paddLeft {padding-left:25px;}
	.inq {padding:4px 2px 4px 2px;width:270px;height:120px; border-bottom:2px solid #555555;border-right:2px solid #999999;border-left:1px solid #AAAAAA;border-top:1px solid #AAAAAA;}
	.upgrade{ border:8px solid #DDF0F8;}
	.upgrade th{ background-color:#DDF0F8; text-align:left; vertical-align:middle; color:#006699; font-size:12px; font-weight:bold; height:20px; font-family:Verdana, Arial, Helvetica, sans-serif; padding-bottom:5px;}
	.txtHl {color:#555555; font-size:12px; font-weight:bold;}
	.noBorder{border:0px;}
	.lineSep { border-bottom:1px dashed #D9D9C1;padding:4px 0; }
	.dvlist{border-right:1px dotted #666666;display:block;padding:5px;color:#333333;vertical-align:top;}
	h1 a { color:#cc0000; }
	#dtgrdViewAlbum td { border:1px solid #f5f5f5; width:20%; text-align:center; }

</style>


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
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/forums/">Car Forums</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Community</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black special-skin-text">What's New in Community</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                <div class="grid-10">
                    <div class="content-box-shadow content-inner-block-10">
						<div class="left_container_top">
		                    <div id="left_container_onethird mid-box">
			
			                    <table width="100%">
				                    <tr>
					                    <td>
						                    <div class="greyboxNormal" style="margin-top:10px;">
							                    <div style="float:left; width:160px;" class="imageBorder margin-right10"><img src="https://img.carwale.com/c/up/no.jpg" id="imgReal" alt="" Title="No Photo Available" /></div>
							                    <h2 style="margin-bottom:10px;">Member of the Month</h2>
							                    T. N. Subramanian popularly known as ‘tnsmani’ at the CarWale Forums is our Community Member of the Month for November 2013. Known for his balanced opinions he has been guiding newbies about the nitty-gritties of cars and enjoys helping others with his vast experience of driving and owning cars.
							                    <p><strong>Name</strong>: T N Subramanian.
							                    <br><strong>Location</strong>: Chennai , TamilNadu.
							                    <br><strong>Car(s) owned</strong>: Ford Ecosport, Maruti Suzuki A-star AT.
							                    <p>To know more about T N Subramanian, <a href="https://www.carwale.com/community/members/tnsmani.html"><strong>view T N Subramanian's complete community profile</strong></a>.</p>
						                    </div>
					                    </td>
				                    </tr>
				                    <tr>
					                    <td>
						                    <h2><a href="/userreviews/" data-cwtccat="ReviewLandingLinkage" data-cwtcact="CommunityPageClick" data-cwtclbl="source=1">User Reviews</a></h2><br />
						                    <h3>Recent Arrivals</h3>
						                    <ur:UserReviews id="urUserReviewsMostRecent" ReviewCount="5" ShowComment="false" RetriveBy="MostRecent" runat="server"/>
					                    </td>
				                    </tr>
				                    <tr>
					                    <td>
						                    <div id="divHotForumDiscussions" runat="server" style="line-height:15px;margin-top:15px;" >
							                    <h2><a href="/mycarwale/forums/">Forums</a></h2><br />
							                    <h3 class="margin-bottom10">Hot Discussions</h3>
							                    <asp:Repeater ID="rptHotForumDiscussions" runat="server">
								                    <itemtemplate>
									                    <div class="lineSep">
										                    &raquo;
										                    <a href='/Mycarwale/forums/<%# DataBinder.Eval(Container.DataItem,"FId")%>-<%# DataBinder.Eval(Container.DataItem,"Url")%>.html' title="Read detailed"><%# DataBinder.Eval(Container.DataItem,"Topic")%></a>
										                    (<%# DataBinder.Eval(Container.DataItem,"TOTALPOSTS")%> new posts)
									                    </div>
								                    </itemtemplate>
							                    </asp:Repeater>
						                    </div>
					                    </td>
				                    </tr>
				                    <tr>					                   
				                    </tr>
			                    </table>
		                    </div><!-- content ends -->
	                    </div>
					</div>
				</div>
				<div class="grid-2">
                    <div class="content-box-shadow content-inner-block-10">
						<div class="right_container">
		                    <div class="addbox">
			                    <div style="padding-bottom:2px;" align="center"><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/icons/ad.gif" /></div>
			                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx("1396440332273", 160, 600, 0, 0, false, 4) %>
		                    </div>
	                    </div>
	                    <div class="left_container" style="float:left"> </div>
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

