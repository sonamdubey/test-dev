<%@ Page Language="C#" AutoEventWireup="false" Inherits=" Carwale.UI.Editorial.VideoDetails" Trace="false" Debug="false" ViewStateMode="Disabled" %>

<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
    <%
        // Define all the necessary meta-tags info here.
        // To know what are the available parameters,
        // check page, headerCommon.aspx in common folder.

        PageId = 55;
        Title = metaTitle;
        Description = metaDescription;
        Keywords = tags;
        AdId = "1396440332273";
        AdPath = "/1017752/ReviewsNews_";
        canonical = "https://www.carwale.com" + Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString();
        altUrl = "https://www.carwale.com/m" + Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString();
        var mainImage = string.Format("https://img.youtube.com/vi/{0}/0.jpg", videoId);
    %>
    <meta property="og:url" content="<%= ("https://www.carwale.com" + Request.ServerVariables["http_x_rewrite_url"]) %>"  />
    <meta property="og:type" content="website" />
    <meta property="og:title" content="<%= Title %>" />
    <meta property="og:description" content="<%= Description %>" />
    <meta property="og:image" content="<%=mainImage %>" />
    <meta property="og:site_name" content="CarWale" />
    <meta name="twitter:card" content="summary_large_image">
    <meta name="twitter:site" content="<%= ("@CarWale")%>">
    <meta name="twitter:title" content="<%= Title %>">
    <meta name="twitter:description" content="<%= Description %>">
    <meta name="twitter:creator" content="<%= ("@CarWale")%>">
    <meta name="twitter:image" content="<%=mainImage %>">
    <meta property="fb:admins" content="154881297559" />
    <meta property="fb:pages" content="154881297559" />
    <!-- #include file="/includes/global/head-script.aspx" -->
    <script type='text/javascript'>
        googletag.cmd.push(function () {
            googletag.defineSlot('<%= AdPath %>300x250', [300, 250], 'div-gpt-ad-<%= AdId %>-0').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
            googletag.pubads().setTargeting("City", "<%= CookiesCustomers.MasterCity.ToString() %>");
            googletag.pubads().setTargeting('UserModelHistory', '<%= CookiesCustomers.UserModelHistory.Replace('~', ',')%>');
            //googletag.pubads().enableSyncRendering();
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();
        });   
    </script> 
    <link rel="stylesheet" href="/static/css/video.css" type="text/css" >
    <link rel="stylesheet" href="/static/css/researchnew.css" type="text/css" >
    <script type="text/javascript" src="https://www.youtube.com/iframe_api"></script>
    <script   src="/static/src/bt.js"  type="text/javascript"></script>
    <script  type="text/javascript"  src="/static/js/videostracking.js" ></script>
    <script src="https://apis.google.com/js/platform.js"></script>
</head>
<script>    
    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/en_US/all.js#xfbml=1";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));
