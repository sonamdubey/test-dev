<%@Page Trace="false" validateRequest="false" Inherits="BikeWaleOpr.Content.PriceMonitoring" Language="C#" AutoEventWireUp="false" EnableEventValidation="false"%>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<!-- #Include file="/includes/headerNew.aspx" -->

<style>
	.tblStock { border-collapse:collapse; border:1px solid #BDBDBD; font-family:Arial, Helvetica, sans-serif; font-size:12px;table-layout:fixed;width:700px;}
	.tblStock th{background:#eeeeee; }
	.sevendays{background-color:#009900; color:#FFFFFF; font-weight:bold; text-decoration:none;}
	.fifteendays{background-color:#FFFF99;color:#000000; font-weight:bold; text-decoration:none;}
	.onemonth{background-color:#FF9900;color:#000000; font-weight:bold; text-decoration:none;}
	.moreThanMonth{background-color:#FF0000;color:#FFFFFF; font-weight:bold; text-decoration:none;}
	.na{background-color:#000000; font-weight:bold;color:#FFFFFF; text-decoration:none;}

</style>
<div class="urh">
	You are here &raquo; <a href="/content/default.aspx">Contents</a> &raquo; Price Monitoring
</div>
<script type="text/javascript" src="/src/AjaxFunctions.js"></script>
<div>
    <!-- #Include file="contentsMenu.aspx" -->
</div>
<div class="left">
    <h1>Price Monitoring report</h1>
<fieldset>
<legend>Price Monitoring Report</legend>
<div>
	<h3>
		Price Monitoring Report &nbsp;&nbsp;
		<span class="na"><strong>&nbsp;&nbsp;NA&nbsp;&nbsp;</strong></span> &nbsp;&nbsp;
		<span class="sevendays"><strong><=7 days</strong></span> &nbsp;&nbsp;
		<span class="fifteendays"><strong> 7 Days < & <=15 Days </strong></span> &nbsp;&nbsp;
		<span class="onemonth"><strong>15 Days < & <=1 Month</strong></span> &nbsp;&nbsp;
		<span class="moreThanMonth"><strong>More Than 1 Month</strong></span> &nbsp;&nbsp;
	</h3>
	<br />
	
		Select Bike Make 
		<asp:DropDownList ID="drpMake" CssClass="drpClass" runat="server"></asp:DropDownList>
		-
		<asp:DropDownList ID="drpModel" Enabled="false" runat="server">
			<asp:ListItem Text="--Select--" Value="0" />				
		</asp:DropDownList>
		<input type="hidden" id="hdn_drpModel" runat="server" />&nbsp;
		<asp:Button runat="server" ID="btnShow" Text="Show"></asp:Button>
		<br /><br />
		<asp:Panel ID="pnlPriceMonitoring" Visible="false" runat="server">
			<div style="overflow:hidden;" id="scrollLeft1">
				<table width="100%" border="1" cellpadding="3" class="tblStock">
					<tr>
						<th style="width:100px;"><strong>City/Model</strong></th>
						<asp:Repeater ID="rptModelVersion" runat="server">
							<itemtemplate>
								<th align="center" style="width:45px;"><%# DataBinder.Eval(Container.DataItem, "BikeName")%></th>
							</itemtemplate>
						</asp:Repeater>
						<th style="width:100px;"><strong>Model/City</strong></th>
					</tr>
				</table>
			</div>
			<div style="height:300px;overflow:auto;" id="scrollLeft2">
				<table width="100%" border="1" cellpadding="3" class="tblStock">
					<asp:Repeater ID="rptCity" runat="server">
						<itemtemplate>						
							<tr>
								<th style="width:100px;" align="left"><%# DataBinder.Eval(Container.DataItem, "CityName")%></th>							
								<asp:Repeater runat="server" DataSource='<%# GetVersionPrices(DataBinder.Eval(Container.DataItem,"ID").ToString(),DataBinder.Eval(Container.DataItem,"CityName").ToString()) %>'>
									<itemtemplate>
										<td style="width:45px;" title='Price for : <%# DataBinder.Eval(Container.DataItem,"CityName") %>,Version : <%# DataBinder.Eval(Container.DataItem,"BikeName") %> '  class='<%# DataBinder.Eval(Container.DataItem,"Class") %>' }>
											<a href="ShowroomPrices.aspx?City=<%#DataBinder.Eval(Container.DataItem,"CityId")%>&Make=<%#DataBinder.Eval(Container.DataItem,"MakeId")%>&Model=<%#DataBinder.Eval(Container.DataItem,"ModelId")%>&Version=<%#DataBinder.Eval(Container.DataItem,"VersionId")%>" target="_blank" class='<%# DataBinder.Eval(Container.DataItem,"Class") %>'>
												<%# DataBinder.Eval(Container.DataItem,"Value") %>
											</a>
										</td>
									</itemtemplate>
								</asp:Repeater>
								<th style="width:100px;" align="left"><%# DataBinder.Eval(Container.DataItem, "CityName")%></th>
							</tr>
						</itemtemplate>
					</asp:Repeater>
				</table>
			</div>
			<a href="javascript:move_up()"><strong>Show Next</strong></a>
		</asp:Panel>
	
</div>
</fieldset>
</div>
<script language="javascript">
	document.getElementById("drpMake").onchange = drpMake_Change;
	function drpMake_Change(e) 
	{
		var response; 	
		var makeId = document.getElementById("drpMake").value;
		var dependentCmbs = new Array;
		dependentCmbs[0] = "drpModel";
		
		response = AjaxFunctions.GetModels(makeId);
		
		FillCombo_Callback(response, document.getElementById("drpModel"), "hdn_drpModel", dependentCmbs);
	}
	
	function move_up() 
	{
    	document.getElementById("scrollLeft1").style.width = "1500px";
		document.getElementById("scrollLeft2").style.width = "1500px";
  	}

</script>
<!-- #Include file="/includes/footerNew.aspx" -->