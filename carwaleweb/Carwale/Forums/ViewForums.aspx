<%@ Page Language="C#" Inherits="Carwale.UI.Forums.ViewForums" AutoEventWireup="false" trace="false"  EnableViewState="false" %>
<%@ Import Namespace="Carwale.UI.Forums" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 305;
	Title 			= ForumName + ( Request["Page"] != null && Request["Page"] != "" ? " - Page " + Request["Page"] : "" ) + " | Car Forums";
	Description 	= "CarWale View forums section involves Discussion / Thread  realated to Finance/Loan , Insurance , latest happenings in the Indian car industry and many more";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Static";
    canonical       = "https://www.carwale.com/forums/" + forumurl + "/";
    AdId            = "1397024466973";
    AdPath          = "/1017752/Carwale_Forums_";
    prevPageUrl     = prevUrl;
    nextPageUrl     = nextUrl;
    mobileSiteForumUrl = "https://carwale.com/m/forums/" + forumurl + "/";
%>
<script lang="c#" runat="server">
    public string mobileSiteForumUrl="";
     
</script>
  
<!-- #include file="/includes/global/head-script.aspx" -->
      <script type='text/javascript'>
            googletag.cmd.push(function () {
            googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath %>160X600', [[120, 240], [120, 600], [160, 600]], 'div-gpt-ad-<%= AdId %>-4').addService(googletag.pubads());
              googletag.pubads().setTargeting('UserModelHistory', '<%= CookiesCustomers.UserModelHistory.Replace('~', ',')%>');
            //googletag.pubads().enableSyncRendering();
            googletag.pubads().collapseEmptyDivs();
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();
        });
</script>
<link rel="stylesheet" href="/static/css/forums.css" type="text/css" >
<style>

.ac {padding:3px;}
.iac {padding:3px;}

</style>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <section class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
        </section>
        <div class="clear"></div>
        <section class="bg-light-grey padding-top10 no-bg-color">
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
                    <h1 class="font30 text-black special-skin-text"><%= ForumName%></h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
               </div>
               <div class="clear"></div>
            </div>
        </section>
        <section class="bg-light-grey">
            <div class="container">
                <div class="grid-10">
                    <div class="content-box-shadow">
	                <div id="left_container_onethird" class="content-inner-block-10"> 
                              
		                <table width="100%">
			                <tr>
				                <td valign="top">
					
					                <p><%= ForumDescription %></p>
					                <div class="font12 margin-top5 rightfloat">
						                <% if (CurrentUser.Id != "-1") { %>
						                <a href="/forums/Subscriptions.aspx"><b>My Subscriptions</b></a> | 
						                <a href="/forums/Search.aspx?get=new"><b>New Posts</b></a> | 
						                <% } %>
						                <a rel="noindex, nofollow" title="View today's posts" href="/forums/Search.aspx?get=today"><b>Today's Posts</b></a> |
						                <a title="Search discussions" href="/forums/Search.aspx"><b>Search Forums</b></a> |					           
						                <a title="View unanswered threads" href="/forums/UnansweredThreads.aspx"><b>Unanswered Threads</b></a>
					                </div>
					                <div class="clear"></div>
						                <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="error" />						
						                <div id="divForum" runat="server">
							                <div class="footerStrip" id="divStripTop" Visible="false" align="right" runat="server"></div>							
							                <table border="0" width="100%" class="bdr" cellpadding="5" cellspacing="0">
								                <tr class="dtHeader">
									                <td width="2" style="border-right:0px;">&nbsp;</td>
									                <td><strong>Thread</strong></td>
									                <td width="140"><strong>Last Post</strong></td>
									                <td width="10"><strong>Replies</strong></td>
									                <td width="10"><strong>Views</strong></td>
								                </tr>
							                <asp:Repeater ID="rptForums" runat="server">								
								                <itemtemplate>
										                <tr>
											                <td style="border-right:0px;padding-top:10px;" valign="middle"><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/arrow_img.gif" /></td>
											                <td>
                                                
                                                              <%# DataBinder.Eval(Container.DataItem, "CatID").ToString() != "0" ? "<b>Sticky : </b>" : "" %>                                                                                                
												                <%#GetLastPost
													                (
														                DataBinder.Eval(Container.DataItem, "Topic").ToString(),
														                DataBinder.Eval(Container.DataItem, "HandleName").ToString(),
														                DataBinder.Eval(Container.DataItem, "StartDateTime").ToString(),
														                DataBinder.Eval(Container.DataItem, "TopicId").ToString(),
														                DataBinder.Eval(Container.DataItem, "Replies").ToString(),
														                DataBinder.Eval(Container.DataItem, "StartedById").ToString(),
                                                                        DataBinder.Eval(Container.DataItem, "Url").ToString()
													                )
												                %>
											                </td>
											                <td>
												                <%#GetLastPostThread
													                (
														                DataBinder.Eval(Container.DataItem, "PostHandleName").ToString(),
														                DataBinder.Eval(Container.DataItem, "LastPostTime").ToString(),
														                DataBinder.Eval(Container.DataItem, "LastPostedById").ToString()
													                )
												                %>
											                </td>
											                <td align="right"><%# ( Convert.ToInt32( DataBinder.Eval(Container.DataItem, "Replies") ) - 1 ).ToString() %></td>
											                <td align="right"><%# DataBinder.Eval(Container.DataItem, "Reads").ToString() %></td>											
										                </tr>
								                </itemtemplate>								
							                </asp:Repeater>
							                </table>
							                <div class="footerStrip" id="divStrip" align="right" Visible="false" runat="server"></div>
						                </div>				
						                <input type="button" value="Create New Thread" style="margin-top:5px;" onclick="location='/forums/CreateNewThread.aspx?forum=<%=forumId%>    '" class="buttons btn btn-orange" />
					
				                </td>
			                </tr>
		                </table>
	                </div>
                    </div>
                </div>
                <div class="grid-2">
	                        <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx("1396440332273", 160, 600, 0, 0, false, 4) %>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <div class="bg-light-grey padding-bottom20"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
  
        </form>
>  <script type='text/javascript'>
       Common.showCityPopup = false;
       </script>
</body>
</html>

