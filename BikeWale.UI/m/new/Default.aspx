<%@ Page Language="C#" AutoEventWireup="False" Inherits="Bikewale.Mobile.New.Default"  EnableViewState="true" %>

<%@ Register Src="~/m/controls/MUpcomingBikes.ascx" TagName="MUpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/MNewLaunchedBikes.ascx" TagName="MNewLaunchedBikes" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/MMostPopularBikes.ascx" TagName="MMostPopularBikes" TagPrefix="BW" %>
<%@ Register Src="/m/controls/CompareBikesMin.ascx" TagName="CompareBike" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewsWidget.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="/m/controls/ExpertReviewsWidget.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="/m/controls/VideosWidget.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>

<!doctype html>
<html>
<head>
    <%
        title = "New Bikes - Bikes Reviews, Photos, Specs, Features, Tips & Advices - BikeWale";
        keywords = "new bikes, new bikes prices, new bikes comparisons, bikes dealers, on-road price, bikes research, bikes india, Indian bikes, bike reviews, bike photos, specs, features, tips & advices";
        description = "New bikes in India. Search for the right new bikes for you, know accurate on-road price and discounts. Compare new bikes and find dealers.";
        canonical = "http://www.bikewale.com/new/";
        AdPath = "/1017752/Bikewale_Mobile_NewBikes_";
        AdId = "1398766302464";
     %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-newbikes.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
    <link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css?<%= staticFileVersion %>" type="text/css"rel="stylesheet" /> 
