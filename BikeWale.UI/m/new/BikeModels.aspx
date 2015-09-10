<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.BikeModels" %>
<%@ Register Src="/m/controls/NewsWidget.ascx" TagName="News" TagPrefix="BW"  %>
<%@ Register Src="/m/controls/ExpertReviewsWidget.ascx" TagName="ExpertReviews" TagPrefix="BW"  %>
<%@ Register Src="/m/controls/VideosWidget.ascx" TagName="Videos" TagPrefix="BW"  %>
<%@ Register Src="~/m/controls/AlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>    
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="/m/css/bwm-model.css" rel="stylesheet" type="text/css" />
</head>
<body>
<form id="form1" runat="server">
    <!-- #include file="/includes/headBW_Mobile.aspx" -->
    <section>
    	<div class="container margin-bottom20 bg-white clearfix">
                <div class="padding-bottom30">
                	<h1 class="padding-top25 padding-bottom20 padding-left20 padding-right20"><%= bikeName %></h1>
                	<div class="jcarousel-wrapper model">
                        <div class="jcarousel">
                            <ul>
                                <asp:Repeater ID="rptModelPhotos" runat="server">                                    
                                    <HeaderTemplate>
                                        <li>
                                            <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(modelPage.ModelDetails.OriginalImagePath,modelPage.ModelDetails.HostUrl,Bikewale.Utility.ImageSize._476x268) %>" title="<%# bikeName %>" alt="<%= bikeName %>">
                                        </li>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <img src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._476x268) %>" title="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" alt="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>">
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>                          
                        </div>
                        <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                        <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                        <p class="text-center jcarousel-pagination margin-bottom30"></p>

                    </div>
                    
                    <div class="margin-top20 padding-left10 padding-right10">
                    	<p class="leftfloat margin-right10 rating-wrap">
                            <%= Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(modelPage.ModelDetails.ReviewRate)) %>
                        </p>
                        <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/user-reviews/" class="<%= modelPage.ModelDetails.ReviewCount > 0 ? "" : "hide"  %>border-solid-left leftfloat margin-right10 padding-left10 line-Ht22">
                        	<%= modelPage.ModelDetails.ReviewCount %> Reviews
                        </a>                        
                        <div class="clear"></div>
                    </div>
                    
                </div>
                
                <div class="grid-12 bg-white">
                	<div class="bike-price-container font22 margin-bottom15">
                        <span class="fa fa-rupee"></span>
                        <span id="bike-price" class="font24 text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(modelPage.ModelDetails.MinPrice)) %></span> <span class="font10"><%= Bikewale.Common.Configuration.GetDefaultCityName %></span>
					</div>
                	<div id="city-list-container" class="city-list-container margin-bottom10">
                    	<div class="text-left margin-bottom15">
                        	<p class="font14 offer-error">Select City for accurate on-road price</p>
                        </div>
                        <ul>
                        	<li><span>Mumbai</span></li>
                            <li><span>Delhi</span></li>
                            <li><span>Banglore</span></li>
                            <li><span>Pune</span></li>
                            <li><span>Chennai</span></li>
                            <li class="city-other-btn"><span>Others</span></li>
                        </ul>
                    </div>
                    <div id="city-area-select-container" class="city-area-select-container margin-bottom20 hide">
                    	<div class="city-select-text text-left margin-bottom15 hide">
                            <p class="font14">Select city for accurate on-road price</p>
                        </div>
                        <div class="area-select-text text-left margin-bottom15 hide">
                            <p class="font14">Select area for on-road price and exclusive offers</p>
                        </div>
                        <div class="city-onRoad-price-container font14 margin-bottom15 hide">
                        	<p class="margin-bottom10">On-road price in <span>Andheri</span>, <span>Mumbai</span><span class="city-edit-btn font12 margin-left10">Edit</span></p>
                            <p class="font12 margin-bottom15">(Ex-showroom + RTO + Insurance + Handling charges)</p>
                            <button class="btn btn-orange btn-full-width">Book now and avail offers</button>
                        </div>	
                        <div class="city-area-wrapper">
                            <div class="city-select">
                                <select class="form-control">
                                    <option value="" disabled selected>Select City</option>
                                    <option value="Mumbai">Mumbai</option>
                                    <option value="Banglore">Banglore</option>
                                    <option value="Pune">Pune</option>
                                    <option value="Delhi">Delhi</option>
                                </select>
                            </div>
                            <div class="area-select margin-top20 hide">
                                <select class="form-control">
                                    <option value="" disabled selected>Select Area</option>
                                    <option value="area1">area 1</option>
                                    <option value="area2">area 2</option>
                                    <option value="area3">area 3</option>
                                    <option value="area4">area 4</option>
                                </select>
                            </div>
                            <div class="clear"></div>
                    	</div>
                    </div>
                    <div class="city-unveil-offer-container position-rel margin-top20 margin-bottom20">
                    	<div class="available-offers-container content-inner-block-10">
                        	<h4 class="border-solid-bottom padding-bottom5 margin-bottom5">Avaiable Offers</h4>
                            <div class="offer-list-container">
                            	<ul>
                                	<li>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</li>
                                    <li>Maecenas rhoncus placerat mauris, id mattis arcu hendrerit in.</li>
                                    <li>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</li>
                                </ul>
                            </div>
                        </div>
                        <div class="unveil-offer-btn-container position-abt pos-left0 pos-top0 text-center">
                            <button class="btn btn-md btn-orange unveil-offer-btn">Show Offers</button>
                        </div>
                        <div class="notify-btn-container position-abt pos-left0 pos-top0 hide">
							<div class="margin-top50 margin-left40">
                                <input type="text" placeholder="Notify me" class="notify-input">
                                <button class="btn btn-orange btn-xs">Notify me</button>
                            </div>
                        </div>
                    </div>
                </div>
           </div>
    </section>
    
    <section>
    	<div class="container bg-white clearfix <%= String.IsNullOrEmpty(modelPage.ModelDesc.SmallDescription) ? "hide" : "" %>">
        	<div class="grid-12">
                <h2 class="text-center margin-top30 margin-bottom20">Overview</h2>  
                <div class="overview-box">
                    <%--<div class="odd btmAftBorder">
                        <span class="font22"><%= modelPage.ModelVersionSpecs.Displacement %> <small class="font16 text-medium-grey">cc</small></span>
                        <span class="font14">Capacity</span>
                    </div>
                    <div class="even btmAftBorder">
                        <span class="font22"><%= modelPage.ModelVersionSpecs.FuelEfficiencyOverall %> <small class="font16 text-medium-grey">kmpl</small></span>
                        <span class="font14">Mileage</span>
                    </div>
                    <div class="odd">
                        <span class="font22"><%= modelPage.ModelVersionSpecs.MaxPower %> <small class="font16 text-medium-grey">PS</small></span>
                        <span class="font14">Max power</span>
                    </div>
                    <div class="even">
                        <span class="font22"><%= modelPage.ModelVersionSpecs.KerbWeight %> <small class="font16 text-medium-grey">kgs</small></span>
                        <span class="font14">Weight</span>
                    </div>--%>
                </div>    
                <p class="font14 margin-bottom20">
                    <%= modelPage.ModelDesc.SmallDescription %> <a href="javascript:void(0)" id="showFullDisc">Read more</a>
                </p>  	
                
                <p class="font14 margin-bottom20 hide">
                    <%= modelPage.ModelDesc.FullDescription %> <a href="javascript:void(0)" id="showSmallDisc">Show less</a>
                </p>
                <div class="border-top1"></div>
            </div>
        </div>
    </section>
    
    <section>
    	<div class="container bg-white clearfix">
        	<div class="grid-12">
                <h2 class="text-center margin-top30 margin-bottom20">Specifications</h2>  
                
                
                <div class="bw-tabs bw-tabs-panel clearfix">
                    <select class="form-control margin-bottom30">
                        <option value="summary">Summary</option>
                        <option value="engineTransmission">Engine &amp; Transmission </option>
                        <option value="brakeWheels">Brakes, Wheels and Suspension</option>
                        <option value="dimensions">Dimensions and Chassis</option>
                        <option value="fuelEffiency">Fuel effieciency and Performance</option>
                    </select>
                    
                    <div class="leftfloat bw-horz-tabs-data font16">
                            <div class="bw-tabs-data" id="summary">
                                <ul>
                                    <li>
                                        <div class="text-light-grey">Displacement (cc)</div>
                                        <div class="text-bold">CC</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Cylinders</div>
                                        <div class="text-bold">1</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Max Power</div>
                                        <div class="text-bold">25 bhp @ 8500 rpm</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Maximum Torque</div>
                                        <div class="text-bold">23 Nm @ 7000 rpm</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Bore (mm)</div>
                                        <div class="text-bold">76</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Stroke (mm)</div>
                                        <div class="text-bold">55</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Valves Per</div>
                                        <div class="text-bold">4</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Fuel Delivery System</div>
                                        <div class="text-bold">Fuel Injection</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Fuel Type</div>
                                        <div class="text-bold">Petrol</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Ignition</div>
                                        <div class="text-bold">Digital ECU Based</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Spark Plugs</div>
                                        <div class="text-bold">1</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Cooling System</div>
                                        <div class="text-bold">Liquid Cooled</div>
                                        </li>
                                    <div class="clear"></div>
                                </ul>
                            </div>
                            <div class="bw-tabs-data hide" id="engineTransmission">
                            	<ul>
                                    <li>
                                        <div class="text-light-grey">Displacement (cc)</div>
                                        <div class="text-bold">CC</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Max Power</div>
                                        <div class="text-bold">25 bhp @ 8500 rpm</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Maximum Torque</div>
                                        <div class="text-bold">23 Nm @ 7000 rpm</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Cylinders</div>
                                        <div class="text-bold">1</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">No. of gears</div>
                                        <div class="text-bold">5</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Bore (mm)</div>
                                        <div class="text-bold">76</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Stroke (mm)</div>
                                        <div class="text-bold">55</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Valves Per</div>
                                        <div class="text-bold">4</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Fuel Delivery System</div>
                                        <div class="text-bold">Fuel Injection</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Fuel Type</div>
                                        <div class="text-bold">Petrol</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Ignition</div>
                                        <div class="text-bold">Digital ECU Based</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Spark Plugs</div>
                                        <div class="text-bold">1</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Cooling System</div>
                                        <div class="text-bold">Liquid Cooled</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Gearbox Type</div>
                                        <div class="text-bold">Manual</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Transmission Type</div>
                                        <div class="text-bold">Chain Drive</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Clutch</div>
                                        <div class="text-bold">Wet Multi-disc</div>
                                        </li>
                                    <div class="clear"></div>
                                </ul>
                            </div>
                            <div class="bw-tabs-data hide" id="brakeWheels">
                            	<ul>
                                    <li>
                                        <div class="text-light-grey">Brake Type</div>
                                        <div class="text-bold">Disc</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Front Disc</div>
                                        <div class="text-bold">Yes</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Rear Disc</div>
                                        <div class="text-bold">-</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Alloy Wheels</div>
                                        <div class="text-bold">Yes</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Tubeless Tyres</div>
                                        <div class="text-bold">Yes</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Front Disc/Drum Size</div>
                                        <div class="text-bold">300 mm</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Rear Disc/Drum Size</div>
                                        <div class="text-bold">230 mm</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Valves Per</div>
                                        <div class="text-bold">4</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Calliper Type</div>
                                        <div class="text-bold">Four piston radially</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Wheel Size (inches)</div>
                                        <div class="text-bold">71 in</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Front Tyre</div>
                                        <div class="text-bold">110/70 x 17</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Radial Tyres</div>
                                        <div class="text-bold">Yes</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Front Suspension</div>
                                        <div class="text-bold">Inverted Telescopic</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Gearbox Type</div>
                                        <div class="text-bold">Manual</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Rear Suspension</div>
                                        <div class="text-bold">Swing Arm, Mono</div>
                                        </li>
                                    <div class="clear"></div>
                                </ul>
                            </div>
                            <div class="bw-tabs-data hide" id="dimensions">
                            	<ul>
                                    <li>
                                        <div class="text-light-grey">Kerb Weight</div>
                                        <div class="text-bold">154 kgs</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Chassis Type</div>
                                        <div class="text-bold">Tubular space fra</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Overall Length</div>
                                        <div class="text-bold">-</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Overall Width</div>
                                        <div class="text-bold">Yes</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Overall Height</div>
                                        <div class="text-bold">Yes</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Wheelbase</div>
                                        <div class="text-bold">1355</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Ground Clearance</div>
                                        <div class="text-bold">177</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Seat Height</div>
                                        <div class="text-bold">820</div>
                                        </li>
                                    <div class="clear"></div>
                                </ul>
                            </div>
                            <div class="bw-tabs-data hide" id="fuelEffiency">
                            	<ul>
                                    <li>
                                        <div class="text-light-grey">Fuel Efficiency</div>
                                        <div class="text-bold">50 kmpl</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Fuel Tank Capacity</div>
                                        <div class="text-bold">14 litres</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Overall Length</div>
                                        <div class="text-bold">-</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Top Speed</div>
                                        <div class="text-bold">250 kpmh</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Reserve Fuel Capacity</div>
                                        <div class="text-bold">1.5 L</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Fuel Efficiency Range</div>
                                        <div class="text-bold">100 kms</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">Ground Clearance</div>
                                        <div class="text-bold">177</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">0 to 60 kmph</div>
                                        <div class="text-bold">-</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">0 to 60 kmph</div>
                                        <div class="text-bold">-</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">0 to 80 kmph</div>
                                        <div class="text-bold">-</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">0 to 40 m</div>
                                        <div class="text-bold">-</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">60 to 0 kmph</div>
                                        <div class="text-bold">-</div>
                                        </li>
                                    <li>
                                        <div class="text-light-grey">80 to 0 kmph</div>
                                        <div class="text-bold">-</div>
                                        </li>
                                    <div class="clear"></div>
                                </ul>
                            </div>
                        </div>
                    
                </div>
                <!-- bw-tabs ends here -->
                
                <!-- features code starts here -->
                <div class="bw-tabs-data margin-bottom20" id="features">
                	<div class="border-top1"></div>
                    <h2 class="text-center margin-top30 margin-bottom20 text-center">Features</h2>
                    <div class="bw-horz-tabs-data font16">
                    	<ul class="bw-tabs-data">
                        	<li>
                            	<div class="text-light-grey">Speedometer</div>
                                <div class="text-bold">Digital</div>
                            </li>
                            <li>
                            	<div class="text-light-grey">Fuel Guage</div>
                                <div class="text-bold">Yes</div>
                            </li>
                            <li>
                            	<div class="text-light-grey">Tachometer Type</div>
                                <div class="text-bold">Analog</div>
                            </li>
                            <li>
                            	<div class="text-light-grey">Digital Fuel Guage</div>
                                <div class="text-bold">No</div>
                            </li>
                            <li>
                            	<div class="text-light-grey">Tripmeter</div>
                                <div class="text-bold">No</div>
                            </li>
                            <li>
                            	<div class="text-light-grey">Electric Start</div>
                                <div class="text-bold">Yes</div>
                            </li>
                            <div class="clear"></div>
                        </ul>
                        <ul class="more-features bw-tabs-data hide">
                        	<li>
                            	<div class="text-light-grey">Speedometer</div>
                                <div class="text-bold">Digital</div>
                            </li>
                            <li>
                            	<div class="text-light-grey">Fuel Guage</div>
                                <div class="text-bold">Yes</div>
                            </li>
                            <li>
                            	<div class="text-light-grey">Tachometer Type</div>
                                <div class="text-bold">Analog</div>
                            </li>
                            <li>
                            	<div class="text-light-grey">Digital Fuel Guage</div>
                                <div class="text-bold">No</div>
                            </li>
                            <li>
                            	<div class="text-light-grey">Tripmeter</div>
                                <div class="text-bold">No</div>
                            </li>
                            <li>
                            	<div class="text-light-grey">Electric Start</div>
                                <div class="text-bold">Yes</div>
                            </li>
                            <div class="clear"></div>
                        </ul>
                    </div>
                    <div class="or-text"><div class="more-features-btn"><span>+</span></div></div>
                </div>
                
                
                <!-- variant code starts here -->
                <div class="bw-tabs-data" id="variants">
                	<h2 class="text-center margin-top30 margin-bottom20 text-center <%= (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0) ? "" : "hide" %>">Variants</h2>
                    <asp:Repeater ID="rptVarients" runat="server">
                        <ItemTemplate>
                        <div>
                    	    <div class="border-solid content-inner-block-10 margin-bottom20">
                        	    <div class="grid-8 alpha">
                            	    <h3 class="font16 margin-bottom10"><%# DataBinder.Eval(Container.DataItem, "VersionName") %></h3>
                            	    <%--<p class="font14">220 CC, 38 Kmpl, 103 bhp @ 11000 rpm</p>--%>
                                    <p class="font14"><%# DataBinder.Eval(Container.DataItem, "BrakeType") %>, <%# DataBinder.Eval(Container.DataItem, "AlloyWheels") %>, <%# DataBinder.Eval(Container.DataItem, "ElectricStart") %>, <%# DataBinder.Eval(Container.DataItem, "AntilockBrakingSystem") %></p>
                                </div>
                                <div class="grid-4 alpha omega">
                            	    <p class="font16 margin-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "Price"))) %></p>
                            	    <p class="font12 text-light-grey">Ex-showroom, <%= Bikewale.Common.Configuration.GetDefaultCityName %></p>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <div class="clear"></div>
                </div>
				
                <!-- colours code starts here -->
                <div class="colours-wrap margin-bottom20 <%= modelPage.ModelColors != null && modelPage.ModelColors.ToList().Count > 0 ? "" : "hide" %>">
                    <h2 class="margin-top30 margin-bottom20 text-center">Colours</h2>
                    <div class="jcarousel-wrapper">
                        <div class="jcarousel">
                            <ul class="text-center">
                                <asp:Repeater ID="rptColors" runat="server">
                                    <ItemTemplate>
                                    <li class="available-colors">
                                        <div class="color-box" style="background-color: #<%# DataBinder.Eval(Container.DataItem, "HexCode")%>;"></div>
                                        <p class="font16 text-medium-grey"><%# DataBinder.Eval(Container.DataItem, "ColorName") %></p>
                                    </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <%--<li class="available-colors">
                                    <div class="color-box blue"></div>
                                    <p class="font16 text-medium-grey">Dazzling Blue</p>
                                </li>
                                <li class="available-colors">
                                    <div class="color-box red"></div>
                                    <p class="font16 text-medium-grey">Dazzling Red</p>
                                </li>
                                <li class="available-colors">
                                    <div class="color-box blue"></div>
                                    <p class="font16 text-medium-grey">Californoia Blue</p>
                                </li>
                                <li class="available-colors">
                                    <div class="color-box red"></div>
                                    <p class="font16 text-medium-grey">Dazzling Red</p>
                                </li>
                                <li class="available-colors">
                                    <div class="color-box red"></div>
                                    <p class="font16 text-medium-grey">Dazzling Red</p>
                                </li>--%>
                            </ul>
                         </div>
                         <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                        <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                        <p class="text-center jcarousel-pagination"></p>

                      </div>
                </div>
               
            </div>
        </div>
    </section>
    
    <section><!--  News, reviews and videos code starts here -->
        <div class="container">
        	<div class="grid-12">
                <h2 class="text-center margin-top30 margin-bottom20">Latest Updates</h2>
                <div class="bw-tabs-panel">
                    <div class="bw-tabs margin-bottom15">
                    	<div class="form-control-box">                        	
                            <select class="form-control">
                                <option class=" <%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0) ? "" : "hide" %> active" value="ctrlNews">News</option>
                                <option class="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0) ? "" : "hide" %>"  value="ctrlExpertReviews">Reviews</option>
                                <option class="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0) ? "" : "hide" %>" value="ctrlVideos">Videos</option>
                            </select>
                        </div>
                    </div>
                    <BW:News runat="server" ID="ctrlNews"/>
                    <BW:ExpertReviews runat="server" ID="ctrlExpertReviews"/>
                    <BW:Videos runat="server" ID="ctrlVideos"/>                    
                </div>        
        	</div>
            <div class="clear"></div>
        </div>
    </section>

    
    <section>
    	<div class="container margin-bottom30">
        	<div class="grid-12">
    		<!-- Most Popular Bikes Starts here-->
			<h2 class="margin-top30px margin-bottom20 text-center"><%= bikeName %> alternatives</h2>
            
            <div class="jcarousel-wrapper discover-bike-carousel">
                <div class="jcarousel">
                <BW:AlternateBikes ID="ctrlAlternateBikes" runat="server" />
              </div>
                <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                <p class="text-center jcarousel-pagination"></p>
             </div>  
        
			</div>
            <div class="clear"></div>
    	</div>
    </section>
    <BW:MPopupWidget runat="server" ID="MPopupWidget1" />
    <!-- #include file="/includes/footerBW_Mobile.aspx" -->
    <!-- all other js plugins -->
    <!-- #include file="/includes/footerscript_Mobile.aspx" -->
    <script type="text/javascript" src="/m/src/bwm-model.js"></script>
</form>
</body>
</html>