</script>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <div id="fb-root"></div>
    <form id="form1" runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <section class="container">
            <div class="grid-12">
                <div class="padding-bottom10 padding-top10 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
            <div class="clear"></div>
        </section>
        <section class="bg-light-grey padding-bottom20">
            <div class="container">
            <div class="grid-12 padding-top10">
                <div>
                    <!-- breadcrumb code starts here -->
                    <ul class="breadcrumb special-skin-text">
                        <li><a href="/">Home</a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span><a href="/<%= makeName %>-cars/"><%= orgMakeName %></a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span><a href="/<%= makeName %>-cars/<%= maskingName %>"><%= orgMakeName + ' ' + orgModelName %></a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span><%= videoTitle %></li>
                    </ul>
                    <div class="clear"></div>
                    <h1 class="content-inner-block"><%= videoTitle %></h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
            </div>
            <div class="clear"></div>
            <div class="grid-8 margin-top10 ">
                <!--Video code starts here-->
                <div class="content-box-shadow">
                    <div class="content-inner-block-5">
                        <iframe id="<%= videoId %>" width="630" height="325" src="<%= videoUrl+"&enablejsapi=1" %>" frameborder="0" allowfullscreen></iframe>
                        <br />
                        <div class="margin-bottom10 yt-link-btn-area">
                            <div class="redirect-lt margin-top10 margin-left10 inline-block" style="font-size: 16px;">
                                <span class="video-sprite review-icon-big"></span>
                                <asp:Label ID="lblViews" runat="server" class="margin-bottom15"></asp:Label>
                                Views
                            </div>
                            <div class=" margin-top15 inline-block margin-left30">
                                <%-- <a href="#" id="spnSubScribe" class="video-sprite subscribe margin-left15 margin-right10"></a>--%>
                                <div class="rightfloat relative-position">
                                    <a href="#" id="spnLike" class="video-sprite like-video margin-left15 margin-right5">Like</a>
                                    <span class="relative-position"><span class="count-box-tail"></span>
                                        <asp:Label ID="lblLikes" CssClass="count-box" runat="server"></asp:Label>
                                    </span>
                                </div>
                                <div class="rightfloat ytsubscribe-area">
                                    <div class="g-ytsubscribe" data-channelid="UCMDV6J2hWXet7ZCfgrXGgeg" data-layout="default" data-count="default"></div>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div>
                            <asp:Label ID="lblDescription" runat="server"></asp:Label>
                        </div>
                        <div class="clear"></div>
                        <div id="divTags" runat="server" class="margin-top10">
                            <span><strong>Tags: </strong></span>
                            <asp:Label ID="lblTags" runat="server"></asp:Label>
                        </div>
                        <div class="clear"></div>
                        <div style="margin-top: 10px;" class="fb-comments" data-href="https://<%=Request.ServerVariables["HTTP_HOST"] %>/<%=makeName %>-cars/<%=modelName%>/videos/<%=subCatName %>-<%=basicId%>/" data-width="630" data-colorscheme="light"></div>
                    </div>
                </div>
                <!--Video code ends here-->
            </div>

            <div class="grid-4 margin-top10">
                <div class="">
                    <!-- Ad block code start here -->
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 20, 20, false, 0) %>
                </div>
                <!-- Ad block code start here -->
                <div class="clear"></div>
                <div class="content-block-white content-box-shadow padding-left10" id="divRelatedVid" runat="server">
                    <h2 class="content-inner-block padding-top5 padding-bottom5">Related Videos</h2>
                    <div class="white-shadow content-inner-block">
                        <ul>
                            <asp:Repeater ID="rptRelatedVid" runat="server">
                                <ItemTemplate>
                                    <li class="margin-bottom10">
                                        <div>
                                            <div class="pic-place leftfloat" style="width: 120px;">
                                                <a href="/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem,"MakeName").ToString()) %>-cars/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem,"MaskingName").ToString()) %>/videos/<%# FormatSubCat(DataBinder.Eval(Container.DataItem,"SubCatName").ToString()) %>-<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>/" target="_blank">
                                                    <div class="video-sprite play-btn"></div>
                                                    <img src="https://img.youtube.com/vi/<%#  DataBinder.Eval(Container.DataItem,"VideoId").ToString() %>/1.jpg" alt="<%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %>" title="<%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %>" border="0" />
                                                </a>
                                            </div>
                                            <div style="width: 155px;" class="rightfloat">
                                                <a href="/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem,"MakeName").ToString()) %>-cars/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem,"MaskingName").ToString()) %>/videos/<%# FormatSubCat(DataBinder.Eval(Container.DataItem,"SubCatName").ToString()) %>-<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>/" target="_blank"><strong><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %></strong></a><br />
                                                <span class="video-sprite review-icon-dark margin-right5 margin-top10"></span><span class="margin-top10"><%# DataBinder.Eval(Container.DataItem,"Views").ToString() %> Views</span>
                                            </div>
                                            <div class="clear"></div>
                                        </div>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="clear"></div></div>
        </section>

    </form>
    <!-- #include file="/includes/footer.aspx"-->
    <!-- #include file="/includes/global/footer-script.aspx"-->

    <script type="text/javascript">
        Common.showCityPopup = false; doNotShowAskTheExpert = false;
        var videoId = '<%= videoId %>';//videoId
        var clientId = '<%= ConfigurationManager.AppSettings["clientId"].ToString() %>';  //clientId
        var redirect_url = "https://<%=Request.ServerVariables["HTTP_HOST"] %>/videos/youtuberesponse.html";  //redirect_url
        var developer_key = '<%= ConfigurationManager.AppSettings["developer_key"].ToString() %>';
        var access_token = "";
        var isError = false;
        var views = "";
        var likes = "";
        var make = "<%=makeName %>", model = "<%=modelName %>", basicId = '<%= basicId %>', encodedstate = "", videotitle = '<%= videoTitleUrl %>';
        //var userId = 'g2Tie2nTt2SwCV45H_RDTw', subSciptionCount = 0, userName = 'kumarvikram'; //subscription related
        onYouTubeIframeReady(videoId);

        $(document).ready(function () {
            //alert("videotitle is : " + videotitle);
            //when access token is null,then retrieve youtube parameters.            
            if (access_token == "") {
                var timesRun = 0;
                var interval = setInterval(function () {
                    timesRun += 1;
                    if (timesRun == 3) {
                        if (access_token == "") {
                            retrieveParameters();
                            //retrieveSubScriptions();
                            sendParameters();
                        }
                    }
                }, 2000);
            }

            if (access_token != "") {
                $('#spnLike').hasClass('like-video')
                {
                    sendLikes();
                    retrieveParameters();
                    sendParameters();
                }

                //$('#spnSubScribe').hasClass('subscribe')
                //{
                //    retrieveSubScriptions();
                //    subScribe();
                //}
            }
        });

        $(".like-video").click(function () {
            if (access_token == "") {
                //alert("encoded state before:"+encode(make, model, basicId, videotitle) );
                encode(make, model, basicId, videotitle)
                window.location = "https://accounts.google.com/o/oauth2/auth?client_id=" + clientId + "&redirect_uri=" + redirect_url + "&scope=https://www.googleapis.com/auth/youtube&response_type=token&state=" + encodedstate;
            }
            else {
                if (isError == true) {
                    alert("You can't like or dislike because you don't have access token");
                }
            }
        });

        if (window.location.hash) {
            var hash = window.location.hash.substring(1); //Puts hash in variable, and removes the # character
            if (hash.indexOf("error") == -1) {
                var pairs = hash.split('&');
                var parts = pairs[0].split('=');
                access_token = parts[1];
                //alert("access token is : " + access_token);
                //window.open("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token=" + access_token, '_blank');
            }
            else {
                isError = true;
                alert("Error in retrieving acess token from youtube.");
            }
        }

        //code for retrieving youtube data api data...
        function retrieveParameters() {
            $.ajax({
                type: "GET",
                url: "https://www.googleapis.com/youtube/v3/videos?part=statistics&id=" + videoId + "&key=AIzaSyCQRyGIUebquGytlnfHQL9CT47T_Fh4WNA",
                async: false,
                dataType: 'json',
                success: function (response) {
                    if (typeof (response.items) == "object" && response.items.length > 0) {
                        views = response.items[0].statistics.viewCount;
                        likes = response.items[0].statistics.likeCount;
                        var formattedLikes = addCommas(likes);
                        var formattedViews = addCommas(views);
                        $("#lblLikes").html(formattedLikes);
                        $("#lblViews").html(formattedViews);
                    }
                }
            });
        }

        //for sending like or dislike
        function sendLikes() {

            $.ajax({
                type: "POST",
                url: "https://www.googleapis.com/auth/youtube/v3/videos/rate/?key=" + Common.googleApiKey,
                async: false,
                data : {id : videoId , rating : "like"},
                dataType: 'json',
                error: function () { alert("Error Occured While Sending Like."); },
                success: function (response) {
                    //alert("Like submitted");
                    $("#spnLike").removeClass('like-video').addClass('liked-video');
                    //$("#imglike").attr('src', '/images/like3.jpg');
                    $(".liked-video").click(function () {
                        alert("Like has been submitted already.");
                    });
                }
            });            
        }

        function sendParameters() {
            var result = "";
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Carwale.UI.Editorial.VideoDetails,Carwale.ashx",
                async: false,
                data: '{"basicId":"' + basicId + '","views":"' + views + '","likes":"' + likes + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdateParameters"); },
                success: function (response) {
                    responseObj = eval('(' + response + ')');
                    result = responseObj.value;
                },
                error: function () { alert("Error in Updating Likes and Views in Carwale Database."); }
            });
        }

        function encode(make, model, basicId, videotitle) {
            var newstate = videotitle;
            encodedstate = window.btoa(newstate);
        }


        function addCommas(nStr) {
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        }
    </script>
</body>
</html>
