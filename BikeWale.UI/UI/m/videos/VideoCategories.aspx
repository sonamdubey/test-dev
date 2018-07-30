<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Videos.VideoCategories" EnableViewState="false" %>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>
<%@ Register Src="~/m/controls/ChangeLocationPopup.ascx" TagPrefix="BW" TagName="LocationWidget" %>
<!DOCTYPE html>
<html>
<head>
    <%        
       Bikewale.Utility.VideoTitleDescription.VideoTitleDesc(categoryIdList,out title,out description, null, null);
       canonical = string.Format("https://www.bikewale.com/bike-videos/category/{0}-{1}/", canonTitle, categoryIdList.Replace(',', '-'));
        %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <style type="text/css">
        #categoryHeader{background:#333;color:#fff;font-size:16px;width:100%;height:50px;position:fixed;overflow:hidden;z-index:2;}.category-back-btn { width:45px; padding:15px 13px 12px; float:left; cursor:pointer; }.fa-arrow-back{width:12px; height:20px; background-position:-63px -162px;} #categoryHeader h1 { width:80%; float:left; color:#fff; margin-top:12px; font-weight:normal; text-overflow:ellipsis; white-space: nowrap; overflow:hidden; }.miscWrapper ul { padding:20px; overflow:hidden; }.miscWrapper li { width:100%; border-top:1px solid #e2e2e2; margin-top: 20px; padding-top: 20px; }.miscWrapper li:first-child { border-top:none; margin-top:0; padding-top:0; }.text-default { color:#4d5057; }.bottom-shadow { -webkit-box-shadow:0 2px 2px #ccc; -moz-box-shadow:0 2px 2px #ccc; box-shadow:0 2px 2px #ccc; }.misc-container { display:table; }.misc-container a { display:table-cell; vertical-align: middle; }.misc-list-image { width:100px; height:54px; background: url('https://imgd.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif') no-repeat center center; overflow:hidden; text-align:center; }.misc-container a img { width:100%; }.video-views-count-container { min-width:140px; }.border-light-right { border-right:1px solid #e2e2e2; }
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
        <section class="bg-white padding-top50 bottom-shadow margin-bottom30">
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
                    <img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/search-loading.gif"   />
                </div>
            </div>
        </section>
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
            var catId = '<%= categoryIdList %>';
            var maxPage = Math.ceil(<%= totalRecords %>/6);
            var isNextPage = true;
            var apiURL = "/api/v1/videos/subcategory/";
            var cacheKey = catId.replace(",","_");
            $(document).ready(function () {
                $("img .lazy").lazyload();
                $("#loading").hide();
                window.location.hash = "";
            });
            $('.category-back-btn').on('click', function () {
                window.location = "/m/bike-videos/";
            });
        </script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script src="<%= staticUrl  %>/src/lscache.min.js?<%= staticFileVersion%>"></script>
        <script type="text/javascript" src="<%= staticUrl  %>/m/src/video/videoByCategory.js?<%= staticFileVersion %>"></script>
        
    </form>
</body>
</html>
