﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.CompareBikeDetails_v2" Trace="false" %>
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

        <section class="container bg-white margin-bottom10">
            <div id="comparison-body" class="bg-white box-shadow sponsored-column-active"> <%-- sponsored-column-active--%>
                <h1 class="box-shadow padding-15-20 margin-bottom3">Bajaj Avenger 150 Street vs Benelli TNT 25</h1>
                <div class="comparison-main-card">
                    <div class="bike-details-block">
                        <span class="position-abt pos-top5 pos-right5 bwmsprite cross-sm-dark-grey"></span>
                        <a href="" title="Bajaj Avenger 150 Street" class="block">
                            <h2 class="font14">Bajaj Avenger 150 Street</h2>
                            <img class="bike-image-block" src="https://imgd3.aeplcdn.com//110x61//bw/models/bajaj-avenger-150-street.jpg" alt="Bajaj Avenger 150 Street" />
                        </a>

                        <p class="label-text">Version:</p>
                        <div class="dropdown-select-wrapper">
                            <select class="dropdown-select" data-title="Version">
                                <option value="1">Kick/Drum/Spokes</option>
                                <option value="2" selected>Electric Start/Drum/Alloy</option>
                                <option value="3">Electric Start/Disc/Alloy</option>
                                <option value="4">CBS</option>
                            </select>
                        </div>

                        <p class="text-truncate label-text">Ex-showroom, Mumbai</p>
                        <p class="margin-bottom10">
                            <span class="bwmsprite inr-xsm-icon"></span> <span class="font16 text-bold">99,999</span>
                        </p>
                        <div>
                            <a href="" class="btn btn-white" rel="nofollow">On-road price</a>
                        </div>
                    </div>
                    <div class="bike-details-block">
                        <span class="position-abt pos-top5 pos-right5 bwmsprite cross-sm-dark-grey"></span>
                        <a href="" title="Benelli TNT 25" class="block">
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
                            <a href="" class="btn btn-white" rel="nofollow">On-road price</a>
                        </div>
                    </div>
                    <div class="bike-details-block">
                        <span class="position-abt pos-top5 label-text">Sponsored</span>
                        <span class="position-abt pos-top5 pos-right5 bwmsprite cross-sm-dark-grey"></span>
                        <a href="" title="Bajaj Pulsar RS200" class="block">
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
                            <a href="" class="font14">Know more</a>
                        </div>
                        <!--<div>
                            <a href="" class="btn btn-white" rel="nofollow">On-road price</a>
                        </div>-->
                    </div>
                    <div class="clear"></div>
                </div>
                <div id="comparison-floating-card" class="box-shadow">
                    <div class="bike-details-block">
                        <a href="" class="bike-title-target">Bajaj Avenger 150 Street</a>
                        <a href="" class="btn btn-white" rel="nofollow">On-road price</a>
                    </div>
                    <div class="bike-details-block">
                        <a href="" class="bike-title-target">Benelli TNT 25</a>
                        <a href="" class="btn btn-white" rel="nofollow">On-road price</a>
                    </div>
                    <div class="bike-details-block">
                        <span class="position-abt pos-top5 label-text">Sponsored</span>
                        <a href="" class="bike-title-target">Bajaj Pulsar RS200</a>
                        <div class="padding-top5 padding-bottom5">
                            <a href="" class="font14">Know more</a>
                        </div>
                        <!--<a href="" class="btn btn-white" rel="nofollow">On-road price</a>-->
                    </div>
                    <div class="clear"></div>
                    <div class="overall-specs-tabs-container">
                        <ul class="overall-specs-tabs-wrapper">
                            <li data-tabs="#specsTabContent" class="active">Specifications</li>
                            <li data-tabs="#featuresTabContent">Features</li>
                            <li data-tabs="#coloursTabContent">Colours</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div id="overall-specs-tabs" class="overall-specs-tabs-container">
                    <ul class="overall-specs-tabs-wrapper">
                        <li data-tabs="#specsTabContent" class="active">Specifications</li>
                        <li data-tabs="#featuresTabContent">Features</li>
                        <li data-tabs="#coloursTabContent">Colours</li>
                    </ul>
                    <div class="clear"></div>
                </div>
                <div id="specsTabContent" class="bw-model-tabs-data active">
                    <div class="model-accordion-tab active">
                        <span class="offers-sprite engine-sm-icon"></span>
                        <span class="accordion-tab-label">Engine</span>
                        <span class="bwmsprite fa-angle-down"></span>
                    </div>
                    <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr class="row-type-heading">
                                <td colspan="2">Displacement (cc)</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>150</td>
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
                                <td>12.5</td>
                                <td>21.61</td>
                                <td>12.5</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Bore (mm)</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>--</td>
                                <td>61</td>
                                <td>--</td>
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
                            <tr class="row-type-heading">
                                <td colspan="2">Fuel Type</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>Petrol</td>
                                <td>Petrol</td>
                                <td>Petrol</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Ignition</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>Digital Twin Spark Ignition</td>
                                <td>--</td>
                                <td>--</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Spark Plugs (Per Cylinder)</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>2</td>
                                <td>1</td>
                                <td>2</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Cooling System</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>Air Cooled</td>
                                <td>Liquid Cooled</td>
                                <td>Air Cooled</td>
                            </tr>
                        </tbody>
                    </table>

                    <div class="model-accordion-tab">
                        <span class="offers-sprite engine-sm-icon"></span>
                        <span class="accordion-tab-label">Transmission</span>
                        <span class="bwmsprite fa-angle-down"></span>
                    </div>
                    <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr class="row-type-heading">
                                <td colspan="2">Gearbox Type</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>Manual</td>
                                <td>Manual</td>
                                <td>Manual</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">No. of Gears</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>5</td>
                                <td>6</td>
                                <td>5</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Transmission Type</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>Chain Drive</td>
                                <td>Chain Drive</td>
                                <td>Chain Drive</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Clutch</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>Wet Multiplate</td>
                                <td>Wet Multiplate</td>
                                <td>Wet Multiplate</td>
                            </tr>
                        </tbody>
                    </table>

                    <div class="model-accordion-tab">
                        <span class="offers-sprite fuel-sm-icon"></span>
                        <span class="accordion-tab-label">Performance</span>
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
                        <span class="accordion-tab-label">Dimensions & Weight</span>
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
                        <span class="accordion-tab-label">Fuel Efficiency & Range</span>
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

                    <div class="model-accordion-tab">
                        <span class="offers-sprite dimension-sm-icon"></span>
                        <span class="accordion-tab-label">Chassis & Suspension</span>
                        <span class="bwmsprite fa-angle-down"></span>
                    </div>
                    <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr class="row-type-heading">
                                <td colspan="2">Chassis Type</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>Tubular Double Cradle</td>
                                <td>Steel trellis frame</td>
                                <td>Tubular Double Cradle</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Front Suspension</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>Telescopic with Anti Friction Bush</td>
                                <td>Inverted front telescopic forks</td>
                                <td>Telescopic with Anti Friction Bush</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Rear Suspension</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>Twin Shock Absorber</td>
                                <td>Hydraulic Monoshock absorber</td>
                                <td>Twin Shock Absorber</td>
                            </tr>
                        </tbody>
                    </table>

                    <div class="model-accordion-tab">
                        <span class="offers-sprite brakes-sm-icon"></span>
                        <span class="accordion-tab-label">Braking</span>
                        <span class="bwmsprite fa-angle-down"></span>
                    </div>
                    <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr class="row-type-heading">
                                <td colspan="2">Brake Type</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>Disc</td>
                                <td>Disc</td>
                                <td>Disc</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Front Disc</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td><span class="bwmsprite tick-grey"></span></td>
                                <td><span class="bwmsprite tick-grey"></span></td>
                                <td><span class="bwmsprite tick-grey"></span></td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Front Disc/Drum Size (mm)</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>240</td>
                                <td>280</td>
                                <td>--</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Rear Disc</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td><span class="bwmsprite cross-grey"></span></td>
                                <td><span class="bwmsprite tick-grey"></span></td>
                                <td><span class="bwmsprite tick-grey"></span></td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Rear Disc/Drum Size (mm)</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>130</td>
                                <td>240</td>
                                <td>--</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Calliper Type</td>
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
                        <span class="offers-sprite brakes-sm-icon"></span>
                        <span class="accordion-tab-label">Wheels & Tyres</span>
                        <span class="bwmsprite fa-angle-down"></span>
                    </div>
                    <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr class="row-type-heading">
                                <td colspan="2">Wheel Size (inches)</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>17</td>
                                <td>17</td>
                                <td>--</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Front Tyre</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>90/90 - 17" 49P</td>
                                <td>110/70 - R17, 54H (MET)</td>
                                <td>--</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Rear Tyre</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>110/70 - R17, 54H (MET)</td>
                                <td>90/90 - 17" 49P</td>
                                <td>--</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Tubeless Tyres</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td><span class="bwmsprite cross-grey"></span></td>
                                <td><span class="bwmsprite tick-grey"></span></td>
                                <td><span class="bwmsprite tick-grey"></span></td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Radial Tyres</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td><span class="bwmsprite cross-grey"></span></td>
                                <td><span class="bwmsprite tick-grey"></span></td>
                                <td><span class="bwmsprite tick-grey"></span></td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Alloy Wheels</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td><span class="bwmsprite tick-grey"></span></td>
                                <td><span class="bwmsprite tick-grey"></span></td>
                                <td><span class="bwmsprite tick-grey"></span></td>
                            </tr>
                        </tbody>
                    </table>

                    <div class="model-accordion-tab">
                        <span class="offers-sprite engine-sm-icon"></span>
                        <span class="accordion-tab-label">Electricals</span>
                        <span class="bwmsprite fa-angle-down"></span>
                    </div>
                    <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr class="row-type-heading">
                                <td colspan="2">Electric System</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>12V</td>
                                <td>12V DC</td>
                                <td>12V DC</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Battery</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>12 V, 4Ah, VRLA</td>
                                <td>Maintenance Free</td>
                                <td>Maintenance Free</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Headlight Type</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>Projector Head Lamp</td>
                                <td>Multi-Reflector Type</td>
                                <td>Multi-Reflector Type</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="2">Headlight Bulb Type</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>55/60W</td>
                                <td>--</td>
                                <td>--</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="3">Brake/Tail Light</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>--</td>
                                <td>LED Tail Lamp</td>
                                <td>LED Tail Lamp</td>
                            </tr>
                            <tr class="row-type-heading">
                                <td colspan="3">Turn Signal</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td>Yes</td>
                                <td>Yes</td>
                                <td>Yes</td>
                            </tr>                            
                            <tr class="row-type-heading">
                                <td colspan="3">Pass Light</td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <td><span class="bwmsprite tick-grey"></span></td>
                                <td><span class="bwmsprite tick-grey"></span></td>
                                <td><span class="bwmsprite tick-grey"></span></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="featuresTabContent" class="bw-model-tabs-data"></div>
                <div id="coloursTabContent" class="bw-model-tabs-data"></div>
            </div>
        </section>


    <div>
        <div class="padding10">
            
            
            <% if(count == 2) { %>
            
            <div class="box2">
        <div id="CD2" class="hide" style="padding: 0px 5px;">
            <table cellspacing="0" cellpadding="0" class="table">
                <tbody>
                    <tr style="font-weight:bold;">
                        <td class="subCategoryBorder" style="text-align:left;font-size:14px;">Features</td>
                        <td class="subCategoryBorder"><div onclick="BoxClicked(this);" class="rightMinus"></div></td>
                    </tr>
                    <tr style="display: table-row;">
                        <td colspan="2">
                        <table cellspacing="0" cellpadding="0" class="table tblItem">
                            <tbody>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Speedometer</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeFeatures.Rows[0]["Speedometer"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeFeatures.Rows[1]["Speedometer"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Tachometer</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["Tachometer"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["Tachometer"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Tachometer Type</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeFeatures.Rows[0]["TachometerType"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeFeatures.Rows[1]["TachometerType"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Shift Light</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["ShiftLight"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["ShiftLight"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Electric Start</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["ElectricStart"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["ElectricStart"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Tripmeter</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["Tripmeter"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["Tripmeter"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">No. of Tripmeters</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeFeatures.Rows[0]["NoOfTripmeters"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeFeatures.Rows[1]["NoOfTripmeters"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Tripmeter Type</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeFeatures.Rows[0]["TripmeterType"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeFeatures.Rows[1]["TripmeterType"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Low Fuel Indicator</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["LowFuelIndicator"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["LowFuelIndicator"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Low Oil Indicator</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["LowOilIndicator"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["LowOilIndicator"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Low Battery Indicator</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["LowBatteryIndicator"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["LowBatteryIndicator"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Fuel Gauge</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["FuelGauge"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["FuelGauge"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Digital Fuel Gauges</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["DigitalFuelGauge"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["DigitalFuelGauge"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Pillion Seat</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["PillionSeat"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["PillionSeat"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Pillion Footrest</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["PillionFootrest"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["PillionFootrest"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Pillion Backrest</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["PillionBackrest"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["PillionBackrest"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Pillion Grabrail</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["PillionGrabrail"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["PillionGrabrail"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Stand Alarm</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["StandAlarm"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["StandAlarm"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Stepped Seat</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["SteppedSeat"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["SteppedSeat"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Antilock Braking System</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["AntilockBrakingSystem"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["AntilockBrakingSystem"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Killswitch</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["Killswitch"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["Killswitch"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Clock</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["Clock"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["Clock"].ToString()) %></td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div id="CD1" class="hide" style="padding: 0px 5px;">
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
	            <tbody>
		            <tr class="compareBikeItemContainer">
			            <td colspan="2">Colors</td>
		            </tr>
		            <tr class="colorDiv">
			            <td class="compareBikeItemBorder-Rt" style="width:50%;padding-top:5px;"><%= GetModelColors(bikeDetails.Rows[0]["BikeVersionId"].ToString())%></td>
			            <td class="" style="width:50%;padding-top:5px;"><%= GetModelColors(bikeDetails.Rows[1]["BikeVersionId"].ToString())%></td>
		            </tr>
	            </tbody>
            </table>
        </div>
    </div>
        </div>
    <% } %>
        <% if(isUsedBikePresent){ %>
        <h2 class="font14 padding-left10 margin-top5 margin-bottom15">Used bikes</h2>
            <div class="usedBikes">
                <table width="100%">
                    <tr><td width="50%"><%= CreateUsedBikeLink(Convert.ToUInt32(bikeDetails.Rows[0]["bikeCount"]),Convert.ToString(bikeDetails.Rows[0]["make"]), Convert.ToString(bikeDetails.Rows[0]["MakeMaskingName"]), Convert.ToString(bikeDetails.Rows[0]["model"]), Convert.ToString(bikeDetails.Rows[0]["ModelMaskingName"]), Bikewale.Common.CommonOpn.FormatPrice(Convert.ToString(bikeDetails.Rows[0]["minPrice"]))) %></td>
                        <td width="50%"><%= CreateUsedBikeLink(Convert.ToUInt32(bikeDetails.Rows[1]["bikeCount"]),Convert.ToString(bikeDetails.Rows[1]["make"]), Convert.ToString(bikeDetails.Rows[1]["MakeMaskingName"]), Convert.ToString(bikeDetails.Rows[1]["model"]), Convert.ToString(bikeDetails.Rows[1]["ModelMaskingName"]), Bikewale.Common.CommonOpn.FormatPrice(Convert.ToString(bikeDetails.Rows[1]["minPrice"]))) %></td>
                    </tr>
                </table>
            </div>
        <% } %>
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
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->

        <script type="text/javascript">
            $(document).ready(function () {
                $("#divFloat").hide();
                var relatedComparisonContainer = $('.related-comparison-container');
                $(".swiper-slide:first").css("margin-left", "10px");
                /*
                $(window).bind("scroll", function () {
                    var offset = $(this).scrollTop(),
                        relatedComparisonContainerOffset = relatedComparisonContainer.offset();

                    if (offset > parseInt($("#divBikeName").position().top) + 180) {
                        $("#divFloat").show();
                    }
                    else {
                        $("#divFloat").hide();
                    }
                    if (offset > relatedComparisonContainerOffset.top - $('#divFloat').height()) {
                        $("#divFloat").hide();
                    }
                });
                */
            });

            function BoxClicked(div) {
                if ($(div).attr("class") == "rightMinus") {
                    $(div).removeClass("rightMinus").addClass("rightPlus");
                    $(div).parent("td").parent("tr").next().hide();
                }
                else if ($(div).attr("class") == "rightPlus") {
                    $(div).removeClass("rightPlus").addClass("rightMinus");
                    $(div).parent("td").parent("tr").next().show();
                }
            }


            $(".divCompareBikeMenu li").click(function () {
                $('html, body').animate({ scrollTop: 270 });
                var contentType = $(this).attr("contentType");
                $(".divCompareBikeMenu li").each(function () {
                    if ($(this).attr("contentType") == contentType) {
                        if ($(this).hasClass("list")) {
                            $(this).removeClass("list").addClass("listActive");
                            $("#" + contentType).show();
                            //window.scrollTo(0,1);
                        }
                    }
                    else {
                        if ($(this).hasClass("listActive")) {
                            $(this).removeClass("listActive").addClass("list");
                            $("#" + $(this).attr("contentType")).hide();
                        }
                    }
                });
            });
        </script>
    </form>
</body>
</html>

