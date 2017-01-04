<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Content.RoadTest"  Async="true" Trace="false"%>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BikeWale" TagName="newPager" Src="/m/controls/LinkPagerControl.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <% 
        if (string.IsNullOrEmpty(modelName) && string.IsNullOrEmpty(makeName))
        {
            title = "Expert Bike Reviews India - Bike Comparison & Road Tests - BikeWale";
            description = "Latest expert reviews on upcoming and new bikes in India. Read bike comparison tests and road tests exclusively on BikeWale";
            keywords = "Expert bike reviews, bike road tests, bike comparison tests, bike reviews, road tests, expert reviews, bike comparison, comparison tests";
            canonical = "https://www.bikewale.com/expert-reviews/";
        }
        // Model Name exists
        else if (!string.IsNullOrEmpty(modelName))
        {
            title = string.Format("{0} {1} Expert Reviews India - Bike Comparison & Road Tests - BikeWale", makeName, modelName);
            description = string.Format("Latest expert reviews on {0} {1} in India. Read {0} {1} comparison tests and road tests exclusively on BikeWale", makeName, modelName);
            keywords = string.Format("{0} {1} expert reviews, {0} {1} road tests, {0} {1} comparison tests, {0} {1} reviews, {0}{1} bike comparison", makeName, modelName);
            canonical = string.Format("https://www.bikewale.com/{0}-bikes/{1}/expert-reviews/", makeMaskingName, modelMaskingName);
        }
        // Make name exists
        else
        {
            title = string.Format("{0} Bikes Expert Reviews India - Bike Comparison & Road Tests - BikeWale", makeName);
            description = string.Format("Latest expert reviews on upcoming and new {0} bikes in India. Read {0} bike comparison tests and road tests exclusively on BikeWale", makeName);
            keywords = string.Format("{0} bike expert reviews, {0} bike road tests, {0} bike comparison tests, {0} bike reviews, {0} road tests, {0} expert reviews, {0} bike comparison, {0} comparison tests.", makeName);
            canonical = string.Format("https://www.bikewale.com/{0}-bikes/expert-reviews/", makeMaskingName);
        }
        AdId = "1395986297721";
        AdPath = "/1017752/Bikewale_Reviews_";
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
            		        
            <% if (!string.IsNullOrEmpty(modelName)) {%>
                <h1 class="box-shadow padding-15-20"><%= makeName  %> <%= modelName %> Expert Reviews</h1>
            <% }
            else if(!string.IsNullOrEmpty(makeName)) { %>
		        <h1 class="box-shadow padding-15-20"><%= makeName  %> Bikes Expert Reviews</h1>
            <% }
            else { %>
                <h1 class="box-shadow padding-15-20">Expert Reviews</h1>
            <% } %>
	        
    	        <div id="divListing" class="article-list">
		            <asp:Repeater id="rptRoadTest" runat="server">
			            <itemtemplate>
				            <a href="<%# string.Format("/m{0}", Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),(Bikewale.Entities.CMS.EnumCMSContentType.RoadTest).ToString())) %>" title="Expert Review: <%# DataBinder.Eval(Container.DataItem, "Title") %>">

					            <div class="article-item-content">
						            <%# Regex.Match(Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")), @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>

						            <div class="article-wrapper">
							            <div class="article-image-wrapper">
                                            <img class="lazy" alt='Expert Review: <%# DataBinder.Eval(Container.DataItem, "Title") %>' title="Expert reviews: <%# DataBinder.Eval(Container.DataItem, "Title") %>" data-original='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>' width="100%" border="0" src="">
							            </div>
							            <div class="padding-left10 article-desc-wrapper">
								            <h2 class="font14">Expert Review: <%# DataBinder.Eval(Container.DataItem, "Title") %></h2>
							            </div>
						            </div>

						            <div class="article-stats-wrapper font12 leftfloat text-light-grey">
							            <span class="bwmsprite calender-grey-icon inline-block"></span><span class="inline-block"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"MMM dd, yyyy") %></span>
						            </div>
						            <div class="article-stats-wrapper font12 leftfloat text-light-grey">
							            <span class="bwmsprite author-grey-icon inline-block"></span><span class="inline-block"><%# DataBinder.Eval(Container.DataItem, "AuthorName") %></span>
						            </div>
						            <div class="clear"></div>

					            </div>
				            </a>
			            </itemtemplate>
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
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->
        <script type="text/javascript">
            ga_pg_id = "12";
        </script>
    </form>
</body>
</html>