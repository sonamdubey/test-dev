<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Content.Features"  Async="true" Trace="false"%>
<%@ Register TagPrefix="BikeWale" TagName="newPager" Src="/m/controls/LinkPagerControl.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <% 
	    title = "Features - Stories, Specials & Travelogues - BikeWale";
        description = "Learn about the trending stories related to bike and bike products. Know more about features, do's and dont's of different bike products exclusively on BikeWale";
	    keywords = "features, stories, travelogues, specials, drives.";
	canonical = "https://www.bikewale.com/features/";
	relPrevPageUrl = String.IsNullOrEmpty(prevPageUrl) ? "" : "https://www.bikewale.com" + prevPageUrl;
	relNextPageUrl = String.IsNullOrEmpty(nextPageUrl) ? "" : "https://www.bikewale.com" + nextPageUrl;
	    AdPath = "/1017752/Bikewale_Mobile_NewBikes";
	    AdId = "1398766302464";
	    Ad_320x50 = true;
	    Ad_Bot_320x50 = true;
    %>

    <!-- #include file="/includes/headscript_mobile_min.aspx" -->

    <link rel="stylesheet" type="text/css" href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/content/listing.css?<%= staticFileVersion %>" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div class="container box-shadow bg-white section-bottom-margin">
                <h1 class="box-shadow padding-15-20">Latest Bike Features</h1>

                <div id="divListing" class="article-list">
			        <asp:Repeater id="rptFeatures" runat="server">
				        <ItemTemplate>
					        <a href='<%# string.Format("/m{0}", Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),(Bikewale.Entities.CMS.EnumCMSContentType.Features).ToString())) %>' title="<%# DataBinder.Eval(Container.DataItem,"Title") %>">

						        <div class='article-item-content <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")).ToLower().Contains("sponsored") ? "sponsored-content" : ""%>'>

						           <%# Regex.Match(Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")), @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>

							        <div class="article-wrapper">
								        <div class="article-image-wrapper">
									        <img class="lazy" alt='<%# DataBinder.Eval(Container.DataItem,"Title") %>' title="<%# DataBinder.Eval(Container.DataItem,"Title") %>" data-original='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>' width="100%" border="0" src="">
								        </div>
								        <div class="padding-left10 article-desc-wrapper">
									        <h2 class="font14"><%# DataBinder.Eval(Container.DataItem,"Title") %></h2>
								        </div>
							        </div>

							        <div class="article-stats-wrapper font12 leftfloat text-light-grey">
								        <span class="bwmsprite calender-grey-icon inline-block"></span><span class="inline-block"><%# Bikewale.Utility.FormatDate.GetFormatDate(Eval("DisplayDate").ToString(),"MMM dd, yyyy") %></span>
							        </div>
							        <div class="article-stats-wrapper font12 leftfloat text-light-grey">
								        <span class="bwmsprite author-grey-icon inline-block"></span><span class="inline-block"><%# DataBinder.Eval(Container.DataItem,"AuthorName") %></span>
							        </div>
							        <div class="clear"></div>

						        </div>

					        </a>
				        </ItemTemplate>
			        </asp:Repeater>
        </div>
		    <div class="margin-right10 margin-left10 padding-top15 padding-bottom15 border-solid-top font14">
                    <div class="grid-5 omega text-light-grey font13">
                        <span class="text-bold text-default"><%=startIndex %>-<%=endIndex %></span> of <span class="text-bold text-default"><%=totalrecords %></span> articles
	         </div>
                        <BikeWale:newPager ID="ctrlPager" runat="server" />
            <div class="clear"></div>
                           </div>
                    <div class="clear"></div>
            </div>
        </section>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->

        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />

    </form>
</body>
</html>