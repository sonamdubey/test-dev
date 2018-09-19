<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Content.RoadTest"  Async="true" Trace="false"%>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BikeWale" TagName="newPager" Src="/UI/m/controls/LinkPagerControl.ascx" %>
<%@ Register Src="~/UI/m/controls/UpcomingBikesMin.ascx" TagPrefix="BW" TagName="MUpcomingBikesMin" %>
<%@ Register Src="~/UI/m/controls/PopularBikesMin.ascx" TagPrefix="BW" TagName="MPopularBikesMin" %>
<%@ Register Src="~/UI/m/controls/PopularBikesByBodyStyle.ascx" TagPrefix="BW" TagName="MBikesByBodyStyle"  %>
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
        Ad_Mid_320x50 = true;
    %>

    <!-- #include file="/UI/includes/headscript_mobile_min.aspx" -->

    <link rel="stylesheet" type="text/css" href="<%= staticUrl  %>/m/css/content/listing.css?<%= staticFileVersion %>" />
    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/UI/includes/headBW_Mobile.aspx" -->
        <section>
            <div class="container box-shadow bg-white section-bottom-margin">
            		        
            <% if (!string.IsNullOrEmpty(modelName)) {%>
                <h1 class="box-shadow padding-15-20"><%= String.Format("{0} {1}",makeName,modelName) %> Expert Reviews</h1>
            <% }
            else if(!string.IsNullOrEmpty(makeName)) { %>
		        <h1 class="box-shadow padding-15-20"><%= makeName  %> Bikes Expert Reviews</h1>
            <% }
            else { %>
                <h1 class="box-shadow padding-15-20">Expert Reviews</h1>
            <% } %>
	        
                <% if(articlesList != null && articlesList.Count > 0) { %>
    	        <div id="divListing" class="article-list">
		            <% foreach(var article in articlesList){ %>        
				            <a href="<%= string.Format("/m{0}", Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(article.BasicId), article.ArticleUrl, (Bikewale.Entities.CMS.EnumCMSContentType.RoadTest).ToString())) %>" title="Expert Review: <%= article.Title %>">

					            <div class="article-item-content">
						            <%= Regex.Match(article.AuthorName, @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>

						            <div class="article-wrapper">
							            <div class="article-image-wrapper">
                                            <img class="lazy" alt='Expert Review: <%= article.Title %>' title="Expert reviews: <%= article.Title %>" data-original='<%= Bikewale.Utility.Image.GetPathToShowImages( article.OriginalImgUrl, article.HostUrl, Bikewale.Utility.ImageSize._110x61) %>' width="100%" border="0" src="">
							            </div>
							            <div class="padding-left10 article-desc-wrapper">
								            <h2 class="font14">Expert Review: <%= article.Title %></h2>
							            </div>
						            </div>

						            <div class="article-stats-wrapper font12 leftfloat text-light-grey">
							            <span class="bwmsprite calender-grey-icon inline-block"></span><span class="inline-block"><%= Bikewale.Utility.FormatDate.GetFormatDate(article.DisplayDate.ToString(),"dd MMMM yyyy") %></span>
						            </div>
						            <div class="article-stats-wrapper font12 leftfloat text-light-grey">
							            <span class="bwmsprite author-grey-icon inline-block"></span><span class="inline-block"><%= article.AuthorName %></span>
						            </div>
						            <div class="clear"></div>

					            </div>
				            </a>	
                    <% } %>		                        
	            </div>  
                <% } %>
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
       <div class="margin-bottom15">
            <!-- #include file="/UI/ads/Ad320x50_Middle_mobile.aspx" -->
        </div>
        <BW:MPopularBikesMin runat="server" ID="ctrlPopularBikes" />
        <%if(modelId>0){%>
        <%if (ctrlBikesByBodyStyle.FetchedRecordsCount > 0){%>
        <section>
            <div class="container box-shadow bg-white section-bottom-margin padding-bottom20">
                              <BW:MBikesByBodyStyle runat="server" ID="ctrlBikesByBodyStyle" />
                </div>
             </section>
        <%} %>
        <%}else{ %>
        <BW:MUpcomingBikesMin runat="server" ID="ctrlUpcomingBikes" />
        <%} %>
       <script type="text/javascript" src="<%= staticUrl  %>/UI/m/src/frameworks.js?<%= staticFileVersion %>"></script>
       <!-- #include file="/UI/includes/footerBW_Mobile.aspx" -->

        <link href="<%= staticUrl  %>/UI/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript_mobile.aspx" -->
        <!-- #include file="/UI/includes/fontBW_Mobile.aspx" -->
        <script type="text/javascript">
            ga_pg_id = "12";
        </script>
    </form>
</body>
</html>