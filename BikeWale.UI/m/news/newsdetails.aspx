<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Content.newsdetails" Trace="false" Async="true" %>
<%@ Register Src="~/m/controls/MUpcomingBikesMin.ascx" TagPrefix="BW" TagName="MUpcomingBikesMin"  %>
<%@ Register Src="~/m/controls/MPopularBikesMin.ascx" TagPrefix="BW" TagName="MPopularBikesMin"  %>
<!DOCTYPE html>
<html>
<head>
    <% 
        title = newsTitle + " - BikeWale News";
        description = "BikeWale coverage on " + newsTitle + ". Get the latest reviews and photos for " + newsTitle + " on BikeWale coverage.";
        canonical = "http://www.bikewale.com/news/" + pageUrl;
        //fbTitle = newsTitle;
        //fbImage = GetMainImagePath();
        AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        AdId = "1398766302464";
        //menu = "6";
    %>

    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/m/css/content/details.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div class="container box-shadow bg-white section-bottom-margin">
                <div class="box-shadow article-head padding-15-20">
                    <h1 class="margin-bottom10"><%= newsTitle %></h1>
                    <div class="grid-6 alpha padding-right5">
                        <span class="bwmsprite calender-grey-sm-icon"></span>
                        <span class="article-stats-content"><%= Bikewale.Utility.FormatDate.GetFormatDate(displayDate, "MMM dd, yyyy hh:mm tt") %></span>
                    </div>
                    <div class="grid-6 alpha omega">
                        <span class="bwmsprite author-grey-sm-icon"></span>
                        <span class="article-stats-content"><%= author  %></span>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="article-content-padding">
                    <div id="divDesc" class="article-content">
                    <%if(!String.IsNullOrEmpty(GetMainImagePath())) %>
                        <img alt='<%= newsTitle%>' title='<%= newsTitle%>' src='<%= GetMainImagePath() %>'>
                    <%= String.IsNullOrEmpty(newsContent) ? "" : newsContent %>
                </div>

                    <p class="margin-bottom10 font14 text-light-grey border-light-top">Share this story</p>
                    <ul class="social-wrapper">
                        <li class="whatsapp-container rounded-corner2 text-center share-btn" data-attr="wp">
                            <span data-text="share this video" data-link="www.google.com" class="social-icons-sprite whatsapp-icon"></span>
                        </li>
                        <li class="fb-container rounded-corner2 text-center share-btn" data-attr="fb">
                            <span class="social-icons-sprite fb-icon"></span>
                        </li>
                        <li class="tweet-container rounded-corner2 text-center share-btn" data-attr="tw">
                            <span class="social-icons-sprite tweet-icon"></span>
                        </li>
                        <li class="gplus-container rounded-corner2 text-center  share-btn" data-attr="gp">
                            <span class="social-icons-sprite gplus-icon"></span>
                        </li>
                    </ul>
                    <div class="clear"></div>

                    <div class="border-solid-top padding-top10">				
					    <div class="grid-6 alpha border-solid-right">
                            <%if( !String.IsNullOrEmpty(prevPageUrl)) {%>
                                <a href="/m/news/<%= prevPageUrl%>" title="<%=prevPageTitle %>" class="text-default next-prev-article-target">
                                    <span class="bwmsprite prev-arrow"></span>
                                    <div class="next-prev-article-box inline-block padding-left5">
                                        <span class="font12 text-light">Previous</span><br>
                                        <span class="next-prev-article-title"><%=prevPageTitle %></span>
                                    </div>
                                </a>
                            <%} %>						
					    </div>
									
					    <div class="grid-6 omega rightfloat">
                            <%if( !String.IsNullOrEmpty(nextPageUrl)) {%>
						        <a href="/m/news/<%= nextPageUrl %>" title="<%=nextPageTitle %>" class="text-default next-prev-article-target">
							        <div class="next-prev-article-box inline-block padding-right5">
								        <span class="font12 text-light">Next</span>
								        <span class="next-prev-article-title"><%=nextPageTitle %></span>
							        </div>
							        <span class="bwmsprite next-arrow"></span>
						        </a>
                            <%} %>
					    </div>
									
				        <div class="clear"></div>
			        </div>
                </div>
            </div>
        </section>
        <BW:MPopularBikesMin runat="server" ID="ctrlPopularBikes" />
        <BW:MUpcomingBikesMin runat="server" ID="ctrlUpcomingBikes" />

        <%--<div class="new-line5">
            <ul class="socialplugins  new-line10">
                <li><fb:like href="http://www.bikewale.com/news/<%= pageUrl%>" send="false" layout="button_count"  show_faces="false"></fb:like></li>
                <li><a href="https://twitter.com/share" class="twitter-share-button" data-url="http://www.bikewale.com/news/<%= pageUrl %>" data-via='<%= title %>' data-lang="en">Tweet</a></li>
                <li><div class="g-plusone" data-size="medium" data-href="http://www.bikewale.com/news/<%= pageUrl %>"></div></li>
            </ul>  
            <div class="clear"></div> 
        </div>--%>                
            
        <div class="back-to-top" id="back-to-top"></div>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->

        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <script type="text/javascript">
            ga_pg_id = "11";
        </script>
    </form>
</body>
</html>