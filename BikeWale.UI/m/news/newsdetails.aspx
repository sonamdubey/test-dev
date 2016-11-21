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
        AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        AdId = "1398766302464";
          %>

    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/content/details.css?<%= staticFileVersion %>" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
    <style>
        .next-page-title { height: 3em;overflow: hidden;}
    </style>
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
                                        <span class="next-prev-article-title next-page-title"><%=prevPageTitle %></span>
                                    </div>
                                </a>
                            <%} %>						
					    </div>
									
					    <div class="grid-6 omega rightfloat">
                            <%if( !String.IsNullOrEmpty(nextPageUrl)) {%>
						        <a href="/m/news/<%= nextPageUrl %>" title="<%=nextPageTitle %>" class="text-default next-prev-article-target">
							        <div class="next-prev-article-box inline-block padding-right5">
								        <span class="font12 text-light">Next</span>
								        <span class="next-prev-article-title next-page-title"><%=nextPageTitle %></span>
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
            
        <div class="back-to-top" id="back-to-top"></div>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->

        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <script type="text/javascript">
            ga_pg_id = "11";

            // share
            $('.share-btn').click(function () {
                var str = $(this).attr('data-attr');
                var cururl = window.location.href;
                switch (str) {
                    case 'fb':
                        url = 'https://www.facebook.com/sharer/sharer.php?u=';
                        break;
                    case 'tw':
                        url = 'https://twitter.com/home?status=';
                        break;
                    case 'gp':
                        url = 'https://plus.google.com/share?url=';
                        break;
                    case 'wp':
                        var text = document.getElementsByTagName("title")[0].innerHTML;
                        var message = encodeURIComponent(text) + " - " + encodeURIComponent(cururl);
                        var whatsapp_url = "whatsapp://send?text=" + message;
                        url = whatsapp_url;
                        window.open(url, '_blank');
                        return;
                }
                url += cururl;
                window.open(url, '_blank');
            });

        </script>
    </form>
</body>
</html>