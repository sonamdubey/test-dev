<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Page Language="C#" Inherits="Carwale.UI.Forums.Default"  AutoEventWireup="false" trace="false" EnableViewState="false" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 305;
	Title 			= "Car Forums | Ask, Answer and Discuss about Cars";
	Description 	= "India's finest car discussion forum. Discuss anything related to cars in India. Ask car related questions and get fast response.";
	Keywords		= "car forum, auto forum, car forum India, car forums, car discussions, car help, car howtos";
	Revisit 		= "15";
	DocumentState 	= "Static";
    AdId            = "1397024466973";
    AdPath          = "/1017752/Carwale_Forums_";
    mobileSiteForumUrl = "https://carwale.com/m/forums/";
    canonical       = "https://www.carwale.com/forums/";
%>
<!-- #include file="/includes/global/head-script.aspx" -->
<link rel="stylesheet" href="/static/css/forums.css" type="text/css" >
<link rel="alternate" type="application/rss+xml" title="CarWale Forums RSS Feed" href="https://www.carwale.com/m/forums/" />
<script language="c#" runat="server">
    private string mobileSiteForumUrl = "-1"; // Url for mobile site (forums.)
</script>
<script type='text/javascript'>
        googletag.cmd.push(function () {
        googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
        googletag.defineSlot('<%= AdPath %>318x220', [318, 220], 'div-gpt-ad-<%= AdId %>-4').addService(googletag.pubads());
        googletag.pubads().setTargeting('UserModelHistory', '<%= CookiesCustomers.UserModelHistory.Replace('~', ',')%>');
        googletag.pubads().collapseEmptyDivs();
        googletag.pubads().enableSingleRequest();
        googletag.enableServices();
    });
</script>
<style>
	
	.forumStats { background-color:#f2faff; border:1px solid #6B9FBF; }
	.forumStats .statHead { padding:5px; color:#164F72; font-weight:bold; border-bottom:1px solid #6B9FBF; }
	.forumStats .statContents { padding:5px; line-height:18px; }
	
</style>
</head>
<body class="bg-white header-fixed-inner rsz-lyt special-page special-skin-body no-bg-color">
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
                            <li><span class="fa fa-angle-right margin-right10"></span>Car Forums</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black special-skin-text">Car Forums</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                <div class="grid-10">
                    <div class="content-box-shadow content-inner-block-10">
                        <div class="text-right margin-bottom10">
			                <% if (CurrentUser.Id != "-1") { %>
			                <a href="Subscriptions.aspx"><b>My Subscriptions</b></a> | 
			                <a href="Search.aspx?get=new"><b>New Posts</b></a> | 
			                <% } %>
			                <a rel="noindex, nofollow" title="View today's posts" href="Search.aspx?get=today"><b>Today's Posts</b></a> |
			                <%--<a title="Search discussions" href="Search.aspx"><b>Search Forums</b></a> | 
			                  <a rel="noindex, nofollow" title="View private message" href="/community/pms/Default.aspx"><b>My Messages <%=inboxTotal%></b></a> |	--%>		
			                <a title="View unanswered threads" href="/forums/UnansweredThreads.aspx"><b>Unanswered Threads</b></a>
		                </div>
                        <asp:Repeater ID="rptParent" runat="server">
				            <headertemplate><table width="100%"  border="0" cellspacing="0" cellpadding="2"></headertemplate>
				            <itemtemplate>
						            <tr><td><h1 class="forums-hd1"><%# GetTitle(DataBinder.Eval(Container.DataItem, "Name").ToString()) %></h1></td></tr>
						            <tr>
							            <td>
								            <!-- contains the forums -->
								            <asp:Repeater ID="rptChild" runat="server"
									            DataSource='<%# GetChild(DataBinder.Eval(Container.DataItem, "ID").ToString()) %>'>
									            <headertemplate>
										            <table border="0" class="bdr" cellpadding="5" cellspacing="0">
											            <tr class="dtHeader">
												            <td width="2" style="border-right:0px;">&nbsp;</td>
												            <td width="55%"><strong>Forum</strong></td>
												            <td width="40%"><strong>Last Post</strong></td>
												            <td width="10"><strong>Threads</strong></td>
												            <td width="10"><strong>Posts</strong></td>
											            </tr>
									            </headertemplate>
									            <itemtemplate>
											            <tr>
												            <td style="border-right:0px;padding-top:10px;" valign="top"><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/icons/arOrange.gif" /></td>
												            <td valign="top">
													            <a href='<%# DataBinder.Eval(Container.DataItem, "SubUrl") %>/'><%# DataBinder.Eval(Container.DataItem, "SubCatName") %></a>  <span class="startBy"><%# GetCategoryViews(DataBinder.Eval(Container.DataItem, "SubCatId").ToString()) %></span><br />
													            <%# DataBinder.Eval(Container.DataItem, "Description") %>
												            </td>
												            <td valign="top"><%# GetLastPost( DataBinder.Eval(Container.DataItem, "LastThreadId").ToString(), DataBinder.Eval(Container.DataItem, "LastThread").ToString(),DataBinder.Eval(Container.DataItem, "Handle").ToString(), DataBinder.Eval(Container.DataItem, "LastPostDate").ToString(), DataBinder.Eval(Container.DataItem, "LastPostedById").ToString(),DataBinder.Eval(Container.DataItem, "Url").ToString() ) %></td>
												            <td align="right"><%# DataBinder.Eval(Container.DataItem, "Threads") %></td>
												            <td align="right"><%# DataBinder.Eval(Container.DataItem, "Posts") %></td>												
											            </tr>
									            </itemtemplate>
									            <footertemplate>
										            </table>
									            </footertemplate>
								            </asp:Repeater>
							            </td>
						            </tr>
						            <tr runat="server" visible='<%# (++serial) == 2 ? true : false %>'><td>&nbsp;</td></tr>	
						            <tr><td style="height:25px">&nbsp;</td></tr>						
				            </itemtemplate>
				            <footertemplate></table></footertemplate>				
			            </asp:Repeater>         
                    </div>
                </div>
                <div class="grid-2">
                        <div class="addbox"><%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx("1396440332273", 160, 600, 0, 0, false, 4) %></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
       <script type='text/javascript'>
           Common.showCityPopup = false;
           </script>
</form>
</body>
</html>