<%@ Page Language="C#" Inherits="Bikewale.Videos.Default" AutoEventWireup="false" EnableViewState="false" Trace="true" %>
<%@ Register TagPrefix="BikeWale" TagName="video" Src="/controls/VideoCarousel.ascx" %>
<%@ Register Src="~/controls/Videos.ascx" TagName="VideosLanding" TagPrefix="BW" %>
<%@ Register Src="~/controls/VideoByCategory.ascx" TagName="ByCategory" TagPrefix="BW" %>
<%@ Register Src="~/controls/ExpertReviewsVideos.ascx" TagName="ExpertReview" TagPrefix="ERV" %>

<!DOCTYPE html>
<html>
<head>
    <%  
        title = "Bike Videos, Expert Video Reviews with Road Test & Bike Comparison - BikeWale";
        description ="Check latest bike and scooter videos, watch BikeWale expert's take on latest bikes and scooters - features, performance, price, fuel economy, handling and more."; 
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/video.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css" />
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
                            <li><span class="fa fa-angle-right margin-right10"></span>Videos</li>
                        </ul>
                    </div>
                    <h1 class="font26 margin-bottom5">Videos</h1>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div id="videoJumbotron" class="container">
                <div class="grid-12">
                    <BW:VideosLanding runat="server" ID="ctrlVideosLanding" />
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container margin-top20 powerdrift-banner">
                <div class="grid-12">
                    <div class="leftfloat margin-left25 margin-top35">
                        <h3 class="text-white">Reviews, Specials, Underground, Launch Alerts &<br />a whole lot more...</h3>
                    </div>
                    <div class="rightfloat powerdrift-subscribe">
                        <script src="https://apis.google.com/js/platform.js"></script>
                        <div class="g-ytsubscribe" data-channel="powerdriftofficial" data-layout="full" data-count="hidden"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <BW:ByCategory runat="server" ID="ctrlFirstRide" />  
        <section class="bg-white">
            <div class="container">
                <ERV:ExpertReview runat="server" ID="ctrlExpertReview" />
            </div>
        </section>

        <BW:ByCategory runat="server" ID="ctrlLatestBike" />

        <script type="text/javascript">
            $(document).ready(function () { $("img.lazy").lazyload(); });
        </script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
    </form>
</body>
</html>