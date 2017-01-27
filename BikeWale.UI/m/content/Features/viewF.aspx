﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Content.viewF" Async="true" Trace="false" %>
<%@ Register Src="~/m/controls/UpcomingBikesMin.ascx" TagPrefix="BW" TagName="MUpcomingBikesMin"  %>
<%@ Register Src="~/m/controls/PopularBikesMin.ascx" TagPrefix="BW" TagName="MPopularBikesMin"  %>
<%@ Register Src="~/m/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery"  %>
<!DOCTYPE html>
<html>
<head>
    <%	
        title = pageTitle + " - Bikewale ";
        keywords = "features, stories, travelogues, specials, drives";
        description = string.Format("Read about {0}. Read through more bike care tips to learn more about your bike maintenance.", pageTitle);
        canonical = Bikewale.Utility.BWConfiguration.Instance.BwHostUrl + url;
        AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        AdId = "1398766302464";
        %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="amphtml" href="<%= ampUrl %>" />
    <link rel="stylesheet" type="text/css" href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/content/details.css?<%= staticFileVersion %>" />
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
                        <span class="article-stats-content"><%= displayDate %></span>
                    </div>
                    <div class="grid-6 alpha omega">
                        <span class="bwmsprite author-grey-sm-icon"></span>
                        <span class="article-stats-content"><%=author %></span>
                    </div>
                    <div class="clear"></div>
                </div>

                <div class="article-content-padding">
                    <div id="divPageContent">
                        <asp:Repeater ID="rptPageContent" runat="server">
					        <itemtemplate>
                                <div class="margin-bottom10">
                                    <h3 class="margin-bottom10" role="heading"><%#Eval("PageName") %></h3>
                                    <div id='<%#Eval("pageId") %>' class="margin-top10 article-content">
                                        <%#Eval("content") %>
                                    </div>
                                </div>
					        </itemtemplate>             
				        </asp:Repeater>
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
                    <%if (objImg!=null)
                      { %>
                    <div class="border-solid-top padding-top10">
                        <h3 class="margin-bottom10">Photos</h3>
                        <div class="swiper-container article-photos-swiper">
                            <div class="swiper-wrapper">
                                <asp:Repeater id="rptPhotos" runat="server">
                                    <itemtemplate>
				                        <div class="swiper-slide">
                                            <img class="swiper-lazy" title="<%#DataBinder.Eval(Container.DataItem, "ImageName").ToString()%>" alt="<%#DataBinder.Eval(Container.DataItem, "ImageName").ToString()%>" data-src='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>'>
                                            <span class="swiper-lazy-preloader"></span>
				                        </div>
			                        </itemtemplate>
                                </asp:Repeater>
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
        <BW:MUpcomingBikesMin runat="server" ID="ctrlUpcomingBikes" />
        <BW:ModelGallery runat="server" ID="photoGallery" />

        <div class="back-to-top" id="back-to-top"></div>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->

        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/src/content/details.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->

        <script type="text/javascript">
            $(document).ready(function () {
                var pageId = 1;
                var pageUrl = '<%= baseUrl%>';
                $("#ddlPages").change(function ()
                {
                    pageId = $(this).val();
                    window.location.href = pageUrl + 'p' + pageId + '/';
            
                });
            });
        </script>
    </form>
</body>
</html>