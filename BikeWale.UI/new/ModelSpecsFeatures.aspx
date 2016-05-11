<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ModelSpecsFeatures.aspx.cs" Inherits="Bikewale.New.ModelSpecsFeatures" %>

<!DOCTYPE html>

<html>
<head>
    <%
        isHeaderFix = false;
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <style type="text/css">
        .model-details-floating-card { width:976px; background:#fff; z-index:4; }.model-details-floating-card.fixed-card { position:fixed; top:0; left:5%; right:5%; margin:0 auto; }.text-truncate { width:100%; text-align:left; text-overflow:ellipsis; white-space:nowrap; overflow:hidden; }.overall-specs-tabs-wrapper { display:table; background:#fff; }.overall-specs-tabs-wrapper a { padding:10px 20px; display:table-cell; font-size:14px; color:#82888b; }.overall-specs-tabs-wrapper a:hover { text-decoration:none; color:#4d5057; }.overall-specs-tabs-wrapper a.active { border-bottom:3px solid #ef3f30; font-weight:bold; color:#4d5057; }.content-inner-block-1020 { padding:10px 20px 10px; }.inline-block-top { display:inline-block; vertical-align:top; }.model-card-image-content { width:117px; height:66px; background:#ccc; }.model-card-image-content img { width:100%; height:66px; }.model-card-title-content { width:245px; }.model-orp-btn .btn { padding:8px 29px; }.model-powered-by-text { width:210px !important; padding:0 10px; }#modelSpecsAndFeaturesWrapper h2 {font-size: 18px;color: #1a1a1a;font-weight: bold;margin-bottom: 15px;}#modelSpecsAndFeaturesWrapper h3 {font-size: 14px;color: #4d5057;margin-bottom: 20px;}#modelSpecsAndFeaturesWrapper .grid-3 p { margin-top:26px; width:100%; text-align:left; text-overflow:ellipsis; white-space:nowrap; overflow:hidden; }#modelSpecsAndFeaturesWrapper .grid-3 p:first-child { margin-top:0; }.border-divider { border-top:1px solid #e2e2e2; }
    </style>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section class="bg-light-grey padding-top10">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <a href="/" itemprop="url"><span itemprop="title">Home</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="fa fa-angle-right margin-right10"></span>
                                <a href="/" itemprop="url"><span itemprop="title">Honda Bikes</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="fa fa-angle-right margin-right10"></span>
                                <a href="/" itemprop="url"><span itemprop="title">Wego</span></a>
                            </li>
                            <li>
                                <span class="fa fa-angle-right margin-right10"></span>
                                <span>Specs & Features</span>
                            </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        
        <section id="modelCardAndDetailsWrapper" class="container margin-bottom30 font14">
            <div class="grid-12">
                <div id="modelFloatingCardContent">
                    <div class="model-details-floating-card content-box-shadow">
                        <div class="content-inner-block-1020">
                            <div class="grid-5 alpha omega">
                                <div class="model-card-image-content inline-block-top margin-right20">
                                    <img src="http://imgd1.aeplcdn.com//110x61//bw/models/tvs-wego-drum-165.jpg?20151209224944" />
                                </div>
                                <div class="model-card-title-content inline-block-top">
                                    <h2 class="font18 text-bold margin-bottom10">Bajaj CT100</h2>
                                    <p class="font14 text-light-grey">Self Start Disc Brake Alloy Wheel</p>
                                </div>
                            </div>
                            <div class="grid-4 padding-left30">
                                <p class="font14 text-light-grey margin-bottom5 text-truncate">On-road price in Andheri, Mumbai</p>
                                <div class="font16">
                                    <span class="fa fa-rupee"></span> <span class="font18 text-bold">1,22,000</span>
                                </div>
                            </div>
                            <div class="grid-3 model-orp-btn alpha omega">
                                <a href="javascript:void(0)" class="btn btn-orange font14 margin-top5">Get offers from this dealer</a>
                                <!-- if no 'powered by' text is present remove margin-top5 add margin-top10 in offers button -->
                                <p class="model-powered-by-text font12 margin-top10 text-truncate"><span class="text-light-grey">Powered by </span>BikeWale Motor</p>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="overall-specs-tabs-wrapper">
                            <a class="active" href="#modelSpecificationContent">Specifications</a>
                            <a href="#modelFeaturesContent">Features</a>
                        </div>
                    </div>
                </div>

                <div id="modelSpecsAndFeaturesWrapper" class="content-box-shadow">
                    <div class="border-divider"></div>
                    <div id="modelSpecificationContent" class="bw-model-tabs-data padding-top20">
                        <h2 class="padding-left20 padding-right20">Specifications</h2>
                        <h3 class="padding-left20">Engine and Transmission</h3>
                        <div class="grid-3 padding-left20 text-light-grey">
                            <p>Displacement</p>
                            <p>Cylinders</p>
                            <p>Max Power</p>
                            <p>Maximum Torque</p>
                            <p>Bore</p>
                            <p>Stroke</p>
                            <p>Valves Per Cylinder</p>
                            <p>Fuel Delivery System</p>
                        </div>
                        <div class="grid-3 text-bold">
                            <p>199 cc</p>
                            <p>1</p>
                            <p>25 bhp @ 10,000 rpm</p>
                            <p>19 Nm @ 8,000 rpm</p>
                            <p>72 mm</p>
                            <p>49 mm</p>
                            <p>4</p>
                            <p>Fuel Injection</p>
                        </div>
                        <div class="grid-3 padding-left30 text-light-grey">
                            <p>Fuel Type</p>
                            <p>Ignition</p>
                            <p>Spark Plugs</p>
                            <p>Cooling System</p>
                            <p>Gearbox Type</p>
                            <p>No. of Gears</p>
                            <p>Transmission Type</p>
                            <p>Clutch</p>
                        </div>
                        <div class="grid-3 padding-right20 text-bold">
                            <p>Petrol</p>
                            <p>Spark Ignition</p>
                            <p>1 Per Cylinder</p>
                            <p>Liquid Cooled</p>
                            <p>Manual</p>
                            <p>6</p>
                            <p>Chain Drive</p>
                            <p>Wet multi-disc</p>
                        </div>
                        <div class="clear"></div>

                        <h3 class="margin-top30 padding-left20">Brakes, Wheels and Suspension</h3>
                        <div class="grid-3 padding-left20 text-light-grey">
                            <p>Brake Type</p>
                            <p>Front Disc</p>
                            <p>Front Disc/Drum Size</p>
                            <p>Rear Disc</p>
                            <p>Rear Disc/Drum Size</p>
                            <p>Calliper Type</p>
                            <p>Wheel Size</p>
                        </div>
                        <div class="grid-3 text-bold">
                            <p>Disc</p>
                            <p>Yes</p>
                            <p>280 mm</p>
                            <p>Yes</p>
                            <p>230 mm</p>
                            <p>Four piston radially bolted piston radially bolted</p>
                            <p>17 inches</p>
                        </div>
                        <div class="grid-3 padding-left30 text-light-grey">
                            <p>Front Tyre</p>
                            <p>Rear Tyre</p>
                            <p>Tubeless Tyres</p>
                            <p>Radial Tyres</p>
                            <p>Alloy Wheels</p>
                            <p>Front Suspension</p>
                            <p>Rear Suspension</p>
                        </div>
                        <div class="grid-3 padding-right20 text-bold">
                            <p>110/70 x 17</p>
                            <p>150/60 x 17</p>
                            <p>Yes</p>
                            <p>Yes</p>
                            <p>Yes</p>
                            <p>Inverted Telescopic Fork</p>
                            <p>Swing Arm, Mono Suspension</p>
                        </div>
                        <div class="clear"></div>

                        <h3 class="margin-top30 padding-left20">Dimensions and Chassis</h3>
                        <div class="grid-3 padding-left20 text-light-grey">
                            <p>Kerb Weight</p>
                            <p>Overall Length</p>
                            <p>Overall Width</p>
                            <p>Overall Height</p>
                        </div>
                        <div class="grid-3 text-bold">
                            <p>147 kg</p>
                            <p>--</p>
                            <p>--</p>
                            <p>--</p>
                        </div>
                        <div class="grid-3 padding-left30 text-light-grey">
                            <p>Wheelbase</p>
                            <p>Ground Clearance</p>
                            <p>Seat Height</p>
                            <p>Chassis Type</p>
                        </div>
                        <div class="grid-3 padding-right20 text-bold">
                            <p>1,355 mm</p>
                            <p>178 mm</p>
                            <p>820 mm</p>
                            <p>Tubular space frame made from</p>
                        </div>
                        <div class="clear"></div>

                        <h3 class="margin-top30 padding-left20">Fuel efficiency and Performance</h3>
                        <div class="grid-3 padding-left20 text-light-grey">
                            <p>Fuel Tank Capacity</p>
                            <p>Reserve Fuel Capacity</p>
                            <p>Fuel Efficiency Overall</p>
                            <p>Fuel Efficiency Range</p>
                            <p>Top Speed</p>
                        </div>
                        <div class="grid-3 text-bold">
                            <p>10 litres</p>
                            <p>1.50 litres</p>
                            <p>35 kmpl</p>
                            <p>350 km</p>
                            <p>--</p>
                        </div>
                        <div class="grid-3 padding-left30 text-light-grey">
                            <p>0 to 60 kmph</p>
                            <p>0 to 80 kmph</p>
                            <p>0 to 40 kmph</p>
                            <p>60 to 0 kmph</p>
                            <p>80 to 0 kmph</p>
                        </div>
                        <div class="grid-3 padding-right20 text-bold">
                            <p>--</p>
                            <p>--</p>
                            <p>--</p>
                            <p>--</p>
                            <p>--</p>
                        </div>
                        <div class="clear"></div>
                    </div>

                    <div class="margin-top30 margin-right10 margin-left10 border-divider"></div>

                    <div id="modelFeaturesContent" class="bw-model-tabs-data padding-top20 padding-bottom40">
                        <h2 class="padding-left20 padding-right20">Features</h2>
                        <div class="grid-3 padding-left20 text-light-grey">
                            <p>Speedometer</p>
                            <p>Fuel Guage</p>
                            <p>Tachometer Type</p>
                            <p>Digital Fuel Guage</p>
                            <p>Tripmeter</p>
                            <p>Electric Start</p>
                            <p>Tachometer</p>
                            <p>Shift Light</p>
                            <p>No. of Tripmeters</p>
                            <p>Tripmeter Type</p>
                            <p>Low Fuel Indicator</p>
                            <p>Low Oil Indicator</p>
                            <p>Low Battery Indicator</p>
                            <p>Pillion Seat</p>
                            <p>Pillion Footrest</p>
                        </div>
                        <div class="grid-3 text-bold">
                            <p>Digital</p>
                            <p>Yes</p>
                            <p>Digital</p>
                            <p>Yes</p>
                            <p>Yes</p>
                            <p>Yes</p>
                            <p>Yes</p>
                            <p>Yes</p>
                            <p>2</p>
                            <p>Digital</p>
                            <p>Yes</p>
                            <p>Yes</p>
                            <p>Yes</p>
                            <p>Yes</p>
                            <p>Yes</p>
                        </div>
                        <div class="grid-3 padding-left30 text-light-grey">
                            <p>Pillion Backrest</p>
                            <p>Pillion Grabrail</p>
                            <p>Stand Alarm</p>
                            <p>Stepped Seat</p>
                            <p>Antilock Braking System</p>
                            <p>Killswitch</p>
                            <p>Clock</p>
                            <p>Electric System</p>
                            <p>Battery</p>
                            <p>Headlight Type</p>
                            <p>Headlight Bulb Type</p>
                            <p>Brake/Tail Light</p>
                            <p>Turn Signal</p>
                            <p>Pass Light</p>
                        </div>
                        <div class="grid-3 padding-right20 text-bold">
                            <p>No</p>
                            <p>Yes</p>
                            <p>Yes</p>
                            <p>Yes</p>
                            <p>No</p>
                            <p>Yes</p>
                            <p>Yes</p>
                            <p>12V DC</p>
                            <p>12V/6Ah</p>
                            <p>Projector Headlamps</p>
                            <p>--</p>
                            <p>LED Tail Lamp</p>
                            <p>Yes</p>
                            <p>Yes</p>
                        </div>
                        <div class="clear"></div>
                    </div>

                    <div id="modelSpecsFeaturesFooter"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>
        
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->

        <script type="text/javascript">
            $(document).ready(function () {
                var $window = $(window),
                    modelCardAndDetailsWrapper = $('#modelCardAndDetailsWrapper'),
                    modelDetailsFloatingCard = $('.model-details-floating-card'),
                    modelSpecsFeaturesFooter = $('#modelSpecsFeaturesFooter');

                $('#modelFloatingCardContent').css({ 'height': modelDetailsFloatingCard.height() });

                $(window).scroll(function () {
                    var windowScrollTop = $window.scrollTop(),
                        modelCardAndDetailsOffsetTop = modelCardAndDetailsWrapper.offset().top,
                        modelSpecsFeaturesFooterOffsetTop = modelSpecsFeaturesFooter.offset().top;

                    if (windowScrollTop > modelCardAndDetailsOffsetTop)
                        modelDetailsFloatingCard.addClass('fixed-card');
                    
                    else if (windowScrollTop < modelCardAndDetailsOffsetTop)
                        modelDetailsFloatingCard.removeClass('fixed-card');

                    if (modelDetailsFloatingCard.hasClass('fixed-card')) {
                        if (windowScrollTop > modelSpecsFeaturesFooterOffsetTop - modelDetailsFloatingCard.height())
                            modelDetailsFloatingCard.removeClass('fixed-card');
                    }

                    $('#modelSpecsAndFeaturesWrapper .bw-model-tabs-data').each(function () {
                        var top = $(this).offset().top - modelDetailsFloatingCard.height(),
                            bottom = top + $(this).outerHeight();
                        if (windowScrollTop >= top && windowScrollTop <= bottom) {
                            modelDetailsFloatingCard.find('a').removeClass('active');
                            $('#modelSpecsAndFeaturesWrapper .bw-mode-tabs-data').removeClass('active');

                            $(this).addClass('active');
                            modelDetailsFloatingCard.find('a[href="#' + $(this).attr('id') + '"]').addClass('active');
                        }
                    });

                });

                $('.overall-specs-tabs-wrapper a[href^="#"]').click(function () {
                    var target = $(this.hash);
                    if (target.length == 0) target = $('a[name="' + this.hash.substr(1) + '"]');
                    if (target.length == 0) target = $('html');
                    $('html, body').animate({ scrollTop: target.offset().top - modelDetailsFloatingCard.height() }, 1000);
                    return false;
                });
            });
        </script>

    </form>
</body>
</html>
