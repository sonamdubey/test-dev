<%@ Page Language="C#" Inherits="Bikewale.Videos.Default" AutoEventWireup="false" EnableViewState="false" %>
<%@ Register TagPrefix="BikeWale" TagName="video" Src="/controls/VideoCarousel.ascx" %>
<%@ Register Src="~/controls/Videos.ascx" TagName="VideosLanding" TagPrefix="BW" %>
<%@ Register Src="~/controls/VideoByCategory.ascx" TagName="ByCategory" TagPrefix="BW" %>
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
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top40 margin-bottom20 font28">Expert reviews</h2>
                    <div class="grid-6 padding-left20">
                        <div class="reviews-image-wrapper rounded-corner2">
                            <a href="">
                                <img class="lazy" data-original="http://imgd1.aeplcdn.com//640x348//bw/ec/21109/Honda-CB-Hornet-160R-Front-threequarter-62325.jpg?wm=2" alt="" title="" src="" border="0"/>
                            </a>
                        </div>
                        <div class="reviews-desc-wrapper">
                            <a href="" class="text-default font14 text-bold">Bajaj Pulsar RS 200 vs Pulsar 220 DTSI - The New Fastest Indian | PowerDrift</a>
                            <p class="font12 text-light-grey margin-top10 margin-bottom10">November 25, 2015</p>
                            <p class="font14 text-light-grey margin-bottom15">The new Avenger's are here! Atleast briefly so! With time not being on our side, this was the time for some Civil war. Watch Varun...</p>
                            <div class="grid-4 alpha omega border-light-right font14">
                                <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default">29,800</span>
                            </div>
                            <div class="grid-8 omega padding-left20 font14">
                                <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default">2,800</span>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <div class="grid-6">
                        <div class="reviews-image-wrapper rounded-corner2">
                            <a href="">
                                <img class="lazy" data-original="http://imgd1.aeplcdn.com//640x348//bw/ec/21109/Honda-CB-Hornet-160R-Front-62323.jpg?wm=2" alt="" title="" src="" border="0"/>
                            </a>
                        </div>
                        <div class="reviews-desc-wrapper">
                            <a href="" class="text-default font14 text-bold">TVS Apache: One Make Racing: PowerDrift</a>
                            <p class="font12 text-light-grey margin-top10 margin-bottom10">November 25, 2015</p>
                            <p class="font14 text-light-grey margin-bottom15">The new Avenger's are here! Atleast briefly so! With time not being on our side, this was the time for some Civil war. Watch Varun...</p>
                            <div class="grid-4 alpha omega border-light-right font14">
                                <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default">29,800</span>
                            </div>
                            <div class="grid-8 omega padding-left20 font14">
                                <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default">2,800</span>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <div class="clear"></div>
                    <a href="" class="font16 text-center padding-top15 more-videos-link">View more videos</a>
                </div>
                <div class="clear"></div>
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