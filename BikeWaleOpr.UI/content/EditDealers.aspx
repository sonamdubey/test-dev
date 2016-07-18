<%@ Page Language="C#" Inherits="BikeWaleOpr.Content.EditDealers" AutoEventWireup="false" Trace="false" Debug="false" EnableEventValidation="false" %>

<!-- #Include file="/includes/headerNew.aspx" -->
<div class="urh">
		You are here &raquo; <a href="/content/default.aspx">Contents</a> &raquo; Add Dealers
</div>
<div>
    <!-- #Include file="contentsMenu.aspx" -->
</div>
<script type="text/javascript" src="/src/AjaxFunctions.js"></script>
    <div class="left">
	<h3>Find Dealer Details</h3><br />
        <fieldset style="white-space:nowrap;width:970px;">
		<legend>Enter delear details</legend>
		<table>
			<tr>
				<td>Make <font color="red">*</font></td>
				<td><asp:DropDownList ID="drpMake" runat="server"></asp:DropDownList><span style="font-weight:bold;color:red;" id="spndrpMake" class="error" /></td>
                <td>State-City <font color="red">*</font></td>
				<td>
					<asp:DropDownList ID="drpState" CssClass="drpClass" runat="server" />-
					<asp:DropDownList ID="drpCity" Enabled="false" CssClass="drpClass" runat="server">
						<asp:ListItem Text="--Select City--" Value="-1" />
					</asp:DropDownList>
					<input type="hidden" id="hdn_drpCity" runat="server" />
					<span style="font-weight:bold;color:red;" id="spndrpCity" class="error" />
				 </td>
                <td><asp:button ID="btnFind" text="Find Dealers" runat="server" /></td>
			</tr>
        </table></fieldset>
        
        <br /><br />
        <asp:Repeater id="MyRepeater" runat="server">       
    <HeaderTemplate>
        <table border="1" style="border-collapse:collapse;" cellpadding ="5">
               <tr style="background-color:#D4CFCF;">
                    <th>Id</th>                                           
                    <th>Make</th>                                            
                    <th>City</th>                                            
                    <th>Dealer Name</th>                                            
                    <th>Address</th>                                            
                    <th>Pincode</th>                                            
                    <th>Contact No</th>                                            
                    <th>Fax No</th>                                           
                    <th>Email Id</th>                                           
                    <th>Website</th>                                            
                    <th>Working Hrs.</th>                                            
                    <th>Last Updated</th>
                    <th>Is Active</th>                                             
                    <th>Is NCD</th>                                                                                     
                    <th>Edit</th>                                           
               </tr>
    </HeaderTemplate>
    <ItemTemplate> 
               <tr>
                    <td><%# DataBinder.Eval(Container.DataItem,"Id") %></td>                                       
                    <td><%# DataBinder.Eval(Container.DataItem,"MakeId") %></td>                                      
                    <td><%# DataBinder.Eval(Container.DataItem,"CityId") %></td>                                            
                    <td><%# DataBinder.Eval(Container.DataItem,"Name") %></td>                                           
                    <td><%# DataBinder.Eval(Container.DataItem, "Address") %></td>                                       
                    <td><%# DataBinder.Eval(Container.DataItem, "Pincode") %></td>                                     
                    <td><%# DataBinder.Eval(Container.DataItem, "ContactNo") %></td>                                        
                    <td><%# DataBinder.Eval(Container.DataItem, "FaxNo")%></td>                                     
                    <td><%# DataBinder.Eval(Container.DataItem, "EMailId") %></td>                                    
                    <td><%# DataBinder.Eval(Container.DataItem, "WebSite") %></td>                                  
                    <td><%# DataBinder.Eval(Container.DataItem, "WorkingHours") %></td>          
                    <td><%# DataBinder.Eval(Container.DataItem, "LastUpdated") %></td>                                   
                    <td><%# DataBinder.Eval(Container.DataItem, "IsActive")%></td>                                    
                    <td><%# DataBinder.Eval(Container.DataItem, "IsNCD") %></td>                                           
                    <td><a href='adddealers.aspx?id=<%# DataBinder.Eval(Container.DataItem,"Id")%>'><img border=0 src=http://opr.carwale.com/images/edit.jpg /></a></td>                  
              </tr>
    </ItemTemplate>
    <FooterTemplate>
         </Table>
    </FooterTemplate>
    </asp:Repeater>
</div>
    <script language="javascript">
        function btnFind_Click() {
            document.getElementById('spndrpMake').innerHTML = "";
            document.getElementById('spndrpCity').innerHTML = "";
            if (document.getElementById('drpMake').value == "-1") {
                document.getElementById('spndrpMake').innerHTML = "Select Make";
                return false;
            }
            if (document.getElementById('drpCity').value == "-1") {
                document.getElementById('spndrpCity').innerHTML = "Select City";
                return false;
            }
        }        
        if (document.getElementById('btnFind'))
            document.getElementById('btnFind').onclick = btnFind_Click;

        document.getElementById("drpState").onchange = drpState_Change;
        function drpState_Change(e) {
            var stateId = document.getElementById("drpState").value;
            var response = AjaxFunctions.GetCities(stateId);
            var dependentCmbs = new Array();
            FillCombo_Callback(response, document.getElementById("drpCity"), "hdn_drpCity");
        }
</script>
<!-- #Include file="/includes/footerNew.aspx" -->   