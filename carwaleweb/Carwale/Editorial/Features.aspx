<%@ Page Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Editorial.FeaturesDefault" Trace="false" Debug="false" ViewStateMode="Disabled" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Import Namespace="Carwale.Utility" %>
<%@ Register TagPrefix="Qr" TagName="QuickResearch" src="/Controls/QuickResearch.ascx" %>
<%@ Register TagPrefix="uc" TagName="TipsAndAdvices" src="/Controls/TipsAndAdvices.ascx" %>
<%@ Register TagPrefix="Vspl" TagName="RepeaterPager" Src="/Controls/CommonPager.ascx" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>

<%
    // Define all the necessary meta-tags info here.
    // To know what are the available parameters,
    // check page, headerCommon.aspx in common folder.

    PageId = 47;
    Title = "Special Reports - Stories, Specials & Travelogues";
    Description = "Special Reports section of CarWale brings specials, stories, travelogues and much more.";
    Revisit = "15";
    DocumentState = "Static";
    //canonical		= "https://www.carwale.com/features/";
    canonical = canonicalUrl;
    altUrl = "https://www.carwale.com/m/features/";
    AdId = "1396440332273";
    AdPath = "/1017752/ReviewsNews_";
    prevPageUrl = prevUrl;
    nextPageUrl = nextUrl;
%>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <script language="c#" runat="server">
        private bool Ad300x600 = true;
    </script>
     <script type='text/javascript'>

         googletag.cmd.push(function () {
             googletag.defineSlot('<%= AdPath %>300x250', [300, 250], 'div-gpt-ad-<%= AdId %>-0').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
            <% if (Ad300x600 == true) { %>googletag.defineSlot('<%= AdPath %>300x600', [[120, 240], [120, 600], [160, 600], [300, 250], [300, 600]], 'div-gpt-ad-<%= AdId %>-4').addService(googletag.pubads());<% } %>
            googletag.pubads().setTargeting("<%= targetKey %>", "<%= targetValue %>");
             googletag.pubads().setTargeting("City", "<%= CookiesCustomers.MasterCity.ToString() %>");
             googletag.pubads().setTargeting('UserModelHistory', '<%= CookiesCustomers.UserModelHistory.Replace('~', ',')%>');
            //googletag.pubads().enableSyncRendering();
            googletag.pubads().collapseEmptyDivs();
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();
        });
    </script>
   
<% if (Request.QueryString["pn"] != null && Request.QueryString["pn"].Length > 0)
   { %>
<META NAME="ROBOTS" CONTENT="NOINDEX, FOLLOW">
<% } %>
    <script src="https://cdn.topsy.com/topsy.js?20160419032055" type="text/javascript"></script>
    <style type="text/css">
        .medPadding {
            padding: 5.5px;
        }

        #ulUCL {
            list-style: none;
        }

            #ulUCL li {
                padding: 0 0 10px 0;
            }

                #ulUCL li a {
                    background: url(https://imgd.aeplcdn.com/0x0/cw-common/ul-arrow.gif) no-repeat center left;
                    margin-left: 5px;
                    padding-left: 10px;
                }
                .news-sprite {
            background: url(https://img.carwale.com/images/news-sprite.png) no-repeat;
            display: inline-block;
            position: relative;
        }

        .v-views-icon {
            background-position: 0px -70px;
            top: 6px;
            width: 16px;
            height: 15px;
        }

    </style>
    <link rel="stylesheet" href="/static/css/colorbox.css" type="text/css" >
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
    <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
        <div class="container">
            <div class="grid-12">
                    <div class="breadcrumb margin-bottom15"> <!-- breadcrumb code starts here -->
                	    <ul class="special-skin-text">
                    	    <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a title="Car Research" href="/reviews-news/">Reviews & News</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Special Reports</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black special-skin-text">Special Reports</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
        </div>
        <div class="clear"></div>
        <div class="container">
            <div class="grid-8">
                    <asp:Repeater ID="rptFeatures" runat="server" EnableViewState="false">
					            <itemtemplate>
                                    <div class="content-box-shadow content-inner-block-10 border-solid margin-bottom20">
						                <div id="post-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>">
							            <h2 class="splh2">
								            <a href="<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>" rel="bookmark" title="Permanent Link to <%# DataBinder.Eval(Container.DataItem,"Title") %>">
									            <%# DataBinder.Eval(Container.DataItem,"Title") %>
								            </a>
							            </h2>
                                        <div class="f-small" style="padding:4px 0 0 0; margin-top:5px; margin-bottom:5px;">
								            <%# DataBinder.Eval(Container.DataItem,"AuthorName") %>, <abbr><%# DataBinder.Eval(Container.DataItem,"DisplayDate", "{0:dd-MMM-yyyy}") %></abbr><div class="f-small light-grey-text rightfloat"><span class="news-sprite v-views-icon margin-right5"></span><%# DataBinder.Eval(Container.DataItem,"Views")!= "" ? DataBinder.Eval(Container.DataItem,"Views"): "0"  %> View(s) </div>
							            </div>                            
							            <div>
								            <%# DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString() != "" ? "<a class='cbBox' href='" + ImageSizes.CreateImageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"HostUrl")),ImageSizes._600X337,Convert.ToString(DataBinder.Eval(Container.DataItem,"OriginalImgUrl"))) + "'><img src='https://imgd.aeplcdn.com/0x0/statics/grey.gif' class='lazy alignright size-thumbnail img-border-news' data-original='" + ImageSizes.CreateImageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"HostUrl")),ImageSizes._174X98,Convert.ToString(DataBinder.Eval(Container.DataItem,"OriginalImgUrl"))) +"' align='right' border='0' /></a>" : "" %>
								            <%# DataBinder.Eval(Container.DataItem,"Description") %>
                                            <div class="float-rt"><a href="<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>">Read full article &raquo;</a></div>
								            <div style="clear:both"></div>
							            </div>							
                                        <div style="clear:both"></div>
					                </div>
                                </div>
				            </itemtemplate>
			            </asp:Repeater>		
                    <div class="footerStrip" id="divStrip" align="right">
                        <Vspl:RepeaterPager id="pagerDetails" Visible="true" runat="server" align="right" ></Vspl:RepeaterPager>
                    </div>
        </div>
                <div class="grid-4">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 20, 20, false, 0) %>
		            <div class="content-box-shadow content-inner-block-5 margin-bottom20"><h2 class="hd2">Quick Research</h2><Qr:QuickResearch id="qrQuickResearch" runat="server" /></div>
                    <div class="content-box-shadow content-inner-block-5 margin-bottom20">            
                        <div class="gray-block"><uc:TipsAndAdvices id="ucTipsAndAdvices" runat="server" /></div>    
                    </div> 
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 600, 10, 10, false, 4) %>      
	            </div>
            <div class="clear"></div>
        </div>
    </section>
    <div class="clear"></div>
</form>
    <!-- #include file="/includes/footer.aspx" -->
    <!-- all other js plugins -->
    <!-- #include file="/includes/global/footer-script.aspx" -->
       <script   src="/static/src/jquery.colorbox.js"  type="text/javascript"></script>
    <script type"text/javascript">
        Common.showCityPopup = false;
        doNotShowAskTheExpert = false;
        $(document).ready(function () {
            $("a.cbBox").colorbox({ rel: "nofollow" });
        });
</script>
<script type="text/javascript" language="javascript">
    $("a[rel='slide']").colorbox({ width: "700px", height: "500px" });
    $('img.lazy').lazyload();
</script>
</body>
</html>
 
