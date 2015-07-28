<%@ Page Inherits="BikeWaleOpr.Content.NewBikeStandardFeaturesStep2" AutoEventWireUp="false" Language="C#" trace="false" Debug="false" %>
<%@ Import Namespace="BikeWaleOpr.Common" %> 

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <style type="text/css">
	    .doNotDisplay { display:none; }
	    td, tr, table { border-color:white; }
	    .error { background-color:Yellow; }
    </style>
</head>
<body>
<div class="left">
	<h3>Bike Standard Features</h3>
	<form runat="server">
		<span id="spnError" class="error" runat="server"></span>
		<table align="left" width="80%" border="0">
			<tr>
			  	<th colspan="3" align="left">Bike Version -- <asp:Label ID="lblBike" runat="server" /></th>
			</tr>
			<tr>
			  <th colspan="4" valign="top">&nbsp;</th>
			  </tr>
			<tr>
			  <td valign="top">Air Conditioner&nbsp;</td>
			  <td width="22%" valign="top">
				<asp:DropDownList ID="cmbAC" runat="server">
					<asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />		  	</asp:DropDownList>		  </td>
			  <td width="27%" valign="top">Cassette&nbsp;Player</td>
			  <td width="21%" valign="top"><asp:DropDownList ID="cmbCassettePlayer" runat="server">
				  <asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />          
				</asp:DropDownList>          </td>
			</tr>
			<tr>
			  <td valign="top">Power Windows </td>
			  <td valign="top">
				<asp:DropDownList ID="cmbPW" runat="server">
					<asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />		  	</asp:DropDownList>		  </td>
			  <td valign="top">CD&nbsp;Player</td>
			  <td valign="top"><asp:DropDownList ID="cmbCD" runat="server">
				  <asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />         
				</asp:DropDownList>          </td>
			</tr>
			<tr>
			  <td valign="top">Power Door Locks</td>
			  <td valign="top">
				<asp:DropDownList ID="cmbPowerDoorLocks" runat="server">
					<asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />		</asp:DropDownList>		  </td>
			  <td valign="top">Sun-Roof</td>
			  <td valign="top"><asp:DropDownList ID="cmbSun" runat="server">
				  <asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />       
				</asp:DropDownList>          </td>
			</tr>
			<tr>
			  <td valign="top">Power Steering</td>
			  <td valign="top">
				<asp:DropDownList ID="cmbPS" runat="server">
					<asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />	  	</asp:DropDownList>		  </td>
			  <td valign="top">Moon-Roof</td>
			  <td valign="top"><asp:DropDownList ID="cmbMoon" runat="server">
				  <asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />        
				</asp:DropDownList>          </td>
			</tr>
			<tr>
			  <td valign="top">Anti-Braking System </td>
			  <td valign="top">
				<asp:DropDownList ID="cmbABS" runat="server">
					<asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />		</asp:DropDownList>		  </td>
			  <td valign="top">Immobilizer</td>
			  <td valign="top"><asp:DropDownList ID="cmbImmobilizer" runat="server">
				  <asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />     
				</asp:DropDownList>          </td>
			</tr>
			<tr>
			  <td valign="top">Traction Control </td>
			  <td valign="top">
				<asp:DropDownList ID="cmbTractionControl" runat="server">
					<asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />		</asp:DropDownList>		  </td>
			  <td valign="top">DriverAirBags</td>
			  <td valign="top"><asp:DropDownList ID="cmbDriverAirBags" runat="server">
				  <asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />    
				</asp:DropDownList>          </td>
			</tr>  
			<tr>
			  <td valign="top">Steering Adjustment</td>
			  <td valign="top">
				<asp:DropDownList ID="cmbSteeringAdjustment" runat="server">
					<asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />		</asp:DropDownList>		  </td>
			  <td valign="top">PassengerAirBags</td>
			  <td valign="top"><asp:DropDownList ID="cmbPassengerAirBags" runat="server">
				  <asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />    
				</asp:DropDownList>          </td>
			</tr>
			<tr>
			  <td valign="top">Tachometer</td>
			  <td valign="top">
				<asp:DropDownList ID="cmbTacho" runat="server">
					<asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />	  	</asp:DropDownList>		  </td>
			  <td valign="top">RemoteBootFuelLid</td>
			  <td valign="top"><asp:DropDownList ID="cmbRemoteBootFuelLid" runat="server">
				  <asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />        
				</asp:DropDownList>          </td>
			</tr>
			<tr>
			  <td valign="top">Child Safety Locks&nbsp;</td>
			  <td valign="top">
				<asp:DropDownList ID="cmbChildSafetyLocks" runat="server">
					<asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />	  	</asp:DropDownList>		  </td>
			  <td valign="top">CupHolder</td>
			  <td valign="top"><asp:DropDownList ID="cmbCupHolder" runat="server">
				  <asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />       
				</asp:DropDownList>          </td>
			</tr>
			<tr>
			  <td valign="top">Front Fog Lights</td>
			  <td valign="top">
				<asp:DropDownList ID="cmbFogLights" runat="server">
					<asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />		</asp:DropDownList>		  </td>
			  <td valign="top">SplitFoldingRearSeats</td>
			  <td valign="top"><asp:DropDownList ID="cmbSplitFoldingRearSeats" runat="server">
				 <asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />        
				</asp:DropDownList>          </td>
			</tr>
			<tr>
			  <td valign="top">Rear Defroster</td>
			  <td valign="top">
				<asp:DropDownList ID="cmbDefroster" runat="server">
					<asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />		</asp:DropDownList>		  </td>
			  <td valign="top">RearWashWiper</td>
			  <td valign="top"><asp:DropDownList ID="cmbRearWashWiper" runat="server">
				  <asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />       
				</asp:DropDownList>          </td>
			</tr>
			<tr>
			  <td valign="top">Defogger (Rear)&nbsp;</td>
			  <td valign="top">
				<asp:DropDownList ID="cmbDefogger" runat="server">
					<asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />		</asp:DropDownList>		  </td>
			  <td valign="top">CentralLocking</td>
			  <td valign="top"><asp:DropDownList ID="cmbCentralLocking" runat="server">
				  <asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />       
				</asp:DropDownList>          </td>
			</tr>
			<tr>
			  <td valign="top">Leather Seats</td>
			  <td valign="top">
				<asp:DropDownList ID="cmbSeats" runat="server">
					<asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />		</asp:DropDownList>		  </td>
			  <td valign="top">AlloyWheels</td>
			  <td valign="top"><asp:DropDownList ID="cmbAlloyWheels" runat="server">
				  <asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />          
				</asp:DropDownList>          </td>
			</tr>
			<tr>
			  <td valign="top">Power Seats</td>
			  <td valign="top">
				<asp:DropDownList ID="cmbPowerSeats" runat="server">
					<asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />			</asp:DropDownList>		  </td>
			  <td valign="top">TubelessTyres</td>
			  <td valign="top"><asp:DropDownList ID="cmbTubelessTyres" runat="server">
				  <asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />        
				</asp:DropDownList>          </td>
			</tr>
			<tr>
			  <td valign="top">AM/FM&nbsp;Radio</td>
			  <td valign="top">
				<asp:DropDownList ID="cmbRadio" runat="server">
					<asp:ListItem Text="-" Value="-" />
					<asp:ListItem Text="Available" Value="A" />
					<asp:ListItem Text="Not Available" Value="N" />
					<asp:ListItem Text="Optional" Value="O" />	  	</asp:DropDownList>		  </td>
			  <td valign="top">&nbsp;</td>
			  <td valign="top">&nbsp;</td>
			</tr>
			<tr>
				<td colspan="4" align="center">&nbsp;			</td>
			</tr>
			<tr>
				<td colspan="4" align="center">
					<asp:Button ID="btnSave" Text="Save Features" runat="server" />			</td>
			</tr>
		</table>
	</form>
</div>
</body>
</html>