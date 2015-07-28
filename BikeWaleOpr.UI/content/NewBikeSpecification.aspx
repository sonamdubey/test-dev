<%@ Page AutoEventWireUp="false" Inherits="BikeWaleOpr.Content.NewBikeSpecification" Language="C#" trace="false" Debug="false" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <style type="text/css">
	    .specs { background-color:#f3f3f3; }
	    .specs th { background-color:#dddddd;text-align:left;padding:4px; }
	    .specs td { padding:3px; }
	    .example { padding:5px 10px; }
	    .error { background-color:Yellow; }
	    .cursor-pointer { cursor:pointer; }
    </style>
</head>
<body>
<div class="left">
	<h3>Bike Specification</h3>
	<form runat="server">
		<span id="spnError" class="error" runat="server"></span>
		<table class="specs" width="100%"  border="0" cellspacing="0" cellpadding="0">
			<tr>
			    <th width="35%">Bike Version</th>
			    <th><asp:Label ID="lblBike" runat="server" /></th>
			</tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <th colspan="2">Engine</th>
            </tr>
            <tr>
                <td>Displacement</td>
                <td><asp:TextBox ID="txtDisplacement" runat="server" /><span class="example">CC</span></td>
            </tr>
            <tr>
                <td>Cylinders</td>
                <td><asp:TextBox ID="txtCylinders" runat="server" /><span class="example">e.g. 1 or 2</span></td>
            </tr>
            <tr>
                <td>Max Power</td>
                <td><asp:TextBox ID="txtMaxPower" runat="server" Columns="3" /> bhp @ 
                    <asp:TextBox ID="txtMaxPowerRpm" runat="server"  Columns="5" /> rpm
                    <span class="example">e.g. 15.2 bhp @ 7000 rpm</span>
                </td>
            </tr>
            <tr>
                <td>Maximum Torque</td>
                <td>
                    <asp:TextBox ID="txtMaximumTorque" runat="server" Columns="3" /> Nm @ 
                    <asp:TextBox ID="txtMaximumTorqueRpm" runat="server"  Columns="5" /> rpm                    
                    <span class="example">e.g. 18.2 Nm @ 5000 rpm</span>
                </td>
            </tr>
            <tr>
                <td>Bore</td>
                <td><asp:TextBox ID="txtBore" runat="server" /><span class="example">mm</span></td>
            </tr>
            <tr>
                <td>Stroke</td>
                <td><asp:TextBox ID="txtStroke" runat="server" /><span class="example">mm</span></td>
            </tr>
            <tr>
                <td>Valves (per cylinder)</td>
                <td><asp:TextBox ID="txtValvesPerCylinder" runat="server" /><span class="example">e.g. 1 or 2</span></td>
            </tr>
            <tr>
                <td>Fuel Delivery System</td>
                <td><asp:TextBox ID="txtFuelDeliverySystem" runat="server" /><span class="example">e.g. Carb, Fuel Injection</span></td>
            </tr>
            <tr>
                <td>Fuel Type</td>
                <td><asp:TextBox ID="txtFuelType" runat="server" /><span class="example">e.g. Petrol</span></td>
            </tr>
            <tr>
                <td>Ignition</td>
                <td><asp:TextBox ID="txtIgnition" runat="server" /><span class="example">e.g. Digital Twin Spark Ignition</span></td>
            </tr>
            <tr>
                <td>Spark Plugs Per Cylinder</td>
                <td><asp:TextBox ID="txtSparkPlugsPerCylinder" runat="server" /><span class="example">e.g. 1, 2</span></td>
            </tr>
            <tr>
                <td>Cooling System</td>
                <td><asp:TextBox ID="txtCoolingSystem" runat="server" /><span class="example">e.g. Liquid cooled/air cooled/ air cooled with oil cooler</span></td>
            </tr>
            <tr>
                <th colspan="2">Transmission</th>
            </tr>
            <tr>
                <td>Gearbox Type</td>
                <td><asp:TextBox ID="txtGearboxType" runat="server" /><span class="example">e.g. Manual / automatic / CVT / Dual clutch automatic</span></td>
            </tr>
            <tr>
                <td>No Of Gears</td>
                <td><asp:TextBox ID="txtNoOfGears" runat="server" /><span class="example">e.g. 5 or 6</span></td>
            </tr>
            <tr>
                <td>Transmission Type</td>
                <td><asp:TextBox ID="txtTransmissionType" runat="server" /><span class="example">e.g. Chain drive / belt drive / shaft drive</span></td>
            </tr>
            <tr>
                <td>Clutch</td>
                <td><asp:TextBox ID="txtClutch" runat="server" /><span class="example">e.g. Wet Multiplate Type</span></td>
            </tr>
            <tr>
                <th colspan="2">Performance</th>
            </tr>
            <tr>
                <td>0-60 kmph</td>
                <td><asp:TextBox ID="txtPerformance_0_60_kmph" runat="server" /><span class="example">Seconds</span></td>
            </tr>
            <tr>
                <td>0-80 kmph</td>
                <td><asp:TextBox ID="txtPerformance_0_80_kmph" runat="server" /><span class="example">Seconds</span></td>
            </tr>
            <tr>
                <td>0-40 m</td>
                <td><asp:TextBox ID="txtPerformance_0_40_m" runat="server" /><span class="example">Seconds</span></td>
            </tr>
            <tr>
                <td>Top Speed</td>
                <td><asp:TextBox ID="txtTopSpeed" runat="server" /><span class="example">Kmph</span></td>
            </tr>
            <tr>
                <td>60-0 Kmph</td>
                <td><asp:TextBox ID="txtPerformance_60_0_kmph" runat="server" /><span class="example">3 sec @ 20 m</span></td>
            </tr>
            <tr>
                <td>80-0 Kmph</td>
                <td><asp:TextBox ID="txtPerformance_80_0_kmph" runat="server" /><span class="example">5 sec @ 35 m</span></td>
            </tr>
            <tr>
                <th colspan="2">Dimensions & Weight</th>
            </tr>
            <tr>
                <td>Kerb Weight</td>
                <td><asp:TextBox ID="txtKerbWeight" runat="server" /><span class="example">Kg</span></td>
            </tr>
            <tr>
                <td>Overall Length</td>
                <td><asp:TextBox ID="txtOverallLength" runat="server" /><span class="example">mm</span></td>
            </tr>
            <tr>
                <td>Overall Width</td>
                <td><asp:TextBox ID="txtOverallWidth" runat="server" /><span class="example">mm</span></td>
            </tr>
            <tr>
                <td>Overall Height</td>
                <td><asp:TextBox ID="txtOverallHeight" runat="server" /><span class="example">mm</span></td>
            </tr>
            <tr>
                <td>Wheelbase</td>
                <td><asp:TextBox ID="txtWheelbase" runat="server" /><span class="example">mm</span></td>
            </tr>
            <tr>
                <td>Ground Clearance</td>
                <td><asp:TextBox ID="txtGroundClearance" runat="server" /><span class="example">mm</span></td>
            </tr>
            <tr>
                <td>Seat Height</td>
                <td><asp:TextBox ID="txtSeatHeight" runat="server" /><span class="example">mm</span></td>
            </tr>
            <tr>
                <th colspan="2">Fuel Efficiency & Range</th>
            </tr>
            <tr>
                <td>Fuel Tank Capacity</td>
                <td><asp:TextBox ID="txtFuelTankCapacity" runat="server" /><span class="example">e.g. 15.2 Liters</span></td>
            </tr>
            <tr>
                <td>Reserve Fuel Capacity</td>
                <td><asp:TextBox ID="txtReserveFuelCapacity" runat="server" /><span class="example">e.g. 3.2 Litres</span></td>
            </tr>
            <tr>
                <td>Fuel Efficiency Overall</td>
                <td><asp:TextBox ID="txtFuelEfficiencyOverall" runat="server" /><span class="example">e.g. 36 Kmpl</span></td>
            </tr>
            <tr>
                <td>Fuel Efficiency Range</td>
                <td><asp:TextBox ID="txtFuelEfficiencyRange" runat="server" /><span class="example">e.g. 650 Km</span></td>
            </tr>
            <tr>
                <th colspan="2">Chassis & Suspension</th>
            </tr>
            <tr>
                <td>Chassis Type</td>
                <td><asp:TextBox ID="txtChassisType" runat="server" /><span class="example">e.g. Beefy frame with 1345 mm wheelbase</span></td>
            </tr>
            <tr>
                <td>Front Suspension</td>
                <td><asp:TextBox ID="txtFrontSuspension" runat="server" /><span class="example">e.g. Telescopic, with anti-friction bush</span></td>
            </tr>
            <tr>
                <td>Rear Suspension</td>
                <td><asp:TextBox ID="txtRearSuspension" runat="server" /><span class="example">e.g. 5 way adjustable, Nitrox shock absorber</span></td>
            </tr>
            <tr>
                <th colspan="2">Braking</th>
            </tr>
            <tr>
                <td>Brake Type</td>
                <td><asp:TextBox ID="txtBrakeType" runat="server" /><span class="example">e.g. Dual front petal discs and rear drum</span></td>
            </tr>
            <tr>
                <td>Front Disc</td>
                <td>
                    <span><asp:RadioButton ID="yesFrontDisc" runat="server" GroupName="FrontDisc" CssClass="cursor-pointer" /><label for="yesFrontDisc" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noFrontDisc" runat="server" GroupName="FrontDisc" CssClass="cursor-pointer" /><label for="noFrontDisc" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="frontDiscNotSure" runat="server" GroupName="FrontDisc" CssClass="cursor-pointer" Checked="true" /><label for="frontDiscNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Front Disc/ Drum Size</td>
                <td><asp:TextBox ID="txtFrontDisc_DrumSize" runat="server" /><span class="example">mm</span></td>
            </tr>
            <tr>
                <td>Rear Disc</td>
                <td>
                    <span><asp:RadioButton ID="yesRearDisc" runat="server" GroupName="RearDisc" CssClass="cursor-pointer" /><label for="yesRearDisc" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noRearDisc" runat="server" GroupName="RearDisc" CssClass="cursor-pointer" /><label for="noRearDisc" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="rearDiscNotSure" runat="server" GroupName="RearDisc" CssClass="cursor-pointer" Checked="true" /><label for="rearDiscNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Rear Disc/ Drum Size</td>
                <td><asp:TextBox ID="txtRearDisc_DrumSize" runat="server" /><span class="example">mm</span></td>
            </tr>
            <tr>
                <td>Calliper Type</td>
                <td><asp:TextBox ID="txtCalliperType" runat="server" /><span class="example">e.g. Radially-mounted six-piston floating front</span></td>
            </tr>
            <tr>
                <th colspan="2">Wheels & Tyres</th>
            </tr>
            <tr>
                <td>Wheel Size</td>
                <td><asp:TextBox ID="txtWheelSize" runat="server" /><span class="example">inches</span></td>
            </tr>
            <tr>
                <td>Front Tyre</td>
                <td><asp:TextBox ID="txtFrontTyre" runat="server" /><span class="example">e.g. 90/90 x 17</span></td>
            </tr>
            <tr>
                <td>Rear Tyre</td>
                <td><asp:TextBox ID="txtRearTyre" runat="server" /><span class="example">e.g. 120/80 x 17</span></td>
            </tr>
            <tr>
                <td>Tubeless Tyres</td>
                <td>
                    <span><asp:RadioButton ID="yesTubelessTyres" runat="server" GroupName="TubelessTyres" CssClass="cursor-pointer" /><label for="yesTubelessTyres" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noTubelessTyres" runat="server" GroupName="TubelessTyres" CssClass="cursor-pointer" /><label for="noTubelessTyres" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="tubelessTyresNotSure" runat="server" GroupName="TubelessTyres" CssClass="cursor-pointer" Checked="true" /><label for="tubelessTyresNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Radial Tyres</td>
                <td>
                    <span><asp:RadioButton ID="yesRadialTyres" runat="server" GroupName="RadialTyres" CssClass="cursor-pointer" /><label for="yesRadialTyres" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noRadialTyres" runat="server" GroupName="RadialTyres" CssClass="cursor-pointer" /><label for="noRadialTyres" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="radialTyresNotSure" runat="server" GroupName="RadialTyres" CssClass="cursor-pointer" Checked="true" /><label for="radialTyresNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Alloy Wheels</td>
                <td>
                    <span><asp:RadioButton ID="yesAlloyWheels" runat="server" GroupName="AlloyWheels" CssClass="cursor-pointer" /><label for="yesAlloyWheels" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noAlloyWheels" runat="server" GroupName="AlloyWheels" CssClass="cursor-pointer" /><label for="noAlloyWheels" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="alloyWheelsNotSure" runat="server" GroupName="AlloyWheels" CssClass="cursor-pointer" Checked="true" /><label for="alloyWheelsNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <th colspan="2">Electricals</th>
            </tr>
            <tr>
                <td>Electric System</td>
                <td><asp:TextBox ID="txtElectricSystem" runat="server" /><span class="example">e.g. 12V AC / 12V DC</span></td>
            </tr>
            <tr>
                <td>Battery</td>
                <td><asp:TextBox ID="txtBattery" runat="server" /><span class="example">e.g. AVRLA* 48V20AH</span></td>
            </tr>
            <tr>
                <td>Headlight Type</td>
                <td><asp:TextBox ID="txtHeadlightType" runat="server" /><span class="example">e.g. bulb and reflector / projector / LED</span></td>
            </tr>
            <tr>
                <td>Headlight Bulb Type</td>
                <td><asp:TextBox ID="txtHeadlightBulbType" runat="server" /><span class="example">e.g. H4 mount, 55/60W halogen</span></td>
            </tr>
            <tr>
                <td>Brake/ Tail Light</td>
                <td><asp:TextBox ID="txtBrake_Tail_Light" runat="server" /><span class="example">e.g. 5/21W x2 / LED</span></td>
            </tr>
            <tr>
                <td>Turn signal</td>
                <td><asp:TextBox ID="txtTurnsignal" runat="server" /></td>
            </tr>
            <tr>
                <td>PassLight</td>
                <td>
                    <span><asp:RadioButton ID="yesPassLight" runat="server" GroupName="PassLight" CssClass="cursor-pointer" /><label for="yesPassLight" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noPassLight" runat="server" GroupName="PassLight" CssClass="cursor-pointer" /><label for="noPassLight" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="passLightNotSure" runat="server" GroupName="PassLight" CssClass="cursor-pointer" Checked="true" /><label for="passLightNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <th colspan="2">Features</th>
            </tr>
            <tr>
                <td>Speedometer</td>
                <td><asp:TextBox ID="txtSpeedometer" runat="server" /><span class="example">e.g. Analogue/ Digital</span></td>
            </tr>
            <tr>
                <td>Tachometer</td>
                <td>
                    <span><asp:RadioButton ID="yesTachometer" runat="server" GroupName="Tachometer" CssClass="cursor-pointer" /><label for="yesTachometer" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noTachometer" runat="server" GroupName="Tachometer" CssClass="cursor-pointer" /><label for="noTachometer" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="tachometerNotSure" runat="server" GroupName="Tachometer" CssClass="cursor-pointer" Checked="true" /><label for="tachometerNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Tachometer Type</td>
                <td><asp:TextBox ID="txtTachometerType" runat="server" /><span class="example">e.g. Analog / digital</span></td>
            </tr>
            <tr>
                <td>ShiftLight</td>
                <td>
                    <span><asp:RadioButton ID="yesShiftLight" runat="server" GroupName="ShiftLight" CssClass="cursor-pointer" /><label for="yesShiftLight" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noShiftLight" runat="server" GroupName="ShiftLight" CssClass="cursor-pointer" /><label for="noShiftLight" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="shiftLightNotSure" runat="server" GroupName="ShiftLight" CssClass="cursor-pointer" Checked="true" /><label for="shiftLightNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Electric Start</td>
                <td>
                    <span><asp:RadioButton ID="yesElectricStart" runat="server" GroupName="ElectricStart" CssClass="cursor-pointer" /><label for="yesElectricStart" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noElectricStart" runat="server" GroupName="ElectricStart" CssClass="cursor-pointer" /><label for="noElectricStart" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="electricStartNotSure" runat="server" GroupName="ElectricStart" CssClass="cursor-pointer" Checked="true" /><label for="electricStartNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Tripmeter</td>
                <td>
                    <span><asp:RadioButton ID="yesTripmeter" runat="server" GroupName="Tripmeter" CssClass="cursor-pointer" /><label for="yesTripmeter" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noTripmeter" runat="server" GroupName="Tripmeter" CssClass="cursor-pointer" /><label for="noTripmeter" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="tripmeterNotSure" runat="server" GroupName="Tripmeter" CssClass="cursor-pointer" Checked="true" /><label for="tripmeterNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>No Of Tripmeters</td>
                <td><asp:TextBox ID="txtNoOfTripmeters" runat="server" /></td>
            </tr>
            <tr>
                <td>Tripmeter Type</td>
                <td><asp:TextBox ID="txtTripmeterType" runat="server" /><span class="example">e.g. Analog / digital</span></td>
            </tr>
            <tr>
                <td>Low Fuel Indicator</td>
                <td>
                    <span><asp:RadioButton ID="yesLowFuelIndicator" runat="server" GroupName="LowFuelIndicator" CssClass="cursor-pointer" /><label for="yesLowFuelIndicator" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noLowFuelIndicator" runat="server" GroupName="LowFuelIndicator" CssClass="cursor-pointer"/><label for="noLowFuelIndicator" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="lowFuelIndicatorNotSure" runat="server" GroupName="LowFuelIndicator" CssClass="cursor-pointer" Checked="true" /><label for="lowFuelIndicatorNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Low Oil Indicator</td>
                <td>
                    <span><asp:RadioButton ID="yesLowOilIndicator" runat="server" GroupName="LowOilIndicator" CssClass="cursor-pointer" /><label for="yesLowOilIndicator" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noLowOilIndicator" runat="server" GroupName="LowOilIndicator" CssClass="cursor-pointer" /><label for="noLowOilIndicator" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="lowOilIndicatorNotSure" runat="server" GroupName="LowOilIndicator" CssClass="cursor-pointer" Checked="true" /><label for="lowOilIndicatorNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Low Battery Indicator</td>
                <td>
                    <span><asp:RadioButton ID="yesLowBatteryIndicator" runat="server" GroupName="LowBatteryIndicator" CssClass="cursor-pointer" /><label for="yesLowBatteryIndicator" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noLowBatteryIndicator" runat="server" GroupName="LowBatteryIndicator" CssClass="cursor-pointer" /><label for="noLowBatteryIndicator" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="lowBatteryIndicatorNotSure" runat="server" GroupName="LowBatteryIndicator" CssClass="cursor-pointer" Checked="true" /><label for="lowBatteryIndicatorNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Fuel Gauge</td>
                <td>
                    <span><asp:RadioButton ID="yesFuelGauge" runat="server" GroupName="FuelGauge" CssClass="cursor-pointer" /><label for="yesFuelGauge" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noFuelGauge" runat="server" GroupName="FuelGauge" CssClass="cursor-pointer" /><label for="noFuelGauge" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="fuelGaugeNotSure" runat="server" GroupName="FuelGauge" CssClass="cursor-pointer" Checked="true" /><label for="fuelGaugeNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Digital Fuel Gauge</td>
                <td>
                    <span><asp:RadioButton ID="yesDigitalFuelGauge" runat="server" GroupName="DigitalFuelGauge" CssClass="cursor-pointer" /><label for="yesDigitalFuelGauge" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noDigitalFuelGauge" runat="server" GroupName="DigitalFuelGauge" CssClass="cursor-pointer" /><label for="noDigitalFuelGauge" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="digitalFuelGaugeNotSure" runat="server" GroupName="DigitalFuelGauge" CssClass="cursor-pointer" Checked="true" /><label for="digitalFuelGaugeNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Pillion Seat</td>
                <td>
                    <span><asp:RadioButton ID="yesPillionSeat" runat="server" GroupName="PillionSeat" CssClass="cursor-pointer" /><label for="yesPillionSeat" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noPillionSeat" runat="server" GroupName="PillionSeat" CssClass="cursor-pointer" /><label for="noPillionSeat" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="pillionSeatNotSure" runat="server" GroupName="PillionSeat" CssClass="cursor-pointer" Checked="true" /><label for="pillionSeatNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Pillion Footrest</td>
                <td>
                    <span><asp:RadioButton ID="yesPillionFootrest" runat="server" GroupName="PillionFootrest" CssClass="cursor-pointer" /><label for="yesPillionFootrest" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noPillionFootrest" runat="server" GroupName="PillionFootrest" CssClass="cursor-pointer" /><label for="noPillionFootrest" class="cursor-pointer" >No</label></span>
                    <span><asp:RadioButton ID="pillionFootrestNotSure" runat="server" GroupName="PillionFootrest" CssClass="cursor-pointer" Checked="true" /><label for="pillionFootrestNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Pillion Backrest</td>
                <td>
                    <span><asp:RadioButton ID="yesPillionBackrest" runat="server" GroupName="PillionBackrest" CssClass="cursor-pointer" /><label for="yesPillionBackrest" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noPillionBackrest" runat="server" GroupName="PillionBackrest" CssClass="cursor-pointer" /><label for="noPillionBackrest" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="pillionBackrestNotSure" runat="server" GroupName="PillionBackrest" CssClass="cursor-pointer" Checked="true" /><label for="pillionBackrestNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Pillion Grabrail</td>
                <td>
                    <span><asp:RadioButton ID="yesPillionGrabrail" runat="server" GroupName="PillionGrabrail" CssClass="cursor-pointer" /><label for="yesPillionGrabrail" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noPillionGrabrail" runat="server" GroupName="PillionGrabrail" CssClass="cursor-pointer" /><label for="noPillionGrabrail" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="pillionGrabrailNotSure" runat="server" GroupName="PillionGrabrail" CssClass="cursor-pointer" Checked="true" /><label for="pillionGrabrailNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Stand Alarm</td>
                <td>
                    <span><asp:RadioButton ID="yesStandAlarm" runat="server" GroupName="StandAlarm" CssClass="cursor-pointer" /><label for="yesStandAlarm" class="cursor-pointer" >Yes</label></span>
                    <span><asp:RadioButton ID="noStandAlarm" runat="server" GroupName="StandAlarm" CssClass="cursor-pointer" /><label for="noStandAlarm" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="standAlarmNotSure" runat="server" GroupName="StandAlarm" CssClass="cursor-pointer" Checked="true" /><label for="standAlarmNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Stepped Seat</td>
                <td>
                    <span><asp:RadioButton ID="yesSteppedSeat" runat="server" GroupName="SteppedSeat" CssClass="cursor-pointer" /><label for="yesSteppedSeat" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noSteppedSeat" runat="server" GroupName="SteppedSeat" CssClass="cursor-pointer" /><label for="noSteppedSeat" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="steppedSeatNotSure" runat="server" GroupName="SteppedSeat" CssClass="cursor-pointer" Checked="true" /><label for="steppedSeatNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Antilock Braking System</td>
                <td>
                    <span><asp:RadioButton ID="yesAntilockBrakingSystem" runat="server" GroupName="AntilockBrakingSystem" CssClass="cursor-pointer" /><label for="yesAntilockBrakingSystem" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noAntilockBrakingSystem" runat="server" GroupName="AntilockBrakingSystem" CssClass="cursor-pointer" /><label for="noAntilockBrakingSystem" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="absNotSure" runat="server" GroupName="AntilockBrakingSystem" CssClass="cursor-pointer" Checked="true" /><label for="absNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Kill Switch</td>
                <td>
                    <span><asp:RadioButton ID="yesKillswitch" runat="server" GroupName="Killswitch" CssClass="cursor-pointer" /><label for="yesKillswitch" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noKillswitch" runat="server" GroupName="Killswitch" CssClass="cursor-pointer" /><label for="noKillswitch" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="killswitchNotSure" runat="server" GroupName="Killswitch" CssClass="cursor-pointer" Checked="true" /><label for="killswitchNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Clock</td>
                <td>
                    <span><asp:RadioButton ID="yesClock" runat="server" GroupName="Clock" CssClass="cursor-pointer" /><label for="yesClock" class="cursor-pointer">Yes</label></span>
                    <span><asp:RadioButton ID="noClock" runat="server" GroupName="Clock" CssClass="cursor-pointer" Checked="true" /><label for="noClock" class="cursor-pointer">No</label></span>
                    <span><asp:RadioButton ID="clockNotSure" runat="server" GroupName="Clock" CssClass="cursor-pointer" Checked="true" /><label for="clockNotSure" class="cursor-pointer">Not Sure</label></span>
                </td>
            </tr>
            <tr>
                <td>Colors</td>
                <td><asp:TextBox ID="txtColors" runat="server" /><span class="example">e.g. Red,Black</span></td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2"><asp:Button ID="btnSubmit" runat="server" Text="Save Data" /></td>
            </tr>
		</table>
	</form>
</div>
</body>
</html>