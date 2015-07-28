<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.BikeModels" Trace="false" %>

<%@ Register TagPrefix="UR" TagName="Reviews" Src="/m/controls/TopUserReviews.ascx" %>
<%@ Register TagPrefix="PW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>

<%@ Import Namespace="Bikewale.Common" %>
<%
    title = objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " Price in India, Review, Mileage & Photos - Bikewale";
    description = objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " Price in India - Rs." + formattedPrice + ". Check out " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " on road price, reviews, mileage, variants, news & photos at Bikewale.";
    canonical = "http://www.bikewale.com/" + objModelEntity.MakeBase.MaskingName + "-bikes/" + objModelEntity.MaskingName + "/";
    AdPath = "/1017752/Bikewale_Mobile_Model";
    AdId = "1398837216327";
    menu = "2";
    ShowTargeting = "1";
    TargetedModel = objModelEntity.ModelName.Trim();
    AdModel_300x250 = "1";
%>



<!-- #include file="/includes/headermobile.aspx" -->

<PW:MPopupWidget runat="server" ID="MPopupWidget" />

<script type="text/javascript" src="http://st2.aeplcdn.com/bikewale/src/BikeWaleCommon.js?v=3.2"></script>
<script type="text/javascript" src="/m/src/placeholder.js?v=1.0"></script>
<style type="text/css">
    .modelColorBox {
        margin: auto;
        width: 80px;
        height: 20px;
        border: 1px solid #C1C1C1;
    }

    .fullWidth {
        width: 100%;
        max-width: 100%;
        height: auto;
    }

    .linkButtonBigBlue {
        height: 35px;
        background-image: url('/m/images/nav_blue.jpg');
        border-radius: 7px;
        -moz-border-radius: 7px;
        -webkit-border-radius: 7px;
        text-align: center;
        line-height: 35px;
        color: #ffffff;
        text-decoration: none;
        font-weight: bold;
        font-size: 14px;
    }

    .linkButtonBigBlueNew {
        height: 35px;
        background-image: url('/m/images/blue_button.jpg');
        border-radius: 7px;
        -moz-border-radius: 7px;
        -webkit-border-radius: 7px;
        text-align: center;
        line-height: 35px;
        color: #ffffff;
        text-decoration: none;
        font-weight: bold;
        font-size: 14px;
    }

    .linkButtonBigRed {
        height: 35px;
        background-image: url('/m/images/red_button.jpg');
        border-radius: 7px;
        -moz-border-radius: 7px;
        -webkit-border-radius: 7px;
        text-align: center;
        line-height: 35px;
        color: #ffffff;
        text-decoration: none;
        font-weight: bold;
        font-size: 14px;
    }

    .tblData {
        width: 100%;
    }

        .tblData td {
            border-bottom: 1px solid #DEDEDE;
            padding: 8px 5px;
        }

    .socialplugins li {
        float: left;
        width: 84px;
    }
</style>

