<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Used.BikeDetails" %>

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
                    <a href="javascript:void(0)">
                        <img src="http://imgd3.aeplcdn.com//310x174//bw/used/S43685/43685_20160713013010469.jpg" alt="" title="" />
                        <div class="model-media-details">
                            <div class="model-media-item">
                                <span class="bwmsprite gallery-photo-icon"></span>
                                <span class="model-media-count">55</span>
                            </div>
                        </div>
                    </a>                           
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

                <div id="modelSimilar" class="bw-model-tabs-data padding-top20 padding-bottom15">
                    <h2 class="margin-right20 margin-bottom15 margin-left20">Similar NS 200 bikes</h2>
                    <div id="similar-bike-swiper" class="swiper-container padding-top5 padding-bottom5">
                        <div class="swiper-wrapper">
                            <div class="swiper-slide swiper-shadow">
                                <div class="model-swiper-image-preview">
                                    <a href="">
                                        <img class="swiper-lazy" data-src="http://imgd1.aeplcdn.com//310x174//bw/models/hero-splendor-ismart-spoke-755.jpg?20151209181902" title="" alt="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </a>
                                </div>
                                <div class="model-swiper-details font11">
                                    <a href="" class="target-link font12 text-truncate margin-bottom5" title="Hero Splendor iSmart">Hero Splendor iSmart</a>
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
                                    <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 getquotation font14" rel="nofollow">Get seller details</a>
                                </div>
                            </div>

                            <div class="swiper-slide swiper-shadow">
                                <div class="model-swiper-image-preview">
                                    <a href="">
                                        <img class="swiper-lazy" data-src="http://imgd1.aeplcdn.com//310x174//bw/models/hero-splendor-ismart-spoke-755.jpg?20151209181902" title="" alt="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </a>
                                </div>
                                <div class="model-swiper-details font11">
                                    <a href="" class="target-link font12 text-truncate margin-bottom5" title="Hero Splendor iSmart">Hero Splendor iSmart</a>
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
                                    <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 getquotation font14" rel="nofollow">Get seller details</a>
                                </div>
                            </div>

                            <div class="swiper-slide swiper-shadow">
                                <div class="model-swiper-image-preview">
                                    <a href="">
                                        <img class="swiper-lazy" data-src="http://imgd1.aeplcdn.com//310x174//bw/models/hero-splendor-ismart-spoke-755.jpg?20151209181902" title="" alt="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </a>
                                </div>
                                <div class="model-swiper-details font11">
                                    <a href="" class="target-link font12 text-truncate margin-bottom5" title="Hero Splendor iSmart">Hero Splendor iSmart</a>
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
                                    <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 getquotation font14" rel="nofollow">Get seller details</a>
                                </div>
                            </div>

                            <div class="swiper-slide swiper-shadow">
                                <div class="model-swiper-image-preview">
                                    <a href="">
                                        <img class="swiper-lazy" data-src="http://imgd1.aeplcdn.com//310x174//bw/models/hero-splendor-ismart-spoke-755.jpg?20151209181902" title="" alt="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </a>
                                </div>
                                <div class="model-swiper-details font11">
                                    <a href="" class="target-link font12 text-truncate margin-bottom5" title="Hero Splendor iSmart">Hero Splendor iSmart</a>
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
                                    <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 getquotation font14" rel="nofollow">Get seller details</a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="margin-top10 margin-right20 margin-left20">
                        <a href="" title="" class="font14">View all NS 200 in Mumbai<span class="bwmsprite blue-right-arrow-icon"></span></a>
                    </div>
                </div>

                <div class="margin-right20 margin-left20 border-solid-bottom"></div>

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
                                    <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 getquotation font14" rel="nofollow">Get seller details</a>
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
                                    <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 getquotation font14" rel="nofollow">Get seller details</a>
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
                                    <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 getquotation font14" rel="nofollow">Get seller details</a>
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
                                    <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 getquotation font14" rel="nofollow">Get seller details</a>
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

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/used-details.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>
