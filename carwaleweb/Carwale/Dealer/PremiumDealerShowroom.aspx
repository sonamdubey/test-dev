<%@ Page Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Dealer.PremiumDealerShowroom" Trace="false" %>

<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Import Namespace="Carwale.Utility" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
    <%
        // Define all the necessary meta-tags info here.
        // To know what are the available parameters,
        // check page, headNoAd.aspx in common folder.

        Title = dealerName + ":  Dealer Showroom";
        PageId = 10;
        Keywords = "Dealer contact detail , Showroom";
        Description = "Carwale UsedShowroom describes dealers contact details and the list of cars that dealer showroom contains.";
        canonical = "https://www.carwale.com/dealer/PremiumDealerShowroom.aspx?dealerId=" + dealerId;
    %>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <!--New Css file required for BBT Page only starts here-->
    <link rel="stylesheet" href="/static/css/premiumdealers.css" type="text/css" >
    <!--New Css file required for BBT Page only ends here-->
    <script type="text/javascript" src="https://st.aeplcdn.com/v2/src/jquery.jcarousel.min.js"></script>
    <!--Common Js and Css files required for BBT Page ends here-->
    <!--New Customize Js file required for BBT Page only starts here-->
    <script type="text/javascript">
        $(document).ready(function (e) {
            //tabs code js starts here
            var latitude = '<%=lattitude %>';
            var longitude = '<%= longitude%>';
            Common.utils.loadGoogleApi(postGoogleApiLoad, { latitude: latitude, longitude: longitude });

            function postGoogleApiLoad(input) {

                if (input.latitude > 0 && input.longitude > 0) {
                    $("#divMapWindow").removeClass("hide");
                    $("#divMap").addClass("map-box");
                    initialize();
                    google.maps.event.addDomListener(window, 'load', initialize);
                }
            };

            function initialize() {
                var myCenter = new google.maps.LatLng(latitude, longitude);
                var mapProp = {
                    center: myCenter,
                    zoom: 16,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };

                var map = new google.maps.Map(document.getElementById("divMap"), mapProp);

                var marker = new google.maps.Marker({
                    position: myCenter,
                });

                marker.setMap(map);
            }
            //Hiding the 'Prev' 'next' buttons If the stock is less than 4
            if ($("#authorCarousel").children().length <= 4) { $("#prev_next").hide() };

            //tabs code js starts here
            $(".cw-tabs li").click(function () {
                var panel = $(this).closest(".panel-group");
                panel.find(".cw-tabs li").removeClass("active").removeClass("activeli");
                $(this).addClass("active").addClass("activeli");

                var panelId = $(this).attr("data-id");
                panel.find(".cw-data").hide();
                $("#" + panelId).show();
            });
            //tabs code js ends here

            // Tab1 carousel Js starts here
            $('#tab1Carousel').jcarousel({
                scroll: 1,
                auto: 0,
                animation: 800,
                wrap: "circular",
                initCallback: initCallbackUCRC, buttonNextHTML: null, buttonPrevHTML: null
            });
            function initCallbackUCRC(carousel) {
                $('#list_carousel_tab1_prev').bind('click', function () {
                    carousel.prev();
                    return false;
                });
                $('#list_carousel_tab1_next').bind('click', function () {
                    carousel.next();
                    return false;
                });
            };
            // Tab1 carousel Js ends here

            // Tab2 carousel Js starts here
            $('#tab2Carousel').jcarousel({
                scroll: 1,
                auto: 0,
                animation: 800,
                wrap: "circular",
                initCallback: initCallbackUCRB, buttonNextHTML: null, buttonPrevHTML: null
            });
            function initCallbackUCRB(carousel) {
                $('#list_carousel_tab2_prev').bind('click', function () {
                    carousel.prev();
                    return false;
                });
                $('#list_carousel_tab2_next').bind('click', function () {
                    carousel.next();
                    return false;
                });
            };
            // Tab2 carousel Js ends here

            // Tab3 carousel Js starts here
            $('#tab3Carousel').jcarousel({
                scroll: 1,
                auto: 0,
                animation: 800,
                wrap: "circular",
                initCallback: initCallbackUCRU, buttonNextHTML: null, buttonPrevHTML: null
            });
            function initCallbackUCRU(carousel) {
                $('#list_carousel_tab3_prev').bind('click', function () {
                    carousel.prev();
                    return false;
                });
                $('#list_carousel_tab3_next').bind('click', function () {
                    carousel.next();
                    return false;
                });
            };
            // Tab3 carousel Js ends here

            //Top Big Image carousel Js starts here
            $('#topCarousel').jcarousel({
                scroll: 1,
                auto: 1,
                animation: 1000,
                wrap: "circular",
                initCallback: initCallbackUCRA, buttonNextHTML: null, buttonPrevHTML: null
            });
            function initCallbackUCRA(carousel) {
                $('#list_carousel_top_prev').bind('click', function () {
                    carousel.prev();
                    return false;
                });
                $('#list_carousel_top_next').bind('click', function () {
                    carousel.next();
                    return false;
                });
            };
            //Top Big Image carousel Js ends here

            //Cars in Stock carousel Js starts here
            $('#authorCarousel').jcarousel({
                scroll: 1,
                auto: 0,
                animation: 800,
                wrap: "circular",
                initCallback: initCallbackUCR, buttonNextHTML: null, buttonPrevHTML: null
            });
            function initCallbackUCR(carousel) {
                $('#list_carousel_widget_prev').bind('click', function () {
                    carousel.prev();
                    return false;
                });
                $('#list_carousel_widget_next').bind('click', function () {
                    carousel.next();
                    return false;
                });
            };
            //Cars in Stock carousel Js ends here

        });
    </script>
    <!--New Customize Js file required for BBT Page only ends here-->
