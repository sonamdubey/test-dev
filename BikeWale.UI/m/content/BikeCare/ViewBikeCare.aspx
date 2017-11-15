<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Content.ViewBikeCare" %>
<%@ Register Src="~/m/controls/UpcomingBikesMin.ascx" TagPrefix="BW" TagName="MUpcomingBikesMin"  %>
<%@ Register Src="~/m/controls/PopularBikesMin.ascx" TagPrefix="BW" TagName="MPopularBikesMin"  %>
<%@ Register Src="~/m/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery"  %>
<%@ Register Src="~/m/controls/PopularBikesByBodyStyle.ascx" TagPrefix="BW" TagName="MBikesByBodyStyle"  %>
<!DOCTYPE html>
<html>
<head>
    <%	
        keywords = pageKeywords;
        title = pageTitle;
        description  = pageDescription;
    canonical = string.Format("https://www.bikewale.com/bike-care/{0}-{1}.html", pageTitle, basicId);
        AdPath = "/1017752/Bikewale_Mobile_NewBikes_";
        AdId = "1398766302464";
      %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="<%= staticUrl  %>/m/css/content/details.css?<%= staticFileVersion %>" />
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
                    <h1 class="margin-bottom10"><%= pageTitle %></h1>
                    <div class="grid-6 alpha padding-right5">
                        <span class="bwmsprite calender-grey-sm-icon"></span>
                        <span class="article-stats-content"><%=Bikewale.Utility.FormatDate.GetFormatDate(displayDate, "dd MMMM yyyy, hh:mm tt") %></span>
                    </div>
                    <div class="grid-6 alpha omega">
                        <span class="bwmsprite author-grey-sm-icon"></span>
                        <span class="article-stats-content"><%=objTipsAndAdvice.AuthorName%></span>
                    </div>
                    <div class="clear"></div>
                    <%= (bikeTested != null && !String.IsNullOrEmpty(bikeTested.ToString())) ? String.Format("<div class='font12 text-light-grey margin-top5'>{0}</div>",bikeTested) : "" %>
                </div>

                <div class="article-content-padding">
                    <% if (objTipsAndAdvice != null) 
                       {%>
                        <div class="margin-bottom10">
                            <% foreach (var page in objTipsAndAdvice.PageList) {%>
                                <div class="margin-bottom10">
                                    <h3 role="heading"><%=page.PageName %></h3>
                                    <div id='<%=page.pageId %>' class="margin-top10 article-content">
                                        <%=page.Content %>
                                    </div>
                                </div>
                            <%} %>
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
                        
                    <%} %>

                    <% if (objTipsAndAdvice != null && objImg!=null)
                       {%>
                        <div class="border-solid-top padding-top10">
                            <h3 class="margin-bottom10">Images</h3>
                            <div class="swiper-container article-photos-swiper">
                                <div class="swiper-wrapper">
                                    <% foreach (var img in objImg){ %>
                                        <div class="swiper-slide">
                                            <img class="swiper-lazy" title="<%=img.ImageName%>" alt="<%=img.ImageName%>" data-src="<%=Bikewale.Utility.Image.GetPathToShowImages(img.OriginalImgPath,img.HostUrl,Bikewale.Utility.ImageSize._110x61) %>">
                                            <span class="swiper-lazy-preloader"></span>
                                        </div>
                                    <% } %>
                                </div>
                                <div class="bwmsprite swiper-button-next"></div>
                                <div class="bwmsprite swiper-button-prev"></div>
                            </div>
                        </div>
                    <%} %>
                </div>
            </div>
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
        <BW:ModelGallery runat="server" ID="photoGallery" />

        <div class="back-to-top" id="back-to-top"></div>

        <script type="text/javascript" src="<%= staticUrl %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->

        <link href="<%= staticUrl  %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl  %>/m/src/content/details.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->

        <script type="text/javascript">
            $(document).ready(function () {
                var pageId = 1;
                var pageUrl = '<%= baseUrl%>';
                $("#ddlPages").change(function () {
                    pageId = $(this).val();
                    window.location.href = pageUrl + 'p' + pageId + '/';
                });
            });

            ga_pg_id = "13";

        </script>
    </form>
</body>
</html>
