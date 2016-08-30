<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Used.BikeDetails" %>
<%@ Register Src="~/m/controls/SimilarUsedBikes.ascx" TagPrefix="BW" TagName="SimilarUsedBikes" %>
<!DOCTYPE html>
<html>
<head>
    <title>Used details</title>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/used-details.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div class="container bg-white clearfix box-shadow margin-bottom10">
                <h1 class="font16 padding-top15 padding-right20 padding-bottom15 padding-left20">2009, Royal Enfield Classic Desert Storm</h1>
                <div id="model-main-image">
                    <a href="javascript:void(0)" class="model-gallery-target hide" rel="nofollow">
                        <img src="http://imgd3.aeplcdn.com//310x174//bw/used/S43685/43685_20160713013010469.jpg" alt="" title="" />
                        <div class="model-media-details">
                            <div class="model-media-item">
                                <span class="bwmsprite gallery-photo-icon"></span>
                                <span class="model-media-count">55</span>
                            </div>
                        </div>
                    </a>
                    <div class="no-image-content">
                        <span class="bwmsprite no-image-icon"></span>
                        <p class="font12 text-bold text-light-grey margin-top5 margin-bottom15">Seller has not uploaded any photos</p>
                        <a href="javascript:void(0)" id="request-media-btn" class="btn btn-inv-teal btn-sm font14 text-bold" rel="nofollow">Request photos</a>
                    </div>
                </div>
                <div class="margin-right20 margin-left20 padding-top15 padding-bottom15 border-bottom-light font14">
                    <div class="grid-6 alpha omega margin-bottom5">
                        <span class="bwmsprite model-date-icon"></span>
                        <span class="model-details-label">2013 model</span>
                    </div>
                    <div class="grid-6 omega margin-bottom5">
                        <span class="bwmsprite kms-driven-icon"></span>
                        <span class="model-details-label">1,45,000 kms</span>
                    </div>
                    <div class="grid-6 alpha omega margin-bottom5">
                        <span class="bwmsprite author-grey-sm-icon"></span>
                        <span class="model-details-label">2nd owner</span>
                    </div>
                    <div class="grid-6 omega margin-bottom5">
                        <span class="bwmsprite model-loc-icon"></span>
                        <span class="model-details-label">Mumbai</span>
                    </div>
                    <div class="clear"></div>
                    <p><span class="bwmsprite inr-md-icon"></span>&nbsp;<span class="font22 text-bold">1,22,000</span></p>
                </div>

                <div class="grid-12 float-button float-fixed clearfix">
                    <div class="grid-12 alpha omega padding-top10 padding-right5 padding-bottom10">
                        <a id="get-seller-button" class="btn btn-orange btn-full-width rightfloat" href="javascript:void(0);" rel="nofollow">Get seller details</a>
                    </div>
                </div>
                
                <div class="margin-right20 margin-left20 padding-top15 padding-bottom20 font14">
                    <p class="text-bold margin-bottom15">Ad details</p>
                    <ul class="specs-features-list">
                        <li>
                            <p class="specs-features-label">Profile ID</p>
                            <p class="specs-features-value">1348762</p>
                        </li>
                        <li>
                            <p class="specs-features-label">Date updated</p>
                            <p class="specs-features-value">02 Nov 2015</p>
                        </li>
                        <li>
                            <p class="specs-features-label">Seller</p>
                            <p class="specs-features-value">Individual</p>
                        </li>
                        <li>
                            <p class="specs-features-label">Manufacturing year</p>
                            <p class="specs-features-value">Aug 2013</p>
                        </li>
                        <li>
                            <p class="specs-features-label">Colour</p>
                            <p class="specs-features-value">Yellow</p>
                        </li>
                        <li>
                            <p class="specs-features-label">Bike registered at</p>
                            <p class="specs-features-value">Thane</p>
                        </li>
                        <li>
                            <p class="specs-features-label">Insurance</p>
                            <p class="specs-features-value">Comprehensive</p>
                        </li>
                        <li>
                            <p class="specs-features-label">Registration no.</p>
                            <p class="specs-features-value">MH-02-BN-5823</p>
                        </li>
                    </ul>
                    <div class="clear"></div>
                    <div class="margin-bottom15 padding-top15 border-bottom-light"></div>
                    <p class="text-bold margin-bottom15">Ad description</p>
                    <p class="text-light-grey">top notch condition original parts,and fitments no accident met till date. brokers r excused.. only original buyers can call.</p>
                </div>

            </div>
        </section>

        <section>
            <div id="model-bottom-card-wrapper" class="container bg-white clearfix box-shadow margin-bottom30">
                <div id="model-overall-specs-wrapper">
                    <div id="overall-specs-tab" class="overall-specs-tabs-container">
                        <ul class="overall-specs-tabs-wrapper">
                            <li data-tabs="#modelSpecs" class="active">Specifications</li>
                            <li data-tabs="#modelFeatures">Features</li>
                            <li data-tabs="#modelSimilar">Similar bikes</li>
                            <li data-tabs="#modelOtherBikes">Other bikes</li>
                        </ul>
                    </div>
                </div>

                <div id="modelSpecs" class="bw-model-tabs-data margin-right20 margin-left20 padding-top15 padding-bottom20 font14 border-solid-bottom">
                    <h2 class="margin-bottom20">Specification summary</h2>
                    <ul class="specs-features-list">
                        <li>
                            <p class="specs-features-label">Displacement</p>
                            <p class="specs-features-value">199.50 cc</p>
                        </li>
                        <li>
                            <p class="specs-features-label">Max Power</p>
                            <p class="specs-features-value">24.50 bhp @ 9,750 rpm</p>
                        </li>
                        <li>
                            <p class="specs-features-label">Maximum Torque</p>
                            <p class="specs-features-value">18.60 Nm @ 8,000 rpm</p>
                        </li>
                        <li>
                            <p class="specs-features-label">No. of gears</p>
                            <p class="specs-features-value">6</p>
                        </li>
                        <li>
                            <p class="specs-features-label">Mileage</p>
                            <p class="specs-features-value">35 kmpl</p>
                        </li>
                        <li>
                            <p class="specs-features-label">Brake Type</p>
                            <p class="specs-features-value">Rear</p>
                        </li>
                    </ul>
                    <div class="clear"></div>

                    <div class="margin-top15">
                        <a href="" title="">View full specifications<span class="bwmsprite blue-right-arrow-icon"></span></a>
                    </div>
                </div>

                <div id="modelFeatures" class="bw-model-tabs-data margin-right20 margin-left20 padding-top20 padding-bottom20 font14 border-solid-bottom">
                    <h2 class="margin-bottom20">Features summary</h2>
                    <ul class="specs-features-list">
                        <li>
                            <p class="specs-features-label">Speedometer</p>
                            <p class="specs-features-value">Analogue</p>
                        </li>
                        <li>
                            <p class="specs-features-label">Fuel Guage</p>
                            <p class="specs-features-value">Yes</p>
                        </li>
                        <li>
                            <p class="specs-features-label">Tachometer Type</p>
                            <p class="specs-features-value">--</p>
                        </li>
                        <li>
                            <p class="specs-features-label">Digital Fuel Guage</p>
                            <p class="specs-features-value">No</p>
                        </li>
                        <li>
                            <p class="specs-features-label">Tripmeter</p>
                            <p class="specs-features-value">No</p>
                        </li>
                        <li>
                            <p class="specs-features-label">Electric Start</p>
                            <p class="specs-features-value">Yes</p>
                        </li>
                    </ul>
                    <div class="clear"></div>
                    <div class="margin-top15">
                        <a href="" title="">View full features<span class="bwmsprite blue-right-arrow-icon"></span></a>
                    </div>
                </div>
                <!-- Similar used bikes starts -->
                <BW:SimilarUsedBikes ID="ctrlTopCityPrices" runat="server" />
                <!-- Similar used bikes ends -->

                <div id="modelOtherBikes" class="bw-model-tabs-data padding-top20 padding-bottom15">
                    <h2 class="margin-right20 margin-bottom15 margin-left20">Other used bikes in Mumbai</h2>
                    <div id="other-bike-swiper" class="swiper-container padding-top5 padding-bottom5">
                        <div class="swiper-wrapper">
                            <div class="swiper-slide swiper-shadow">
                                <div class="model-swiper-image-preview">
                                    <a href="">
                                        <img class="swiper-lazy" data-src="http://imgd1.aeplcdn.com//310x174//bw/models/honda-dream-neo-self-start-drum-brake-alloy-451.jpg?20151209184804" title="" alt="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </a>
                                </div>
                                <div class="model-swiper-details font11">
                                    <a href="" class="target-link font12 text-truncate margin-bottom5" title="Honda Dream Neo">Honda Dream Neo</a>
                                    <div class="grid-6 alpha padding-right5">
                                        <span class="bwmsprite model-date-icon-xs"></span>
                                        <span class="model-details-label">2013 model</span>
                                    </div>
                                    <div class="grid-6 omega padding-left5">
                                        <span class="bwmsprite kms-driven-icon-xs"></span>
                                        <span class="model-details-label">1,45,000 kms</span>
                                    </div>
                                    <div class="grid-6 alpha padding-right5">
                                        <span class="bwmsprite author-grey-icon-xs"></span>
                                        <span class="model-details-label">2nd owner</span>
                                    </div>
                                    <div class="grid-6 omega padding-left5">
                                        <span class="bwmsprite model-loc-icon-xs"></span>
                                        <span class="model-details-label">Mumbai</span>
                                    </div>
                                    <div class="clear"></div>
                                    <p class="margin-top5"><span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-bold">1,22,000</span></p>
                                    <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 font14" rel="nofollow">Get seller details</a>
                                </div>
                            </div>

                            <div class="swiper-slide swiper-shadow">
                                <div class="model-swiper-image-preview">
                                    <a href="">
                                        <img class="swiper-lazy" data-src="http://imgd1.aeplcdn.com//310x174//bw/models/honda-dream-neo-self-start-drum-brake-alloy-451.jpg?20151209184804" title="" alt="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </a>
                                </div>
                                <div class="model-swiper-details font11">
                                    <a href="" class="target-link font12 text-truncate margin-bottom5" title="Honda Dream Neo">Honda Dream Neo</a>
                                    <div class="grid-6 alpha padding-right5">
                                        <span class="bwmsprite model-date-icon-xs"></span>
                                        <span class="model-details-label">2013 model</span>
                                    </div>
                                    <div class="grid-6 omega padding-left5">
                                        <span class="bwmsprite kms-driven-icon-xs"></span>
                                        <span class="model-details-label">1,45,000 kms</span>
                                    </div>
                                    <div class="grid-6 alpha padding-right5">
                                        <span class="bwmsprite author-grey-icon-xs"></span>
                                        <span class="model-details-label">2nd owner</span>
                                    </div>
                                    <div class="grid-6 omega padding-left5">
                                        <span class="bwmsprite model-loc-icon-xs"></span>
                                        <span class="model-details-label">Mumbai</span>
                                    </div>
                                    <div class="clear"></div>
                                    <p class="margin-top5"><span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-bold">1,22,000</span></p>
                                    <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 font14" rel="nofollow">Get seller details</a>
                                </div>
                            </div>

                            <div class="swiper-slide swiper-shadow">
                                <div class="model-swiper-image-preview">
                                    <a href="">
                                        <img class="swiper-lazy" data-src="http://imgd1.aeplcdn.com//310x174//bw/models/honda-dream-neo-self-start-drum-brake-alloy-451.jpg?20151209184804" title="" alt="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </a>
                                </div>
                                <div class="model-swiper-details font11">
                                    <a href="" class="target-link font12 text-truncate margin-bottom5" title="Honda Dream Neo">Honda Dream Neo</a>
                                    <div class="grid-6 alpha padding-right5">
                                        <span class="bwmsprite model-date-icon-xs"></span>
                                        <span class="model-details-label">2013 model</span>
                                    </div>
                                    <div class="grid-6 omega padding-left5">
                                        <span class="bwmsprite kms-driven-icon-xs"></span>
                                        <span class="model-details-label">1,45,000 kms</span>
                                    </div>
                                    <div class="grid-6 alpha padding-right5">
                                        <span class="bwmsprite author-grey-icon-xs"></span>
                                        <span class="model-details-label">2nd owner</span>
                                    </div>
                                    <div class="grid-6 omega padding-left5">
                                        <span class="bwmsprite model-loc-icon-xs"></span>
                                        <span class="model-details-label">Mumbai</span>
                                    </div>
                                    <div class="clear"></div>
                                    <p class="margin-top5"><span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-bold">1,22,000</span></p>
                                    <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 font14" rel="nofollow">Get seller details</a>
                                </div>
                            </div>

                            <div class="swiper-slide swiper-shadow">
                                <div class="model-swiper-image-preview">
                                    <a href="">
                                        <img class="swiper-lazy" data-src="http://imgd1.aeplcdn.com//310x174//bw/models/honda-dream-neo-self-start-drum-brake-alloy-451.jpg?20151209184804" title="" alt="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </a>
                                </div>
                                <div class="model-swiper-details font11">
                                    <a href="" class="target-link font12 text-truncate margin-bottom5" title="Honda Dream Neo">Honda Dream Neo</a>
                                    <div class="grid-6 alpha padding-right5">
                                        <span class="bwmsprite model-date-icon-xs"></span>
                                        <span class="model-details-label">2013 model</span>
                                    </div>
                                    <div class="grid-6 omega padding-left5">
                                        <span class="bwmsprite kms-driven-icon-xs"></span>
                                        <span class="model-details-label">1,45,000 kms</span>
                                    </div>
                                    <div class="grid-6 alpha padding-right5">
                                        <span class="bwmsprite author-grey-icon-xs"></span>
                                        <span class="model-details-label">2nd owner</span>
                                    </div>
                                    <div class="grid-6 omega padding-left5">
                                        <span class="bwmsprite model-loc-icon-xs"></span>
                                        <span class="model-details-label">Mumbai</span>
                                    </div>
                                    <div class="clear"></div>
                                    <p class="margin-top5"><span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-bold">1,22,000</span></p>
                                    <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 font14" rel="nofollow">Get seller details</a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="margin-top10 margin-right20 margin-left20">
                        <a href="" title="" class="font14">View all used bikes in Mumbai<span class="bwmsprite blue-right-arrow-icon"></span></a>
                    </div>
                </div>
            </div>
            <div id="modelSpecsFooter"></div>
        </section>

        <!-- gallery start -->
        <div id="model-gallery-container">
            <p class="font16 text-white">2009, Royal Enfield Classic Desert Storm Photos</p>
            <div class="gallery-close-btn position-abt pos-top15 pos-right15 bwmsprite cross-md-white cur-pointer"></div>

            <div id="bike-gallery-popup">
                <div class="font14 text-white margin-bottom15">
                    <span class="leftfloat media-title"></span>
                    <span class="rightfloat gallery-count"></span>
                    <div class="clear"></div>
                </div>
                <div class="connected-carousels-photos">
                    <div class="stage-photos">
                        <div class="swiper-container noSwiper carousel-photos carousel-stage-photos">
                            <div class="swiper-wrapper">
                                <div class="swiper-slide">
                                    <img class="swiper-lazy" data-src="http://imgd1.aeplcdn.com//476x268//bw/models/honda-cb-shine-electric-start/drum/alloy-112.jpg?20151209184344" alt=""  title="" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                                <div class="swiper-slide">
                                    <img class="swiper-lazy" data-src="http://imgd8.aeplcdn.com//476x268//bw/ec/23814/Honda-CB-Shine-Front-threequarter-74768.jpg?wm=2" alt=""  title="" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                                <div class="swiper-slide">
                                    <img class="swiper-lazy" data-src="http://imgd4.aeplcdn.com//476x268//bw/ec/22012/Honda-CB-Shine-Side-66803.jpg?wm=2&t=125705630&t=125705630" alt=""  title="" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                                <div class="swiper-slide">
                                    <img class="swiper-lazy" data-src="http://imgd4.aeplcdn.com//476x268//bw/ec/22012/Honda-CB-Shine-Side-66802.jpg?wm=2&t=125659890&t=125659890" alt=""  title="" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                                <div class="swiper-slide">
                                    <img class="swiper-lazy" data-src="http://imgd4.aeplcdn.com//476x268//bw/ec/22012/Honda-CB-Shine-Side-66801.jpg?wm=2&t=125653770&t=125653770" alt=""  title="" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                            </div>
                            <div class="bwmsprite swiper-button-next"></div>
                            <div class="bwmsprite swiper-button-prev"></div>
                        </div>
                    </div>

                    <div class="navigation-photos">
                        <div class="swiper-container noSwiper carousel-navigation-photos">
                            <div class="swiper-wrapper">
                                <div class="swiper-slide">
                                    <img class="swiper-lazy" data-src="http://imgd1.aeplcdn.com//110x61//bw/models/honda-cb-shine-electric-start/drum/alloy-112.jpg?20151209184344" alt=""  title="a" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                                <div class="swiper-slide">
                                    <img class="swiper-lazy" data-src="http://imgd8.aeplcdn.com//110x61//bw/ec/23814/Honda-CB-Shine-Front-threequarter-74768.jpg?wm=2" alt=""  title="b" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                                <div class="swiper-slide">
                                    <img class="swiper-lazy" data-src="http://imgd4.aeplcdn.com//110x61//bw/ec/22012/Honda-CB-Shine-Side-66803.jpg?wm=2&t=125705630&t=125705630" alt=""  title="c" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                                <div class="swiper-slide">
                                    <img class="swiper-lazy" data-src="http://imgd4.aeplcdn.com//110x61//bw/ec/22012/Honda-CB-Shine-Side-66802.jpg?wm=2&t=125659890&t=125659890" alt=""  title="d" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                                <div class="swiper-slide">
                                    <img class="swiper-lazy" data-src="http://imgd4.aeplcdn.com//110x61//bw/ec/22012/Honda-CB-Shine-Side-66801.jpg?wm=2&t=125653770&t=125653770" alt=""  title="e" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                            </div>
                            <div class="bwmsprite swiper-button-next hide"></div>
                            <div class="bwmsprite swiper-button-prev hide"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- gallery end -->

        <!-- get seller details pop up start  -->
        <div id="get-seller-details-popup" class="bw-popup bwm-fullscreen-popup">
            <div class="popup-inner-container text-center">
                <div class="bwmsprite close-btn seller-details-close position-abt pos-top20 pos-right20"></div>
                <div id="user-details-section">
                    <div class="icon-outer-container rounded-corner50percent margin-bottom10">
                        <div class="icon-inner-container rounded-corner50percent">
                            <span class="bwmsprite user-contact-details-icon margin-top15"></span>
                        </div>
                    </div>
                    <p class="font18 text-bold margin-bottom10">Get seller details</p>
                    <p class="font14 text-light-grey margin-bottom25">For privacy concerns, we hide owner details. Please fill this form to get owner's details.</p>

                    <div class="input-box form-control-box margin-bottom10">
                        <input type="text" id="getUserName" />
                        <label for="getUserName">Name<sup>*</sup></label>
                        <span class="boundary"></span>
                        <span class="error-text"></span>
                    </div>
                    <div class="input-box form-control-box margin-bottom10">
                        <input type="email" id="getUserEmailID" />
                        <label for="getUserEmailID">Email<sup>*</sup></label>
                        <span class="boundary"></span>
                        <span class="error-text"></span>
                    </div>
                    <div class="input-box input-number-box form-control-box margin-bottom15">
                        <input type="tel" id="getUserMobile" maxlength="10"/>
                        <label for="getUserMobile">Mobile number<sup>*</sup></label>
                        <span class="input-number-prefix">+91</span>
                        <span class="boundary"></span>
                        <span class="error-text"></span>
                    </div>
                    <a class="btn btn-orange btn-fixed-width" id="submit-user-details-btn">Get details</a>
                </div>
                
                <div id="mobile-verification-section">
                    <div class="icon-outer-container rounded-corner50percent margin-bottom10">
                        <div class="icon-inner-container rounded-corner50percent">
                            <span class="bwmsprite otp-icon margin-top15"></span>
                        </div>
                    </div>
                    <p class="font18 text-bold margin-bottom10">Mobile verification</p>
                    <p class="font14 text-light-grey margin-bottom25">We have just sent a 5-digit verification code on your mobile number.</p>

                    
                    <div id="verify-otp-content">
                        <div class="margin-bottom35">
                            <div class="leftfloat text-left">
                                <p class="font12 text-light-grey">Mobile number</p>
                                <div class="font16 text-bold">
                                    <span class="text-light-grey">+91</span>
                                    <span class="user-submitted-mobile"></span>
                                </div>
                            </div>
                            <div class="rightfloat bwmsprite edit-blue-icon" id="edit-mobile-btn"></div>
                            <div class="clear"></div>
                        </div>

                        <div class="input-box form-control-box margin-bottom15">
                            <input type="tel" id="getUserOTP" maxlength="5" />
                            <label for="getUserOTP">One-time password</label>
                            <span class="boundary"></span>
                            <span class="error-text"></span>
                        </div>
                        <a class="btn btn-orange btn-fixed-width" id="submit-user-otp-btn">Verify</a>
                    </div>

                    <div id="update-mobile-content">
                        <div class="input-box input-number-box form-control-box margin-bottom15">
                            <input type="tel" id="getUpdatedMobile" maxlength="10" />
                            <label for="getUpdatedMobile">Mobile number<sup>*</sup></label>
                            <span class="input-number-prefix">+91</span>
                            <span class="boundary"></span>
                            <span class="error-text"></span>
                        </div>
                        <a class="btn btn-orange btn-fixed-width" id="submit-updated-mobile-btn">Done</a>
                    </div>                    
                </div>

                <div id="seller-details-section">
                    <div class="icon-outer-container rounded-corner50percent margin-bottom10">
                        <div class="icon-inner-container rounded-corner50percent">
                            <span class="bwmsprite user-contact-details-icon margin-top15"></span>
                        </div>
                    </div>
                    <p class="font18 text-bold margin-bottom10">Seller details</p>
                    <p class="font14 text-light-grey margin-bottom20">We have also sent you these details through SMS and e-mail.</p>

                    <ul class="dealer-details-list text-left">
                        <li>
                            <p class="data-key">Name</p>
                            <p class="data-value">Piyush Thakar</p>
                        </li>
                        <li>
                            <p class="data-key">Email</p>
                            <p class="data-value">piyush@bikewale.com</p>
                        </li>
                        <li>
                            <p class="data-key">Mobile number</p>
                            <p class="data-value">9876543210</p>
                        </li>
                        <li>
                            <p class="data-key">City</p>
                            <p class="data-value">Mumbai, Maharashtra</p>
                        </li>
                    </ul>                   

                </div>
                <!-- OTP Popup ends here -->
            </div>
        </div>
        <!-- get seller details pop up end  -->

        <!-- request for image popup start -->
        <div id="request-media-popup" class="bw-popup bwm-fullscreen-popup">
            <div class="popup-inner-container text-center">
                <div class="bwmsprite close-btn request-media-close position-abt pos-top20 pos-right20"></div>
                <div id="requester-details-section">
                    <div class="icon-outer-container rounded-corner50percent margin-bottom10">
                        <div class="icon-inner-container rounded-corner50percent">
                            <span class="bwmsprite request-media-icon margin-top15"></span>
                        </div>
                    </div>
                    <p class="font18 text-bold margin-bottom10">Request photos</p>
                    <p class="font14 text-light-grey margin-bottom25">Request the seller to upload photos of this bike</p>

                    <div class="input-box form-control-box margin-bottom10">
                        <input type="text" id="requesterName" />
                        <label for="requesterName">Name<sup>*</sup></label>
                        <span class="boundary"></span>
                        <span class="error-text"></span>
                    </div>
                    <div class="input-box form-control-box margin-bottom10">
                        <input type="email" id="requesterEmail" />
                        <label for="requesterEmail">Email<sup>*</sup></label>
                        <span class="boundary"></span>
                        <span class="error-text"></span>
                    </div>
                    <div class="input-box input-number-box form-control-box margin-bottom15">
                        <input type="tel" id="requesterMobile" maxlength="10" />
                        <label for="requesterMobile">Mobile number<sup>*</sup></label>
                        <span class="input-number-prefix">+91</span>
                        <span class="boundary"></span>
                        <span class="error-text"></span>
                    </div>
                    <a class="btn btn-orange btn-fixed-width" id="submit-requester-details-btn">Get details</a>
                </div>

                <div id="request-sent-section">
                    <div class="icon-outer-container rounded-corner50percent margin-bottom10">
                        <div class="icon-inner-container rounded-corner50percent">
                            <span class="bwmsprite thankyou-icon margin-top15"></span>
                        </div>
                    </div>
                    <p class="font18 text-bold margin-bottom10">Request sent!</p>
                    <p class="font14 text-light-grey margin-bottom25">We have requested seller to upload photos.<br />
                        We will let you know as soon as the seller uploads it.</p>

                    <a class="btn btn-orange" id="submit-request-sent-btn">Done</a>
                </div>
            </div>
        </div>
        <!-- request for image popup ends -->

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/used-details.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>
