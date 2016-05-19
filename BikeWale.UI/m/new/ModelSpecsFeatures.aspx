<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ModelSpecsFeatures.aspx.cs" Inherits="Bikewale.Mobile.New.ModelSpecsFeatures" %>

<!DOCTYPE html>
<html>
<head>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <style type="text/css">
        .content-inner-block-120{padding:10px 20px 0;}.text-dark-black{color:#1a1a1a;}.text-truncate{width:100%; text-align:left; text-overflow:ellipsis; white-space:nowrap; overflow:hidden;}.model-price-content{width:110px;}.model-area-content{width:60%;position:relative;top:6px;}.specs-features-wrapper{height:44px;}#specsFeaturesTabsWrapper{width:100%; background:#fff;z-index:3; display:block; border-bottom:1px solid #e2e2e2; overflow-x:auto;}.model-specs-features-tabs-wrapper{display:table; background:#fff;}.model-specs-features-tabs-wrapper li{padding:10px 20px; display:table-cell; text-align:center; white-space:nowrap; font-size:14px; color:#82888b;}.model-specs-features-tabs-wrapper li.active{border-bottom:3px solid #ef3f30; font-weight:bold; color:#4d5057;}.border-divider{ border-top:1px solid #e2e2e2;}.specs-features-list{overflow:hidden;}.specs-features-list li{margin-bottom:20px;}.specs-features-list p {width:50%;float:left; text-align:left; text-overflow:ellipsis; white-space:nowrap; overflow:hidden;}.specs-features-label{color:#82888b;}.specs-features-value {padding-left:20px;font-weight:bold;}.fixed-topNav{position:fixed;top:0;left:0;}.float-button{background-color:#fff;padding:10px}.float-button.float-fixed{position:fixed;bottom:0;z-index:8;left:0;right:0;background:#f5f5f5;}
    </style>
</head>
<body>
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section class="bg-white box-shadow margin-bottom25">
            <div id="modelPriceDetails" class="content-inner-block-120">
                <h1 class="font18 text-dark-black margin-bottom5">Hero Splendor Pro</h1>
                <p class="font20 text-bold model-price-content leftfloat"><span class="bwmsprite inr-sm-icon"></span>&nbsp;1,22,000</p>
                <p class="font14 text-truncate text-light-grey model-area-content leftfloat">On-road price in Andheri, Mumbai</p>
                <div class="clear"></div>
            </div>

            <div class="specs-features-wrapper">
                <div id="specsFeaturesTabsWrapper">
                    <ul class="model-specs-features-tabs-wrapper">
                        <li class="active" data-tabs="#modelSpecifications">Specifications</li>
                        <li data-tabs="#modelFeatures">Features</li>
                    </ul>
                </div>
            </div>

            <div id="specsFeaturesDetailsWrapper" class="padding-right20 padding-left20 font14">
                <div id="modelSpecifications" class="bw-model-tabs-data padding-top15">
                    <h2 class="font18 text-dark-black margin-bottom15">Specifications</h2>
                    <h3 class="font14 text-bold margin-bottom20">Engine and Transmission</h3>
                    <ul class="specs-features-list margin-bottom5">
                        <li>
                            <p class="specs-features-label">Displacement</p>
                            <p class="specs-features-value">199 cc</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Cylinders</p>
                            <p class="specs-features-value">1</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Max Power</p>
                            <p class="specs-features-value">25 bhp @ 10,000 rpm</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Maximum Torque</p>
                            <p class="specs-features-value">19 Nm @ 8,000 rpm</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Bore</p>
                            <p class="specs-features-value">72 mm</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Stroke</p>
                            <p class="specs-features-value">49 mm</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Valves Per Cylinder</p>
                            <p class="specs-features-value">4</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Fuel Delivery System</p>
                            <p class="specs-features-value">Fuel Injection</p>
                            <div class="clear"></div>
                        </li>
                    </ul>

                    <h3 class="font14 text-bold margin-bottom20">Brakes, Wheels and Suspension</h3>
                    <ul class="specs-features-list margin-bottom5">
                        <li>
                            <p class="specs-features-label">Brake Type</p>
                            <p class="specs-features-value">Disc</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Front Disc</p>
                            <p class="specs-features-value">Yes</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Front Disc/Drum Size</p>
                            <p class="specs-features-value">280 mm</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Rear Disc</p>
                            <p class="specs-features-value">Yes</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Rear Disc/Drum Size</p>
                            <p class="specs-features-value">230 mm</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Calliper Type</p>
                            <p class="specs-features-value">Four piston radially</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Wheel Size</p>
                            <p class="specs-features-value">17 inches</p>
                            <div class="clear"></div>
                        </li>
                    </ul>

                    <h3 class="font14 text-bold margin-bottom20">Dimensions and Chassis</h3>
                    <ul class="specs-features-list margin-bottom5">
                        <li>
                            <p class="specs-features-label">Kerb Weight</p>
                            <p class="specs-features-value">147 kg</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Overall Length</p>
                            <p class="specs-features-value">--</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Overall Width</p>
                            <p class="specs-features-value">--</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Overall Height</p>
                            <p class="specs-features-value">--</p>
                            <div class="clear"></div>
                        </li>
                    </ul>

                    <h3 class="font14 text-bold margin-bottom20">Fuel efficiency and Performance</h3>
                    <ul class="specs-features-list margin-bottom5">
                        <li>
                            <p class="specs-features-label">Fuel Tank Capacity</p>
                            <p class="specs-features-value">10 litres</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Reserve Fuel Capacity</p>
                            <p class="specs-features-value">1.50 litres</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Fuel Efficiency Overall</p>
                            <p class="specs-features-value">35 kmpl</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Fuel Efficiency Range</p>
                            <p class="specs-features-value">350 km</p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Top Speed</p>
                            <p class="specs-features-value">--</p>
                            <div class="clear"></div>
                        </li>
                    </ul>
                </div>
                <div class="border-divider"></div>

                <div id="modelFeatures" class="padding-top15 bw-model-tabs-data">
                    <h2 class="font18 text-dark-black margin-bottom20">Features</h2>
                    <ul class="specs-features-list">
                    <li>
                        <p class="specs-features-label">Speedometer</p>
                        <p class="specs-features-value">Digital</p>
                        <div class="clear"></div>
                    </li>
                    <li>
                        <p class="specs-features-label">Fuel Guage</p>
                        <p class="specs-features-value">Yes</p>
                        <div class="clear"></div>
                    </li>
                    <li>
                        <p class="specs-features-label">Tachometer Type</p>
                        <p class="specs-features-value">Digital</p>
                        <div class="clear"></div>
                    </li>
                    <li>
                        <p class="specs-features-label">Digital Fuel Guage</p>
                        <p class="specs-features-value">Yes</p>
                        <div class="clear"></div>
                    </li>
                    <li>
                        <p class="specs-features-label">Tripmeter</p>
                        <p class="specs-features-value">Yes</p>
                        <div class="clear"></div>
                    </li>
                    <li>
                        <p class="specs-features-label">Electric Start</p>
                        <p class="specs-features-value">Yes</p>
                        <div class="clear"></div>
                    </li>
                    <li>
                        <p class="specs-features-label">Tachometer</p>
                        <p class="specs-features-value">Yes</p>
                        <div class="clear"></div>
                    </li>
                    <li>
                        <p class="specs-features-label">Shift Light</p>
                        <p class="specs-features-value">Yes</p>
                        <div class="clear"></div>
                    </li>
                    <li>
                        <p class="specs-features-label">No. of Tripmeters</p>
                        <p class="specs-features-value">2</p>
                        <div class="clear"></div>
                    </li>
                    <li>
                        <p class="specs-features-label">Tripmeter Type</p>
                        <p class="specs-features-value">Digital</p>
                        <div class="clear"></div>
                    </li>
                    <li>
                        <p class="specs-features-label">Low Fuel Indicator</p>
                        <p class="specs-features-value">Yes</p>
                        <div class="clear"></div>
                    </li>
                    <li>
                        <p class="specs-features-label">Low Oil Indicator</p>
                        <p class="specs-features-value">Yes</p>
                        <div class="clear"></div>
                    </li>
                    <li>
                        <p class="specs-features-label">Low Battery Indicator</p>
                        <p class="specs-features-value">Yes</p>
                        <div class="clear"></div>
                    </li>
                    <li>
                        <p class="specs-features-label">Pillion Seat</p>
                        <p class="specs-features-value">Yes</p>
                        <div class="clear"></div>
                    </li>
                    <li>
                        <p class="specs-features-label">Pillion Footrest</p>
                        <p class="specs-features-value">Yes</p>
                        <div class="clear"></div>
                    </li>
                </ul>
                </div>

            </div>

            <div class="grid-12 float-button float-fixed">
                <a class="btn btn-full-width font18 btn-orange" href="javascript:void(0)" rel="nofollow">Check on-road price</a>
            </div>
            <div class="clear"></div>

            <div id="specsFeaturesFooter"></div>

        </section>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->    
        <script type="text/javascript">

            $(document).ready(function () {
                var $window = $(window),
                    topNavBarWrapper = $('.specs-features-wrapper'),
                    topNavBar = $('#specsFeaturesTabsWrapper'),
                    specsFeaturesFooter = $('#specsFeaturesFooter'),
                    floatButton = $('.float-button'),
                    footer = $('footer');

                $window.scroll(function () {
                    var windowScrollTop = $window.scrollTop(),
                        topNavBarWrapperOffset = topNavBarWrapper.offset(),
                        topNavBarOffset = topNavBar.offset(),
                        specsFeaturesFooterOffset = specsFeaturesFooter.offset();

                    if (windowScrollTop > topNavBarOffset.top) {
                        topNavBar.addClass('fixed-topNav');
                    }

                    else if (windowScrollTop < topNavBarWrapperOffset.top) {
                        topNavBar.removeClass('fixed-topNav');
                    }

                    if (topNavBar.hasClass('fixed-topNav')) {
                        if (windowScrollTop > specsFeaturesFooterOffset.top - topNavBar.height())
                            topNavBar.removeClass('fixed-topNav');
                    }

                    if (floatButton.offset().top < footer.offset().top - 50)
                        floatButton.addClass('float-fixed');
                    if (floatButton.offset().top > footer.offset().top - 50)
                        floatButton.removeClass('float-fixed');

                    $('#specsFeaturesDetailsWrapper .bw-model-tabs-data').each(function () {
                        var top = $(this).offset().top - topNavBar.height(),
                            bottom = top + $(this).outerHeight();
                        if (windowScrollTop >= top && windowScrollTop <= bottom) {
                            topNavBar.find('li').removeClass('active');
                            $('#specsFeaturesDetailsWrapper .bw-mode-tabs-data').removeClass('active');

                            $(this).addClass('active');

                            var currentActiveTab = topNavBar.find('li[data-tabs="#' + $(this).attr('id') + '"]');
                            topNavBar.find(currentActiveTab).addClass('active');

                        }
                    });
                    
                });

                $('.model-specs-features-tabs-wrapper li').click(function () {
                    var target = $(this).attr('data-tabs');
                    $('html, body').animate({ scrollTop: $(target).offset().top - topNavBar.height()}, 1000);
                    return false;
                });

            });
        </script>
    </form>
</body>
</html>
