<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Videos.VideoCategories" EnableViewState="false" %>

<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" Src="/controls/LinkPagerControl.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <%        
        //Bikewale.Utility.VideoTitleDescription.VideoTitleDesc(categoryId, out title, out description, make, model);
         title = titleName;
         description = descName;
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/videocategory.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css" />
    <%
        isAd970x90Shown = false;
         %>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-top15 margin-bottom10">
                        <ul>
                            <li><a href="/"><span>Home</span></a></li>
                            <li><a href="/bike-videos/"><span class="fa fa-angle-right margin-right10"></span>Videos</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><%= pageHeading %></li>
                        </ul>
                    </div>
                    <h1 class="font26 margin-bottom5"><%= pageHeading %></h1>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="miscWrapper container">
                <ul id="listVideos1">
                    <asp:Repeater ID="rptVideos" runat="server">
                        <ItemTemplate>
                            <li>
                                <div class="video-image-wrapper rounded-corner2">
                                    <a href="<%# "/bike-videos/" + (DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString() + "-" + DataBinder.Eval(Container.DataItem,"BasicId").ToString()) + "/" %>">
                                        <img class="lazy" data-original="<%#String.Format("http://img.youtube.com/vi/{0}/mqdefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId")) %>"
                                            alt="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="" border="0" />
                                    </a>
                                </div>
                                <div class="video-desc-wrapper">
                                    <a href="<%# "/bike-videos/" + (DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString() + "-" + DataBinder.Eval(Container.DataItem,"BasicId").ToString()) + "/" %> " class="font14 text-bold text-default"><%# DataBinder.Eval(Container.DataItem,"VideoTitle") %></a>
                                    <p class="font12 text-light-grey margin-top10 margin-bottom10"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"MMMM dd, yyyy")  %></p>
                                    <div class="grid-6 alpha omega border-light-right font14">
                                        <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Views").ToString()) %></span>
                                    </div>
                                    <div class="grid-6 omega padding-left20 font14">
                                        <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Likes").ToString() )%></span>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <div class="clear"></div>
            </div>
        </section>
        <section>
            <div style="text-align: center;">
                <div id="loading">
                    <img src="http://img2.aeplcdn.com/bikewaleimg/images/search-loading.gif"   />
                </div>
            </div>
        </section>
        <script type="text/html" id="templetVideos">
            <li>
                <div class="video-image-wrapper rounded-corner2">
                    <a data-bind="attr: { href: '/bike-videos/' + VideoTitleUrl() + '-' + BasicId() + '/' }">
                        <img class="lazy" data-bind="attr: { title: VideoTitle(), alt: VideoTitle(), src: '' }, lazyload: 'http://img.youtube.com/vi/' + VideoId() + '/mqdefault.jpg' "
                            src="" border="0" />
                    </a>
                </div>
                <div class="video-desc-wrapper">
                    <a href="" class="font14 text-bold text-default" data-bind="text: VideoTitle(), attr: { href: '/bike-videos/' + VideoTitleUrl() + '-' + BasicId() + '/' }"></a>
                    <p class="font12 text-light-grey margin-top10 margin-bottom10" data-bind="formateDate: DisplayDate()"></p>
                    <div class="grid-6 alpha omega border-light-right font14">
                        <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default" data-bind="CurrencyText: Views()"></span>
                    </div>
                    <div class="grid-6 omega padding-left20 font14">
                        <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default" data-bind="CurrencyText: Likes()"></span>
                    </div>
                    <div class="clear"></div>
                </div>
            </li>
        </script>
        <script type="text/javascript">
            var catId = '<%= categoryIdList %>';
            var maxPage = Math.ceil(<%= totalRecords %>/9);
            var isNextPage = true;
            $(document).ready(function () {
                $("img .lazy").lazyload();
                $("#loading").hide();
                window.location.hash = "";
            });
        </script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/lscache.min.js?<%= staticFileVersion%>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/Videos/videoByCategory.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>