</head>
<body class="header-fixed-inner">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <section class="padding-top10 padding-bottom20">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul class="special-skin-text">
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/used/">Used Cars</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/used/dealers-in-<%= UrlRewrite.FormatSpecial(dealerCity)%>/">Dealers in <%= dealerCity %></a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><%=dealerName%></li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
                <!-- BBT UI Code starts here-->
                <div class="grid-12 cwd-bbt margin-bottom20">
                    <!-- Top Banner code starts here-->
                    <div class="content-box-shadow">
                        <!-- Top Banner Middle content code starts here-->
                        <div class="dlp-cw-carousel">
                            <div class="main-carousel-div">
                                <div class="list_carousel_top">
                                    <div class="jcarousel-container">
                                        <div>
                                            <div id="prev_next_top" class="rightfloat arrow-position">
                                                <a class="prev" id="list_carousel_top_prev"></a>
                                                <a class="next" id="list_carousel_top_next"></a>
                                            </div>
                                            <div class="clear"></div>
                                        </div>
                                        <div>
                                            <ul class="jcarousel-list" id="topCarousel">
                                                <li><a href="#">
                                                    <img border="0" alt="Big Boy Toyz" title="" src="<%= Carwale.Utility.CWConfiguration._imgHostUrl %>0x0/cw/used-cars/banner1.jpg"></a></li>
                                                <%--<li><a href="#"><img border="0" alt="Big Boy Toyz" title=""  src="<%= ImageSizes.CreateImageUrl(ConfigurationManager.AppSettings["CDNHostURL"].ToString(),ImageSizes._891X501,"/images/used/luxuryshowrooms/bbt/banner1.jpg")%>"></a></li>--%>
                                                <li><a href="#">
                                                    <img border="0" alt="Big Boy Toyz" title="" src="<%= Carwale.Utility.CWConfiguration._imgHostUrl %>0x0/cw/used-cars/banner2.jpg"></a></li>
                                                <li><a href="#">
                                                    <img border="0" alt="Big Boy Toyz" title="" src="<%= Carwale.Utility.CWConfiguration._imgHostUrl %>0x0/cw/used-cars/banner3.jpg"></a></li>
                                                <li><a href="#">
                                                    <img border="0" alt="Big Boy Toyz" title="" src="<%= Carwale.Utility.CWConfiguration._imgHostUrl %>0x0/cw/used-cars/banner4.jpg"></a></li>
                                                <%--<li><a href="#"><img border="0" alt="Big Boy Toyz" title=""  src="<%= ImageSizes.CreateImageUrl(ConfigurationManager.AppSettings["CDNHostURL"].ToString(),ImageSizes._891X501,"/images/used/luxuryshowrooms/bbt/banner2.jpg")%>"></a></li>--%>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <!-- Top Banner Middle content code ends here-->
                        <div class="clear"></div>
                    </div>
                    <div class="clear"></div>
                    <!--  Top Banner ends starts here-->
                </div>
                <div class="grid-12 cwd-bbt margin-bottom20">
                    <!-- Tabing section code starts here -->
                    <div class="panel-group content-box-shadow">
                        <div class="cw-tabs">
                            <ul>
                                <li data-id="tab1" class="active">Reasons to Buy
                                	<div class="border-right"></div>
                                    <div class="activeli activeli-1">
                                        <div class="active-border"></div>
                                        <div class="active-arrow"></div>
                                    </div>
                                </li>
                                <li data-id="tab2">Stars at BBT
                                	    <div class="border-right"></div>
                                    <div class="activeli activeli-2">
                                        <div class="active-border"></div>
                                        <div class="active-arrow"></div>
                                    </div>
                                </li>
                                <%--<li data-id="tab3">Events
                                	    <div class="activeli activeli-3">
                                        <div class="active-border"></div>
                                        <div class="active-arrow"></div>
                                    </div>
                                </li>--%>
                            </ul>
                            <div class="clear"></div>
                        </div>
                        <div class="clear"></div>
                        <!-- Tabing section Middle content code starts here -->
                        <div class="content-inner-block-10 bordertop">
                            <div class="dlp-cw-carousel">
                                <div class="main-carousel-div">
                                    <div class="cw-data list_carousel_tab1 " id="tab1">
                                        <div class="jcarousel-container">
                                            <div>
                                                <div>
                                                    <div id="prev_next_tab1" class="rightfloat arrow-position">
                                                        <a class="prev" id="list_carousel_tab1_prev"></a>
                                                        <a class="next" id="list_carousel_tab1_next"></a>
                                                    </div>
                                                    <div class="clear"></div>
                                                </div>
                                                <div>
                                                    <ul class="jcarousel-list" id="tab1Carousel">
                                                        <li>
                                                            <div class="leftfloat alignStarsTab" style="width: 499px">
                                                                Purchasing your car from Big Boy Toyz ensures not only guarantee on the best maintained cars but it is the starting of a qualitative mutual relationship. It is only after a very strict History, Service and Document check does a car become a part of the BBT Hangar.
                                                            </div>
                                                            <div class="rightfloat">
                                                                <img border="0" alt="" title="" src="https://img2.aeplcdn.com/images/used/luxuryshowrooms/bbt/10_Reasons.jpg" style="width: 405px;">
                                                                <%--<img border="0" alt="" title=""  src="<%= ImageSizes.CreateImageUrl(ConfigurationManager.AppSettings["CDNHostURL"].ToString(),ImageSizes._393X221,"/images/used/luxuryshowrooms/bbt/10_Reasons.jpg") %>" />--%>
                                                            </div>
                                                            <div class="clear"></div>
                                                        </li>
                                                        <li>
                                                            <h3>NCR-</h3>
                                                            <div class="leftfloat margin-top10">
                                                                Every car goes through a National Crime Record check at the time of procurement to eliminate any possibility of purchasing a car with an NCR case. Service History – A detailed record of Service History which includes body, engine, chassis, accidental repairs & any and all servicing and maintenance work done on the car is provided to you.
                                                            </div>
                                                            <div class="rightfloat">
                                                                <!--<img border="0" alt="" title=""  src="https://img1.aeplcdn.com/cars/530ub.jpg?v=1">-->
                                                            </div>
                                                            <div class="clear"></div>
                                                        </li>
                                                        <li>
                                                            <h3>Insurance History</h3>
                                                            <div class="leftfloat margin-top10">
                                                                BBT procurement team does a thorough study on the ongoing and previous insurance history certify insurance claims till date (if any).
                                                            </div>
                                                            <div class="rightfloat">
                                                                <!--<img border="0" alt="" title=""  src="https://img1.aeplcdn.com/cars/530ub.jpg?v=1">-->
                                                            </div>
                                                            <div class="clear"></div>
                                                        </li>
                                                        <li>
                                                            <h3>Buy Back Agreement</h3>
                                                            <div class="leftfloat margin-top10">
                                                                Big Boy Toyz enjoys a staunch customer loyalty, courtesy our Buy Back Agreement empowering you with a program that allows for re-selling of your car, back to us with a mere depreciation of 25%.
                                                            </div>
                                                            <div class="rightfloat">
                                                                <!--<img border="0" alt="" title=""  src="https://img1.aeplcdn.com/cars/530ub.jpg?v=1">-->
                                                            </div>
                                                            <div class="clear"></div>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="cw-data list_carousel_tab2 hide" id="tab2">
                                        <div class="jcarousel-container">
                                            <div>
                                                <div>
                                                    <div id="prev_next_tab2" class="rightfloat arrow-position">
                                                        <a class="prev" id="list_carousel_tab2_prev"></a>
                                                        <a class="next" id="list_carousel_tab2_next"></a>
                                                    </div>
                                                    <div class="clear"></div>
                                                </div>
                                                <div>
                                                    <!--<div><a href="http://www.bigboytoyz.com/stars-at-bbt.html"></a></div>-->
                                                    <ul class="jcarousel-list" id="tab2Carousel">
                                                        <li>
                                                            <div class="leftfloat alignStarsTab">
                                                                <h3>Yo Yo Honey Singh</h3>
                                                                <div class="margin-top10">
                                                                    Honey Singh, better known by his stage name Yo Yo Honey Singh, is an Indian rapper, music producer, singer and film actor. He started as a session and recording artist, and became a Bhangra producer. This modern day Prince of Punjabi Rap and Hip Hop has had record after record hits such as Sunny Sunny, Blue Eyes, Brown Rang and hundreds more.
                                                                </div>
                                                            </div>
                                                            <div class="rightfloat">
                                                                <img border="0" alt="" title="" src="https://img1.aeplcdn.com/images/used/luxuryshowrooms/bbt/star1.jpg?v=1">
                                                                <%--<img border="0" alt="" title=""  src="<%= ImageSizes.CreateImageUrl(ConfigurationManager.AppSettings["CDNHostURL"].ToString(),ImageSizes._227X128,"/images/used/luxuryshowrooms/bbt/star1.jpg?v=1")%>">--%>
                                                            </div>
                                                            <div class="clear"></div>
                                                        </li>
                                                        <li>
                                                            <div class="leftfloat alignStarsTab">
                                                                <h3>Neil Nitin Mukesh</h3>
                                                                <div class="margin-top10">
                                                                    Neil Nitin Mukesh is an Indian actor who appears in Bollywood movies. You know him from hits such as 3G, Players and Jail.
                                                                </div>
                                                            </div>
                                                            <div class="rightfloat">
                                                                <img border="0" alt="" title="" src="https://img2.aeplcdn.com/images/used/luxuryshowrooms/bbt/Neil-Nitin-Mukesh.jpg?v=1">
                                                                <%--<img border="0" alt="" title=""  src="<%= ImageSizes.CreateImageUrl(ConfigurationManager.AppSettings["CDNHostURL"].ToString(),ImageSizes._227X128,"/images/used/luxuryshowrooms/bbt/Neil-Nitin-Mukesh.jpg?v=1")%>">--%>
                                                            </div>
                                                            <div class="clear"></div>
                                                        </li>
                                                        <li>
                                                            <div class="leftfloat alignStarsTab">
                                                                <h3>Rajeev Makhani</h3>
                                                                <div class="margin-top10">
                                                                    Rajiv Makhani is an Indian technology journalist, analyst and television presenter. He was recently awarded The Television Anchor of the year 2012 by the Indian Television Academy. You would instantly recognize him from Tech Grand Master.
                                                                </div>
                                                            </div>
                                                            <div class="rightfloat">
                                                                <img border="0" alt="" title="" src="https://img1.aeplcdn.com/images/used/luxuryshowrooms/bbt/Rajeev-Makhani.jpg?v=1">
                                                                <%--<img border="0" alt="" title=""  src="<%= ImageSizes.CreateImageUrl(ConfigurationManager.AppSettings["CDNHostURL"].ToString(),ImageSizes._227X128,"/images/used/luxuryshowrooms/bbt/Rajeev-Makhani.jpg?v=1")%>">--%>
                                                            </div>
                                                            <div class="clear"></div>
                                                        </li>
                                                        <li>
                                                            <div class="leftfloat alignStarsTab">
                                                                <h3>Harris Jayaraj</h3>
                                                                <div class="margin-top10">
                                                                    Harris Jayaraj is an Indian film composer from Chennai, Tamil Nadu. He has written scores and soundtracks for Tamil, Telugu and Hindi films winning the following awards Filmfare Award for Best Music Director – Tamil, Vijay Award for Favourite Song, Vijay Award for Best Music Director.
                                                                </div>
                                                            </div>
                                                            <div class="rightfloat">
                                                                <img border="0" alt="" title="" src="https://img2.aeplcdn.com/images/used/luxuryshowrooms/bbt/Harris-Jayaraj.jpg?v=1">
                                                                <%--<img border="0" alt="" title=""  src="<%= ImageSizes.CreateImageUrl("img2.aeplcdn.com",ImageSizes._227X128,"/images/used/luxuryshowrooms/bbt/Harris-Jayaraj.jpg?v=1")%>">--%>
                                                            </div>
                                                            <div class="clear"></div>
                                                        </li>
                                                        <li>
                                                            <div class="leftfloat alignStarsTab">
                                                                <h3>Jackie Shroff</h3>
                                                                <div class="margin-top10">
                                                                    Jackie Shroff is an Indian actor. He has been in the Hindi cinema industry for almost 3 decades and has appeared in over 151 films in nine languages. His most famous works being Khalnayak, Ram Lakhan, Dhoom3, Saudagar and Border.
                                                                </div>
                                                            </div>
                                                            <div class="rightfloat">
                                                                <img border="0" alt="" title="" src="https://img1.aeplcdn.com/images/used/luxuryshowrooms/bbt/Jackie-Shroff.jpg?v=1">
                                                                <%--<img border="0" alt="" title=""  src="<%= ImageSizes.CreateImageUrl("img1.aeplcdn.com",ImageSizes._227X128,"/images/used/luxuryshowrooms/bbt/Jackie-Shroff.jpg?v=1")%>">--%>
                                                            </div>
                                                            <div class="clear"></div>
                                                        </li>
                                                        <li>
                                                            <div class="leftfloat alignStarsTab">
                                                                <h3>Piyush Chawla</h3>
                                                                <div class="margin-top10">
                                                                    Piyush Pramod Chawla; born 24 December 1988) is an Indian cricketer who plays for the Indian national cricket team. Currently he plays for Kings XI Punjab in the Indian Premiere League.
                                                                </div>
                                                            </div>
                                                            <div class="rightfloat">
                                                                <!--<img border="0" alt="" title=""  src="/images/star1.jpg?v=1">-->
                                                            </div>
                                                            <div class="clear"></div>
                                                        </li>
                                                        <li>
                                                            <div class="leftfloat alignStarsTab">
                                                                <h3>Alfaaz</h3>
                                                                <div class="margin-top10">
                                                                    Alfaaz is a famous Punjabi Singer who made his name from the song- Haye Mera Dil.
                                                                </div>
                                                            </div>
                                                            <div class="rightfloat">
                                                                <img border="0" alt="" title="" src="https://img1.aeplcdn.com/images/used/luxuryshowrooms/bbt/Alfaaz.jpg?v=1">
                                                                <%--<img border="0" alt="" title=""  src="<%= ImageSizes.CreateImageUrl(ConfigurationManager.AppSettings["CDNHostURL"].ToString(),ImageSizes._227X128,"/images/used/luxuryshowrooms/bbt/Alfaaz.jpg?v=1")%>">--%>
                                                            </div>
                                                            <div class="clear"></div>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="cw-data list_carousel_tab3 hide" id="tab3">
                                        <div class="jcarousel-container">
                                            <div>
                                                <div>
                                                    <div id="prev_next_tab3" class="rightfloat arrow-position">
                                                        <a class="prev" id="list_carousel_tab3_prev"></a>
                                                        <a class="next" id="list_carousel_tab3_next"></a>
                                                    </div>
                                                    <div class="clear"></div>
                                                </div>
                                                <!--<div>
                                                    <ul class="jcarousel-list" id="tab3Carousel">
                                                        <li>
                                            	            <div class="leftfloat">tab333 content1</div>
                                                            <div class="rightfloat">
                                                             <img border="0" alt="" title=""  src="https://img1.aeplcdn.com/cars/530ub.jpg?v=1">
                                                            </div>
                                                            <div class="clear"></div>
                    						            </li>
                                                        <li>
                                            	            <div class="leftfloat">tab3333 content2</div>
                                                            <div class="rightfloat">
                                                             <img border="0" alt="" title=""  src="https://img1.aeplcdn.com/cars/530ub.jpg?v=1">
                                                            </div>
                                                            <div class="clear"></div>
                    						            </li>
                                                         <li>
                                            	            <div class="leftfloat">tab3333 content3</div>
                                                            <div class="rightfloat">
                                                             <img border="0" alt="" title=""  src="https://img1.aeplcdn.com/cars/530ub.jpg?v=1">
                                                            </div>
                                                            <div class="clear"></div>
                    						            </li>
                                                         <li>
                                            	            <div class="leftfloat">tab3333 content4</div>
                                                            <div class="rightfloat">
                                                             <img border="0" alt="" title=""  src="https://img1.aeplcdn.com/cars/530ub.jpg?v=1">
                                                            </div>
                                                            <div class="clear"></div>
                    						            </li>
                                                    </ul>
                                                </div>-->
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                        <!-- Tabing section Middle content code ends here-->
                        <div class="clear"></div>
                    </div>
                    <div class="clear"></div>
                    <!--  Tabing section ends starts here-->
                </div>

                <div class="grid-12 margin-bottom20 cwd-bbt">
                    <!-- Cars in Stock code starts here-->
                    <div class="content-box-shadow">
                        <div>
                            <div class="content-inner-block-10">
                                <h2>Cars in Stock</h2>
                            </div>
                            <div id="prev_next" class="rightfloat arrow-position">
                                <a class="prev" id="list_carousel_widget_prev"></a>
                                <a class="next" id="list_carousel_widget_next"></a>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="clear"></div>
                        <!--Cars in Stock Middle content code starts here-->
                        <div class="content-inner-block-10">
                            <div class="dlp-cw-carousel">
                                <div class="main-carousel-div">
                                    <div class="list_carousel_widget">
                                        <div class="jcarousel-container">
                                            <div>
                                                <ul class="jcarousel-list" id="authorCarousel">
                                                    <asp:Repeater ID="rptStock" runat="server">
                                                        <ItemTemplate>
                                                            <li>
                                                                <div class="margin-bottom10 imgheight">
                                                                    <a href="http://<%#ConfigurationManager.AppSettings["HostUrl"].ToString()%>/used/cars-in-<%#Format.RemoveSpecialCharacters(Eval("CityName").ToString())%>/<%#Format.RemoveSpecialCharacters(Eval("MakeName").ToString())%>-<%#Eval("MaskingName").ToString()%>-<%#Format.RemoveSpecialCharacters(Eval("ProfileId").ToString())%>/" target="_blank">
                                                                        <img border="0" alt="<%#Eval("MakeName") %>" title="<%#Eval("MakeName") %>" src="<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString()) ?  "https://img.carwale.com/used/no-cars.jpg": DataBinder.Eval(Container.DataItem, "HostUrl").ToString()+ImageSizes._310X174+DataBinder.Eval(Container.DataItem, "OriginalImgPath")%>" />
                                                                    </a>
                                                                </div>
                                                                <h3 class="margin-bottom10"><a title="<%#Eval("Car") %>" href="http://<%#ConfigurationManager.AppSettings["HostUrl"].ToString()%>/used/cars-in-<%#Format.RemoveSpecialCharacters(Eval("CityName").ToString())%>/<%#Format.RemoveSpecialCharacters(Eval("MakeName").ToString())%>-<%#Eval("MaskingName").ToString()%>-<%#Format.RemoveSpecialCharacters(Eval("ProfileId").ToString())%>/" target="_blank"><%#Eval("Car") %></a></h3>
                                                                <div class="border-dotted margin-bottom10"></div>
                                                                <div>
                                                                    <div class="leftfloat">₹ <span><%# CommonOpn.FormatNumeric(Eval("Price").ToString())%></span></div>
                                                                    <div class="rightfloat"><span class="cw-bbt-sprite cw-kms-icon"></span><span>kms <%#Eval("Kilometers") %></span></div>
                                                                    <div class="clear"></div>
                                                                </div>
                                                                <div class="border-dotted margin-top10 margin-bottom10"></div>
                                                                <div>
                                                                    <a href="http://<%#ConfigurationManager.AppSettings["HostUrl"].ToString()%>/used/cars-in-<%#Format.RemoveSpecialCharacters(Eval("CityName").ToString())%>/<%#Format.RemoveSpecialCharacters(Eval("MakeName").ToString())%>-<%#Eval("MaskingName").ToString()%>-<%#Format.RemoveSpecialCharacters(Eval("ProfileId").ToString())%>/" target="_blank">View More Details
                                                                    </a>
                                                                </div>
                                                            </li>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                        <!--Cars in Stock Middle content code ends here-->
                        <div class="clear"></div>
                    </div>
                    <div class="clear"></div>
                    <!-- Cars in Stock ends starts here-->
                </div>
                <div class="grid-12 margin-bottom20">
                    <!-- google Map code starts here -->
                    <div class="content-box-shadow">
                        <div class="content-inner-block-10">
                            <h2>Reach Us here</h2>
                        </div>
                        <!-- google Map section Middle content code starts here-->
                        <div class="googlemap margin-bottom10">
                            <div class="grid-8" id="divMapWindow">
                                <!--<iframe width="560" height="280" frameborder="0" style="border:0" src="https://maps.google.com/maps?f=q&amp;source=s_q&amp;hl=en&amp;geocode=&amp;q=CM+MOTORS,+Ahmedabad,+Gujarat,+India&amp;aq=0&amp;oq=cm+motors+ah&amp;sll=37.0625,-95.677068&amp;sspn=34.861942,86.572266&amp;ie=UTF8&amp;hq=CM+MOTORS,+Ahmedabad,+Gujarat,+India&amp;ll=23.039568,72.566004&amp;spn=0.039567,0.084543&amp;t=m&amp;z=14&amp;iwloc=A&amp;cid=4180321332648923293&amp;output=embed"></iframe>-->
                                <div class="leftfloat cwd-googlemap-l" id="divMap" style="width: 600px; height: 258px;"></div>
                            </div>

                            <div class="grid-4">
                                <div class="">
                                    <h3>Get In Touch</h3>
                                    <div class="margin-top10">
                                        <!--391, Opp. Metro Pillar No. 115-->
                                        <!--M.G Road, Ghitorni, New Delhi 110030-->
                                        <% =dealerAddress1 %>, <% =dealerAddress2 %>
                                        <% =dealerArea %>, <% =dealerCity %>, <% =dealerState %>, PinCode:<% =dealerPincode %>
                                    </div>                                  
                                    <div class="margin-top5">
                                        <!--sales@bigboytoyz.com-->
                                        <% =dealerEmail %></br>
                                    </div>
                                    <a href="<%= dealerWebsiteUrl.IndexOf("http", StringComparison.OrdinalIgnoreCase) == 0 ? dealerWebsiteUrl :"http://" + dealerWebsiteUrl%>" target="_blank">
                                        <div class="margin-top5">
                                            <% =dealerWebsiteUrl %></br>
                                        </div>
                                    </a>
                                </div>
                                <div class="margin-top30">
                                    <h3>Follow Big Boy Toyz</h3>
                                    <div class="margin-top10">
                                        <div class="leftfloat margin-right10">
                                            <a href="https://www.facebook.com/BBToyz" target="_blank"><span class="cw-bbt-sprite cw-bbt-fb"></span></a>
                                        </div>
                                        <div class="leftfloat margin-right10">
                                            <a href="https://twitter.com/BigBoyToyz" target="_blank"><span class="cw-bbt-sprite cw-bbt-tw"></span></a>
                                        </div>
                                        <div class="leftfloat margin-right10">
                                            <a href="https://www.youtube.com/user/bigboytoyzindia" target="_blank"><span class="cw-bbt-sprite cw-bbt-yt"></span></a>
                                        </div>
                                        <div class="leftfloat margin-right10">
                                            <a href="https://plus.google.com/+BigBoyToyzNewDelhi/about?hl=en" target="_blank"><span class="cw-bbt-sprite cw-bbt-gp"></span></a>
                                        </div>
                                        <div class="leftfloat margin-right10">
                                            <a href="https://instagram.com/bigboytoyz_india" target="_blank"><span class="cw-bbt-sprite cw-bbt-ig"></span></a>
                                        </div>
                                        <div class="leftfloat">
                                            <a href="https://www.pinterest.com/bbtindia/" target="_blank"><span class="cw-bbt-sprite cw-bbt-pi"></span></a>
                                        </div>

                                        <div class="clear"></div>
                                    </div>
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <!-- google Map section Middle content code ends here-->
                        <div class="clear"></div>
                    </div>
                    <div class="clear"></div>
                    <!--  google Map section ends starts here -->
                </div>
                <!-- BBT UI  Code ends here -->
            </div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
        <link rel="stylesheet" href="/static/css/premiumdealers.css" type="text/css" >
        <!--New Css file required for BBT Page only ends here-->
        <script  type="text/javascript"  src="/static/src/jquery.jcarousel.min.js" ></script>
    </form>
</body>
</html>