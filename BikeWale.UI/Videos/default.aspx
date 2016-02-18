<%@ Page Language="C#" Inherits="Bikewale.Videos.Default" AutoEventWireup="false" EnableViewState="false" Trace="true" %>
<%@ Register TagPrefix="BikeWale" TagName="video" Src="/controls/VideoCarousel.ascx" %>
<%@ Register Src="~/controls/Videos.ascx" TagName="VideosLanding" TagPrefix="BW" %>
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

        <section>
            <div class="container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top40 margin-bottom20 font28">First ride</h2>
                    <div class="jcarousel-wrapper firstride-jcarousel">
                        <div class="jcarousel">
                            <ul>
                                <li class="front">
                                    <div class="videocarousel-image-wrapper rounded-corner2">
                                        <a href="">
                                            <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/hero-ignitor-drum-brake-99.jpg?20151209181302" alt="" title="" src="" border="0"/>
                                        </a>
                                    </div>
                                    <div class="videocarousel-desc-wrapper">
                                        <a href="" class="font14 text-bold text-default">Benelli TNT 300 vs Kawasaki Z250 l Review l Powerdrift</a>
                                        <p class="font12 text-light-grey margin-top10 margin-bottom10">November 25, 2015</p>
                                        <div class="grid-6 alpha omega border-light-right font14">
                                            <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default">29,800</span>
                                        </div>
                                        <div class="grid-6 omega padding-left20 font14">
                                            <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default">2,800</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </li>
                                <li class="front">
                                    <div class="videocarousel-image-wrapper rounded-corner2">
                                        <a href="">
                                            <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/hero-achiever-disc-self-90.jpg?20151209181112" alt="" title="" src="" border="0"/>
                                        </a>
                                    </div>
                                    <div class="videocarousel-desc-wrapper">
                                        <a href="" class="font14 text-bold text-default">Exclusive Review: 2015 DSK Benelli TreK 113  l  Review l Powerdrift</a>
                                        <p class="font12 text-light-grey margin-top10 margin-bottom10">November 25, 2015</p>
                                        <div class="grid-6 alpha omega border-light-right font14">
                                            <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default">29,800</span>
                                        </div>
                                        <div class="grid-6 omega padding-left20 font14">
                                            <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default">2,800</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </li>
                                <li class="front">
                                    <div class="videocarousel-image-wrapper rounded-corner2">
                                        <a href="">
                                            <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/honda-livo-self-drum-alloy-828.jpg?20151209184857" alt="" title="" src="" border="0"/>
                                        </a>
                                    </div>
                                    <div class="videocarousel-desc-wrapper">
                                        <a href="" class="font14 text-bold text-default">Kawasaki Z800 Review  I  PowerDrift</a>
                                        <p class="font12 text-light-grey margin-top10 margin-bottom10">November 25, 2015</p>
                                        <div class="grid-6 alpha omega border-light-right font14">
                                            <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default">29,800</span>
                                        </div>
                                        <div class="grid-6 omega padding-left20 font14">
                                            <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default">2,800</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </li>
                                <li class="front">
                                    <div class="videocarousel-image-wrapper rounded-corner2">
                                        <a href="">
                                            <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/hero-glamour-fi-standard-480.jpg?20151209181142" alt="" title="" src="" border="0"/>
                                        </a>
                                    </div>
                                    <div class="videocarousel-desc-wrapper">
                                        <a href="" class="font14 text-bold text-default">Benelli TNT 300 vs Kawasaki Z250 l Review l Powerdrift</a>
                                        <p class="font12 text-light-grey margin-top10 margin-bottom10">November 25, 2015</p>
                                        <div class="grid-6 alpha omega border-light-right font14">
                                            <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default">29,800</span>
                                        </div>
                                        <div class="grid-6 omega padding-left20 font14">
                                            <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default">2,800</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </li>
                                <li class="front">
                                    <div class="videocarousel-image-wrapper rounded-corner2">
                                        <a href="">
                                            <img class="lazy" data-original="http://imgd4.aeplcdn.com//310x174//bw/models/honda-cb-shinesp.jpg?20151911151047" alt="" title="" src="" border="0"/>
                                        </a>
                                    </div>
                                    <div class="videocarousel-desc-wrapper">
                                        <a href="" class="font14 text-bold text-default">Exclusive Review: 2015 DSK Benelli TreK 113  l  Review l Powerdrift</a>
                                        <p class="font12 text-light-grey margin-top10 margin-bottom10">November 25, 2015</p>
                                        <div class="grid-6 alpha omega border-light-right font14">
                                            <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default">29,800</span>
                                        </div>
                                        <div class="grid-6 omega padding-left20 font14">
                                            <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default">2,800</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </li>
                                <li class="front">
                                    <div class="videocarousel-image-wrapper rounded-corner2">
                                        <a href="">
                                            <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/bajaj-discover-150s-drum-768.jpg?20151209174336" alt="" title="" src="" border="0"/>
                                        </a>
                                    </div>
                                    <div class="videocarousel-desc-wrapper">
                                        <a href="" class="font14 text-bold text-default">Kawasaki Z800 Review  I  PowerDrift</a>
                                        <p class="font12 text-light-grey margin-top10 margin-bottom10">November 25, 2015</p>
                                        <div class="grid-6 alpha omega border-light-right font14">
                                            <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default">29,800</span>
                                        </div>
                                        <div class="grid-6 omega padding-left20 font14">
                                            <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default">2,800</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                    </div>
                    <a href="" class="font16 text-center more-videos-link">View more videos</a>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section class="bg-white">
            <div class="container">
                <ERV:ExpertReview runat="server" ID="ctrlExpertReview" />
            </div>
        </section>

        <section>
            <div class="container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top40 margin-bottom20 font28">Launch alert</h2>
                    <div class="jcarousel-wrapper firstride-jcarousel">
                        <div class="jcarousel">
                            <ul>
                                <li class="front">
                                    <div class="videocarousel-image-wrapper rounded-corner2">
                                        <a href="">
                                            <img src="" class="lazy" data-original="http://imgd2.aeplcdn.com//310x174//bw/models/honda-cb-hornet-160r.jpg?20151012195209" alt="" title=""  border="0"/>
                                        </a>
                                    </div>
                                    <div class="videocarousel-desc-wrapper">
                                        <a href="" class="font14 text-bold text-default">Benelli TNT 300 vs Kawasaki Z250 l Review l Powerdrift</a>
                                        <p class="font12 text-light-grey margin-top10 margin-bottom10">November 25, 2015</p>
                                        <div class="grid-6 alpha omega border-light-right font14">
                                            <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default">29,800</span>
                                        </div>
                                        <div class="grid-6 omega padding-left20 font14">
                                            <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default">2,800</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </li>
                                <li class="front">
                                    <div class="videocarousel-image-wrapper rounded-corner2">
                                        <a href="">
                                            <img src="" class="lazy" data-original="http://imgd2.aeplcdn.com//310x174//bw/models/honda-cb-hornet-160r.jpg?20151012195209" alt="" title="" border="0"/>
                                        </a>
                                    </div>
                                    <div class="videocarousel-desc-wrapper">
                                        <a href="" class="font14 text-bold text-default">Exclusive Review: 2015 DSK Benelli TreK 113  l  Review l Powerdrift</a>
                                        <p class="font12 text-light-grey margin-top10 margin-bottom10">November 25, 2015</p>
                                        <div class="grid-6 alpha omega border-light-right font14">
                                            <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default">29,800</span>
                                        </div>
                                        <div class="grid-6 omega padding-left20 font14">
                                            <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default">2,800</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </li>
                                <li class="front">
                                    <div class="videocarousel-image-wrapper rounded-corner2">
                                        <a href="">
                                            <img src="" class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/honda-cb-shine-kick/drum/spokes-111.jpg?20151209184344" alt="" title="" border="0"/>
                                        </a>
                                    </div>
                                    <div class="videocarousel-desc-wrapper">
                                        <a href="" class="font14 text-bold text-default">Kawasaki Z800 Review  I  PowerDrift</a>
                                        <p class="font12 text-light-grey margin-top10 margin-bottom10">November 25, 2016</p>
                                        <div class="grid-6 alpha omega border-light-right font14">
                                            <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default">29,800</span>
                                        </div>
                                        <div class="grid-6 omega padding-left20 font14">
                                            <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default">2,800</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </li>
                                <li class="front">
                                    <div class="videocarousel-image-wrapper rounded-corner2">
                                        <a href="">
                                            <img src="" class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/bajaj-pulsar-rs200.jpg?20150710124439" alt="" title="" border="0"/>
                                        </a>
                                    </div>
                                    <div class="videocarousel-desc-wrapper">
                                        <a href="" class="font14 text-bold text-default">Benelli TNT 300 vs Kawasaki Z250 l Review l Powerdrift</a>
                                        <p class="font12 text-light-grey margin-top10 margin-bottom10">November 25, 2015</p>
                                        <div class="grid-6 alpha omega border-light-right font14">
                                            <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default">29,800</span>
                                        </div>
                                        <div class="grid-6 omega padding-left20 font14">
                                            <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default">2,800</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </li>
                                <li class="front">
                                    <div class="videocarousel-image-wrapper rounded-corner2">
                                        <a href="">
                                            <img src="" class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/royal-enfield-classic-350-standard-136.jpg?20151209202137" alt="" title="" border="0"/>
                                        </a>
                                    </div>
                                    <div class="videocarousel-desc-wrapper">
                                        <a href="" class="font14 text-bold text-default">Exclusive Review: 2015 DSK Benelli TreK 113  l  Review l Powerdrift</a>
                                        <p class="font12 text-light-grey margin-top10 margin-bottom10">November 25, 2015</p>
                                        <div class="grid-6 alpha omega border-light-right font14">
                                            <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default">29,800</span>
                                        </div>
                                        <div class="grid-6 omega padding-left20 font14">
                                            <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default">2,800</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </li>
                                <li class="front">
                                    <div class="videocarousel-image-wrapper rounded-corner2">
                                        <a href="">
                                            <img src="" class="lazy" data-original="http://imgd3.aeplcdn.com//310x174//bw/models/bajaj-avenger-150-street.jpg?20152710145912" alt="" title="" border="0"/>
                                        </a>
                                    </div>
                                    <div class="videocarousel-desc-wrapper">
                                        <a href="" class="font14 text-bold text-default">Kawasaki Z800 Review  I  PowerDrift</a>
                                        <p class="font12 text-light-grey margin-top10 margin-bottom10">November 25, 2015</p>
                                        <div class="grid-6 alpha omega border-light-right font14">
                                            <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default">29,800</span>
                                        </div>
                                        <div class="grid-6 omega padding-left20 font14">
                                            <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default">2,800</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                    </div>
                    <a href="" class="font16 text-center more-videos-link">View more videos</a>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <script type="text/javascript">
            $(document).ready(function () { $("img.lazy").lazyload(); });
        </script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
    </form>
</body>
</html>