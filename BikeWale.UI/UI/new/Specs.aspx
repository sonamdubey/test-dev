<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.Specs" Debug="false" Trace="false" %>
<%@ Register TagPrefix="BikeWale" TagName="BikeRatings" Src="~/UI/controls/BikeRatings.ascx" %>
<%@ Register TagPrefix="NBL" TagName="NewBikeLaunches" Src="/UI/controls/NewBikeLaunches.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<%
    title = "Technical Specifications: " + mmv.Make + " " + mmv.Model + " " + mmv.Version;
    keywords = mmv.Model + " " + mmv.Version + " specs, " + mmv.Model + " " + mmv.Version + " specifications, " + mmv.Model + " " + mmv.Version + " technical specifications, tech specs";
	description = "BikeWale&reg; - " + mmv.Make + " " + mmv.Model + " " + mmv.Version + " technical specifications. Research details specs including bike's dimensions, weight, engine, capacity, performance, tyres, suspensions etc.";  
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
%>
<!-- #include file="/UI/includes/headNew.aspx" --> 

<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li><a href="/">Home</a></li>           
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/<%= MakeMaskingName%>-bikes/"><%=make %> Bikes</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName%>/"><%= String.Format("{0} {1}", make,model) %></a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong><%= String.Format("{0} {1}", make,model) %> Specifications</strong></li>
        </ul><div class="clear"></div>
    </div>
    <div class="grid_8 margin-top10">        
        <h1><%=bike %> Specifications</h1>        
        <div class="grid_3 alpha margin-top15">
            <a href="/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName%>/images/"><img src="<%=imagePath %>" class="padding-5 border-light" title="<%= bike%>" alt="<%= bike%>" /></a>
        </div>
        <div class="grid_5 omega margin-top15" runat="server">
            <BikeWale:BikeRatings runat="server" ID="ctrl_BikeRatings" />
            <div class="padding5 text-highlight">Starts At Rs. <%=estimatedPrice %></div>
            <div class="action-btn padding5 <%= String.IsNullOrEmpty(mmv.MinPrice) ? "hide" : "" %>">Avg Ex-Showroom Price&nbsp;&nbsp;<a href="/pricequote/default.aspx?version=<%=versionId %>" class="getquotation">Check On-Road Price</a></div>
        </div><div class="clear"></div> 

        <h2 class="margin-top20">Features</h2>
        <div class="grid_4 alpha">            
            <table width="100%" class="tbl-specs margin-top5">                
                <tr>
                    <td width="180">Speedometer</td>
                    <td>
                        <asp:literal id="ltr_Speedometer" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Tachometer</td>
                    <td>
                        <asp:literal id="ltr_Tachometer" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Tachometer Type</td>
                    <td>
                        <asp:literal id="ltr_TachometerType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Shift Light</td>
                    <td>
                        <asp:literal id="ltr_ShiftLight" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Electric Start</td>
                    <td>
                        <asp:literal id="ltr_ElectricStart" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Tripmeter</td>
                    <td>
                        <asp:literal id="ltr_Tripmeter" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>No Of Tripmeters</td>
                    <td>
                        <asp:literal id="ltr_NoOfTripmeters" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Tripmeter Type</td>
                    <td>
                        <asp:literal id="ltr_TripmeterType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Low Fuel Indicator</td>
                    <td>
                        <asp:literal id="ltr_LowFuelIndicator" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Low Oil Indicator</td>
                    <td>
                        <asp:literal id="ltr_LowOilIndicator" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Low Battery Indicator</td>
                    <td>
                        <asp:literal id="ltr_LowBatteryIndicator" runat="server" />
                    </td>
                </tr>                              
            </table>
            <h2 class="margin-top15">Colors</h2>
            <div class="margin-top10" style="width:200%;"><asp:literal id="ltr_Colors" runat="server"  /></div>
        </div>
        <div class="grid_4 omega">            
            <table width="100%" class="tbl-specs margin-top5">
                <tr>
                    <td>Fuel Gauge</td>
                    <td>
                        <asp:literal id="ltr_FuelGauge" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Digital Fuel Gauge</td>
                    <td>
                        <asp:literal id="ltr_DigitalFuelGauge" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Pillion Seat</td>
                    <td>
                        <asp:literal id="ltr_PillionSeat" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Pillion Footrest</td>
                    <td>
                        <asp:literal id="ltr_PillionFootrest" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Pillion Backrest</td>
                    <td>
                        <asp:literal id="ltr_PillionBackrest" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Pillion Grabrail</td>
                    <td>
                        <asp:literal id="ltr_PillionGrabrail" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Stand Alarm</td>
                    <td>
                        <asp:literal id="ltr_StandAlarm" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Stepped Seat</td>
                    <td>
                        <asp:literal id="ltr_SteppedSeat" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Antilock Braking System</td>
                    <td>
                        <asp:literal id="ltr_AntilockBrakingSystem" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Killswitch</td>
                    <td>
                        <asp:literal id="ltr_Killswitch" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Clock</td>
                    <td>
                        <asp:literal id="ltr_Clock" runat="server" />
                    </td>
                </tr> 
            </table>
        </div>
        <div class="clear"></div>
        <h2 class="margin-top15">Specs</h2>
        <div class="grid_4 alpha">           
            <table width="100%" class="tbl-specs margin-top10">                
                <tr>
                    <th colspan="2">Engine</th>
                </tr>
                <tr>
                    <td width="200">Displacement (cc)</td>
                    <td>
                        <asp:literal id="ltr_Displacement" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Cylinders</td>
                    <td>
                        <asp:literal id="ltr_Cylinders" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Max Power</td>
                    <td>
                        <asp:literal id="ltr_MaxPower" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Maximum Torque</td>
                    <td>
                        <asp:literal id="ltr_MaximumTorque" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Bore (mm)</td>
                    <td>
                        <asp:literal id="ltr_Bore" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Stroke (mm)</td>
                    <td>
                        <asp:literal id="ltr_Stroke" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Valves Per Cylinder</td>
                    <td>
                        <asp:literal id="ltr_ValvesPerCylinder" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Fuel Delivery System</td>
                    <td>
                        <asp:literal id="ltr_FuelDeliverySystem" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Fuel Type</td>
                    <td>
                        <asp:literal id="ltr_FuelType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Ignition</td>
                    <td>
                        <asp:literal id="ltr_Ignition" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Spark Plugs (Per Cylinder)</td>
                    <td>
                        <asp:literal id="ltr_SparkPlugsPerCylinder" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Cooling System</td>
                    <td>
                        <asp:literal id="ltr_CoolingSystem" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th colspan="2">Transmission</th>
                </tr>
                <tr>
                    <td>Gearbox Type</td>
                    <td>
                        <asp:literal id="ltr_GearboxType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>No Of Gears</td>
                    <td>
                        <asp:literal id="ltr_NoOfGears" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Transmission Type</td>
                    <td>
                        <asp:literal id="ltr_TransmissionType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Clutch</td>
                    <td>
                        <asp:literal id="ltr_Clutch" runat="server" />
                    </td>
                </tr>                
                <tr>
                    <th colspan="2">Dimensions &amp; Weight</th>
                </tr>
                <tr>
                    <td>Kerb Weight (Kg)</td>
                    <td>
                        <asp:literal id="ltr_KerbWeight" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Overall Length (mm)</td>
                    <td>
                        <asp:literal id="ltr_OverallLength" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Overall Width (mm)</td>
                    <td>
                        <asp:literal id="ltr_OverallWidth" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Overall Height (mm)</td>
                    <td>
                        <asp:literal id="ltr_OverallHeight" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Wheelbase (mm)</td>
                    <td>
                        <asp:literal id="ltr_Wheelbase" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Ground Clearance (mm)</td>
                    <td>
                        <asp:literal id="ltr_GroundClearance" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Seat Height (mm)</td>
                    <td>
                        <asp:literal id="ltr_SeatHeight" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Fuel Efficiency &amp; Range</td>
                </tr>
                <tr>
                    <td>Fuel Tank Capacity (Litres)</td>
                    <td>
                        <asp:literal id="ltr_FuelTankCapacity" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Reserve Fuel Capacity (Litres)</td>
                    <td>
                        <asp:literal id="ltr_ReserveFuelCapacity" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>FuelEfficiency Overall (Kmpl)</td>
                    <td>
                        <asp:literal id="ltr_FuelEfficiencyOverall" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Fuel Efficiency Range (Km)</td>
                    <td>
                        <asp:literal id="ltr_FuelEfficiencyRange" runat="server" />
                    </td>
                </tr>                
            </table>
        </div>
        <div class="grid_4 omega">
            <table width="100%" class="tbl-specs margin-top10">
                <tr>
                    <th colspan="2">Chassis&nbsp;&amp; Suspension</th>
                </tr>
                <tr>
                    <td>Chassis Type</td>
                    <td>
                        <asp:literal id="ltr_ChassisType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Front Suspension</td>
                    <td>
                        <asp:literal id="ltr_FrontSuspension" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Rear Suspension</td>
                    <td>
                        <asp:literal id="ltr_RearSuspension" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th colspan="2">Braking</th>
                </tr>
                <tr>
                    <td>Brake Type</td>
                    <td>
                        <asp:literal id="ltr_BrakeType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Front Disc</td>
                    <td>
                        <asp:literal id="ltr_FrontDisc" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Front Disc/Drum Size (mm)</td>
                    <td>
                        <asp:literal id="ltr_FrontDisc_DrumSize" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Rear Disc</td>
                    <td>
                        <asp:literal id="ltr_RearDisc" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Rear Disc/Drum Size (mm)</td>
                    <td>
                        <asp:literal id="ltr_RearDisc_DrumSize" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Calliper Type</td>
                    <td>
                        <asp:literal id="ltr_CalliperType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th colspan="2">Wheels &amp; Tyres</th>
                </tr>
                <tr>
                    <td>Wheel Size (inches)</td>
                    <td>
                        <asp:literal id="ltr_WheelSize" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Front Tyre</td>
                    <td>
                        <asp:literal id="ltr_FrontTyre" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Rear Tyre</td>
                    <td>
                        <asp:literal id="ltr_RearTyre" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Tubeless Tyres</td>
                    <td>
                        <asp:literal id="ltr_TubelessTyres" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Radial Tyres</td>
                    <td>
                        <asp:literal id="ltr_RadialTyres" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Alloy Wheels</td>
                    <td>
                        <asp:literal id="ltr_AlloyWheels" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th colspan="2">Electricals</th>
                </tr>
                <tr>
                    <td>Electric System</td>
                    <td>
                        <asp:literal id="ltr_ElectricSystem" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Battery</td>
                    <td>
                        <asp:literal id="ltr_Battery" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Headlight Type</td>
                    <td>
                        <asp:literal id="ltr_HeadlightType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Headlight Bulb Type</td>
                    <td>
                        <asp:literal id="ltr_HeadlightBulbType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Brake/Tail Light</td>
                    <td>
                        <asp:literal id="ltr_Brake_Tail_Light" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Turn Signal</td>
                    <td>
                        <asp:literal id="ltr_TurnSignal" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Pass Light</td>
                    <td>
                        <asp:literal id="ltr_PassLight" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th colspan="2">Performance</th>
                </tr>
                <tr>
                    <td>0 to 60 kmph (Seconds)</td>
                    <td>
                        <asp:literal id="ltr_Performance_0_60_kmph" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>0 to 80 kmph (Seconds)</td>
                    <td>
                        <asp:literal id="ltr_Performance_0_80_kmph" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>0 to 40 m (Seconds)</td>
                    <td>
                        <asp:literal id="ltr_Performance_0_40_m" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Top Speed (Kmph)</td>
                    <td>
                        <asp:literal id="ltr_TopSpeed" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>60 to 0 Kmph (Seconds, metres)</td>
                    <td>
                        <asp:literal id="ltr_Performance_60_0_kmph" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>80 to 0 kmph (Seconds, metres)</td>
                    <td>
                        <asp:literal id="ltr_Performance_80_0_kmph" runat="server" />
                    </td>
                </tr>
            </table>
        </div>               
    </div>
    <div class="grid_4">        
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/UI/ads/Ad300x250.aspx" -->
        </div>
        <div class="grey-bg content-block border-radius5 padding-bottom20 margin-top15">
            <NBL:NewBikeLaunches ID="ctrl_NewBikeLaunches" runat="server" />                    
            <div class="clear"></div>
        </div>
        <div class="margin-top15">
           <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/UI/ads/Ad300x250BTF.aspx" -->
       </div>
    </div>    
</div>
<!-- #include file="/UI/includes/footerInner.aspx" -->
