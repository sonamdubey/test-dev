<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.Content.BikeVersions" Trace="false" Debug="false" EnableEventValidation="false"%>
<!-- #Include file="/includes/headerNew.aspx" -->
<script type="text/javascript" language="javascript" src="/src/AjaxFunctions.js"></script>
<div class="left">

<h1>Bike Versions</h1>
<style type="text/css">
	.doNotDisplay { display:none; }
</style>
	<span style="font-weight:bold;color:red;" id="spnError" class="error" runat="server"></span>
	
	<fieldset>
		<legend>View Existing Versions</legend>
		<asp:DropDownList ID="cmbMakes" runat="server" />&nbsp;<span style="font-weight:bold;color:red;" id="selectMake" class="error"></span>
		<asp:DropDownList ID="cmbModels" runat="server">
			<asp:ListItem Value="0" Text="Select Model" />
		</asp:DropDownList>&nbsp;<span style="font-weight:bold;color:red;" id="selectModel" class="error"></span>
		<asp:Button ID="btnShow" Text="Show Versions" runat="server" />
	</fieldset>
	<br/>
	<asp:DataGrid ID="dtgrdMembers" runat="server"
			DataKeyField="ID"
			CellPadding="5"
			BorderWidth="1"
			width="100%"
			AllowPaging="false"
			AllowSorting="true" 
			AutoGenerateColumns="false">
		<itemstyle CssClass="dtItem"></itemstyle>
		<headerstyle CssClass="dtHeader"></headerstyle>
		<alternatingitemstyle CssClass="dtAlternateRow"></alternatingitemstyle>
		<edititemstyle CssClass="dtEditItem"></edititemstyle>
		<columns>
			<asp:TemplateColumn HeaderText="Version" SortExpression="Name" ItemStyle-Width="200">
				<itemtemplate>
					<%# DataBinder.Eval( Container.DataItem, "Name" ) %>
				</itemtemplate>
				<edititemtemplate>
					<asp:TextBox ID="txtVersion" MaxLength="50" Columns="15" Text='<%# DataBinder.Eval( Container.DataItem, "Name" ) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="BikeModelId" ReadOnly="true" ItemStyle-CssClass="doNotDisplay" HeaderStyle-CssClass="doNotDisplay" />
			<asp:BoundColumn DataField="SegmentId" ReadOnly="true" ItemStyle-CssClass="doNotDisplay" HeaderStyle-CssClass="doNotDisplay" />
			<asp:BoundColumn DataField="SubSegmentId" ReadOnly="true" ItemStyle-CssClass="doNotDisplay" HeaderStyle-CssClass="doNotDisplay" />
			<asp:BoundColumn DataField="BodyStyleId" ReadOnly="true" ItemStyle-CssClass="doNotDisplay" HeaderStyle-CssClass="doNotDisplay" />
            <asp:BoundColumn DataField="BikeFuelType" ReadOnly="true" ItemStyle-CssClass="doNotDisplay" HeaderStyle-CssClass="doNotDisplay" />
            <asp:BoundColumn DataField="BikeTransmission" ReadOnly="true" ItemStyle-CssClass="doNotDisplay" HeaderStyle-CssClass="doNotDisplay" />
			<asp:TemplateColumn HeaderText="Segment">
				<itemtemplate>
					<%# DataBinder.Eval( Container.DataItem, "Segment" ) %>
				</itemtemplate>
				<edititemtemplate>
					<select id="cmbGridSegment" name="cmbGridSegment"></select>
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="SubSegment">
				<itemtemplate>
					<%# DataBinder.Eval( Container.DataItem, "SubSegmentName" ) %>
				</itemtemplate>
				<edititemtemplate>
					<select id="cmbGridSubSegment" name="cmbGridSubSegment"></select>
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="BodyStyle">
				<itemtemplate>
					<%# DataBinder.Eval( Container.DataItem, "BodyStyle" ) %>
				</itemtemplate>
				<edititemtemplate>
					<select id="cmbGridBodyStyle" name="cmbGridBodyStyle"></select>
				</edititemtemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="FuelType">
				<itemtemplate>
					<%# DataBinder.Eval( Container.DataItem, "BikeFuelType" ).ToString() == "1" ? "Petrol" : ( DataBinder.Eval(Container.DataItem, "BikeFuelType" ).ToString() == "2" ? "Diesel" : ( DataBinder.Eval(Container.DataItem, "BikeFuelType" ).ToString() == "3" ? "CNG" : ( DataBinder.Eval(Container.DataItem, "BikeFuelType" ).ToString() == "4" ? "LPG" : ( DataBinder.Eval(Container.DataItem, "BikeFuelType" ).ToString() == "5" ? "Electric" : "None" ) ) ) ) %> 
				</itemtemplate>
				<edititemtemplate>
					<select id="cmbGridFuelType" name="cmbGridFuelType">
                        <option value="1">Petrol</option>
                        <option value="2">Diesel</option>
                        <option value="3">CNG</option>
                        <option value="4">LPG</option>
                        <option value="5">Electric</option>
                    </select>
				</edititemtemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="BikeTransmission">
				<itemtemplate>
                    <%# DataBinder.Eval( Container.DataItem, "BikeTransmission" ).ToString() == "1" ? "Automatic" : ( DataBinder.Eval(Container.DataItem, "BikeTransmission" ).ToString() == "2" ? "Manual" : "None" ) %> 
				</itemtemplate>
				<edititemtemplate>
					<select id="cmbGridBikeTrans" name="cmbGridBikeTrans">
                        <option value="1">Automatic</option>
                        <option value="2">Manual</option>
                    </select>
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Used">
				<itemtemplate>
					<asp:CheckBox ID="chkUsed" Checked='<%# Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Used" )) %>' Enabled="false" runat="server" />
				</itemtemplate>
				<edititemtemplate>
					<asp:CheckBox ID="chkUsed" Checked='<%#  Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Used" )) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="New">
				<itemtemplate>
					<asp:CheckBox ID="chkNew" Checked='<%#  Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "New" )) %>' Enabled="false" runat="server" />
				</itemtemplate>
				<edititemtemplate>
					<asp:CheckBox ID="chkNew" Checked='<%#  Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "New" )) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Ind.">
				<itemtemplate>
					<asp:CheckBox ID="chkIndian" Checked='<%#  Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Indian" )) %>' Enabled="false" runat="server" />
				</itemtemplate>
				<edititemtemplate>
					<asp:CheckBox ID="chkIndian" Checked='<%#  Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Indian" )) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Imp.">
				<itemtemplate>
					<asp:CheckBox ID="chkImported" Checked='<%#  Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Imported" )) %>' Enabled="false" runat="server" />
				</itemtemplate>
				<edititemtemplate>
					<asp:CheckBox ID="chkImported" Checked='<%#  Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Imported" )) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Clas.">
				<itemtemplate>
					<asp:CheckBox ID="chkClassic" Checked='<%#  Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Classic" )) %>' Enabled="false" runat="server" />
				</itemtemplate>
				<edititemtemplate>
					<asp:CheckBox ID="chkClassic" Checked='<%#  Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Classic" )) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Mod.">
				<itemtemplate>
					<asp:CheckBox ID="chkModified" Checked='<%#  Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Modified" )) %>' Enabled="false" runat="server" />
				</itemtemplate>
				<edititemtemplate>
					<asp:CheckBox ID="chkModified" Checked='<%#  Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Modified" )) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Futur.">
				<itemtemplate>
					<asp:CheckBox ID="chkFuturistic" Checked='<%#  Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Futuristic" )) %>' Enabled="false" runat="server" />
				</itemtemplate>
				<edititemtemplate>
					<asp:CheckBox ID="chkFuturistic" Checked='<%#  Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Futuristic" )) %>' runat="server" />
				</edititemtemplate>				
			</asp:TemplateColumn>
			
			<asp:EditCommandColumn EditText="<img border=0 src=https://opr.carwale.com/images/icons/edit.gif />" CancelText="Cancel" UpdateText="Update" />
            <asp:TemplateColumn>
              <itemtemplate>
                    <div class="alignCenter">
                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="https://opr.carwale.com/images/icons/delete.ico" CommandName="Delete" class="deleteBike"/>
                    </div>
                </itemtemplate>
            </asp:TemplateColumn>
			<%--<asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="<img border=0 src=https://opr.carwale.com/images/icons/delete.ico />" />--%>
			<asp:TemplateColumn HeaderText="Created On" ItemStyle-Width="350">
			    <itemtemplate>
			        <%# DataBinder.Eval( Container.DataItem, "CreatedOn" ) %>
			    </itemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Updated On" ItemStyle-Width="350">
			    <itemtemplate>
			        <%# DataBinder.Eval( Container.DataItem, "UpdatedOn" ) %>
			    </itemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Last Updated By" ItemStyle-Width="350">
			    <itemtemplate>
			        <%# DataBinder.Eval( Container.DataItem, "UpdatedBy" ) %>
			    </itemtemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Colors" ItemStyle-Width="350">
			    <itemtemplate>
			        <input type="button" value="Add" onclick="javascript:window.open('NewBikeVersionColors.aspx?Model=<%# DataBinder.Eval( Container.DataItem, "bikemodelid" ) %>&Version=<%# DataBinder.Eval( Container.DataItem, "id" ) %>','','left=200,width=900,height=600,scrollbars=yes')" />
			    </itemtemplate>
			</asp:TemplateColumn>
            <%--<asp:TemplateColumn HeaderText="Std Features" ItemStyle-Width="350">
			    <itemtemplate>
			        <input type="button" value="Add" onclick="javascript:window.open('newbikestandardfeaturesstep2.aspx?versionId=<%# DataBinder.Eval( Container.DataItem, "id" ) %>','','left=200,width=900,height=600,scrollbars=yes')" />
			    </itemtemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Available Features" ItemStyle-Width="350">
			    <itemtemplate>
			        <input type="button" value="Add" onclick="javascript:window.open('newBikefeatureitems.aspx','','left=200,width=900,height=600,scrollbars=yes')" />
			    </itemtemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Link Features" ItemStyle-Width="350">
			    <itemtemplate>
			        <input type="button" value="Add" onclick="javascript:window.open('newbikefeatures.aspx?versionId=<%# DataBinder.Eval( Container.DataItem, "id" ) %>','','left=200,width=900,height=600,scrollbars=yes')" />
			    </itemtemplate>
			</asp:TemplateColumn>--%>
		</columns>
	</asp:DataGrid>
	<br />
	<fieldset>
		<legend>Add New Bike Version</legend>
		<table width="100%">
			<tr>
				<td>Version Name</td>
				<td>
					<asp:TextBox ID="txtVersion" MaxLength="50" Width="100" runat="server" />
					<span style="font-weight:bold;color:red;" id="provideVersion" class="error"></span>
				</td>
			</tr>
			<tr>
				<td>Segment</td>
				<td><asp:DropDownList ID="cmbSegments" runat="server" /></td>
			</tr>
			<tr>
				<td>Sub-Segment</td>
				<td><asp:DropDownList ID="cmbSubSegments" runat="server" /></td>
			</tr>
			<tr>
				<td>Body-Style </td>
				<td><asp:DropDownList ID="cmbBodyStyles" runat="server" /></td>
			</tr>
            <tr>
                <td>Fuel Type</td>
                <td>
                    <asp:DropDownList ID="cmbFuelType" runat="server">
                        <asp:ListItem Value="1" Text="Petrol"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Diesel"></asp:ListItem>
                        <asp:ListItem Value="3" Text="CNG"></asp:ListItem>
                        <asp:ListItem Value="4" Text="LPG"></asp:ListItem>
                        <asp:ListItem Value="5" Text="Electric"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Transmission</td>
                <td>
                    <asp:DropDownList ID="cmbTransmission" runat="server">
                        <asp:ListItem Value="1" Text="Automatic"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Manual"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
			<tr>
				<td>
					Category(s)
				</td>
				<td>
					<asp:CheckBox ID="chkUsed" Text="Used" Checked="true" runat="server" />
					<asp:CheckBox ID="chkNew" Text="New" Checked="true" runat="server" />
					<asp:CheckBox ID="chkIndian" Text="Indian" Checked="true" runat="server" />
					<asp:CheckBox ID="chkImported" Text="Imported" runat="server" />
					<asp:CheckBox ID="chkClassic" Text="Classic" runat="server" />
					<asp:CheckBox ID="chkModified" Text="Modified" runat="server" />
					<asp:CheckBox ID="chkFuturistic" Text="Futuristic" runat="server" />
				</td>
			</tr>
			<tr>
				<td colspan="2"><asp:Button ID="btnSave" Text="Add Version" runat="server" /></td>
			</tr>
		</table>
	</fieldset>

