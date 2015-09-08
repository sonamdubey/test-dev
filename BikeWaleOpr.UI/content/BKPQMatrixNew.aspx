<%@ Page Language="C#" Inherits="BikeWaleOpr.Content.BKPQMatrixNew" SmartNavigation="false" AutoEventWireup="false" Trace="false" %>
<%@ Register TagPrefix="Vspl" TagName="Calendar" src="../Controls/DateControl.ascx" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<style type="text/css">
	.tbh {background-color:#F0F0F0;font-weight:bold}
	.tdh {background-color:#D5DFBF;color:#64773C;font-weight:bold}
	.child td{background-color:#F8F2E0;}
</style>

<div class="urh">
	You are here &raquo; <a href="/content/default.aspx">Contents</a> &raquo; Price Quote Matrix Daily
</div>

	<table width="100%" style="font-size:12px;">
		<tr>
			<td width="190px" >
				<strong>Select Month</strong>
				<Vspl:Calendar id="selDate" DateId="selDate" runat="server" FutureTolerance="1" MonthYear="true"/>
            </td>
            <td width="120px">
                <asp:RadioButtonList id="rbtnlOption" runat="server" RepeatDirection="horizontal">
                    <asp:ListItem Text="City" value="2" selected="true" ></asp:ListItem>
                    <asp:ListItem Text="State" value="1" ></asp:ListItem>        
                </asp:RadioButtonList>
                <input type="hidden" id="hdn_rbtn" runat="server" />
            </td>
            <td width="100px">
                <asp:DropDownList ID="drpState" runat="server"></asp:DropDownList>
				<asp:DropDownList ID="drpCities" runat="server"></asp:DropDownList>
                </td>
                <td>
				<span id="spnCities" class="errorMessage"></span>
				<asp:Button ID="butShow" runat="server" Text="View" CssClass="Submit" />
			</td>
			<td align="right">
				<asp:Label ID="lblMessage" CssClass="errorMessage" runat="server" display="none"></asp:Label>            
				<asp:Button ID="butExcel" runat="server" CssClass="button" Text="Send to Excel" />
			</td>
            <td>
               
            </td>
		</tr>
	</table>
    <asp:TextBox  id="txt_City" runat="server" style=" display:none;" ></asp:TextBox>
    <asp:TextBox  id="txt_State" runat="server" style=" display:none;" ></asp:TextBox>
    <asp:TextBox  id="txt_Month" runat="server" style=" display:none;" ></asp:TextBox>
    <asp:TextBox  id="txt_Year" runat="server" style=" display:none;" ></asp:TextBox>
	<table width="100%"  border="0" cellspacing="0" cellpadding="5">
		<tr>
        	<td >
				<asp:Panel ID="pnlReport" runat="server" Visible="false">
					<h3 align="center">Price Quote Matrix</h3> 
					<%--<span class="errorMessage">Note : X/Y - Y Represents the number of price quotes taken and X represents the number of price quotes where customer opted to buy a Bike.</span>--%>
	   		  		<br /><br />
					<table border="1" width="100%" style="border-collapse:collapse" class="lstTable" cellpadding="0" cellspacing="0" align="center">
						<tr>
							<td width="22%" style="background-color:#F0F0F0;font-weight:bold">Makes</td>
							<td style="background-color:#F0F0F0;font-weight:bold">Total</td>
							<td style="background-color:#F0F0F0;font-weight:bold">Proj</td>
							<asp:Repeater runat="server" ID="rptDays">
								<itemtemplate>
									<td style="background-color:#F0F0F0;font-weight:bold">
										<%# DataBinder.Eval(Container.DataItem, "Value")%> <br />
										<%# DataBinder.Eval(Container.DataItem, "Text")%>
									</td>
								</itemtemplate>
							</asp:Repeater>
							<td style="background-color:#F0F0F0;font-weight:bold">Total</td>
							<td width="22%" style="background-color:#F0F0F0;font-weight:bold">Makes</td>
						</tr>

						<asp:Repeater ID="rptMakes" runat="server">
							<itemtemplate>
								<tr>
									<td bgcolor="#F0F0F0" nowrap="nowrap">
										<a href='javascript:showHideChild(<%# DataBinder.Eval(Container.DataItem, "MakeId")%>)'>
											<img id='img_<%# DataBinder.Eval(Container.DataItem, "MakeId")%>' src="/images/plus.gif" border="0">
											<strong><%# DataBinder.Eval(Container.DataItem, "Make")%></strong>
										</a>
                                        <%--<img src="/images/graph.png" alt="Graph" style="cursor:pointer;" title="ViewTrend" onclick='javascript:GoUrl(<%# DataBinder.Eval(Container.DataItem, "MakeId")%>);'  />                         --%>
									</td>
									<asp:Repeater runat="server" DataSource='<%# GetMakesData(DataBinder.Eval(Container.DataItem, "MakeId").ToString()) %>'>
										<itemtemplate>
											<td align="right">
												<%# DataBinder.Eval(Container.DataItem, "Value")%>
											</td>		
										</itemtemplate>
									</asp:Repeater>
									<td bgcolor="#F0F0F0"><strong><%# DataBinder.Eval(Container.DataItem, "Make")%></strong></td>
								</tr>
								<asp:Repeater runat="server" DataSource='<%# GetModels(DataBinder.Eval(Container.DataItem, "MakeId").ToString()) %>'>
									<itemtemplate>
										<tr id='tr_<%# DataBinder.Eval(Container.DataItem, "MakeId_Model")%>_<%# DataBinder.Eval(Container.DataItem, "ModelId")%>' class="child" style="display:none">
											<td align="right">
												<%# DataBinder.Eval(Container.DataItem, "Model")%>
											</td>
											<asp:Repeater runat="server" DataSource='<%# GetModelsData(DataBinder.Eval(Container.DataItem, "ModelId").ToString()) %>'>
												<itemtemplate>
													<td align="right">
														<%# DataBinder.Eval(Container.DataItem, "Value")%>
													</td>		
												</itemtemplate>
											</asp:Repeater>
											<td>
												<%# DataBinder.Eval(Container.DataItem, "Model")%>
											</td>
										</tr>
									</itemtemplate>
								</asp:Repeater>
							</itemtemplate>
						</asp:Repeater>
						<tr>
							<td width="22%" style="background-color:#F0F0F0;font-weight:bold">Total</td>
							<asp:Repeater runat="server" ID="rptSum">
								<itemtemplate>
									<td style="background-color:#F0F0F0;font-weight:bold" align="right">
										<%# DataBinder.Eval(Container.DataItem, "Value")%>
									</td>		
								</itemtemplate>
							</asp:Repeater>	
							<td width="22%" style="background-color:#F0F0F0;font-weight:bold">Total</td>
						</tr>
						<tr>
							<td width="22%" style="background-color:#F0F0F0;font-weight:bold">Unique</td>
							<asp:Repeater runat="server" ID="rptUnique">
								<itemtemplate>
									<td style="background-color:#F0F0F0;font-weight:bold" align="right">
										<%# DataBinder.Eval(Container.DataItem, "Value")%>
									</td>		
								</itemtemplate>
							</asp:Repeater>	
							<td width="22%" style="background-color:#F0F0F0;font-weight:bold">Total</td>
						</tr>
					</table>
				</asp:Panel>
			</td>
		</tr>
	</table>
</form>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        var option = $("#hdn_rbtn").val();
        if (option == 1) {
            $("#drpState").css("display", "block");
            $("#drpCities").css("display", "none");
        }
        else {
            $("#drpState").css("display", "none");
            $("#drpCities").css("display", "block");
        }
     });

    $("#rbtnlOption").click(function () {

        var selVal = $("#rbtnlOption input:checked").val();
        if (selVal == 1) {
            $("#drpState").css("display", "block");
            $("#drpCities").css("display", "none");
        }
        else {
            $("#drpState").css("display", "none");
            $("#drpCities").css("display", "block");
        }
    });

    function GoUrl(make) {
        var year = $("#txt_Year").val();
        var city = $("#txt_City").val();
        var month = $("#txt_Month").val();
        var state = $("#txt_State").val()

       
        var url = "PQMatrixgraph.aspx?Make=" + make + "&Month=" + month + "&Year=" + year + "&State=" + state + "&City=" + city;
        var newwindow = window.open(url, "mywindow", 'height=800, width=900, left=200,top=50');
       if (window.focus) { newwindow.focus() }

    }


    function butShow_click(e) {

        
        var selVal = $("#rbtnlOption input:checked").val();
        $("#hdn_rbtn").val(selVal);
        if (selVal == 2) {

            document.getElementById("spnCities").innerHTML = " ";
            if (document.getElementById("drpCities").selectedIndex == 0 || document.getElementById("drpCities").selectedIndex == 13) {
                document.getElementById("spnCities").innerHTML = "Required ";
                return false;
            }
        }
        else {

            if ($("#drpState :selected ").val() == "0") {
                document.getElementById("spnCities").innerHTML = "Required ";
                return false;
            }
        
        }
    }

    function showHideChild(id) {
        var trs = document.getElementsByTagName("tr");
        var img = document.getElementById("img_" + id);

        if (img.src.indexOf("plus") == -1) {
            //hide
            img.src = "/images/plus.gif";
            for (var i = 0; i < trs.length; i++) {
                if (trs[i].id.indexOf("tr_" + id + "_") != -1)
                    trs[i].style.display = "none";
            }
        }
        else {
            //show
            img.src = "/images/minus.gif";
            for (var i = 0; i < trs.length; i++) {
                if (trs[i].id.indexOf("tr_" + id + "_") != -1)
                    trs[i].style.display = "";
            }
        }
    }

    document.getElementById("butShow").onclick = butShow_click;
</script>
<!-- #Include file="/includes/footerNew.aspx" -->