<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.News.Default"  Trace="false" Async="true"%>
<%@ Register TagPrefix="BikeWale" TagName="newPager" Src="/m/controls/LinkPagerControl.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <% 
	    title       = "Bike News - Latest Indian Bike News & Views - BikeWale";
	    description = "Latest news updates on Indian bikes industry, expert views and interviews exclusively on BikeWale.";
	    keywords    = "news, bike news, auto news, latest bike news, indian bike news, bike news of india"; 
	    canonical   = "https://www.bikewale.com/news/";
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
                <h1 class="box-shadow padding-15-20">Latest Bike News</h1>

                <div id="divListing" class="article-list">
			        <asp:Repeater id="rptNews" runat="server">
				        <ItemTemplate>
					        <a href="<%# string.Format("/m{0}", Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Convert.ToString(DataBinder.Eval(Container.DataItem,"CategoryId")))) %>" title="<%# DataBinder.Eval(Container.DataItem,"Title") %>">
                                <div class="article-item-content <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")).ToLower().Contains("sponsored") ? "sponsored-content" : ""%>">

                                    <%# Regex.Match(Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")), @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>
                                    
                                    <div class="article-wrapper">
								        <div class="article-image-wrapper">
									        <img class="lazy" alt='<%# DataBinder.Eval(Container.DataItem,"Title") %>' title="<%# DataBinder.Eval(Container.DataItem,"Title") %>" data-original='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>' width="100%" border="0" src="">
								        </div>
								        <div class="padding-left10 article-desc-wrapper">
									        <div class="article-category">
										        <span class="text-uppercase font12 text-bold"><%# GetContentCategory(DataBinder.Eval(Container.DataItem,"CategoryId").ToString()) %></span>
									        </div>
									        <h2 class="font14"><%# DataBinder.Eval(Container.DataItem,"Title") %></h2>
								        </div>
							        </div>

							        <div class="article-stats-wrapper font12 leftfloat text-light-grey">
								        <span class="bwmsprite calender-grey-icon inline-block"></span><span class="inline-block"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"MMM dd, yyyy") %></span>
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
                        <span class="text-bold text-default"><%=startIndex %>-<%=endIndex %></span> of <span class="text-bold text-default"><%=totalrecords %></span> articles</div>
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
            ga_pg_id = "10";
        </script>
    </form>
</body>
</html>