</div>
<script language="javascript">

    $(".deleteBike").click(function () {
        if (!confirm("Do you really want to delete this version."))
        {
            return false;
        }        
    });

    var model = document.getElementById('cmbModels');
    //cmbMakes_OnChange();

    for (var i = 0; i < model.options.length; i++) {
        if (model.options[i].value == '<%=Request.Form["cmbModels"]%>') model.options[i].selected = true;
    }

    function verifySave(e) {


        if (document.getElementById('cmbModels').options[0].selected) {
            document.getElementById('selectModel').innerHTML = "Select Model First";
            document.getElementById('cmbModels').focus();
            return false;
        }
        else document.getElementById('selectModel').innerHTML = "";

        if (document.getElementById('txtVersion').value == '') {
            document.getElementById('provideVersion').innerHTML = "Version Name Required";
            document.getElementById('txtVersion').focus();
            return false;
        }
        else document.getElementById('provideVersion').innerHTML = "";
    }

    function verifyShow(e) {

        if (document.getElementById('cmbMakes').options[0].selected) {
            document.getElementById('selectMake').innerHTML = "Select Make First";
            document.getElementById('cmbMakes').focus();
            return false;
        }
        else document.getElementById('selectMake').innerHTML = "";

        if (document.getElementById('cmbModels').options[0].selected) {
            document.getElementById('selectModel').innerHTML = "Select Model";
            document.getElementById('cmbModels').focus();
            return false;
        }
    }

    document.getElementById('btnSave').onclick = verifySave;
    document.getElementById('btnShow').onclick = verifyShow;

    var grdSeg = document.getElementById('cmbGridSegment');
    if (grdSeg) {
        var seg = document.getElementById('cmbSegments');
        for (var i = 0; i < seg.options.length; i++) {
            grdSeg.options[i] = new Option(seg.options[i].text, seg.options[i].value);
            if (parseInt(grdSeg.parentNode.parentNode.getElementsByTagName('td')[2].innerHTML) == seg.options[i].value)
                grdSeg.options[i].selected = true;
        }
    }

    var grdSubSeg = document.getElementById('cmbGridSubSegment');
    if (grdSubSeg) {
        var seg = document.getElementById('cmbSubSegments');
        for (var i = 0; i < seg.options.length; i++) {
            grdSubSeg.options[i] = new Option(seg.options[i].text, seg.options[i].value);
            if (parseInt(grdSubSeg.parentNode.parentNode.getElementsByTagName('td')[3].innerHTML) == seg.options[i].value)
                grdSubSeg.options[i].selected = true;
        }
    }

    var grdBody = document.getElementById('cmbGridBodyStyle');
    if (grdBody) {
        var bod = document.getElementById('cmbBodyStyles');
        for (var i = 0; i < bod.options.length; i++) {
            grdBody.options[i] = new Option(bod.options[i].text, bod.options[i].value);
            if (parseInt(grdBody.parentNode.parentNode.getElementsByTagName('td')[4].innerHTML) == bod.options[i].value)
                grdBody.options[i].selected = true;
        }
    }

    var grdFuel = document.getElementById('cmbGridFuelType');
    if (grdFuel) {
        var Fuel = document.getElementById('cmbFuelType');
        for (var i = 0; i < Fuel.options.length; i++) {
            grdFuel.options[i] = new Option(Fuel.options[i].text, Fuel.options[i].value);
            if (parseInt(grdFuel.parentNode.parentNode.getElementsByTagName('td')[5].innerHTML) == Fuel.options[i].value)
                grdFuel.options[i].selected = true;
        }
    }

    var grdTrans = document.getElementById('cmbGridBikeTrans');
    if (grdTrans) {
        var Trans = document.getElementById('cmbTransmission');
        for (var i = 0; i < Trans.options.length; i++) {
            grdTrans.options[i] = new Option(Trans.options[i].text, Trans.options[i].value);
            if (parseInt(grdTrans.parentNode.parentNode.getElementsByTagName('td')[6].innerHTML) == Trans.options[i].value)
                grdTrans.options[i].selected = true;
        }
    }

</script>
<!-- #Include file="/includes/footerNew.aspx" -->

