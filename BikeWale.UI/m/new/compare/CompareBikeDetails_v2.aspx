<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.CompareBikeDetails_v2" Trace="false" %>
<%@ Register Src="~/m/controls/SimilarCompareBikes.ascx" TagPrefix="BW" TagName="SimilarBikes" %>
<!DOCTYPE html>
<html>
<head>
<%
    if (count == 2)
    {
        title = "Compare " + bikeDetails.Rows[0]["Make"] + " " + bikeDetails.Rows[0]["Model"] + " vs " + bikeDetails.Rows[1]["Make"] + " " + bikeDetails.Rows[1]["Model"] + " - BikeWale";
        keywords = "bike compare, compare bike, compare bikes, bike comparison, bike comparison india";
        description = "Compare " + bikeDetails.Rows[0]["Make"] + " " + bikeDetails.Rows[0]["Model"] + " and " + bikeDetails.Rows[1]["Make"] + " " + bikeDetails.Rows[1]["Model"] + " at Bikewale. Compare Price, Mileage, Engine Power, Space, Features, Specifications, Colors and much more.";
        canonical = "https://www.bikewale.com/comparebikes/" + bikeDetails.Rows[0]["MakeMaskingName"] + "-" + bikeDetails.Rows[0]["ModelMaskingName"] + "-vs-" + bikeDetails.Rows[1]["MakeMaskingName"] + "-" + bikeDetails.Rows[1]["ModelMaskingName"];
        AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        AdId = "1398766302464";
        //menu = "11";
        //ShowTargeting = "1";
        TargetedModels = targetedModels;
    }
