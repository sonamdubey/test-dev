<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Content.newsdetails" Trace="false" Async="true" %>
<%@ Register Src="~/UI/m/controls/PopularBikesByBodyStyle.ascx" TagPrefix="BW" TagName="MBikesByBodyStyle"  %>
<%@ Register Src="~/UI/m/controls/UpcomingBikesMin.ascx" TagPrefix="BW" TagName="MUpcomingBikesMin" %>
<%@ Register Src="~/UI/m/controls/PopularBikesMin.ascx" TagPrefix="BW" TagName="MPopularBikesMin" %>
<%@ Register TagPrefix="BW" TagName="GenericBikeInfo" Src="~/UI/m/controls/GenericBikeInfoControl.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <% 
        if (metas != null)
        {
            title = metas.Title;
            description = metas.Description;
            canonical = metas.CanonicalUrl;
         }
        AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        AdId = "1398766302464";
    %>

    <!-- #include file="/UI/includes/headscript_mobile_min.aspx" -->
    <%if(objArticle!=null && metas!=null) {%>
    <link rel="amphtml" href="<%= metas.AmpUrl %>" />
    <%} %>
    <link rel="stylesheet" type="text/css" href="<%= staticUrl  %>/m/css/content/details.css?<%= staticFileVersion %>" />
    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_mobile.aspx" -->
    </script>
    <style>
        .next-page-title { height: 3em; overflow: hidden; }
    </style>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/UI/includes/headBW_Mobile.aspx" -->
        <section>
            <%if(objArticle!=null){ %>
            <div class="container box-shadow bg-white section-bottom-margin">
                <div class="box-shadow article-head padding-15-20">
                    
                    <h1 class="margin-bottom10"><%= objArticle.Title %></h1>
                    <div class="grid-6 alpha padding-right5">
                        <span class="bwmsprite calender-grey-sm-icon"></span>
                        <span class="article-stats-content"><%= Bikewale.Utility.FormatDate.GetFormatDate(Convert.ToString(objArticle.DisplayDate), "dd MMMM yyyy, hh:mm tt") %></span>
                    </div>
                    <div class="grid-6 alpha omega">
                        <span class="bwmsprite author-grey-sm-icon"></span>
                        <span class="article-stats-content"><%= objArticle.AuthorName  %></span>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="article-content-padding">
                    <div id="divDesc" class="article-content">
                        <%if(metas!=null) %>
                        <%if (!String.IsNullOrEmpty(metas.ShareImage)) %>
                        <img alt='<%= metas.Title%>' title='<%= metas.Title%>' src='<%= metas.ShareImage %>'>
                        <%= String.IsNullOrEmpty(objArticle.Content) ? "" : objArticle.Content %>
                    </div>
                  <BW:GenericBikeInfo ID="ctrlGenericBikeInfo" runat="server" />

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
                            <%if (!String.IsNullOrEmpty(objArticle.PrevArticle.Title))
                              {%>
                            <a href="/m<%= metas.PreviousPageUrl%>" title="<%=objArticle.PrevArticle.Title %>" class="text-default next-prev-article-target">
                                <span class="bwmsprite prev-arrow"></span>
                                <div class="next-prev-article-box inline-block padding-left5">
                                    <span class="font12 text-light">Previous</span><br>
                                    <span class="next-prev-article-title next-page-title"><%=objArticle.PrevArticle.Title %></span>
                                </div>
                            </a>
                            <%} %>
                        </div>

                        <div class="grid-6 omega rightfloat">
                            <%if (!String.IsNullOrEmpty(objArticle.NextArticle.Title) && metas!=null)
                              {%>
                            <a href="/m<%= metas.NextPageUrl %>" title="<%=objArticle.NextArticle.Title %>" class="text-default next-prev-article-target">
                                <div class="next-prev-article-box inline-block padding-right5">
                                    <span class="font12 text-light">Next</span>
                                    <span class="next-prev-article-title next-page-title"><%=objArticle.NextArticle.Title %></span>
                                </div>
                                <span class="bwmsprite next-arrow"></span>
                            </a>
                            <%} %>
                        </div>

                        <div class="clear"></div>
                    </div>
                </div>
            </div>
               <%} %>
        </section>
        <BW:MPopularBikesMin runat="server" ID="ctrlPopularBikes" />
         <%if(isModelTagged){ %>
        <%if (ctrlBikesByBodyStyle.FetchedRecordsCount > 0){%>
        <section>
            <div class="container box-shadow bg-white section-bottom-margin padding-bottom20">
         <BW:MBikesByBodyStyle ID="ctrlBikesByBodyStyle" runat="server"/>
          </div>
             </section>
        <%} %>
           <%} else{%>
         <BW:MUpcomingBikesMin ID="ctrlUpcomingBikes" runat="server" />
          <%} %>
        <div class="back-to-top" id="back-to-top"></div>

        <script type="text/javascript" src="<%= staticUrl  %>/UI/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/UI/includes/footerBW_Mobile.aspx" -->

        <link href="<%= staticUrl  %>/UI/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript_mobile.aspx" -->
        <!-- #include file="/UI/includes/fontBW_Mobile.aspx" -->
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
