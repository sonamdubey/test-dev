<%@ Page Language="C#" Inherits="Bikewale.Videos.video" AutoEventWireup="false" %>
<!DOCTYPE html>
<%@ Register Src="~/controls/SimilarVideos.ascx" TagName="SimilarVideos" TagPrefix="BW" %>
<html>
<head>
    <%   
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <style type="text/css">
        #embedVideo iframe { width:934px; height:527px; }
        .video-views-counts { min-width:140px; }
        .border-light-right { border-right:1px solid #e2e2e2; }
        .video-social-wrapper { position:fixed; left:0; top:230px; width:54px; height:162px; overflow:hidden; background:#e2e2e2; }
        .video-social-wrapper li { height:54px; font-size:11px; padding-top:8px; padding-bottom:8px; }
        .video-social-wrapper a { display:block; color:#fff; text-align:center; }
        .video-social-wrapper a:hover { text-decoration:none; }
        .video-social-wrapper span { display:block; margin:0 auto; }
        .video-social-wrapper span.fa { font-size:25px; }
        .fb-counter { background:#3b5998; } .tw-counter { background:#00aced; } .gp-counter { background:#dd4b39; }
        .related-video-jcarousel li { height:312px; border: 1px solid #e2e2e2; padding:20px; }
        .videocarousel-image-wrapper { width:271px; height:153px; margin-bottom:15px; overflow:hidden; text-align:center; }
        .videocarousel-image-wrapper a { width:100%; height:100%; display:block; background:url('http://img.aeplcdn.com/bikewaleimg/images/loader.gif') no-repeat center center; }
        .videocarousel-image-wrapper img { width:100%; height:100%; }
        .video-views-icon { width:17px; height:13px; background-position:-59px -303px; }
        .video-likes-icon { width:15px; height:15px; background-position:-84px -277px; position:relative; top:2px; }
        .more-videos-link { display:block; margin-top:5px; margin-bottom:25px; }
    </style>
    <link rel="stylesheet" href="../css/jquery.floating-social-share.css">

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
                    <h1 class="font26 margin-bottom5"><%=videoModel.Title%></h1>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section>
            <div class="container">
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-20">
                        <div id="embedVideo" class="margin-bottom15">
                            <iframe width="934" height="527" src="<%=videoModel.VideoUrl %>" frameborder="0" allowfullscreen></iframe>
                        </div>
                        <p class="font14 text-light-grey margin-bottom10"><%=videoModel.Description %>
                        <p class="clear"></p>
                        <p class="video-views-counts border-light-right font14 leftfloat">
                            <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default"><%=videoModel.Views %></span>
                        </p>
                        <p class="video-views-counts padding-left20 font14 leftfloat">
                            <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default"><%=videoModel.Likes %></span>
                        </p>
                        <p class="rightfloat text-light-grey font12">November 25, 2015</p>
                        <p class="clear"></p>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section class="margin-bottom30 <%= (rptSimilarVideos.FetchedRecordsCount > 0) ? string.Empty : "hide" %>">
         <BW:SimilarVideos ID="rptSimilarVideos" runat="server" />              
        </section>
        <%--<section>
            <div class="container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top40 margin-bottom20 font28">Related videos</h2>
                    <div class="jcarousel-wrapper related-video-jcarousel">
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
                                            <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default">54,564</span>
                                        </div>
                                        <div class="grid-6 omega padding-left20 font14">
                                            <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default">11,254</span>
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
        </section>--%>
        <script type="text/javascript">
            $(function () {
                $("body").floatingSocialShare();
            });
        </script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
    </form>
    <script type="text/javascript" src="../src/jquery.floating-social-share.js">
    </script>
</body>
</html>