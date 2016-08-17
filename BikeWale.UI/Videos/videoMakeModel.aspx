<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Videos.VideoMakeModel" EnableViewState="false" %>
<!DOCTYPE html>
<html>
<head>
    <%
        
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;    
    
        isAd970x90Shown = false;
        title = titleName;
        description = metaDescription;
        keywords = metaKeywords;
        if (isModel)
        {
            canonical = string.Format("http://www.bikewale.com/{0}-bikes/{1}/videos/", makeMaskingName, modelMaskingName);
            alternate = string.Format("http://www.bikewale.com/m/{0}-bikes/{1}/videos/", makeMaskingName, modelMaskingName);
        }
        else
        {
            canonical = string.Format("http://www.bikewale.com/{0}-bikes/videos/", makeMaskingName);
            alternate = string.Format("http://www.bikewale.com/m/{0}-bikes/videos/", makeMaskingName);
        }
         %>
    <!-- #include file="/includes/headscript.aspx" -->
    
    <style type="text/css">
        .miscWrapper li { width:312px; height:312px; background:#fff; float:left; border: 1px solid #e2e2e2; padding:20px; margin-right:10px; margin-bottom:20px; margin-left:10px; }.video-image-wrapper { width:271px; height:153px; margin-bottom:15px; overflow:hidden; text-align:center; }.video-image-wrapper a { width:100%; height:100%; display:block; background:url('http://img.aeplcdn.com/bikewaleimg/images/loader.gif') no-repeat center center; }.video-image-wrapper img { width:100%; height:100%; }.border-light-right { border-right:1px solid #e2e2e2; }
    </style>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-top15 margin-bottom10">
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/"><span itemprop="title">Home</span></a></li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><span class="bwsprite fa-angle-right margin-right10"></span><a href="/<%=makeMaskingName %>-bikes/"><span  itemprop="title"><%=make %> Bikes</span></a></li>
                            <% if(isModel){ %>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><span class="bwsprite fa-angle-right margin-right10"></span><a href="/<%=makeMaskingName %>-bikes/<%=modelMaskingName %>/"><span itemprop="title"><%=make %> <%=model %></span></a></li>
                            <% } %>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><span class="bwsprite fa-angle-right margin-right10"></span><span  itemprop="title">Bike Videos</span></li>
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
                                    <a href="<%# string.Format("/bike-videos/{0}-{1}/", DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(), DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>">
                                        <img class="lazy" data-original="<%#String.Format("http://img.youtube.com/vi/{0}/mqdefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId")) %>"
                                            alt="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="" border="0" />
                                    </a>
                                </div>
                                <div class="video-desc-wrapper">
                                    <a href="<%# string.Format("/bike-videos/{0}-{1}/", DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(), DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %> " class="font14 text-bold text-default"><%# DataBinder.Eval(Container.DataItem,"VideoTitle") %></a>
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
            <div class="text-center">
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
                            border="0" />
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
            var cwHostUrl = "<%= Bikewale.Utility.BWConfiguration.Instance.CwApiHostUrl %>";
            var catId = <%= isModel ? modelId : makeId %>;
            var maxPage = 10000000;//Number.MAX_VALUE; 
            var isModel = false;
            var isNextPage = true;  //change it
            var apiURL = isModel ? "/api/v1/videos/model/" : "/api/v1/videos/make/";
            var cacheKey = isModel ? "model_" + catId : "make_" + catId;
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
