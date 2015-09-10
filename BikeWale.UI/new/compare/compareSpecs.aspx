<%@ Page Language="C#" Inherits="Bikewale.New.BikeComparison" AutoEventWireUp="false" Trace="false" Debug="false" %>
<%
    title = "Specs Comparison - " + pageTitle;
    description = "BikeWale&reg; - Compare " + pageTitle + " specifications. Compare dimensions, engine, gearbox, fuel economy and other technical specifications of the bikes on BikeWale";
    keywords = "";
%>
<!-- #include file="/includes/headNew.aspx" -->
<script language="javascript" type="text/javascript">
	$(document).ready(function(){
		if( $("#tblCompare").length > 0 ){
			var featuredBikeIndex = '<%= featuredBikeIndex + 2 %>';					
			$("#tblCompare tr td:nth-child("+ featuredBikeIndex +")").css({"background-color":"#FCF8D0"});
			$("#tblCompare tr:first td:nth-child("+ featuredBikeIndex +")").html("Sponsored Bike").addClass("td-featured");
		}
	});
</script>
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li><a href="/">Home</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/new/compare/">Compare Bikes in India</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>Bike Comparison</strong></li>
        </ul><div class="clear"></div>
    </div>
    <div class="grid_12 margin-top10"><!--    Left Container starts here -->       
		    <span id="spnError" class="error" runat="server"></span>
		    <h1 class="red margin-bottom15">Specs Comparison - <asp:Literal id="ltrTitle" runat="server"></asp:Literal></h1>		    
            <div class="padding-top20 featured-bike-tabs">
                <ul class="featured-bike-tabs-inner padding-top20">
                    <li class="fbike-active-tab first_tab">Specs</li>
                    <li><a href="compareFeatures.aspx?bike1=<%=Request["bike1"]%>&amp;bike2=<%=Request["bike2"]%>&amp;bike3=<%=Request["bike3"]%>&amp;bike4=<%=Request["bike4"]%>">Features</a></li>
                    <li><a href="compareColors.aspx?bike1=<%=Request["bike1"]%>&amp;bike2=<%=Request["bike2"]%>&amp;bike3=<%=Request["bike3"]%>&amp;bike4=<%=Request["bike4"]%>">Colours</a></li>                          
                </ul>
            </div> 	
		    <div class="tab_inner_container">
			    <table width="100%" id="tblCompare" runat="server" class="tbl-compare" cellpadding="5" border="0" cellspacing="0">
                    <!-- #include file="compareCommon.aspx" -->
				    <tr class="headerSpecs">
					    <th>Engine</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
				    </tr>
				    <tr>
				      <td class="headerSpecs">Displacement (cc)</td>
				      <td><%=ShowFormatedData(displacement[0]) %></td>
				      <td><%=ShowFormatedData(displacement[1]) %></td>
				      <td><%=ShowFormatedData(displacement[2]) %></td>
				      <td><%=ShowFormatedData(displacement[3]) %></td>
				      <td><%=ShowFormatedData(displacement[4]) %></td>
				    </tr>
				    <tr>
				      <td class="headerSpecs">Cylinders</td>
				      <td><%= ShowFormatedData(cylinders[0]) %></td>
				      <td><%= ShowFormatedData(cylinders[1]) %></td>
				      <td><%= ShowFormatedData(cylinders[2]) %></td>
				      <td><%= ShowFormatedData(cylinders[3]) %></td>
				      <td><%= ShowFormatedData(cylinders[4]) %></td>
				    </tr>
				    <tr>
				      <td class="headerSpecs">Max Power</td>
				      <td><%=ShowFormatedData(maxPower[0]) %></td>
				      <td><%=ShowFormatedData(maxPower[1]) %></td>
				      <td><%=ShowFormatedData(maxPower[2]) %></td>
				      <td><%=ShowFormatedData(maxPower[3]) %></td>
				      <td><%=ShowFormatedData(maxPower[4]) %></td>
				    </tr>
				    <tr>
				      <td class="headerSpecs">Maximum Torque</td>
				      <td><%=ShowFormatedData(maximumTorque[0]) %></td>
				      <td><%=ShowFormatedData(maximumTorque[1]) %></td>
				      <td><%=ShowFormatedData(maximumTorque[2]) %></td>
				      <td><%=ShowFormatedData(maximumTorque[3]) %></td>
				      <td><%=ShowFormatedData(maximumTorque[4]) %></td>
				    </tr>
				    <tr>
				      <td class="headerSpecs">Bore (mm)</td>
				      <td><%=ShowFormatedData(bore[0]) %></td>
				      <td><%=ShowFormatedData(bore[1]) %></td>
				      <td><%=ShowFormatedData(bore[2]) %></td>
				      <td><%=ShowFormatedData(bore[3]) %></td>
				      <td><%=ShowFormatedData(bore[4]) %></td>
				    </tr>
				    <tr>
				      <td class="headerSpecs">Stroke (mm)</td>
				      <td><%=ShowFormatedData(stroke[0]) %></td>
				      <td><%=ShowFormatedData(stroke[1]) %></td>
				      <td><%=ShowFormatedData(stroke[2]) %></td>
				      <td><%=ShowFormatedData(stroke[3]) %></td>
				      <td><%=ShowFormatedData(stroke[4]) %></td>
				    </tr>
				    <tr>
				      <td class="headerSpecs">Valves Per Cylinder</td>
				      <td><%=ShowFormatedData(valvesPerCylinder[0]) %></td>
				      <td><%=ShowFormatedData(valvesPerCylinder[1]) %></td>
				      <td><%=ShowFormatedData(valvesPerCylinder[2]) %></td>
				      <td><%=ShowFormatedData(valvesPerCylinder[3]) %></td>
				      <td><%=ShowFormatedData(valvesPerCylinder[4]) %></td>
				    </tr>
				    <tr>
				      <td class="headerSpecs">Fuel Delivery System</td>
				      <td><%=ShowFormatedData(fuelDeliverySystem[0]) %></td>
				      <td><%=ShowFormatedData(fuelDeliverySystem[1]) %></td>
				      <td><%=ShowFormatedData(fuelDeliverySystem[2]) %></td>
				      <td><%=ShowFormatedData(fuelDeliverySystem[3]) %></td>
				      <td><%=ShowFormatedData(fuelDeliverySystem[4]) %></td>
				    </tr>
				    <tr>
				      <td class="headerSpecs">Fuel Type</td>
				      <td><%=ShowFormatedData(fuelType[0]) %></td>
				      <td><%=ShowFormatedData(fuelType[1]) %></td>
				      <td><%=ShowFormatedData(fuelType[2]) %></td>
				      <td><%=ShowFormatedData(fuelType[3]) %></td>
				      <td><%=ShowFormatedData(fuelType[4]) %></td>
				    </tr>
				    <tr>
				      <td class="headerSpecs">Ignition</td>
				      <td><%=ShowFormatedData(ignition[0]) %></td>
				      <td><%=ShowFormatedData(ignition[1]) %></td>
				      <td><%=ShowFormatedData(ignition[2]) %></td>
				      <td><%=ShowFormatedData(ignition[3]) %></td>
				      <td><%=ShowFormatedData(ignition[4]) %></td>
				    </tr>
				    <tr>
				      <td class="headerSpecs">Spark Plugs (Per Cylinder)</td>
				      <td><%=ShowFormatedData(sparkPlugsPerCylinder[0]) %></td>
				      <td><%=ShowFormatedData(sparkPlugsPerCylinder[1]) %></td>
				      <td><%=ShowFormatedData(sparkPlugsPerCylinder[2]) %></td>
				      <td><%=ShowFormatedData(sparkPlugsPerCylinder[3]) %></td>
				      <td><%=ShowFormatedData(sparkPlugsPerCylinder[4]) %></td>
				    </tr>
				    <tr>
				      <td class="headerSpecs">Cooling System</td>
				      <td><%=ShowFormatedData(coolingSystem[0]) %></td>
				      <td><%=ShowFormatedData(coolingSystem[1]) %></td>
				      <td><%=ShowFormatedData(coolingSystem[2]) %></td>
				      <td><%=ShowFormatedData(coolingSystem[3]) %></td>
				      <td><%=ShowFormatedData(coolingSystem[4]) %></td>
				    </tr>
                    <tr>
                        <th class="headerSpecs">Transmission</th>
                        <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
                    </tr>
                
                    <tr>
				        <td class="headerSpecs">Gearbox Type</td>
				        <td><%=ShowFormatedData(gearboxType[0]) %></td>
				        <td><%=ShowFormatedData(gearboxType[1]) %></td>
				        <td><%=ShowFormatedData(gearboxType[2]) %></td>
				        <td><%=ShowFormatedData(gearboxType[3]) %></td>
				        <td><%=ShowFormatedData(gearboxType[4]) %></td>
				    </tr>
                    <tr>
				        <td class="headerSpecs">No Of Gears</td>
				        <td><%=ShowFormatedData(noOfGears[0]) %></td>
				        <td><%=ShowFormatedData(noOfGears[1]) %></td>
				        <td><%=ShowFormatedData(noOfGears[2]) %></td>
				        <td><%=ShowFormatedData(noOfGears[3]) %></td>
				        <td><%=ShowFormatedData(noOfGears[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Transmission Type</td>
				        <td><%=ShowFormatedData(transmissionType[0]) %></td>
				        <td><%=ShowFormatedData(transmissionType[1]) %></td>
				        <td><%=ShowFormatedData(transmissionType[2]) %></td>
				        <td><%=ShowFormatedData(transmissionType[3]) %></td>
				        <td><%=ShowFormatedData(transmissionType[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Clutch</td>
				        <td><%=ShowFormatedData(clutch[0]) %></td>
				        <td><%=ShowFormatedData(clutch[1]) %></td>
				        <td><%=ShowFormatedData(clutch[2]) %></td>
				        <td><%=ShowFormatedData(clutch[3]) %></td>
				        <td><%=ShowFormatedData(clutch[4]) %></td>
				    </tr>
                    <tr>
                        <th class="headerSpecs">Performance</th>
				        <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
				    </tr>
                    <tr>
                        <td class="headerSpecs">0 to 60 kmph (Seconds)</td>
				        <td><%=ShowFormatedData(performance_0_60_kmph[0]) %></td>
				        <td><%=ShowFormatedData(performance_0_60_kmph[1]) %></td>
				        <td><%=ShowFormatedData(performance_0_60_kmph[2]) %></td>
				        <td><%=ShowFormatedData(performance_0_60_kmph[3]) %></td>
				        <td><%=ShowFormatedData(performance_0_60_kmph[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">0 to 80 kmph (Seconds)</td>
				        <td><%=ShowFormatedData(performance_0_80_kmph[0]) %></td>
				        <td><%=ShowFormatedData(performance_0_80_kmph[1]) %></td>
				        <td><%=ShowFormatedData(performance_0_80_kmph[2]) %></td>
				        <td><%=ShowFormatedData(performance_0_80_kmph[3]) %></td>
				        <td><%=ShowFormatedData(performance_0_80_kmph[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">0 to 40 m (Seconds)</td>
				        <td><%=ShowFormatedData(performance_0_40_m[0]) %></td>
				        <td><%=ShowFormatedData(performance_0_40_m[1]) %></td>
				        <td><%=ShowFormatedData(performance_0_40_m[2]) %></td>
				        <td><%=ShowFormatedData(performance_0_40_m[3]) %></td>
				        <td><%=ShowFormatedData(performance_0_40_m[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Top Speed (Kmph)</td>
				        <td><%=ShowFormatedData(topSpeed[0]) %></td>
				        <td><%=ShowFormatedData(topSpeed[1]) %></td>
				        <td><%=ShowFormatedData(topSpeed[2]) %></td>
				        <td><%=ShowFormatedData(topSpeed[3]) %></td>
				        <td><%=ShowFormatedData(topSpeed[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">60 to 0 Kmph (Seconds, metres)</td>
				        <td><%=ShowFormatedData(performance_60_0_kmph[0]) %></td>
				        <td><%=ShowFormatedData(performance_60_0_kmph[1]) %></td>
				        <td><%=ShowFormatedData(performance_60_0_kmph[2]) %></td>
				        <td><%=ShowFormatedData(performance_60_0_kmph[3]) %></td>
				        <td><%=ShowFormatedData(performance_60_0_kmph[4])  %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">80 to 0 kmph (Seconds, metres)</td>
				        <td><%=ShowFormatedData(performance_80_0_kmph[0]) %></td>
				        <td><%=ShowFormatedData(performance_80_0_kmph[1]) %></td>
				        <td><%=ShowFormatedData(performance_80_0_kmph[2]) %></td>
				        <td><%=ShowFormatedData(performance_80_0_kmph[3]) %></td>
				        <td><%=ShowFormatedData(performance_80_0_kmph[4]) %></td>
				    </tr>
                    <tr>
                        <th class="headerSpecs">Dimensions & Weight</th>
				        <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Kerb Weight (Kg)</td>
				        <td><%=ShowFormatedData(kerbWeight[0]) %></td>
				        <td><%=ShowFormatedData(kerbWeight[1]) %></td>
				        <td><%=ShowFormatedData(kerbWeight[2]) %></td>
				        <td><%=ShowFormatedData(kerbWeight[3]) %></td>
				        <td><%=ShowFormatedData(kerbWeight[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Overall Length (mm)</td>
				        <td><%=ShowFormatedData(overallLength[0]) %></td>
				        <td><%=ShowFormatedData(overallLength[1]) %></td>
				        <td><%=ShowFormatedData(overallLength[2]) %></td>
				        <td><%=ShowFormatedData(overallLength[3]) %></td>
				        <td><%=ShowFormatedData(overallLength[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Overall Width (mm)</td>
				        <td><%=ShowFormatedData(overallWidth[0]) %></td>
				        <td><%=ShowFormatedData(overallWidth[1]) %></td>
				        <td><%=ShowFormatedData(overallWidth[2]) %></td>
				        <td><%=ShowFormatedData(overallWidth[3]) %></td>
				        <td><%=ShowFormatedData(overallWidth[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Overall Height (mm)</td>
				        <td><%=ShowFormatedData(overallHeight[0]) %></td>
				        <td><%=ShowFormatedData(overallHeight[1]) %></td>
				        <td><%=ShowFormatedData(overallHeight[2]) %></td>
				        <td><%=ShowFormatedData(overallHeight[3]) %></td>
				        <td><%=ShowFormatedData(overallHeight[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Wheelbase (mm)</td>
				        <td><%=ShowFormatedData(wheelbase[0]) %></td>
				        <td><%=ShowFormatedData(wheelbase[1]) %></td>
				        <td><%=ShowFormatedData(wheelbase[2]) %></td>
				        <td><%=ShowFormatedData(wheelbase[3]) %></td>
				        <td><%=ShowFormatedData(wheelbase[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Ground Clearance (mm)</td>
				        <td><%=ShowFormatedData(groundClearance[0]) %></td>
				        <td><%=ShowFormatedData(groundClearance[1]) %></td>
				        <td><%=ShowFormatedData(groundClearance[2]) %></td>
				        <td><%=ShowFormatedData(groundClearance[3]) %></td>
				        <td><%=ShowFormatedData(groundClearance[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Seat Height (mm)</td>
				        <td><%=ShowFormatedData(seatHeight[0]) %></td>
				        <td><%=ShowFormatedData(seatHeight[1]) %></td>
				        <td><%=ShowFormatedData(seatHeight[2]) %></td>
				        <td><%=ShowFormatedData(seatHeight[3]) %></td>
				        <td><%=ShowFormatedData(seatHeight[4]) %></td>
				    </tr>
                    <tr>
                        <th class="headerSpecs">Fuel Efficiency & Range</th>
				        <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Fuel Tank Capacity (Litres)</td>
				        <td><%=ShowFormatedData(fuelTankCapacity[0]) %></td>
				        <td><%=ShowFormatedData(fuelTankCapacity[1]) %></td>
				        <td><%=ShowFormatedData(fuelTankCapacity[2]) %></td>
				        <td><%=ShowFormatedData(fuelTankCapacity[3]) %></td>
				        <td><%=ShowFormatedData(fuelTankCapacity[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Reserve Fuel Capacity (Litres)</td>
				        <td><%=ShowFormatedData(reserveFuelCapacity[0]) %></td>
				        <td><%=ShowFormatedData(reserveFuelCapacity[1]) %></td>
				        <td><%=ShowFormatedData(reserveFuelCapacity[2]) %></td>
				        <td><%=ShowFormatedData(reserveFuelCapacity[3]) %></td>
				        <td><%=ShowFormatedData(reserveFuelCapacity[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">FuelEfficiency Overall (Kmpl)</td>
				        <td><%=ShowFormatedData(fuelEfficiencyOverall[0]) %></td>
				        <td><%=ShowFormatedData(fuelEfficiencyOverall[1]) %></td>
				        <td><%=ShowFormatedData(fuelEfficiencyOverall[2]) %></td>
				        <td><%=ShowFormatedData(fuelEfficiencyOverall[3]) %></td>
				        <td><%=ShowFormatedData(fuelEfficiencyOverall[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Fuel Efficiency Range (Km)</td>
				        <td><%=ShowFormatedData(fuelEfficiencyRange[0]) %></td>
				        <td><%=ShowFormatedData(fuelEfficiencyRange[1]) %></td>
				        <td><%=ShowFormatedData(fuelEfficiencyRange[2]) %></td>
				        <td><%=ShowFormatedData(fuelEfficiencyRange[3]) %></td>
				        <td><%=ShowFormatedData(fuelEfficiencyRange[4]) %></td>
				    </tr>
                    <tr>
                        <th class="headerSpecs">Chassis & Suspension</th>
				        <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Chassis Type</td>
				        <td><%=ShowFormatedData(chassisType[0]) %></td>
				        <td><%=ShowFormatedData(chassisType[1]) %></td>
				        <td><%=ShowFormatedData(chassisType[2]) %></td>
				        <td><%=ShowFormatedData(chassisType[3]) %></td>
				        <td><%=ShowFormatedData(chassisType[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Front Suspension</td>
				        <td><%=ShowFormatedData(frontSuspension[0]) %></td>
				        <td><%=ShowFormatedData(frontSuspension[1]) %></td>
				        <td><%=ShowFormatedData(frontSuspension[2]) %></td>
				        <td><%=ShowFormatedData(frontSuspension[3]) %></td>
				        <td><%=ShowFormatedData(frontSuspension[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Rear Suspension</td>
				        <td><%=ShowFormatedData(rearSuspension[0]) %></td>
				        <td><%=ShowFormatedData(rearSuspension[1]) %></td>
				        <td><%=ShowFormatedData(rearSuspension[2]) %></td>
				        <td><%=ShowFormatedData(rearSuspension[3]) %></td>
				        <td><%=ShowFormatedData(rearSuspension[4]) %></td>
				    </tr>
                    <tr>
                        <th class="headerSpecs">Braking</th>
				        <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Brake Type</td>
				        <td><%=ShowFormatedData(brakeType[0]) %></td>
				        <td><%=ShowFormatedData(brakeType[1]) %></td>
				        <td><%=ShowFormatedData(brakeType[2]) %></td>
				        <td><%=ShowFormatedData(brakeType[3]) %></td>
				        <td><%=ShowFormatedData(brakeType[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Front Disc</td>
				        <td><%=ShowFeature(frontDisc[0]) %></td>
				        <td><%=ShowFeature(frontDisc[1]) %></td>
				        <td><%=ShowFeature(frontDisc[2]) %></td>
				        <td><%=ShowFeature(frontDisc[3]) %></td>
				        <td><%=ShowFeature(frontDisc[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Front Disc/Drum Size (mm)</td>
				        <td><%=ShowFormatedData(frontDisc_DrumSize[0]) %></td>
				        <td><%=ShowFormatedData(frontDisc_DrumSize[1]) %></td>
				        <td><%=ShowFormatedData(frontDisc_DrumSize[2]) %></td>
				        <td><%=ShowFormatedData(frontDisc_DrumSize[3]) %></td>
				        <td><%=ShowFormatedData(frontDisc_DrumSize[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Rear Disc</td>
				        <td><%=ShowFeature(rearDisc[0]) %></td>
				        <td><%=ShowFeature(rearDisc[1]) %></td>
				        <td><%=ShowFeature(rearDisc[2]) %></td>
				        <td><%=ShowFeature(rearDisc[3]) %></td>
				        <td><%=ShowFeature(rearDisc[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Rear Disc/Drum Size (mm)</td>
				        <td><%=ShowFormatedData(rearDisc_DrumSize[0]) %></td>
				        <td><%=ShowFormatedData(rearDisc_DrumSize[1]) %></td>
				        <td><%=ShowFormatedData(rearDisc_DrumSize[2]) %></td>
				        <td><%=ShowFormatedData(rearDisc_DrumSize[3]) %></td>
				        <td><%=ShowFormatedData(rearDisc_DrumSize[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Calliper Type</td>
				        <td><%=ShowFormatedData(calliperType[0]) %></td>
				        <td><%=ShowFormatedData(calliperType[1]) %></td>
				        <td><%=ShowFormatedData(calliperType[2]) %></td>
				        <td><%=ShowFormatedData(calliperType[3]) %></td>
				        <td><%=ShowFormatedData(calliperType[4])%></td>
				    </tr>
                    <tr>
                        <th class="headerSpecs">Wheels & Tyres</th>
				        <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Wheel Size (inches)</td>
				        <td><%=ShowFormatedData(wheelSize[0]) %></td>
				        <td><%=ShowFormatedData(wheelSize[1]) %></td>
				        <td><%=ShowFormatedData(wheelSize[2]) %></td>
				        <td><%=ShowFormatedData(wheelSize[3]) %></td>
				        <td><%=ShowFormatedData(wheelSize[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Front Tyre</td>
				        <td><%=ShowFormatedData(frontTyre[0]) %></td>
				        <td><%=ShowFormatedData(frontTyre[1]) %></td>
				        <td><%=ShowFormatedData(frontTyre[2]) %></td>
				        <td><%=ShowFormatedData(frontTyre[3]) %></td>
				        <td><%=ShowFormatedData(frontTyre[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Rear Tyre</td>
				        <td><%=ShowFormatedData(rearTyre[0]) %></td>
				        <td><%=ShowFormatedData(rearTyre[1]) %></td>
				        <td><%=ShowFormatedData(rearTyre[2]) %></td>
				        <td><%=ShowFormatedData(rearTyre[3]) %></td>
				        <td><%=ShowFormatedData(rearTyre[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Tubeless Tyres</td>
				        <td><%=ShowFeature(tubelessTyres[0]) %></td>
				        <td><%=ShowFeature(tubelessTyres[1]) %></td>
				        <td><%=ShowFeature(tubelessTyres[2]) %></td>
				        <td><%=ShowFeature(tubelessTyres[3]) %></td>
				        <td><%=ShowFeature(tubelessTyres[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Radial Tyres</td>
				        <td><%=ShowFeature(radialTyres[0]) %></td>
				        <td><%=ShowFeature(radialTyres[1]) %></td>
				        <td><%=ShowFeature(radialTyres[2]) %></td>
				        <td><%=ShowFeature(radialTyres[3]) %></td>
				        <td><%=ShowFeature(radialTyres[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Alloy Wheels</td>
				        <td><%=ShowFeature(alloyWheels[0]) %></td>
				        <td><%=ShowFeature(alloyWheels[1]) %></td>
				        <td><%=ShowFeature(alloyWheels[2]) %></td>
				        <td><%=ShowFeature(alloyWheels[3]) %></td>
				        <td><%=ShowFeature(alloyWheels[4]) %></td>
				    </tr>
                    <tr>
                        <th class="headerSpecs">Electricals</th>
				        <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
					    <th>&nbsp;</th>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Electric System</td>
				        <td><%=ShowFormatedData(electricSystem[0]) %></td>
				        <td><%=ShowFormatedData(electricSystem[1]) %></td>
				        <td><%=ShowFormatedData(electricSystem[2]) %></td>
				        <td><%=ShowFormatedData(electricSystem[3]) %></td>
				        <td><%=ShowFormatedData(electricSystem[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Battery</td>
				        <td><%=ShowFormatedData(battery[0]) %></td>
				        <td><%=ShowFormatedData(battery[1]) %></td>
				        <td><%=ShowFormatedData(battery[2]) %></td>
				        <td><%=ShowFormatedData(battery[3]) %></td>
				        <td><%=ShowFormatedData(battery[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Headlight Type</td>
				        <td><%=ShowFormatedData(headlightType[0]) %></td>
				        <td><%=ShowFormatedData(headlightType[1]) %></td>
				        <td><%=ShowFormatedData(headlightType[2]) %></td>
				        <td><%=ShowFormatedData(headlightType[3]) %></td>
				        <td><%=ShowFormatedData(headlightType[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Headlight Bulb Type</td>
				        <td><%=ShowFormatedData(headlightBulbType[0]) %></td>
				        <td><%=ShowFormatedData(headlightBulbType[1]) %></td>
				        <td><%=ShowFormatedData(headlightBulbType[2]) %></td>
				        <td><%=ShowFormatedData(headlightBulbType[3]) %></td>
				        <td><%=ShowFormatedData(headlightBulbType[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Brake/Tail Light</td>
				        <td><%=ShowFormatedData(brake_Tail_Light[0]) %></td>
				        <td><%=ShowFormatedData(brake_Tail_Light[1]) %></td>
				        <td><%=ShowFormatedData(brake_Tail_Light[2]) %></td>
				        <td><%=ShowFormatedData(brake_Tail_Light[3]) %></td>
				        <td><%=ShowFormatedData(brake_Tail_Light[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Turn Signal</td>
				        <td><%=ShowFeature(turnSignal[0]) %></td>
				        <td><%=ShowFeature(turnSignal[1]) %></td>
				        <td><%=ShowFeature(turnSignal[2]) %></td>
				        <td><%=ShowFeature(turnSignal[3]) %></td>
				        <td><%=ShowFeature(turnSignal[4]) %></td>
				    </tr>
                    <tr>
                        <td class="headerSpecs">Pass Light</td>
				        <td><%=ShowFeature(passLight[0]) %></td>
				        <td><%=ShowFeature(passLight[1]) %></td>
				        <td><%=ShowFeature(passLight[2]) %></td>
				        <td><%=ShowFeature(passLight[3]) %></td>
				        <td><%=ShowFeature(passLight[4]) %></td>
				    </tr>
			    </table>
		    </div>	  
    </div><!--    Left Container ends here -->
</div>
<!-- #include file="/includes/footerInner.aspx" -->