<%@ Page Language="C#" AutoEventWireup="true" Inherits="Bikewale.dealerlocator.dealerlisting" %>

<!DOCTYPE html>
<html>
<head>
    <%
        isAd970x90Shown = false;
    %>
    <title></title>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/dealerlisting.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDjG8tpNdQI86DH__-woOokTaknrDQkMC8"></script>
</head>
<body class="bg-light-grey padding-top50">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="opacity0 grid-12 padding-right20 padding-left20">
                <div class="breadcrumb">
                    <!-- breadcrumb code starts here -->
                    <ul>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a href="/" itemprop="url">
                                <span itemprop="title">Home</span>
                            </a>
                        </li>
                        <li><span class="fa fa-angle-right margin-right10"></span>Dealer locator</li>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section>
            <div class="dealer-filter-dropdown grid-11 text-center margin-left30">
                <div id="locateDealerFilter" class="box-light-shadow bg-white content-inner-block-12">
                    <p class="font16 leftfloat margin-top8 margin-right20">Locate dealers:</p>
                    <div class="leftfloat margin-right10">
                        <div class="brand-city-filter form-control-box">
                            <div class="placeholder-loading-text form-control">Loading brands...<span class="fa fa-spinner fa-spin position-abt text-black btnSpinner"></span></div>
                            <select class="chosen-select"></select>
                            <span class="bwsprite error-icon error-tooltip-siblings"></span>
                            <div class="bw-blackbg-tooltip error-tooltip-siblings"></div>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <div class="leftfloat margin-right10">
                        <div class="brand-city-filter form-control-box">
                            <div class="placeholder-loading-text form-control">Loading city...<span class="fa fa-spinner fa-spin position-abt text-black btnSpinner"></span></div>
                            <select class="chosen-select"></select>
                            <span class="bwsprite error-icon error-tooltip-siblings"></span>
                            <div class="bw-blackbg-tooltip error-tooltip-siblings"></div>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <input type="button" id="applyFiltersBtn" class="btn btn-orange leftfloat" value="Apply" />
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>   
        </section>

        <section>
            <div class="grid-12 alpha omega">
                
                <div id="dealerListingSidebar" class="bg-white position-abt pos-right0">
                    <div class="dealerSidebarHeading padding-top15 padding-right20 padding-left20">
                        <h1 id="sidebarHeader" class="font16 border-solid-bottom padding-bottom15">Hero dealers in Mumbai <span class="font14 text-light-grey">(4)</span></h1>
                    </div>
                    <ul id="dealersList">
                        <li data-dealer-id="10">
                            <div class="font14">
                                <h2 class="font16 margin-bottom10">
                                    <span class="featured-tag text-white text-center font14 margin-bottom5">
                                        Featured
                                    </span>
                                    <span class="dealer-pointer-arrow"></span>
                                    <a href="javascript:void(0)" class="dealer-sidebar-link text-black text-bold">Dealer 1</a>
                                </h2>
                                <p class="text-light-grey margin-bottom5">Andheri, Mumbai</p>
                                <p class="text-light-grey margin-bottom5"><span class="bwsprite phone-grey-icon"></span>9876543210</p>
                                <a href="mailto:bikewale@motors.com" class="text-light-grey"><span class="bwsprite mail-grey-icon"></span>bikewale@motors.com</a>
                                <input type="button" class="btn btn-white-orange margin-top15 get-assistance-btn" value="Get assistance">
                            </div>
                        </li>
                        <li data-dealer-id="21">
                            <div class="font14">
                                <h2 class="font16 margin-bottom10"><a href="javascript:void(0)" class="dealer-sidebar-link text-black text-bold">Dealer 2</a></h2>
                                <p class="text-light-grey margin-bottom5">Andheri, Mumbai</p>
                                <p class="text-light-grey margin-bottom5"><span class="bwsprite phone-grey-icon"></span>9876543210</p>
                                <a href="mailto:bikewale@motors.com" class="text-light-grey"><span class="bwsprite mail-grey-icon"></span>bikewale@motors.com</a>
                                <input type="button" class="btn btn-white-orange margin-top15 get-assistance-btn" value="Get assistance">
                            </div>
                        </li>
                        <li data-dealer-id="32">
                            <div class="font14">
                                <h2 class="font16 margin-bottom10"><a href="javascript:void(0)" class="dealer-sidebar-link text-black text-bold">Dealer 3</a></h2>
                                <p class="text-light-grey margin-bottom5">Andheri, Mumbai</p>
                                <p class="text-light-grey margin-bottom5"><span class="bwsprite phone-grey-icon"></span>9876543210</p>
                                <a href="mailto:bikewale@motors.com" class="text-light-grey"><span class="bwsprite mail-grey-icon"></span>bikewale@motors.com</a>
                                <input type="button" class="btn btn-white-orange margin-top15 get-assistance-btn" value="Get assistance">
                            </div>
                        </li>
                        <li data-dealer-id="43">
                            <div class="font14">
                                <h2 class="font16 margin-bottom10"><a href="javascript:void(0)" class="dealer-sidebar-link text-black text-bold">Dealer 4</a></h2>
                                <p class="text-light-grey margin-bottom5">Andheri, Mumbai</p>
                                <p class="text-light-grey margin-bottom5"><span class="bwsprite phone-grey-icon"></span>9876543210</p>
                                <a href="mailto:bikewale@motors.com" class="text-light-grey"><span class="bwsprite mail-grey-icon"></span>bikewale@motors.com</a>
                                <input type="button" class="btn btn-white-orange margin-top15 get-assistance-btn" value="Get assistance">
                            </div>
                        </li>
                        <li data-dealer-id="54">
                            <div class="font14">
                                <h2 class="font16 margin-bottom10"><a href="javascript:void(0)" class="dealer-sidebar-link text-black text-bold">Dealer 5</a></h2>
                                <p class="text-light-grey margin-bottom5">Andheri, Mumbai</p>
                                <p class="text-light-grey margin-bottom5"><span class="bwsprite phone-grey-icon"></span>9876543210</p>
                                <a href="mailto:bikewale@motors.com" class="text-light-grey"><span class="bwsprite mail-grey-icon"></span>bikewale@motors.com</a>
                                <input type="button" class="btn btn-white-orange margin-top15 get-assistance-btn" value="Get assistance">
                            </div>
                        </li>
                        <li class="dummy-card">
                        </li>
                    </ul>
                </div>

                <div id="dealerDetailsSliderCard" class="bg-white font14">
                    <div class="dealer-slider-close-btn position-abt pos-top20 pos-right20 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                    <div class="padding-top20 padding-right20 padding-left20">
                        <p class="featured-tag text-white text-center margin-bottom5">
                            Featured
                        </p>
                        <div class="padding-bottom20 border-solid-bottom">
                            <h3 class="font18 text-dark-black margin-bottom10">Kamala Landmarc Motorbikes</h3>
                            <p class="text-light-grey margin-bottom5">Vishwaroop IT Park, Sector 30, Navi Mumbai, Maharashtra, 400067</p>
                            <div class="margin-bottom5">
                                <span class="font16 text-bold margin-right10"><span class="bwsprite phone-black-icon"></span>9876543210</span>
                                <a href="mailto:bikewale@motors.com" class="text-light-grey"><span class="bwsprite mail-grey-icon"></span>bikewale@motors.com</a>
                            </div>
                            <p class="text-light-grey margin-bottom5">Working hours: Monday- Saturday 9.00 am- 6.00 pm<span class="border-dark-left margin-left10 padding-left10">Sunday 9.00 am- 2.00 pm</span></p>
                            <a href=""><span class="bwsprite get-direction-icon"></span> Get directions</a>
                            <a href="" class="border-dark-left margin-left10 padding-left10"><span class="bwsprite sendto-phone-icon"></span> Send to phone</a>
                        </div>
                        <div class="padding-top15">
                            <p class="font14 text-bold margin-bottom15">Get commute distance and time:</p>
                            <div class="commute-distance-form form-control-box">
                                <input type="text" class="form-control" placeholder="Enter your location" />
                            </div>
                        </div>
                    </div>
                    <div id="buyingAssistanceForm" class="margin-top20 content-inner-block-1520">
                        <p class="font14 text-bold margin-bottom15">Get buying assistance from this dealer:</p>
                        <div class="name-email-mobile-box form-control-box leftfloat margin-right20">
                            <input type="text" class="form-control" placeholder="Name" id="assistGetName" />
                            <span class="bwsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="name-email-mobile-box form-control-box leftfloat margin-right40">
                            <input type="text" class="form-control" placeholder="Email id" id="assistGetEmail" />
                            <span class="bwsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="name-email-mobile-box form-control-box leftfloat">
                            <p class="mobile-prefix">+91</p>
                            <input type="text" class="mobile-box form-control" placeholder="Mobile" maxlength="10" id="assistGetMobile" />
                            <span class="bwsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="clear"></div>
                        <div class="margin-top20">
                            <div class="select-model-box form-control-box leftfloat margin-right40">
                                <input type="text" class="form-control" placeholder="Type to select model" id="assistGetModel" />
                                <span class="bwsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <input type="button" class="btn btn-orange btn-md" id="submitAssistanceFormBtn" value="Submit" />
                        </div>
                        <div class="clear"></div>
                        <div class="hide">
                            <p class="leftfloat font14">Thank you for your interest. Kamala Landmarc Motorbikes will get in touch shortly</p>
                            <span class="rightfloat bwsprite cross-lg-lgt-grey cur-pointer"></span>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <div>
                        <p class="font14 text-bold padding-top20 padding-right20 padding-left20 margin-bottom15">Models available with the dealer:</p>
                        <ul id="modelsAvailable">
                            <li>
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a href="">
                                            <img title="" alt="" src="http://imgd1.aeplcdn.com//310X174//bw/models/royal-enfield-classic-350-standard-136.jpg?20151209202137">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom7">
                                            <h3 class="font16 text-dark-black"><a href="" title="">Royal Enfield Classic 350</a></h3>
                                        </div>
                                        <div class="font16 text-bold margin-bottom5">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font18">1,29,356</span> <span class="font16">onwards</span>
                                        </div>
                                        <div class="font14 text-light-grey">
                                            <span>346 CC, 37 Kmpl, 19.8 bhp</span>
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a href="">
                                            <img title="" alt="" src="http://imgd1.aeplcdn.com//310X174//bw/models/honda-cb-shine-kick/drum/spokes-111.jpg?20151209184344">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom7">
                                            <h3 class="font16 text-dark-black"><a href="" title="">Royal Enfield Classic 350</a></h3>
                                        </div>
                                        <div class="font16 text-bold margin-bottom5">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font18">1,29,356</span> <span class="font16">onwards</span>
                                        </div>
                                        <div class="font14 text-light-grey">
                                            <span>346 CC, 37 Kmpl, 19.8 bhp</span>
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a href="">
                                            <img title="" alt="" src="http://imgd1.aeplcdn.com//310X174//bw/models/bajaj-pulsar-rs200.jpg?20150710124439">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom7">
                                            <h3 class="font16 text-dark-black"><a href="" title="">Royal Enfield Classic 350</a></h3>
                                        </div>
                                        <div class="font16 text-bold margin-bottom5">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font18">1,29,356</span> <span class="font16">onwards</span>
                                        </div>
                                        <div class="font14 text-light-grey">
                                            <span>346 CC, 37 Kmpl, 19.8 bhp</span>
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a href="">
                                            <img title="" alt="" src="http://imgd2.aeplcdn.com//310X174//bw/models/honda-cb-hornet-160r.jpg?20151012195209">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom7">
                                            <h3 class="font16 text-dark-black"><a href="" title="">Royal Enfield Classic 350</a></h3>
                                        </div>
                                        <div class="font16 text-bold margin-bottom5">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font18">1,29,356</span> <span class="font16">onwards</span>
                                        </div>
                                        <div class="font14 text-light-grey">
                                            <span>346 CC, 37 Kmpl, 19.8 bhp</span>
                                        </div>
                                    </div>
                                </div>
                            </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>

            </div>
            <div class="clear"></div>
        </section>

        <section>
            <div class="grid-12 alpha omega">
                <div class="dealer-map-wrapper">
                    <div id="dealerMapWrapper" style="position: fixed; top: 50px; width: 100%; height: 530px;">
                        <div id="dealersMap" style="width: 100%; height: 530px;">
                        </div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/dealerlisting.js?<%= staticFileVersion %>1234"></script>
    </form>
</body>
</html>