</head>
<body class="bg-light-grey">
    <form runat="server">
    <!-- #include file="/includes/headBW_Mobile.aspx" -->
    <section>
        <div class="container">
            <div class="newbikes-banner-div">
                <!-- Top banner code starts here -->
                <h1 class="text-uppercase text-white text-center padding-top25 font24">NEW Bikes</h1>
                <p class=" font16 text-white text-center">View every bike under one roof</p>
            </div>
            <!-- Top banner code ends here -->
        </div>
    </section> 

    <section class="container">
        <!-- Brand section code starts here -->
        <div class="grid-12">
            <div class="bg-white brand-wrapper content-box-shadow margin-minus60">
                <h2 class="content-inner-block-10 text-uppercase text-center margin-top30 margin-bottom20">Brand</h2>
                <div class="brand-type-container">
                    <ul class="text-center">
                        <li>
                            <a href="/m/honda-bikes/">
                                <span class="brandlogosprite brand-honda"></span>
                                <span class="brand-type-title">honda</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/bajaj-bikes/">
                                <span class="brandlogosprite brand-bajaj"></span>
                                <span class="brand-type-title">Bajaj</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/hero-bikes/">
                                <span class="brandlogosprite brand-hero"></span>
                                <span class="brand-type-title">hero</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/tvs-bikes/">
                                <span class="brandlogosprite brand-tvs"></span>
                                <span class="brand-type-title">tvs</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/royalenfield-bikes/">
                                <span class="brandlogosprite brand-royal"></span>
                                <span class="brand-type-title">Royal Enfield</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/yamaha-bikes/">
                                <span class="brandlogosprite brand-yamaha"></span>
                                <span class="brand-type-title">yamaha</span>
                            </a>
                        </li>
                    </ul>
                    <ul id="more-brand-nav" class="text-center hide">
                        <li>
                            <a href="/m/aprilia-bikes/">
                                <span class="one brandlogosprite brand-aprilia"></span>
                                <span class="brand-type-title">aprilia</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/benelli-bikes/">
                                <span class="brandlogosprite brand-benelli"></span>
                                <span class="brand-type-title">benelli</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/bmw-bikes/">
                                <span class="brandlogosprite brand-bmw"></span>
                                <span class="brand-type-title">bmw</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/ducati-bikes/">
                                <span class="brandlogosprite brand-ducati"></span>
                                <span class="brand-type-title">ducati</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/harleydavidson-bikes/">
                                <span class="brandlogosprite brand-harley"></span>
                                <span class="brand-type-title">harley</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/heroelectric-bikes/">
                                <span class="brandlogosprite brand-hero-elec"></span>
                                <span class="brand-type-title">hero-elec</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/hyosung-bikes/">
                                <span class="brandlogosprite brand-hyosung"></span>
                                <span class="brand-type-title">hyosung</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/indian-bikes/">
                                <span class="brandlogosprite brand-indian"></span>
                                <span class="brand-type-title">indian</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/kawasaki-bikes/">
                                <span class="brandlogosprite brand-kawasaki"></span>
                                <span class="brand-type-title">kawasaki</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/ktm-bikes/">
                                <span class="brandlogosprite brand-ktm"></span>
                                <span class="brand-type-title">ktm</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/lml-bikes/">
                                <span class="brandlogosprite brand-lml"></span>
                                <span class="brand-type-title">lml</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/mahindra-bikes/">
                                <span class="brandlogosprite brand-mahindra"></span>
                                <span class="brand-type-title">mahindra</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/motoguzzi-bikes/">
                                <span class="brandlogosprite brand-guzzi"></span>
                                <span class="brand-type-title">moto guzzi</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/suzuki-bikes/">
                                <span class="brandlogosprite brand-suzuki"></span>
                                <span class="brand-type-title">suzuki</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/triumph-bikes/">
                                <span class="brandlogosprite brand-triumph"></span>
                                <span class="brand-type-title">triumph</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/vespa-bikes/">
                                <span class="brandlogosprite brand-vespa"></span>
                                <span class="brand-type-title">vespa</span>
                            </a>
                        </li>
                        <li>
                            <a href="/m/yo-bikes/">
                                <span class="brandlogosprite brand-yo"></span>
                                <span class="brand-type-title">yo</span>
                            </a>
                        </li>

                    </ul>
                </div>
                <div class="text-center padding-bottom20">
                    <a href="javascript:void(0)" id="more-brand-tab" class="view-more-btn font16">View <span>more</span> Brands</a>
                </div>
            </div>
        </div>
        <div class="clear"></div>
    </section>

    <section> <!--  Upcoming, New Launches and Top Selling code starts here -->
        <div class="container <%= ((mctrlMostPopularBikes.FetchedRecordsCount + mctrlMostPopularBikes.FetchedRecordsCount + mctrlMostPopularBikes.FetchedRecordsCount) > 0 )?"":"hide" %> ">
            <div class="grid-12">
                <h2 class="text-center margin-top30 margin-bottom20">Discover your bike</h2>
                <div class="bw-tabs-panel">
                    <div class="bw-tabs margin-bottom15">
                        <div class="form-control-box">
                            <select class="form-control">
                                <option value="mctrlMostPopularBikes"  style="<%= (mctrlMostPopularBikes.FetchedRecordsCount > 0)?"":"display:none" %>">Most Popular</option>
                                <option value="mctrlNewLaunchedBikes"  style="<%= (mctrlNewLaunchedBikes.FetchedRecordsCount > 0)?"":"display:none" %>">New Launches</option>
                                <option value="mctrlUpcomingBikes"  style="<%= (mctrlUpcomingBikes.FetchedRecordsCount > 0)?"":"display:none" %>">Upcoming</option>
                            </select>
                        </div>
                    </div>
                    <div class="bw-tabs-data " id="mctrlMostPopularBikes">
                        <div class="jcarousel-wrapper discover-bike-carousel">
                            <div class="jcarousel">
                                <ul>
                                    <BW:MMostPopularBikes PageId="4" runat="server" ID="mctrlMostPopularBikes" />
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                            <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                            <p class="jcarousel-pagination text-center"></p>
                        </div>
                    </div>
                    <div class="bw-tabs-data hide" id="mctrlNewLaunchedBikes">
                        <div class="jcarousel-wrapper discover-bike-carousel">
                            <div class="jcarousel">
                                <ul>
                                    <BW:MNewLaunchedBikes PageId="4" runat="server" ID="mctrlNewLaunchedBikes" />
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                            <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                            <p class="text-center jcarousel-pagination"></p>
                        </div>
                    </div>
                    <div class="bw-tabs-data hide" id="mctrlUpcomingBikes">
                        <div class="jcarousel-wrapper upComingBikes">
                            <div class="jcarousel">
                                <ul>
                                    <BW:MUpcomingBikes runat="server" ID="mctrlUpcomingBikes" />
                                    <!-- Upcoming Bikes Control-->
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                            <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                            <p class="text-center jcarousel-pagination"></p>
                        </div>
                    </div>

                </div>
                <div class="clear"></div>
            </div>
        </div>
    </section>

    <section>
        
        <!--  Compare section code starts here -->
        <BW:CompareBike ID="ctrlCompareBikes" runat="server" />
        
    </section>

    <section class="container">
        <!-- Tools you may need code starts here -->
        <div class="grid-12">
            <h2 class="text-center margin-top30 margin-bottom20">Tools you may need</h2>
            <div class="tools-need-container margin-bottom30 text-center">
                <ul>
                    <li class="bg-white content-inner-block-20 content-box-shadow margin-bottom20">
                        <a href="/m/pricequote/">
                            <span class="tools-need-logo">
                                <span class="bwm-circle-icon getfinalprice-icon"></span>
                            </span>
                            <span class="tools-need-desc text-left">
                                <span class="font18 text-bold">Get final price</span>
                                <br>
                                <span class="font14 tools-need-text">Get final price without filling any form</span>
                            </span>
                        </a>
                    </li>
                    <li class="bg-white content-inner-block-20 content-box-shadow margin-bottom20">
                        <a href="/m/new/locate-dealers/">
                            <span class="tools-need-logo">
                                <span class="bwm-circle-icon locatedealer-icon"></span>
                            </span>
                            <span class="tools-need-desc text-left">
                                <span class="font18 text-bold">Locate dealer</span>
                                <br>
                                <span class="font14 tools-need-text">Find a dealer near your current location</span>
                            </span>
                        </a>
                    </li>
                    <li class="bg-white content-inner-block-20 content-box-shadow margin-bottom20 hide">
                        <a href="javascript:void(0)">
                            <span class="tools-need-logo">
                                <span class="bwm-circle-icon checkcarvalue-icon"></span>
                            </span>
                            <span class="tools-need-desc text-left">
                                <span class="font18 text-bold">Calculate EMI's</span>
                                <br>
                                <span class="font14 tools-need-text">Instant calculate loan EMI</span>
                            </span>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
        <div class="clear"></div>
    </section>

    <%
        if (ctrlNews.FetchedRecordsCount > 0)
        {
            reviewTabsCnt++;
            isNewsZero = false;
            isNewsActive = true;
        }
        if (ctrlExpertReviews.FetchedRecordsCount > 0)
        {
            reviewTabsCnt++;
            isExpertReviewZero = false;
            if (!isNewsActive)
            {
                isExpertReviewActive = true;
            }
        }
        if (ctrlVideos.FetchedRecordsCount > 0)
        {
            reviewTabsCnt++;
            isVideoZero = false;
            if (!isExpertReviewActive && !isNewsActive)
            {
                isVideoActive = true;
            }
        }
         %>
    <section class="container <%= reviewTabsCnt == 0 ? "hide" : "" %>">
            <!--  News, reviews and videos code starts here -->
            <div class="container">
                <div class="grid-12">
                    <h2 class="text-center margin-top30 margin-bottom20">Latest Updates</h2>
                    <div class="bw-tabs-panel">
                        <div class="bw-tabs margin-bottom15 <%= reviewTabsCnt == 1 ? "hide" : "" %>">
                            <div class="form-control-box">
                                <select class="form-control">
                                    <%if ((Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0)) {%> <option class="<%= isNewsActive ? "active" : "" %>" value="ctrlNews">News</option> <% } %>
                                    <%if ((Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0)) {%><option class="<%= isExpertReviewActive ? "active" : "" %>" value="ctrlExpertReviews">Expert Reviews</option> <% } %>
                                    <%if ((Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0)) {%><option class="<%= isVideoActive ? "active" : "" %>" value="ctrlVideos">Videos</option> <% } %>                                   
                                </select>
                            </div>
                        </div>
                         <%if (!isNewsZero) { %>         <BW:News runat="server" ID="ctrlNews" />    <% } %>
                        <%if (!isExpertReviewZero) { %> <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />  <% } %>                         
                        <%if (!isVideoZero) { %>        <BW:Videos runat="server" ID="ctrlVideos" />    <% } %>                       
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
    <!--  News, reviews and videos code ends here -->
    
    <BW:MPopupWidget runat="server" ID="MPopupWidget" />
    <!-- #include file="/includes/footerBW_Mobile.aspx" -->
    <!-- all other js plugins -->
    <!-- #include file="/includes/footerscript_Mobile.aspx" -->
    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/bwm-newbikes.js?<%= staticFileVersion %>"></script>
    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/chosen-jquery-min-mobile.js?<%= staticFileVersion %>"></script>
    <script type="text/javascript" >
            ga_pg_id = '4';
            if ('<%=isNewsActive%>' == "False") $("#ctrlNews").addClass("hide");
            if ('<%=isExpertReviewActive%>' == "False") $("#ctrlExpertReviews").addClass("hide");
            if ('<%=isVideoActive%>' == "False") $("#ctrlVideos").addClass("hide");    
     </script>
     </form>
</body>
</html>