%>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/m/css/compare/details.css" />    
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div id="sponsored-column-active"> <%-- add sponsored-column-active for sponsored bike--%>
                <div class="container box-shadow bg-white card-bottom-margin bw-tabs-panel">
                    <h1 class="box-shadow padding-15-20 margin-bottom3 text-bold">Bajaj Avenger 150 Street vs Benelli TNT 25</h1>
                    <div class="comparison-main-card">
                        <div class="bike-details-block">
                            <span class="close-selected-bike position-abt pos-right5 bwmsprite cross-sm-dark-grey"></span>
                            <a href="" title="Bajaj Avenger 150 Street" class="block margin-top10">
                                <h2 class="font14">Bajaj Avenger 150 Street</h2>
                                <img class="bike-image-block" src="https://imgd3.aeplcdn.com//110x61//bw/models/bajaj-avenger-150-street.jpg" alt="Bajaj Avenger 150 Street" />
                            </a>
                            <p class="label-text">Version:</p>
                            <p class="dropdown-selected-item option-count-one dropdown-width">Standard</p>

                            <p class="text-truncate label-text">Ex-showroom, Mumbai</p>
                            <p class="margin-bottom10">
                                <span class="bwmsprite inr-xsm-icon"></span> <span class="font16 text-bold">99,999</span>
                            </p>
                            <div>
                                <a href="" class="btn btn-white bike-orp-btn" rel="nofollow">On-road price</a>
                            </div>
                        </div>
                        <div class="bike-details-block">
                            <span class="close-selected-bike position-abt pos-right5 bwmsprite cross-sm-dark-grey"></span>
                            <a href="" title="Benelli TNT 25" class="block margin-top10">
                                <h2 class="font14">Benelli TNT 25</h2>
                                <img class="bike-image-block" src="https://imgd4.aeplcdn.com//110x61//bw/models/benelli-tnt25.jpg" alt="Benelli TNT 25" />
                            </a>
                            <p class="label-text">Version:</p>
                            <div class="dropdown-select-wrapper">
                                <select class="dropdown-select" data-title="Version">
                                    <option value="1">Standard</option>
                                    <option value="2" selected>Deluxe</option>
                                </select>
                            </div>

                            <p class="text-truncate label-text">Ex-showroom, Mumbai</p>
                            <p class="margin-bottom10">
                                <span class="bwmsprite inr-xsm-icon"></span> <span class="font16 text-bold">9,99,999</span>
                            </p>
                            <div>
                                <a href="" class="btn btn-white bike-orp-btn" rel="nofollow">On-road price</a>
                            </div>
                        </div>
                        <div class="bike-details-block sponsored-bike-details-block">
                            <span class="position-abt pos-top5 label-text">Sponsored</span>
                            <span id="close-sponsored-bike" class="position-abt pos-right5 bwmsprite cross-sm-dark-grey"></span>
                            <a href="" title="Bajaj Pulsar RS200" class="block margin-top10">
                                <h2 class="font14">Bajaj Pulsar RS200</h2>
                                <img class="bike-image-block" src="https://imgd1.aeplcdn.com//110x61//bw/models/bajaj-pulsar-rs200.jpg" alt="Bajaj Pulsar RS200" />
                            </a>
                            <p class="label-text">Version:</p>
                            <div class="dropdown-select-wrapper">
                                <select class="dropdown-select" data-title="Version">
                                    <option value="1" selected>Drum/Kick</option>
                                    <option value="2">Drum/Electric start</option>
                                    <option value="3">Disc/Electric start</option>
                                </select>
                            </div>

                            <p class="text-truncate label-text">Ex-showroom, Mumbai</p>
                            <p class="margin-bottom10">
                                <span class="bwmsprite inr-xsm-icon"></span> <span class="font16 text-bold">1,26,980</span>
                            </p>
                            <div class="padding-top5 padding-bottom5">
                                <a href="" class="font14">Know more <span class="bwmsprite know-more-icon"></span></a>
                            </div>
                            <!--<a href="" class="btn btn-white bike-orp-btn" rel="nofollow">On-road price</a>-->
                        </div>
                        <div class="clear"></div>
                    </div>
                    <div id="comparison-floating-card" class="box-shadow slideIn-transition">
                        <div class="bike-details-block">
                            <a href="" class="bike-title-target">Bajaj Avenger 150 Street</a>
                            <a href="" class="btn btn-white bike-orp-btn" rel="nofollow">On-road price</a>
                        </div>
                        <div class="bike-details-block">
                            <a href="" class="bike-title-target">Benelli TNT 25</a>
                            <a href="" class="btn btn-white bike-orp-btn" rel="nofollow">On-road price</a>
                        </div>
                        <div class="bike-details-block sponsored-bike-details-block">
                            <span class="position-abt pos-top5 label-text">Sponsored</span>
                            <a href="" class="bike-title-target">Bajaj Pulsar RS200</a>
                            <div class="padding-top5 padding-bottom5">
                                <a href="" class="font14">Know more <span class="bwmsprite know-more-icon"></span></a>
                            </div>
                            <!--<a href="" class="btn btn-white" rel="nofollow">On-road price</a>-->
                        </div>
                        <div class="clear"></div>
                        <div class="overall-specs-tabs-container">
                            <ul class="overall-specs-tabs-wrapper">
                                <li data-tabs="specsTabContent" class="active">Specifications</li>
                                <li data-tabs="featuresTabContent">Features</li>
                                <li data-tabs="coloursTabContent">Colours</li>
                            </ul>
                            <div class="clear"></div>
                        </div>
                    </div>

                    <div id="overall-specs-tabs" class="overall-specs-tabs-container">
                        <ul class="overall-specs-tabs-wrapper">
                            <li data-tabs="specsTabContent" class="active"><h3>Specifications</h3></li>
                            <li data-tabs="featuresTabContent"><h3>Features</h3></li>
                            <li data-tabs="coloursTabContent"><h3>Colours</h3></li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <div id="specsTabContent" class="bw-tabs-data active">
                        <div class="model-accordion-tab active">
                            <span class="offers-sprite engine-sm-icon"></span>
                            <span class="accordion-tab-label">Engine & transmission</span>
                            <span class="bwmsprite fa-angle-down"></span>
                        </div>
                        <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <tr class="row-type-heading">
                                    <td colspan="2">Displacement (cc)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>249</td>
                                    <td>249</td>
                                    <td>249</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Cylinders</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>1</td>
                                    <td>1</td>
                                    <td>1</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Max Power</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>14.3</td>
                                    <td>28.16</td>
                                    <td>14.3</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Maximum Torque</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td><span class="bwmsprite tick-grey"></span></td>
                                    <td><span class="bwmsprite tick-grey"></span></td>
                                    <td><span class="bwmsprite tick-grey"></span></td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Bore (mm)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td><span class="bwmsprite tick-grey"></span></td>
                                    <td><span class="bwmsprite cross-grey"></span></td>
                                    <td><span class="bwmsprite cross-grey"></span></td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Stroke (mm)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>--</td>
                                    <td>72</td>
                                    <td>--</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Valves Per Cylinder</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>2</td>
                                    <td>4</td>
                                    <td>2</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Fuel Delivery System</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>Carburetor</td>
                                    <td>Electronic Fuel Injection</td>
                                    <td>Carburetor</td>
                                </tr>
                            </tbody>
                        </table>

                        <div class="model-accordion-tab">
                            <span class="offers-sprite brakes-sm-icon"></span>
                            <span class="accordion-tab-label">Brakes, wheels & suspension</span>
                            <span class="bwmsprite fa-angle-down"></span>
                        </div>
                        <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <tr class="row-type-heading">
                                    <td colspan="2">0 to 60 kmph (Seconds)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>--</td>
                                    <td>--</td>
                                    <td>--</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">0 to 80 kmph (Seconds)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>--</td>
                                    <td>--</td>
                                    <td>--</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">0 to 40 m (Seconds)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>--</td>
                                    <td>--</td>
                                    <td>--</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Top Speed (Kmph)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>110</td>
                                    <td>--</td>
                                    <td>--</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">60 to 0 Kmph (Seconds, metres)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>--</td>
                                    <td>--</td>
                                    <td>--</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">80 to 0 Kmph (Seconds, metres)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>--</td>
                                    <td>--</td>
                                    <td>--</td>
                                </tr>
                            </tbody>
                        </table>

                        <div class="model-accordion-tab">
                            <span class="offers-sprite dimension-sm-icon"></span>
                            <span class="accordion-tab-label">Dimensions & chassis</span>
                            <span class="bwmsprite fa-angle-down"></span>
                        </div>
                        <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <tr class="row-type-heading">
                                    <td colspan="2">Kerb Weight (Kg)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>123</td>
                                    <td>125</td>
                                    <td>123</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Overall Length (mm)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>344</td>
                                    <td>345</td>
                                    <td>234</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Overall Width (mm)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>234</td>
                                    <td>236</td>
                                    <td>234</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Overall Height (mm)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>1070</td>
                                    <td>1125</td>
                                    <td>1070</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Wheelbase (mm)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>1480</td>
                                    <td>1400</td>
                                    <td>1400</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Ground Clearance (mm)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>169</td>
                                    <td>160</td>
                                    <td>160</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Seat Height (mm)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>725</td>
                                    <td>--</td>
                                    <td>--</td>
                                </tr>
                            </tbody>
                        </table>

                        <div class="model-accordion-tab">
                            <span class="offers-sprite fuel-sm-icon"></span>
                            <span class="accordion-tab-label">Fuel efficiency & performance</span>
                            <span class="bwmsprite fa-angle-down"></span>
                        </div>
                        <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <tr class="row-type-heading">
                                    <td colspan="2">Fuel Tank Capacity (Litres)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>14</td>
                                    <td>15</td>
                                    <td>--</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Reserve Fuel Capacity (Litres)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>3.4</td>
                                    <td>--</td>
                                    <td>--</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Fuel Efficiency Overall (Kmpl)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>50</td>
                                    <td>--</td>
                                    <td>--</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Fuel Efficiency Range (Km)</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>910</td>
                                    <td>--</td>
                                    <td>--</td>
                                </tr>
                            </tbody>
                        </table>

                    </div>
                    <div id="featuresTabContent" class="bw-tabs-data">
                        <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <tr class="row-type-heading">
                                    <td colspan="2">Speedometer</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>Analogue</td>
                                    <td>Digital</td>
                                    <td>Analogue</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Tachometer</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td><span class="bwmsprite cross-grey"></span></td>
                                    <td><span class="bwmsprite tick-grey"></span></td>
                                    <td><span class="bwmsprite cross-grey"></span></td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Shift Light</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td><span class="bwmsprite cross-grey"></span></td>
                                    <td><span class="bwmsprite cross-grey"></span></td>
                                    <td><span class="bwmsprite cross-grey"></span></td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Tripmeter Type</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>Digital</td>
                                    <td>Digital</td>
                                    <td>Digital</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Fuel Gauge</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td><span class="bwmsprite cross-grey"></span></td>
                                    <td>--</td>
                                    <td>--</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Pillion Seat</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td><span class="bwmsprite cross-grey"></span></td>
                                    <td><span class="bwmsprite tick-grey"></span></td>
                                    <td><span class="bwmsprite cross-grey"></span></td>
                                </tr>
                                
                                <tr class="row-type-heading">
                                    <td colspan="2">Stand Alarm</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>--</td>
                                    <td>--</td>
                                    <td>--</td>
                                </tr>
                                <tr class="row-type-heading">
                                    <td colspan="2">Stepped Seat</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td><span class="bwmsprite cross-grey"></span></td>
                                    <td><span class="bwmsprite tick-grey"></span></td>
                                    <td><span class="bwmsprite cross-grey"></span></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="coloursTabContent" class="bw-tabs-data">
                        <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <tr class="row-type-heading">
                                    <td colspan="2"></td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <td>
                                        <div class="color-box color-count-two">
                                            <span style="background-color: #00A8DD"></span>
                                            <span style="background-color: #040004"></span>
                                        </div>
                                        <p>Dual Tone Blue</p>
                                        <div class="color-box color-count-two">
                                            <span style="background-color: #DA251F"></span>
                                            <span style="background-color: #040004"></span>
                                        </div>
                                        <p>Dual Tone Red</p>
                                    </td>
                                    <td>
                                        <div class="color-box color-count-one">
                                            <span style="background-color: #848483"></span>
                                        </div>
                                        <p>Mercury Grey</p>
                                    </td>
                                    <td>
                                        <div class="color-box color-count-one">
                                            <span style="background-color: #00AEE8"></span>
                                        </div>
                                        <p>Deep Sky Blue</p>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                                
                    <div id="toggle-float-button" class="grid-12 float-button float-fixed clearfix slideIn-transition">
                        <button type="button" id="toggle-features-btn" class="btn btn-teal btn-full-width">Hide common features</button>
                    </div>
                    <div class="clear"></div>
                    <div id="comparison-footer"></div>
                </div>

                <div id="used-bikes-container" class="container box-shadow bg-white card-bottom-margin">
                    <h2 class="content-inner-block-15">Used bikes you may like</h2>
                    <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr class="row-type-heading">
                                <td colspan="2"></td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td><a href="" class="block">23 Used Bajaj Avenger</a><p class="text-light-grey text-unbold">starting at<br /><span class="bwmsprite inr-grey-xxsm-icon"></span>98,000</p></td>
                                <td><a href="" class="block">45 Used Benelli TNT</a><p class="text-light-grey text-unbold">starting at<br /><span class="bwmsprite inr-grey-xxsm-icon"></span>76,000</p></td>
                                <td><a href="" class="block">67 Used Bajaj Pulsar</a><p class="text-light-grey text-unbold">starting at<br /><span class="bwmsprite inr-grey-xxsm-icon"></span>54,000</p></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </section>

        <section>
            <div class="container box-shadow bg-white padding-bottom10 card-bottom-margin">
                <h2 class="content-inner-block-15">Similar comparisons</h2>
                <div id="comparisonSwiper" class="swiper-container padding-top5 padding-bottom5 comparison-swiper card-container">
                    <div class="swiper-wrapper model-comparison-list">
                        <div class="swiper-slide">
                            <div class="swiper-card rounded-corner2">
                                <a href="" title="" class="block">
                                    <h3 class="font12 text-black text-center margin-bottom10">Wego vs Jupiter</h3>
                                    <div class="grid-6">
                                        <div class="model-img-content">
                                            <img class="swiper-lazy" data-src="https://imgd1.aeplcdn.com//144x81//bw/models/tvs-wego-drum-165.jpg?20151209224944" src="" alt="" title="" />
                                            <span class="swiper-lazy-preloader"></span>
                                        </div>
                                        <p class="font11 text-light-grey text-truncate">Ex-showroom, Mumbai</p>
                                        <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-default text-bold">51,985</span>
                                    </div>
                                    <div class="grid-6">
                                        <div class="model-img-content">
                                            <img class="swiper-lazy" data-src="https://imgd1.aeplcdn.com//144x81//bw/models/tvs-jupiter-standard-505.jpg?20151209224723" src="" alt="" title="" />
                                            <span class="swiper-lazy-preloader"></span>
                                        </div>
                                        <p class="font11 text-light-grey text-truncate">Ex-showroom, Mumbai</p>
                                        <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-default text-bold">51,817</span>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="margin-top15 text-center">
                                        <span class="btn btn-white btn-size-1">Compare now</span>
                                    </div>
                                </a>
                            </div>
                        </div>

                        <div class="swiper-slide">
                            <div class="swiper-card rounded-corner2">
                                <a href="" title="" class="block">
                                    <h3 class="font12 text-black text-center margin-bottom10">Wego vs Jupiter</h3>
                                    <div class="grid-6">
                                        <div class="model-img-content">
                                            <img class="swiper-lazy" data-src="https://imgd1.aeplcdn.com//144x81//bw/models/tvs-wego-drum-165.jpg?20151209224944" src="" alt="" title="" />
                                            <span class="swiper-lazy-preloader"></span>
                                        </div>
                                        <p class="font11 text-light-grey text-truncate">Ex-showroom, Mumbai</p>
                                        <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-default text-bold">51,985</span>
                                    </div>
                                    <div class="grid-6">
                                        <div class="model-img-content">
                                            <img class="swiper-lazy" data-src="https://imgd1.aeplcdn.com//144x81//bw/models/tvs-jupiter-standard-505.jpg?20151209224723" src="" alt="" title="" />
                                            <span class="swiper-lazy-preloader"></span>
                                        </div>
                                        <p class="font11 text-light-grey text-truncate">Ex-showroom, Mumbai</p>
                                        <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-default text-bold">51,817</span>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="margin-top15 text-center">
                                        <span class="btn btn-white btn-size-1">Compare now</span>
                                    </div>
                                </a>
                            </div>
                        </div>

                        <div class="swiper-slide">
                            <div class="swiper-card rounded-corner2">
                                <a href="" title="" class="block">
                                    <h3 class="font12 text-black text-center margin-bottom10">Wego vs Jupiter</h3>
                                    <div class="grid-6">
                                        <div class="model-img-content">
                                            <img class="swiper-lazy" data-src="https://imgd1.aeplcdn.com//144x81//bw/models/tvs-wego-drum-165.jpg?20151209224944" src="" alt="" title="" />
                                            <span class="swiper-lazy-preloader"></span>
                                        </div>
                                        <p class="font11 text-light-grey text-truncate">Ex-showroom, Mumbai</p>
                                        <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-default text-bold">51,985</span>
                                    </div>
                                    <div class="grid-6">
                                        <div class="model-img-content">
                                            <img class="swiper-lazy" data-src="https://imgd1.aeplcdn.com//144x81//bw/models/tvs-jupiter-standard-505.jpg?20151209224723" src="" alt="" title="" />
                                            <span class="swiper-lazy-preloader"></span>
                                        </div>
                                        <p class="font11 text-light-grey text-truncate">Ex-showroom, Mumbai</p>
                                        <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-default text-bold">51,817</span>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="margin-top15 text-center">
                                        <span class="btn btn-white btn-size-1">Compare now</span>
                                    </div>
                                </a>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </section>
        
        <!-- select bike starts here -->
        <div id="select-bike-cover-popup" class="cover-window-popup">
            <div class="ui-corner-top">
                <div id="close-bike-popup" class="cover-popup-back cur-pointer leftfloat" data-bind="click: closeBikePopup">
                    <span class="bwmsprite fa-angle-left"></span>
                </div>
                <div class="cover-popup-header leftfloat">Select bikes</div>
                <div class="clear"></div>
            </div>
            <div class="bike-banner"></div>
            <div id="select-make-wrapper" class="cover-popup-body">
                <div class="cover-popup-body-head">
                    <p class="no-back-btn-label head-label inline-block">Select Make</p>
                </div>
                <ul class="cover-popup-list with-arrow">
                    <li data-bind="click: makeChanged" data-id="2"><span>Aprilla</span></li>
                    <li data-bind="click: makeChanged" data-id="1"><span>Bajaj</span></li>
                    <li data-bind="click: makeChanged" data-id="40"><span>Benelli</span></li>
                    <li data-bind="click: makeChanged" data-id="3"><span>BMW1</span></li>
                    <li data-bind="click: makeChanged" data-id="4"><span>Ducati</span></li>
                </ul>                                
            </div>

            <div id="select-model-wrapper" class="cover-popup-body">
                <div class="cover-popup-body-head">
                    <div data-bind="click: modelBackBtn" class="body-popup-back cur-pointer inline-block">
                        <span class="bwmsprite back-long-arrow-left"></span>
                    </div><p class="head-label inline-block">Select Model</p>
                </div>
                <ul class="cover-popup-list with-arrow" data-bind="foreach: modelArray">
                    <li data-bind="click: $parent.modelChanged">
                        <span data-bind="text: modelName, attr: { 'data-id': modelId }" ></span>
                    </li>
                </ul>
            </div>

            <div id="select-version-wrapper" class="cover-popup-body">
                <div class="cover-popup-body-head">
                    <div data-bind="click: versionBackBtn" class="body-popup-back cur-pointer inline-block">
                        <span id="arrow-version-back" class="bwmsprite back-long-arrow-left" ></span>
                    </div><p class="head-label inline-block">Select Version</p>
                </div>
                <ul class="cover-popup-list" data-bind="foreach: versionArray">
                    <li data-bind="click: $parent.versionChanged">
                        <span data-bind="text: versionName, attr: { 'data-id': versionId }" ></span>
                    </li>                                    
                </ul>
            </div>

            <div class="cover-popup-loader-body">
                <div class="cover-popup-loader"></div>
                <div class="cover-popup-loader-text font14">Loading...</div>
            </div>
        </div>
        <!-- select bike ends here -->

    <div>
        
    <section class="container related-comparison-container margin-bottom20 <%= (ctrlSimilarBikes.fetchedCount > 0) ? string.Empty : "hide" %>">
    <h2 class="font14 padding-left10 margin-top5 margin-bottom15">Related comparisons</h2>
    <div class="swiper-container">
        <div class="swiper-wrapper">             
                <BW:SimilarBikes ID="ctrlSimilarBikes" runat="server" />                       
        </div>
    </div>
    </section>
    
    </div>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/compare/details.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,600,700' rel='stylesheet' type='text/css' />

    </form>
</body>
</html>