<form id="form1" runat="server">
    <div data-role="popup" id="requestEmi" class="ui-content new-line15" style="height: 100%;">
        <h3 class="pgsubhead">Free Bike Loan Assistance</h3>
        <a href="#" data-rel="back" data-role="button" data-theme="b" data-icon="delete" data-iconpos="notext" class="ui-btn-right" id="closeBox">Close</a>
        <div class="box1">
            <div style="float: right"><span class="f-10"><font color="red">*</font><i>All fields are mandatory</i></span></div>
            <div style="clear: both"></div>
            <div class="new-line15">
                <asp:textbox id="txtName" runat="server" placeholder="Your Name*" />
            </div>
            <div class="new-line15">
                <asp:textbox id="txtEmail" runat="server" type="email" placeholder="Your Email*" />
            </div>
            <div class="new-line15">
                <input id="numMobile" type="Tel" placeholder="Mobile No.*" maxlength="10" />
            </div>
            <asp:dropdownlist id="ddlStates" runat="server" class="textAlignLeft" />
            <select id="ddlCity" class="textAlignLeft">
                <option value="0">--Select City--</option>
            </select>
            <asp:button text="Submit" data-theme="b" id="btnSubmit" data-role="button" class="ui-corner-all" runat="server" />
            <div>
                <p class="f-10">
                    By clicking on the button above, I agree with BikeWale <a href="/visitoragreement.aspx" target="_blank">Visitor Agreement</a> and privacy policy. By providing your contact details you agree to be contacted for assistance in your bike buying by us and/or any of our partners including dealers, bike manufacturers, banks like HDFC bank etc. 
                </p>
            </div>
            <div>
                <span id="spnError" class="error"><%=errMsg %></span>
            </div>
        </div>
    </div>
    <div class="padding5">
        <div id="br-cr" itemscope itemtype="http://data-vocabulary.org/Breadcrumb">
            <a itemprop='url' href='/m/' class="normal"><span itemprop="title">Home</span></a> &rsaquo; 
        <a itemprop='url' href='/m/<%= objModelEntity.MakeBase.MaskingName %>-bikes/' class="normal"><span itemprop="title"><%= objModelEntity.MakeBase.MakeName %></span></a> &rsaquo; 
            <%--        <%if (modelCount > 1)
          { %>       
            <a itemprop="url" href='/m/<%= objModelEntity.MakeBase.MaskingName %>-bikes/<%= objModelEntity.ModelSeries.MaskingName %>-series/' class="normal"><span itemprop="title"><%= objModelEntity.ModelSeries.SeriesName %></span></a> &rsaquo;      
        <%} %>--%>
            <span class="lightgray" itemprop="title"><%= objModelEntity.ModelName %></span>
        </div>
        <%if (!objModelEntity.Futuristic)
          {
              if (objModelEntity.ReviewCount > 0)
              {%>
        <div itemtype="http://data-vocabulary.org/Review-aggregate" itemscope>
            <h1 itemprop="itemreviewed"><%= objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName %> Bikes<% if (!objModelEntity.New && objModelEntity.Used)
                                                                                                                         { %><span style="color: #ff0000;"> (Discontinued)</span><% } %></h1>
            <div id="divModel" class="box1 new-line5">
                <div class="container10" style="text-decoration: none;">
                    <table style="width: 100%;" cellpadding="0" cellspacing="0">
                        <tbody>
                            <tr>
                                <td style="width: 100px; vertical-align: top;">
                                    <img itemprop="photo" title="<%= objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName %>" alt="<%= objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName %>" src="<%= MakeModelVersion.GetModelImage(objModelEntity.HostUrl, "/bikewaleimg/models/" + objModelEntity.SmallPicUrl) %>" width="90" />
                                </td>
                                <td valign="top">
                                    <div class="darkgray"><%=objModelEntity.New ? "Ex-Showroom Price: " : "Last Recorded Price: " %><%= Bikewale.Common.Configuration.GetDefaultCityName %></div>
                                    <div class="darkgray new-line">
                                        <h2><b>Rs. <%= CommonOpn.FormatPrice(objModelEntity.MinPrice.ToString()) %></b></h2>
                                        <span class="<%= (objModelEntity.MinPrice == 0) || (!objModelEntity.New && objModelEntity.Used) ? "hide" : "f-12" %>"><a id="emiCalculator" href="#requestEmi" data-rel="popup" data-position-to="window" style="text-decoration: underline">Get EMI Assistance</a></span>
                                    </div>
                                    <div class="darkgray new-line5">Avg User Rating</div>
                                    <div class="darkgray new-line">
                                        <div><%= CommonOpn.GetRateImage( objModelEntity.ReviewRate) %></div>
                                        <div itemprop='rating' itemscope itemtype='http://data-vocabulary.org/Rating'>
                                            <span itemprop='value'><%=objModelEntity.ReviewRate%></span> /
                                    <span itemprop='best'>5</span>
                                        </div>
                                        <div>
                                            <% if (objModelEntity.ReviewCount > 0)
                                               {%>
                                        Based on <a id="review" href='/m/<%=objModelEntity.MakeBase.MaskingName %>-bikes/<%= objModelEntity.MaskingName %>/user-reviews/' class="normal" title="<%= objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName %> Reviews"><span itemprop='votes' style="position: relative; top: 2px;"><%= objModelEntity.ReviewCount %> reviews</span></a>
                                            <% }
                                               else
                                               {%>
                                            <span itemprop='votes' style="position: relative; top: 2px;"><%= objModelEntity.ReviewCount %> reviews</span>
                                            <%} %>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <ul class="socialplugins  new-line10">
                                        <li>
                                            <fb:like href="http://www.bikewale.com/<%= objModelEntity.MakeBase.MaskingName%>-bikes/<%= objModelEntity.MaskingName %>/" send="false" layout="button_count" show_faces="false"></fb:like>
                                        </li>
                                        <li><a href="https://twitter.com/share" class="twitter-share-button" data-url="http://www.bikewale.com/<%= objModelEntity.MakeBase.MaskingName%>-bikes/<%= objModelEntity.MaskingName %>/" data-via='<%= title %>' data-lang="en">Tweet</a></li>
                                        <li>
                                            <div class="g-plusone" data-size="medium" data-href="http://www.bikewale.com/<%= objModelEntity.MakeBase.MaskingName%>-bikes/<%= objModelEntity.MaskingName %>/"></div>
                                        </li>
                                        <li style="clear: both;"></li>
                                    </ul>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class='<%=objModelEntity.New ? "new-line10" : "hide" %>'>
                    <a class="fillPopupData" modelId="<%= modelId %>" href="/m/pricequote/default.aspx?model=<%= modelId %>" data-role="button" data-theme="b" data-mini="true" style="color: #fff !important;">
                        Check On-Road Price
                    </a>
                </div>
            </div>
        </div>
        <%}
              else
              { %>
        <div>
            <h1><%= objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName %> Bikes<% if (!objModelEntity.New && objModelEntity.Used)
                                                                                                 { %><span style="color: #ff0000;"> (Discontinued)</span><% } %></h1>
            <div id="divModelNotRated" class="box1 new-line5">
                <div class="container10" style="text-decoration: none;">
                    <table style="width: 100%;" cellpadding="0" cellspacing="0">
                        <tbody>
                            <tr>
                                <td style="width: 100px; vertical-align: top;">
                                    <img itemprop="photo" title="<%= objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName %>" alt="<%= objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName %>" src="<%= MakeModelVersion.GetModelImage(objModelEntity.HostUrl, "/bikewaleimg/models/" + objModelEntity.SmallPicUrl) %>" width="90" />
                                </td>
                                <td valign="top">
                                    <div class="darkgray"><%=objModelEntity.New ? "Ex-Showroom Price: " : "Last Recorded Price: " %><%= Bikewale.Common.Configuration.GetDefaultCityName %></div>
                                    <div class="darkgray new-line">
                                        <h2><b>Rs. <%= CommonOpn.FormatPrice(objModelEntity.MinPrice.ToString()) %></b></h2>
                                        <span class="<%= (objModelEntity.MinPrice == 0) || (!objModelEntity.New && objModelEntity.Used) ? "hide" : "f-12" %>"><a id="emiCalculator" href="#requestEmi" data-rel="popup" data-position-to="window" style="text-decoration: underline">Get EMI Assistance</a></span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <ul class="socialplugins  new-line10">
                                        <li>
                                            <fb:like href="http://www.bikewale.com/<%= objModelEntity.MakeBase.MaskingName%>-bikes/<%= objModelEntity.MaskingName %>/" send="false" layout="button_count" show_faces="false"></fb:like>
                                        </li>
                                        <li><a href="https://twitter.com/share" class="twitter-share-button" data-url="http://www.bikewale.com/<%= objModelEntity.MakeBase.MaskingName%>-bikes/<%= objModelEntity.MaskingName %>/" data-via='<%= title %>' data-lang="en">Tweet</a></li>
                                        <li>
                                            <div class="g-plusone" data-size="medium" data-href="http://www.bikewale.com/<%= objModelEntity.MakeBase.MaskingName%>-bikes/<%= objModelEntity.MaskingName %>/"></div>
                                        </li>
                                        <li style="clear: both;"></li>
                                    </ul>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class='<%=objModelEntity.New ? "new-line10" : "hide" %>'>
                    <a class="fillPopupData" modelId="<%= modelId %>" href="/m/pricequote/default.aspx?model=<%= modelId %>" data-role="button" data-theme="b" data-mini="true" style="color: #fff !important;">
                        Check On-Road Price
                    </a>
                </div>
            </div>
        </div>

        <% }
          }
          else
          { %>
        <div>
            <h1><%= objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName %> Bikes<span style="color: #ff0000;"> (Upcoming)</span></h1>
            <div id="divFuturistic" class="box1 new-line5">
                <div class="container10" style="text-decoration: none;">
                    <table style="width: 100%;" cellpadding="0" cellspacing="0">
                        <tbody>

                            <%if (objUpcomingBike != null)
                              { %>
                            <tr>
                                <td style="width: 100px; vertical-align: top;">
                                    <img title="<%= objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName %>" alt="<%= objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName %>" src="<%= Bikewale.Common.ImagingFunctions.GetPathToShowImages(objUpcomingBike.LargePicImagePath,objUpcomingBike.HostUrl) %>" width="90" />
                                </td>
                                <td valign="top">
                                    <div class="darkgray">Expected Launch:</div>
                                    <div class="darkgray new-line"><b><%= objUpcomingBike.ExpectedLaunchDate %></b></div>
                                    <div class="darkgray new-line5">Expected Price:</div>
                                    <div class="darkgray new-line"><b>Rs. <%=Bikewale.Common.CommonOpn.FormatPrice(objUpcomingBike.EstimatedPriceMin.ToString(),objUpcomingBike.EstimatedPriceMax.ToString()) %></b></div>
                                </td>
                            </tr>
                            <%} %>
                            <tr>
                                <td colspan="2">
                                    <ul class="socialplugins  new-line10">
                                        <li>
                                            <fb:like href="http://www.bikewale.com/<%= objModelEntity.MakeBase.MaskingName%>-bikes/<%= objModelEntity.MaskingName %>/" send="false" layout="button_count" show_faces="false"></fb:like>
                                        </li>
                                        <li><a href="https://twitter.com/share" class="twitter-share-button" data-url="http://www.bikewale.com/<%= objModelEntity.MakeBase.MaskingName%>-bikes/<%= objModelEntity.MaskingName %>/" data-via='<%= title %>' data-lang="en">Tweet</a></li>
                                        <li>
                                            <div class="g-plusone" data-size="medium" data-href="http://www.bikewale.com/<%= objModelEntity.MakeBase.MaskingName%>-bikes/<%= objModelEntity.MaskingName %>/"></div>
                                        </li>
                                        <li style="clear: both;"></li>
                                    </ul>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <%} %>

        <div class="box1 new-line5">
            <!-- Bikewale_Mobile_Model_300x250 -->
            <div id='div-gpt-ad-1398837216327-2' style='width: 300px; height: 250px;'>
                <script type='text/javascript'>
                    googletag.cmd.push(function () { googletag.display('div-gpt-ad-1398837216327-2'); });
                </script>
            </div>
        </div>
        <%if (!objModelEntity.Futuristic && versionCount > 0)
          {%>
        <h2 class="pgsubhead"><%= objModelEntity.ModelName %> Versions</h2>
        <div id="divVersions" class="box1 new-line5" style="padding: 0px 5px;">
            <asp:repeater id="rptVersions" runat="server">
            <itemtemplate>
	        <div class="container">
		        <div class="sub-heading">
			        <b><%# DataBinder.Eval(Container.DataItem, "VersionName") %></b>
		        </div>
		        <div class="darkgray new-line5">
			       <%=objModelEntity.New ? "Ex-Showroom Price: " : "Last Recorded Price: " %><b>Rs. <%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem, "Price").ToString()) %></b>
		        </div>
	        </div>
            </itemtemplate>
        </asp:repeater>
        </div>
        <% } %>
        <% if (!objModelEntity.Futuristic && !String.IsNullOrEmpty(objDesc.SmallDescription))
           { %>
        <h2 class="pgsubhead"><%= objModelEntity.ModelName %> Review</h2>
        <div id="divShortSynopsis" class="box1 new-line5">
            <div id="divShortDescContent">
                <%= objDesc.SmallDescription %>
            </div>
            <div class="new-line5 darkblue">
                <span id="spnShortSynopsis">Read full review&nbsp;&nbsp;<span class="arr-small">&raquo;</span></span>
            </div>
        </div>
        <div id="divFullSynopsis" class="box1 new-line5" style="display: none;">
            <div>
                <%= objDesc.FullDescription %>
            </div>
            <div class="new-line5 darkblue">
                <span id="spnFullSynopsis">Hide full review&nbsp;&nbsp;<span class="arr-small">&raquo;</span></span>
            </div>
        </div>
        <% } %>
        <% if (!String.IsNullOrEmpty(objDesc.FullDescription) && objModelEntity.Futuristic)
           { %>
        <h2 class="pgsubhead"><%= objModelEntity.ModelName %> Review</h2>
        <div id="divFullSyn" class="box1 new-line5">
            <div>
                <%= objDesc.FullDescription %>
            </div>
        </div>
        <% } %>

        <% if (!objModelEntity.Futuristic && versionCount > 0)
           { %>
        <% if (objSpecs != null)
           { %><h2 class="pgsubhead"><%= objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " " + versionName %> Specifications</h2>
        <% } %>
        <% if ((int)ViewState["VersionCount"] > 1)
           { %>
        <span id="changeVer" class="ui-btn ui-icon-carat-d ui-btn-icon-right ui-corner-all ui-mini ui-btn-inline ui-btn-a normal">Change Version</span>
        <div class="hide" id="divDrpVersion">
            <asp:dropdownlist id="drpVersion" runat="server" autopostback="true"></asp:dropdownlist>
        </div>
        <% } %>
        <% if (objSpecs != null)
           { %>
        <a class="normal" id="Features">
            <div class="box1 new-line5">
                <table class="table" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="head-big ui-icon ui-icon-carat-d ui-icon-shadow ui-btn-icon-right">Features</div>
                        </td>
                    </tr>
                </table>
            </div>
        </a>
        <div class="box1 new-line5" id="divFeatures">
            <table cellspacing="0" class="tblData">
                <tr>
                    <td style="width: 200px;">Speedometer</td>
                    <td><%= ShowNotAvailable(objSpecs.Speedometer) %></td>
                </tr>
                <tr>
                    <td>Tachometer</td>
                    <td><%=GetFeatures(objSpecs.Tachometer.ToString()) %></td>
                </tr>
                <tr>
                    <td>Tachometer Type</td>
                    <td><%= ShowNotAvailable(objSpecs.TachometerType) %></td>
                </tr>
                <tr>
                    <td>Shift Light</td>
                    <td><%=GetFeatures(objSpecs.ShiftLight.ToString()) %></td>
                </tr>
                <tr>
                    <td>Electric Start</td>
                    <td><%=GetFeatures(objSpecs.ElectricStart.ToString()) %></td>
                </tr>
                <tr>
                    <td>Tripmeter</td>
                    <td><%=GetFeatures(objSpecs.Tripmeter.ToString()) %></td>
                </tr>
                <tr>
                    <td>No Of Tripmeters</td>
                    <td><%=ShowNotAvailable(objSpecs.NoOfTripmeters) %></td>
                </tr>
                <tr>
                    <td>Tripmeter Type</td>
                    <td><%=ShowNotAvailable(objSpecs.TripmeterType) %></td>
                </tr>
                <tr>
                    <td>Low Fuel Indicator</td>
                    <td><%=GetFeatures(objSpecs.LowFuelIndicator.ToString()) %></td>
                </tr>
                <tr>
                    <td>Low Oil Indicator</td>
                    <td><%=GetFeatures(objSpecs.LowOilIndicator.ToString()) %></td>
                </tr>
                <tr>
                    <td>Low Battery Indicator</td>
                    <td><%=GetFeatures(objSpecs.LowBatteryIndicator.ToString()) %></td>
                </tr>
                <tr>
                    <td>Fuel Gauge</td>
                    <td><%=GetFeatures(objSpecs.FuelGauge.ToString()) %></td>
                </tr>
                <tr>
                    <td>Digital Fuel Gauge</td>
                    <td><%=GetFeatures(objSpecs.DigitalFuelGauge.ToString()) %></td>
                </tr>
                <tr>
                    <td>Pillion Seat</td>
                    <td><%=GetFeatures(objSpecs.PillionSeat.ToString()) %></td>
                </tr>
                <tr>
                    <td>Pillion Footrest</td>
                    <td><%=GetFeatures(objSpecs.PillionFootrest.ToString()) %></td>
                </tr>
                <tr>
                    <td>Pillion Backrest</td>
                    <td><%=GetFeatures(objSpecs.PillionBackrest.ToString()) %></td>
                </tr>
                <tr>
                    <td>Pillion Grabrail</td>
                    <td><%=GetFeatures(objSpecs.PillionGrabrail.ToString()) %></td>
                </tr>
                <tr>
                    <td>Stand Alarm</td>
                    <td><%=GetFeatures(objSpecs.StandAlarm.ToString()) %></td>
                </tr>
                <tr>
                    <td>Stepped Seat</td>
                    <td><%=GetFeatures(objSpecs.SteppedSeat.ToString()) %></td>
                </tr>
                <tr>
                    <td>Antilock Braking System</td>
                    <td><%=GetFeatures(objSpecs.AntilockBrakingSystem.ToString()) %></td>
                </tr>
                <tr>
                    <td>Killswitch</td>
                    <td><%=GetFeatures(objSpecs.Killswitch.ToString()) %></td>
                </tr>
                <tr>
                    <td>Clock</td>
                    <td><%=GetFeatures(objSpecs.Clock.ToString()) %></td>
                </tr>
            </table>
        </div>
        <a class="normal" id="Specs">
            <div class="box1 new-line5">
                <table class="table" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="head-big ui-icon ui-icon-carat-d ui-icon-shadow ui-btn-icon-right">Specification</div>
                        </td>
                    </tr>
                </table>
            </div>
        </a>
        <div class="box1 new-line5 hide" id="divSpecs">
            <div class="box1 new-line5 bot-rad-0">
                <div><b>Engine</b></div>
            </div>
            <div class="box-bot" style="padding: 0px 5px;">
                <table class="tblData">
                    <tr>
                        <td style="width: 200px;">Displacement (cc)</td>
                        <td><%=ShowNotAvailable(objSpecs.Displacement.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Cylinders</td>
                        <td><%=ShowNotAvailable(objSpecs.Cylinders.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Max Power</td>
                        <td><%=ShowNotAvailable(objSpecs.MaxPower.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Maximum Torque</td>
                        <td><%=ShowNotAvailable(objSpecs.MaximumTorque.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Bore (mm)</td>
                        <td><%=ShowNotAvailable(objSpecs.Bore.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Stroke (mm)</td>
                        <td><%=ShowNotAvailable(objSpecs.Stroke.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Valves Per Cylinder</td>
                        <td><%=ShowNotAvailable(objSpecs.ValvesPerCylinder.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Fuel Delivery System</td>
                        <td><%=ShowNotAvailable(objSpecs.FuelDeliverySystem) %></td>
                    </tr>
                    <tr>
                        <td>Fuel Type</td>
                        <td><%=ShowNotAvailable(objSpecs.FuelType) %></td>
                    </tr>
                    <tr>
                        <td>Ignition</td>
                        <td><%=ShowNotAvailable(objSpecs.Ignition) %></td>
                    </tr>
                    <tr>
                        <td>Spark Plugs (Per Cylinder)</td>
                        <td><%=ShowNotAvailable(objSpecs.SparkPlugsPerCylinder) %></td>
                    </tr>
                    <tr>
                        <td>Cooling System</td>
                        <td><%=ShowNotAvailable(objSpecs.CoolingSystem) %></td>
                    </tr>
                </table>
            </div>

            <div class="box1 new-line5 bot-rad-0">
                <div><b>Transmission</b></div>
            </div>
            <div class="box-bot" style="padding: 0px 5px;">
                <table class="tblData">
                    <tr>
                        <td style="width: 200px;">Gearbox Type</td>
                        <td><%=ShowNotAvailable(objSpecs.GearboxType) %></td>
                    </tr>
                    <tr>
                        <td>No Of Gears</td>
                        <td><%=ShowNotAvailable(objSpecs.NoOfGears.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Transmission Type</td>
                        <td><%=ShowNotAvailable(objSpecs.TransmissionType) %></td>
                    </tr>
                    <tr>
                        <td>Clutch</td>
                        <td><%=ShowNotAvailable(objSpecs.Clutch) %></td>
                    </tr>
                </table>
            </div>

            <div class="box1 new-line5 bot-rad-0">
                <div><b>Dimensions &amp; Weight</b></div>
            </div>
            <div class="box-bot" style="padding: 0px 5px;">
                <table class="tblData">
                    <tr>
                        <td style="width: 200px;">Kerb Weight (Kg)</td>
                        <td><%=ShowNotAvailable(objSpecs.KerbWeight.ToString())%></td>
                    </tr>
                    <tr>
                        <td>Overall Length (mm)</td>
                        <td><%=ShowNotAvailable(objSpecs.OverallLength.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Overall Width (mm)</td>
                        <td><%=ShowNotAvailable(objSpecs.OverallWidth.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Overall Height (mm)</td>
                        <td><%=ShowNotAvailable(objSpecs.OverallHeight.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Wheelbase (mm)</td>
                        <td><%=ShowNotAvailable(objSpecs.Wheelbase.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Ground Clearance (mm)</td>
                        <td><%=ShowNotAvailable(objSpecs.GroundClearance.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Seat Height (mm)</td>
                        <td><%=ShowNotAvailable(objSpecs.SeatHeight.ToString()) %></td>
                    </tr>
                </table>
            </div>

            <div class="box1 new-line5 bot-rad-0">
                <div><b>Fuel Efficiency &amp; Range</b></div>
            </div>
            <div class="box-bot" style="padding: 0px 5px;">
                <table class="tblData">
                    <tr>
                        <td style="width: 200px;">Fuel Tank Capacity (Litres)</td>
                        <td><%=ShowNotAvailable(objSpecs.FuelTankCapacity.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Reserve Fuel Capacity (Litres)</td>
                        <td><%=ShowNotAvailable(objSpecs.ReserveFuelCapacity.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>FuelEfficiency Overall (Kmpl)</td>
                        <td><%=ShowNotAvailable(objSpecs.FuelEfficiencyOverall.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Fuel Efficiency Range (Km)</td>
                        <td><%=ShowNotAvailable(objSpecs.FuelEfficiencyRange.ToString()) %></td>
                    </tr>
                </table>
            </div>
            <div class="box1 new-line5 bot-rad-0">
                <div><b>Chassis&nbsp;&amp; Suspension</b></div>
            </div>
            <div class="box-bot" style="padding: 0px 5px;">
                <table class="tblData">
                    <tr>
                        <td style="width: 200px;">Chassis Type</td>
                        <td><%=ShowNotAvailable(objSpecs.ChassisType) %></td>
                    </tr>
                    <tr>
                        <td>Front Suspension</td>
                        <td><%=ShowNotAvailable(objSpecs.FrontSuspension) %></td>
                    </tr>
                    <tr>
                        <td>Rear Suspension</td>
                        <td><%=ShowNotAvailable(objSpecs.RearSuspension) %></td>
                    </tr>
                </table>
            </div>
            <div class="box1 new-line5 bot-rad-0">
                <div><b>Braking</b></div>
            </div>
            <div class="box-bot" style="padding: 0px 5px;">
                <table class="tblData">
                    <tr>
                        <td style="width: 200px;">Brake Type</td>
                        <td><%=ShowNotAvailable(objSpecs.BrakeType) %></td>
                    </tr>
                    <tr>
                        <td>Front Disc</td>
                        <td><%=GetFeatures(objSpecs.FrontDisc.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Front Disc/Drum Size (mm)</td>
                        <td><%=ShowNotAvailable(objSpecs.FrontDisc_DrumSize.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Rear Disc</td>
                        <td><%=GetFeatures(objSpecs.RearDisc.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Rear Disc/Drum Size (mm)</td>
                        <td><%=ShowNotAvailable(objSpecs.RearDisc_DrumSize.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Calliper Type</td>
                        <td><%=ShowNotAvailable(objSpecs.CalliperType) %></td>
                    </tr>
                </table>
            </div>
            <div class="box1 new-line5 bot-rad-0">
                <div><b>Wheels &amp; Tyres</b></div>
            </div>
            <div class="box-bot" style="padding: 0px 5px;">
                <table class="tblData">
                    <tr>
                        <td style="width: 200px;">Wheel Size (inches)</td>
                        <td><%=ShowNotAvailable(objSpecs.WheelSize.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Front Tyre</td>
                        <td><%=ShowNotAvailable(objSpecs.FrontTyre) %></td>
                    </tr>
                    <tr>
                        <td>Rear Tyre</td>
                        <td><%=ShowNotAvailable(objSpecs.RearTyre) %></td>
                    </tr>
                    <tr>
                        <td>Tubeless Tyres</td>
                        <td><%=GetFeatures(objSpecs.TubelessTyres.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Radial Tyres</td>
                        <td><%=GetFeatures(objSpecs.RadialTyres.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Alloy Wheels</td>
                        <td><%=GetFeatures(objSpecs.AlloyWheels.ToString()) %></td>
                    </tr>
                </table>
            </div>
            <div class="box1 new-line5 bot-rad-0">
                <div><b>Electricals</b></div>
            </div>
            <div class="box-bot" style="padding: 0px 5px;">
                <table class="tblData">
                    <tr>
                        <td style="width: 200px;">Electric System</td>
                        <td><%=ShowNotAvailable(objSpecs.ElectricSystem) %></td>
                    </tr>
                    <tr>
                        <td>Battery</td>
                        <td><%=ShowNotAvailable(objSpecs.Battery) %></td>
                    </tr>
                    <tr>
                        <td>Headlight Type</td>
                        <td><%=ShowNotAvailable(objSpecs.HeadlightType) %></td>
                    </tr>
                    <tr>
                        <td>Headlight Bulb Type</td>
                        <td><%=ShowNotAvailable(objSpecs.HeadlightBulbType) %></td>
                    </tr>
                    <tr>
                        <td>Brake/Tail Light</td>
                        <td><%=ShowNotAvailable(objSpecs.Brake_Tail_Light) %></td>
                    </tr>
                    <tr>
                        <td>Turn Signal</td>
                        <td><%=ShowNotAvailable(objSpecs.TurnSignal) %></td>
                    </tr>
                    <tr>
                        <td>Pass Light</td>
                        <td><%=GetFeatures(objSpecs.PassLight.ToString()) %></td>
                    </tr>
                </table>
            </div>
            <div class="box1 new-line5 bot-rad-0">
                <div><b>Performance</b></div>
            </div>
            <div class="box-bot" style="padding: 0px 5px;">
                <table class="tblData">
                    <tr>
                        <td style="width: 200px;">0 to 60 kmph (Seconds)</td>
                        <td><%=ShowNotAvailable(objSpecs.Performance_0_60_kmph.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>0 to 80 kmph (Seconds)</td>
                        <td><%=ShowNotAvailable(objSpecs.Performance_0_80_kmph.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>0 to 40 m (Seconds)</td>
                        <td><%=ShowNotAvailable(objSpecs.Performance_0_40_m.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>Top Speed (Kmph)</td>
                        <td><%=ShowNotAvailable(objSpecs.TopSpeed.ToString()) %></td>
                    </tr>
                    <tr>
                        <td>60 to 0 Kmph (Seconds, metres)</td>
                        <td><%=ShowNotAvailable(objSpecs.Performance_60_0_kmph) %></td>
                    </tr>
                    <tr>
                        <td>80 to 0 kmph (Seconds, metres)</td>
                        <td><%=ShowNotAvailable(objSpecs.Performance_80_0_kmph)%></td>
                    </tr>
                </table>
            </div>
        </div>

        <a class="normal" id="Colors">
            <div class="box1 new-line5">
                <table class="table" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="head-big ui-icon ui-icon-carat-d ui-icon-shadow ui-btn-icon-right">Colors</div>
                        </td>
                    </tr>
                </table>
            </div>
        </a>
        <div class="box1 new-line5 hide" id="divColors">
            <table>
                <tr>
                    <td><%=ShowNotAvailable(objSpecs.Colors) %></td>
                </tr>
            </table>
        </div>
        <% } %>
        <% } %>

        <UR:Reviews ID="reviewList" runat="server"></UR:Reviews>
    </div>

    <script type="text/javascript">
        $(function () { $('.m-carousel').carousel(); });
        $(document).ready(function () {
            customerName = '';
            customerEmail = '';
            customerMobile = '';
            customerCity = '';
            customerState = '';
            modelId = '<%= modelId%>';

            $("#ddlCity").val("0").attr("disabled", true);

            $("#Specs,#Features,#Colors").click(function () {
                if ($("#div" + $(this).attr("id")).hasClass("hide")) {
                    $("#divSpecs,#divFeatures,#divColors").addClass("hide").slideUp();
                    $("#div" + $(this).attr("id")).slideDown();
                    $("#div" + $(this).attr("id")).removeClass("hide");
                }
                else {
                    $("#div" + $(this).attr("id")).addClass("hide");
                    $("#div" + $(this).attr("id")).slideUp();

                }
            });

            $("#spnShortSynopsis").click(function () {
                $("#divShortSynopsis").hide();
                $("#divFullSynopsis").show();
            });

            $("#spnFullSynopsis").click(function () {
                $("#divFullSynopsis").hide();
                $("#divShortSynopsis").show();
            });

            $("#changeVer").click(function () {
                if ($("#divDrpVersion").hasClass("hide"))
                    $("#divDrpVersion").removeClass("hide");
                else
                    $("#divDrpVersion").addClass("hide");
            });

            $("#drpVersion").selectmenu('refresh', true);

        });

        $("#closeBox").click(function () {
            $("#spnError").html("");
            document.body.style.overflow = "visible";

        });

        $("#emiCalculator").click(function () {
            document.body.style.overflow = "hidden";
        });

        $("#ddlStates").change(function () {
            stateId = $(this).val();

            $("#ddlCity").val(0);
            $("#ddlCity").selectmenu("refresh", true);

            if (stateId <= 0) {
                $("#ddlCity").val("0").attr("disabled", true);

            }
            else {

                $("#ddlCity").val("0").attr("disabled", false);
                FillCities(stateId);
            }
        });

        $("#btnSubmit").click(function () {
            $("spnError").html("");

            if (validateDetails()) {
                SaveEMIRequest();
                document.body.style.overflow = "visible";
                $("#spnError").html("");
                $("#requestEmi").popup("close");

                return true;
            }
            else
                return false;
        });

        function SaveEMIRequest() {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"custName":"' + customerName + '", "email":"' + customerEmail + '", "mobile":"' + customerMobile + '", "modelId":"' + modelId + '", "selectedCityId":"' + customerCity + '", "leadtype":"' + '' + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SaveEMIAssistaneRequest"); },
                success: function (response) {
                    //var responseJSON = eval('(' + response + ')');
                    //var resObj = eval('(' + responseJSON.value + ')');
                    alert("Your EMI request has been submitted successfully");
                }
            });
        }

        function validateDetails() {

            var retVal = true;
            var errorMsg = "";

            var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
            var reMobile = /^[0-9]*$/;
            var name = /^[a-zA-Z& ]+$/;

            // $("#hdnMobile").val($("#numMobile").val());

            customerName = $("#txtName").val().trim();

            if (customerName == "") {
                retVal = false;
                errorMsg += "Please enter your Name<br>";
            }
            else if (!name.test(customerName)) {
                retVal = false;
                errorMsg += "Please enter valid Name<br>";
            }

            var _email = $("#txtEmail").val().trim().toString().toLowerCase();
            customerEmail = _email;
            if (_email == "") {
                retVal = false;
                errorMsg += "Please enter your Email<br>";
            }
            else if (!reEmail.test(_email)) {
                retVal = false;
                errorMsg += "Invalid Email<br>";
            }


            var _custMobile = $("#numMobile").val().trim();
            customerMobile = _custMobile;
            if (_custMobile == "") {
                retVal = false;
                errorMsg += "Please enter your Mobile Number<br>";
            }
            else if (!reMobile.test(_custMobile)) {
                retVal = false;
                errorMsg += "Mobile Number should be numeric<br>";
            }
            else if (_custMobile.length != 10) {
                retVal = false;
                errorMsg += "Mobile Number should be of 10 digits<br>";
            }

            customerState = $("#ddlStates").val();
            if (customerState <= 0) {
                retVal = false;
                errorMsg += "Please select State<br>";
            }


            customerCity = $("#ddlCity").val();
            if (customerCity <= 0) {
                retVal = false;
                errorMsg += "Please select City<br>";
            }

            if (retVal == false) {
                $("#spnError").html(errorMsg);
            }

            return retVal;
        }

        function FillCities(stateId) {
            var requestType = 'ALL';
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"requestType":"' + requestType + '", "stateId":"' + stateId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetCities"); },
                success: function (response) {

                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');

                    var dependentCmbs = new Array();
                    bindDropDownList(resObj, $("#ddlCity"), "", dependentCmbs, "--Select City--");
                },
            });
        }
    </script>



</form>
<!-- #include file="/includes/footermobile.aspx" -->

