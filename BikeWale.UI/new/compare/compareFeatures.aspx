<%@ Page Language="C#" Inherits="Bikewale.New.BikeComparison"  trace="false" Debug="false" AutoEventWireUp="false" %>
<%
    title = "Features Comparison - " + pageTitle;
    description = "BikeWale&reg; - Compare " + pageTitle + " features. Compare interior, exterior, comfort, security and safety features of the bikes on BikeWale.";
	keywords = ""; 
%>
<!-- #include file="/includes/headNew.aspx" -->
<script language="javascript" type="text/javascript">
	$(document).ready(function(){
		if( $("#tblCompare").length > 0 ){
		    var featuredBikeIndex = <%= featuredBikeIndex + 2 %>;					
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
		<h1 class="margin-bottom15">Features Comparison - <asp:Literal id="ltrTitle" runat="server"></asp:Literal></h1>		
		<div class="padding-top20 featured-bike-tabs">
            <ul class="featured-bike-tabs-inner padding-top20">
                <li class="first_tab"><a href="compareSpecs.aspx?bike1=<%=Request["bike1"]%>&amp;bike2=<%=Request["bike2"]%>&amp;bike3=<%=Request["bike3"]%>&amp;bike4=<%=Request["bike4"]%>">Specs</a></li>
                <li class="fbike-active-tab">Features</li>
                <li><a href="compareColors.aspx?bike1=<%=Request["bike1"]%>&amp;bike2=<%=Request["bike2"]%>&amp;bike3=<%=Request["bike3"]%>&amp;bike4=<%=Request["bike4"]%>">Colours</a></li>                          
            </ul>
        </div>
		<div class="tab_inner_container">
			<table width="100%" id="tblCompare" runat="server" class="tbl-compare" cellpadding="5" cellspacing="0">
			        <!-- #include file="compareCommon.aspx" -->						
				        <tr class="headerSpecs">
					        <th>Features</th>
					        <th>&nbsp;</th>
					        <th>&nbsp;</th>
					        <th>&nbsp;</th>
					        <th>&nbsp;</th>
					        <th>&nbsp;</th>
				        </tr>
				        <tr>
				          <td class="headerSpecs">Speedometer</td>
				          <td><%=ShowFormatedData(speedometer[0]) %></td>
				          <td><%=ShowFormatedData(speedometer[1]) %></td>
				          <td><%=ShowFormatedData(speedometer[2]) %></td>
				          <td><%=ShowFormatedData(speedometer[3]) %></td>
				          <td><%=ShowFormatedData(speedometer[4]) %></td>
				        </tr>
				        <tr >
				          <td class="headerSpecs">Tachometer</td>
				          <td><%=ShowFeature( tachometer[0] ) %></td>
				          <td><%=ShowFeature( tachometer[1] ) %></td>
				          <td><%=ShowFeature( tachometer[2] ) %></td>
				          <td><%=ShowFeature( tachometer[3] ) %></td>
				          <td><%=ShowFeature( tachometer[4] ) %></td>
				        </tr>
				        <tr>
				          <td class="headerSpecs">Tachometer Type</td>
				          <td><%=ShowFormatedData(tachometerType[0]) %></td>
				          <td><%=ShowFormatedData(tachometerType[1]) %></td>
				          <td><%=ShowFormatedData(tachometerType[2]) %></td>
				          <td><%=ShowFormatedData(tachometerType[3]) %></td>
				          <td><%=ShowFormatedData(tachometerType[4]) %></td>
				        </tr>
				        <tr>
				          <td class="headerSpecs">Shift Light</td>
				          <td><%=ShowFeature( shiftLight[0] ) %></td>
				          <td><%=ShowFeature( shiftLight[1] ) %></td>
				          <td><%=ShowFeature( shiftLight[2] ) %></td>
				          <td><%=ShowFeature( shiftLight[3] ) %></td>
				          <td><%=ShowFeature( shiftLight[4] ) %></td>
				        </tr>
				        <tr>
				          <td class="headerSpecs">Electric Start</td>
				          <td><%=ShowFeature( electricStart[0] ) %></td>
				          <td><%=ShowFeature( electricStart[1] ) %></td>
				          <td><%=ShowFeature( electricStart[2] ) %></td>
				          <td><%=ShowFeature( electricStart[3] ) %></td>
				          <td><%=ShowFeature( electricStart[4] ) %></td>
				        </tr>
				        <tr >
				          <td class="headerSpecs">Tripmeter</td>
				          <td><%=ShowFeature( tripmeter[0] ) %></td>
				          <td><%=ShowFeature( tripmeter[1] ) %></td>
				          <td><%=ShowFeature( tripmeter[2] ) %></td>
				          <td><%=ShowFeature( tripmeter[3] ) %></td>
				          <td><%=ShowFeature( tripmeter[4] ) %></td>
				        </tr>
				        <tr>
				          <td class="headerSpecs">No Of Tripmeters</td>
				          <td><%=ShowFormatedData( noOfTripmeters[0] ) %></td>					
				          <td><%=ShowFormatedData( noOfTripmeters[1] ) %></td>					
				          <td><%=ShowFormatedData( noOfTripmeters[2] ) %></td>					
				          <td><%=ShowFormatedData( noOfTripmeters[3] ) %></td>
				          <td><%=ShowFormatedData( noOfTripmeters[4] ) %></td>
				        </tr>
				        <tr >
				          <td class="headerSpecs">Tripmeter Type</td>
				          <td><%=ShowFormatedData(tripmeterType[0]) %></td>
				          <td><%=ShowFormatedData(tripmeterType[1]) %></td>
				          <td><%=ShowFormatedData(tripmeterType[2]) %></td>
				          <td><%=ShowFormatedData(tripmeterType[3]) %></td>
				          <td><%=ShowFormatedData(tripmeterType[4]) %></td>
				        </tr>
				        <tr>
				          <td class="headerSpecs">Low Fuel Indicator</td>
				          <td><%=ShowFeature( lowFuelIndicator[0] ) %></td>
				          <td><%=ShowFeature( lowFuelIndicator[1] ) %></td>
				          <td><%=ShowFeature( lowFuelIndicator[2] ) %></td>
				          <td><%=ShowFeature( lowFuelIndicator[3] ) %></td>
				          <td><%=ShowFeature( lowFuelIndicator[4] ) %></td>
				        </tr>
				        <tr >
				          <td class="headerSpecs">Low Oil Indicator</td>
				          <td><%=ShowFeature( lowOilIndicator[0] ) %></td>
				          <td><%=ShowFeature( lowOilIndicator[1] ) %></td>
				          <td><%=ShowFeature( lowOilIndicator[2] ) %></td>
				          <td><%=ShowFeature( lowOilIndicator[3] ) %></td>
				          <td><%=ShowFeature( lowOilIndicator[4] ) %></td>
				        </tr>
				        <tr>
				          <td class="headerSpecs">Low Battery Indicator</td>
				          <td><%=ShowFeature( lowBatteryIndicator[0] ) %></td>
				          <td><%=ShowFeature( lowBatteryIndicator[1] ) %></td>
				          <td><%=ShowFeature( lowBatteryIndicator[2] ) %></td>
				          <td><%=ShowFeature( lowBatteryIndicator[3] ) %></td>
				          <td><%=ShowFeature( lowBatteryIndicator[4] ) %></td>
				        </tr>
				        <tr >
				          <td class="headerSpecs">Fuel Gauge</td>
				          <td><%=ShowFeature( fuelGauge[0] ) %></td>
				          <td><%=ShowFeature( fuelGauge[1] ) %></td>
				          <td><%=ShowFeature( fuelGauge[2] ) %></td>
				          <td><%=ShowFeature( fuelGauge[3] ) %></td>
				          <td><%=ShowFeature( fuelGauge[4] ) %></td>
				        </tr>
				        <tr>
				          <td class="headerSpecs">Digital Fuel Gauges</td>
				          <td><%=ShowFeature( digitalFuelGauge[0] ) %></td>
				          <td><%=ShowFeature( digitalFuelGauge[1] ) %></td>
				          <td><%=ShowFeature( digitalFuelGauge[2] ) %></td>
				          <td><%=ShowFeature( digitalFuelGauge[3] ) %></td>
				          <td><%=ShowFeature( digitalFuelGauge[4] ) %></td>
				        </tr>
				        <tr >
				          <td class="headerSpecs">Pillion Seat</td>
				          <td><%=ShowFeature( pillionSeat[0] ) %></td>
				          <td><%=ShowFeature( pillionSeat[1] ) %></td>
				          <td><%=ShowFeature( pillionSeat[2] ) %></td>
				          <td><%=ShowFeature( pillionSeat[3] ) %></td>
				          <td><%=ShowFeature( pillionSeat[4] ) %></td>
				        </tr>
				        <tr>
				          <td class="headerSpecs">Pillion Footrest</td>
				          <td><%=ShowFeature( pillionFootrest[0] ) %></td>
				          <td><%=ShowFeature( pillionFootrest[1] ) %></td>
				          <td><%=ShowFeature( pillionFootrest[2] ) %></td>
				          <td><%=ShowFeature( pillionFootrest[3] ) %></td>
				          <td><%=ShowFeature( pillionFootrest[4] ) %></td>		
				        </tr>
				        <tr >
				          <td class="headerSpecs">Pillion Backrest</td>
				          <td><%=ShowFeature( pillionBackrest[0] ) %></td>
				          <td><%=ShowFeature( pillionBackrest[1] ) %></td>
				          <td><%=ShowFeature( pillionBackrest[2] ) %></td>
				          <td><%=ShowFeature( pillionBackrest[3] ) %></td>
				          <td><%=ShowFeature( pillionBackrest[4] ) %></td>
				        </tr>
				        <tr>
				          <td class="headerSpecs">Pillion Grabrail</td>
				          <td><%=ShowFeature( pillionGrabrail[0] ) %></td>
				          <td><%=ShowFeature( pillionGrabrail[1] ) %></td>
				          <td><%=ShowFeature( pillionGrabrail[2] ) %></td>
				          <td><%=ShowFeature( pillionGrabrail[3] ) %></td>
				          <td><%=ShowFeature( pillionGrabrail[4] ) %></td>
				        </tr>
				        <tr >
				          <td class="headerSpecs">Stand Alarm</td>
				          <td><%=ShowFeature( standAlarm[0] ) %></td>
				          <td><%=ShowFeature( standAlarm[1] ) %></td>
				          <td><%=ShowFeature( standAlarm[2] ) %></td>
				          <td><%=ShowFeature( standAlarm[3] ) %></td>
				          <td><%=ShowFeature( standAlarm[4] ) %></td>
				        </tr>
				        <tr>
				          <td class="headerSpecs">Stepped Seat</td>
				          <td><%=ShowFeature( steppedSeat[0] ) %></td>
				          <td><%=ShowFeature( steppedSeat[1] ) %></td>
				          <td><%=ShowFeature( steppedSeat[2] ) %></td>
				          <td><%=ShowFeature( steppedSeat[3] ) %></td>
				          <td><%=ShowFeature( steppedSeat[4] ) %></td>
				        </tr>
				        <tr >
				          <td class="headerSpecs">Antilock Braking System</td>
				          <td><%=ShowFeature( antilockBrakingSystem[0] ) %></td>
				          <td><%=ShowFeature( antilockBrakingSystem[1] ) %></td>
				          <td><%=ShowFeature( antilockBrakingSystem[2] ) %></td>
				          <td><%=ShowFeature( antilockBrakingSystem[3] ) %></td>
				          <td><%=ShowFeature( antilockBrakingSystem[4] ) %></td>
				        </tr>
				        <tr>
				          <td class="headerSpecs">Killswitch</td>
				          <td><%=ShowFeature( killswitch[0] ) %></td>
				          <td><%=ShowFeature( killswitch[1] ) %></td>
				          <td><%=ShowFeature( killswitch[2] ) %></td>
				          <td><%=ShowFeature( killswitch[3] ) %></td>
				          <td><%=ShowFeature( killswitch[4] ) %></td>
				        </tr>
				        <tr >
				          <td class="headerSpecs">Clock</td>
				          <td><%=ShowFeature( clock[0] ) %></td>
				          <td><%=ShowFeature( clock[1] ) %></td>
				          <td><%=ShowFeature( clock[2] ) %></td>
				          <td><%=ShowFeature( clock[3] ) %></td>
				          <td><%=ShowFeature( clock[4] ) %></td>
				        </tr>
				        <tr>
				          <td class="headerSpecs">Colors</td>
				          <td><%=ShowFormatedData(colors[0]) %></td>
				          <td><%=ShowFormatedData(colors[1]) %></td>
				          <td><%=ShowFormatedData(colors[2]) %></td>
				          <td><%=ShowFormatedData(colors[3]) %></td>
				          <td><%=ShowFormatedData(colors[4]) %></td>
				        </tr>
			        </table>
		</div>	  
    </div>
</div>
<!-- #include file="/includes/footerInner.aspx" -->
