<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Videos.VideoMakeModel" EnableViewState="false" %>

<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>
<%@ Register Src="~/m/controls/ChangeLocationPopup.ascx" TagPrefix="BW" TagName="LocationWidget" %>
<%@ Register Src="~/m/controls/SimilarBikeVideos.ascx" TagName="SimilarBikeVideos" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/PopularBikesByBodyStyle.ascx" TagPrefix="BW" TagName="MBikesByBodyStyle" %>
<%@ Register TagPrefix="BW" TagName="GenericBikeInfo" Src="~/m/controls/GenericBikeInfoControl.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <%
        title = titleName;
        description = metaDescription;
        keywords = metaKeywords;
        if (isModel)
        {
            canonical = string.Format("https://www.bikewale.com/{0}-bikes/{1}/videos/", makeMaskingName, modelMaskingName);
        }
        else
        {
            canonical = string.Format("https://www.bikewale.com/{0}-bikes/videos/", makeMaskingName);
        }
    %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <style type="text/css">
        @charset "utf-8";

        .swiper-card, .swiper-slide:first-child {
            margin-left: 5px;
        }

            .btn-white:hover, .swiper-card a:hover {
                text-decoration: none;
            }

        #categoryHeader {
            background: #333;
            color: #fff;
            font-size: 16px;
            width: 100%;
            height: 50px;
            position: fixed;
            overflow: hidden;
            z-index: 2;
        }

        .category-back-btn {
            width: 45px;
            padding: 15px 13px 12px;
            float: left;
            cursor: pointer;
        }

        .fa-arrow-back {
            width: 12px;
            height: 20px;
            background-position: -63px -162px;
        }

        #categoryHeader h1 {
            width: 80%;
            float: left;
            color: #fff;
            margin-top: 12px;
            font-weight: 400;
            text-overflow: ellipsis;
            white-space: nowrap;
            overflow: hidden;
        }

        .miscWrapper ul {
            padding: 20px;
            overflow: hidden;
            border-bottom: 1px solid #e2e2e2;
        }

        .miscWrapper li {
            width: 100%;
            border-top: 1px solid #e2e2e2;
            margin-top: 20px;
            padding-top: 20px;
        }

            .miscWrapper li:first-child {
                border-top: none;
                margin-top: 0;
                padding-top: 0;
            }

        .bottom-shadow {
            -webkit-box-shadow: 0 2px 2px #ccc;
            -moz-box-shadow: 0 2px 2px #ccc;
            box-shadow: 0 2px 2px #ccc;
        }

        .misc-container {
            display: table;
        }

            .misc-container a {
                display: table-cell;
                vertical-align: middle;
            }

        .misc-list-image {
            width: 100px;
            height: 54px;
            background: url(https://img.aeplcdn.com/bikewaleimg/images/circleloader.gif) center center no-repeat;
            overflow: hidden;
            text-align: center;
        }

        .misc-container a img {
            width: 100%;
        }

        .video-views-count-container {
            min-width: 140px;
        }

        .border-light-right {
            border-right: 1px solid #e2e2e2;
        }

        .card-container {
            padding-top: 5px;
            padding-bottom: 5px;
        }

            .card-container .swiper-slide {
                width: 200px;
            }

        .swiper-card {
            width: 200px;
            min-height: 160px;
            border: 1px solid #e2e2e2\9;
            background: #fff;
            -webkit-box-shadow: 0 1px 4px rgba(0, 0, 0, .2);
            -moz-box-shadow: 0 1px 4px rgba(0, 0, 0, .2);
            -ms-box-shadow: 0 1px 4px rgba(0, 0, 0, .2);
            box-shadow: 0 1px 4px rgba(0, 0, 0, .2);
            -webkit-border-radius: 2px;
            -moz-border-radius: 2px;
            -ms-border-radius: 2px;
            border-radius: 2px;
        }

        .swiper-image-preview {
            height: 95px;
            padding: 5px 5px 0;
            position: relative;
        }

            .swiper-image-preview img {
                height: 90px;
            }

        .swiper-details-block {
            padding: 3px 15px 0;
        }

        .swiper-btn-block {
            padding: 10px 15px;
        }
    </style>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <header id="categoryHeader">
            <div class="category-back-btn">
                <span class="bwmsprite fa-arrow-back"></span>
            </div>
            <h1 class="font18"><%= pageHeading %></h1>
        </header>
        <section class="bg-white padding-top50 bottom-shadow margin-bottom10" id="section-videos-list">
            <div class="miscWrapper container">
                <ul id="listVideos1">
                    <asp:Repeater ID="rptVideos" runat="server">
                        <ItemTemplate>
                            <li>
                                <div class="misc-container margin-bottom10">
                                    <a href="<%# string.Format("/m/bike-videos/{0}-{1}/", DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(), DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>"
                                        class="misc-list-image margin-right20">
                                        <img class="lazy" data-original="<%#String.Format("https://img.youtube.com/vi/{0}/mqdefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId")) %>" alt="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="" border="0" /></a>
                                    <a href="<%# string.Format("/m/bike-videos/{0}-{1}/", DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(), DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>"
                                        class="font14 text-default text-bold padding-left20"><%# DataBinder.Eval(Container.DataItem,"VideoTitle") %></a>
                                </div>
                                <div class="video-views-count-container font14 leftfloat padding-right10 border-light-right">
                                    <span class="bwmsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Views").ToString()) %></span>
                                </div>
                                <div class="video-views-count-container font14 leftfloat padding-left20 padding-right10">
                                    <span class="bwmsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Likes").ToString()) %></span>
                                </div>
                                <div class="clear"></div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </section>
        <section>
            <div class="text-center">
                <div id="loading">
                    <img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/search-loading.gif" />
                </div>
            </div>
        </section>
        <%if (ctrlGenericBikeInfo.ModelId > 0)
          { %>
        <section>
            <div class="container bg-white box-shadow padding-15-20 section-bottom-margin margin-bottom10">
                <BW:GenericBikeInfo ID="ctrlGenericBikeInfo" runat="server" />
            </div>
        </section>
        <%} %>
        <% if (ctrlSimilarBikeVideos.FetchCount > 0)
           {%>
        <div class="container content-box-shadow margin-bottom10 padding-bottom20">
            <div class="padding-top20 font14">
                <h2 class="padding-left20 padding-right20 margin-bottom15">Videos of similar bikes</h2>
                <BW:SimilarBikeVideos runat="server" ID="ctrlSimilarBikeVideos" />
            </div>
        </div>
        <% } %>
        <%if (ctrlBikesByBodyStyle.FetchedRecordsCount > 0)
          {%>
        <section>
            <div class="container box-shadow bg-white section-bottom-margin margin-bottom20">
            <div class="padding-bottom20 font14">
                        <BW:MBikesByBodyStyle ID="ctrlBikesByBodyStyle" runat="server" />
                </div>
            </div>
        </section>
        <%} %>
        <!--template script-->
        <script type="text/html" id="templetVideos">
            <li>
                <div class="misc-container margin-bottom10">
                    <a data-bind="attr: { href: '/m/bike-videos/' + VideoTitleUrl() + '-' + BasicId() + '/' }" class="misc-list-image margin-right20">
                        <img class="lazy" data-bind="attr: { title: VideoTitle(), alt: VideoTitle(), src: '' }, lazyload: 'https://img.youtube.com/vi/' + VideoId() + '/mqdefault.jpg' " border="0" />
                    </a>
                    <a data-bind="text: VideoTitle(), attr: { href: '/m/bike-videos/' + VideoTitleUrl() + '-' + BasicId() + '/' }" class="font14 text-default text-bold padding-left20"></a>
                </div>
                <div class="video-views-count-container font14 leftfloat padding-right10 border-light-right">
                    <span class="bwmsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default" data-bind="CurrencyText: Views()"></span>
                </div>
                <div class="video-views-count-container font14 leftfloat padding-left20 padding-right10">
                    <span class="bwmsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default" data-bind="CurrencyText: Likes()"></span>
                </div>
                <div class="clear"></div>
            </li>
        </script>
        <script type="text/javascript">
            var cwHostUrl = "<%= Bikewale.Utility.BWConfiguration.Instance.CwApiHostUrl %>";
            var catId = <%= isModel ? modelId : makeId %>;
            var maxPage = 10000000;
            var isModel = <%= isModel.ToString().ToLower() %>;
            var isNextPage = true;
            var apiURL = isModel ? "/api/v1/videos/model/" : "/api/v1/videos/make/";
            var cacheKey = isModel ? "model_" + catId : "make_" + catId;
            $(document).ready(function () {
                $("img .lazy").lazyload();
                $("#loading").hide();
                window.location.hash = "";
            }); 
            $('.category-back-btn').on('click', function () {
                window.location = document.referrer;
            });
        </script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script src="<%= staticUrl  %>/src/lscache.min.js?<%= staticFileVersion%>"></script>
        <script type="text/javascript" src="<%= staticUrl  %>/m/src/video/videoByCategory.js?<%= staticFileVersion %>"></script>

    </form>
</body>
</html>
