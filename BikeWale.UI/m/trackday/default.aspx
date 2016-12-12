<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.TrackDay.Default" %>
<!DOCTYPE html>
<html>
<head>
    <% 
        //title       = "Bike News - Latest Indian Bike News & Views - BikeWale";
        //description = "Latest news updates on Indian bikes industry, expert views and interviews exclusively on BikeWale.";
        //keywords    = "news, bike news, auto news, latest bike news, indian bike news, bike news of india"; 
        //canonical   = "https://www.bikewale.com/news/";
        //AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        //AdId = "1398766302464";
        //Ad_320x50 = true;
        //Ad_Bot_320x50 = true;
    %>
   
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->

    <link rel="stylesheet" type="text/css" href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/trackday/css/landing.css?<%= staticFileVersion %>" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <% if(!androidApp) { %>
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <% } %>

        <section>
            <div class="container trackday-banner text-center section-container">
                <h1 class="font24 text-uppercase text-white padding-bottom10">Track Day 2016</h1>
                <h2 class="font14 text-unbold text-white">Lorem ipsum dolor sit amet, consecte</h2>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <h2 class="section-heading">Articles</h2>
                <div class="content-box-shadow padding-right20 padding-left20">
                    <ul class="article-list">
                        <li>
                            <div class="review-image-wrapper">
                                <a href="" title="Honda Brazil launches fuel injected CG125i motorcycle" >
                                    <img class="lazy" data-original="https://imgd2.aeplcdn.com//370x208//bw/ec/21691/Honda-Exterior-65001.jpg" alt="Honda Brazil launches fuel injected CG125i motorcycle" src="">
                                </a>
                            </div>
                            <div class="review-heading-wrapper">
                                <a href="/m/news/21691-honda-brazil-launches-fuel-injected-cg125i-motorcycle.html" class="target-link">Honda Brazil launches fuel injected...</a>
                                <div class="grid-7 alpha padding-right5">
                                    <span class="bwmsprite calender-grey-sm-icon"></span>
                                    <span class="article-stats-content">Nov 15, 2016</span>
                                </div>
                                <div class="grid-5 alpha omega">
                                    <span class="bwmsprite author-grey-sm-icon"></span>
                                    <span class="article-stats-content">Santosh Nair</span>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <p class="margin-top10">Honda motorcycles Brazil has launched the CG125i for the Brazilian market at 6,790 Real (Rs 1.12 lakh). Honda’s Brazilian arm introduced the upgraded version in a bid...</p>
                        </li>
                        <li>
                            <div class="review-image-wrapper">
                                <a href="" title="Honda Brazil launches fuel injected CG125i motorcycle">
                                    <img class="lazy" data-original="https://imgd2.aeplcdn.com//370x208//bw/ec/21691/Honda-Exterior-65001.jpg" alt="Honda Brazil launches fuel injected CG125i motorcycle" src="">
                                </a>
                            </div>
                            <div class="review-heading-wrapper">
                                <a href="" class="target-link">TVS Apache RTR 200 4V vs Bajaj Pulsar...</a>
                                <div class="grid-7 alpha padding-right5">
                                    <span class="bwmsprite calender-grey-sm-icon"></span>
                                    <span class="article-stats-content">Nov 15, 2016</span>
                                </div>
                                <div class="grid-5 alpha omega">
                                    <span class="bwmsprite author-grey-sm-icon"></span>
                                    <span class="article-stats-content">Santosh Nair</span>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <p class="margin-top10">Honda motorcycles Brazil has launched the CG125i for the Brazilian market at 6,790 Real (Rs 1.12 lakh). Honda’s Brazilian arm introduced the upgraded version in a bid...</p>
                        </li>
                    </ul>
                </div>
            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <% if(!androidApp) { %>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <% } %>

        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />

    </form>
</body>
</html>