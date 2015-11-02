<%@ Page Inherits="BikeWaleOpr.Content.ShowroomPrices" AutoEventWireUp="false" Language="C#" trace="false" Debug="false" EnableEventValidation="false" %>
<!-- #Include file="/includes/headerNew.aspx" -->

<style type="text/css">
	<!--
	#prices { border-collapse:collapse; border-color:#cccccc; }
	#prices td { text-align:center; }
	#prices th { padding:4px; background-color:#DDEEFF; }
	#prices input { background-color:#f3f3f3; border:1px solid #dddddd; }
	.vasi { background-color:#DDEEFF; }
	.met{background-color:#FFFF66;}
	-->
</style>
<div class="urh">
	You are here &raquo; <a href="/content/default.aspx">Contents</a> &raquo; Showroom Prices
</div>
<div>
    <!-- #Include file="contentsMenu.aspx" -->
</div>
<script language="javascript" src="/src/AjaxFunctions.js"></script>
<div class="left">
	<h3>Add Showroom Prices</h3>
	
		<span id="spnError"  style="color:#FF3300; font-weight:bold;" runat="server"></span>
		
		<fieldset style="white-space:nowrap;">
			<legend>Search Vehicle</legend>
			<label>Make</label>
			<asp:DropDownList ID="cmbMake" runat="server" tabindex="1"></asp:DropDownList>
			<asp:DropDownList ID="cmbModel" Enabled="true" runat="server" tabindex="2">
				<asp:ListItem Value="0" Text="--Select--" />
			</asp:DropDownList>
			<span  style="color:#FF3300; font-weight:bold;" id="selectModel"></span>
			<asp:RadioButton ID="optNew" runat="server" GroupName="Type" Text="New" Checked="true" tabindex="3"/>
			<asp:RadioButton ID="optUsed" runat="server" GroupName="Type" Text="Used" />
            <asp:DropDownList ID="ddlStates" runat="server" tabindex="4"/>
			<asp:DropDownList ID="drpCity" runat="server" tabindex="5" >
                	<asp:ListItem Value="0" Text="--Select City--" />
			</asp:DropDownList>
            <span  style="color:#FF3300; font-weight:bold;" id="spnCity"></span>
            <input type="hidden" id="hdn_ddlCities" runat="server" />
			<asp:Button ID="btnShow" Text="Search" runat="server" tabindex="6"></asp:Button>
		</fieldset>
		<br>
		<asp:Button ID="btnRemove" Text="Remove Price" runat="server"></asp:Button>
		<br>
		<br>
		<strong>Non-Metalic Prices</strong>
		<asp:Repeater ID="rptPrices" runat="server" >
			<headertemplate><br>
				<table width="100%" align="center" border="1" id="prices" cellpadding="2" cellspacing="0">
					<tr>
						<th rowspan="2">UP</th>
						<th rowspan="2">Version</th>
						<th colspan="4">Non-Metalic price</th>
						<td class="met" colspan="4"><strong>Metalic price</strong></td>
					</tr>
					<tr>
						<th>Ex-Showroom</th>
						<th>Insurance</th>
						<th>RTO</th>
						<th>RTO-Corporate</th>
						
						<td class="met"><strong>Ex-Showroom</strong></td>
						<td class="met"><strong>Insurance</strong></td>
						<td class="met"><strong>RTO</strong></td>
						<td class="met"><strong>RTO-Corporate</strong></td>
					</tr>
		
			</headertemplate>
			<itemtemplate>
					<asp:Label ID="lblVersionId" style="display:none;" Text='<%# DataBinder.Eval( Container.DataItem, "Id" ) %>' runat="server" />
					<tr>
						<td>
							<asp:CheckBox Checked='<%# qryStrVersion == DataBinder.Eval( Container.DataItem, "Id" ).ToString() ? true : false%>' ID="chkUpdate" runat="server"></asp:CheckBox>
						</td>
						<th nowrap="nowrap" align="right"><%# DataBinder.Eval( Container.DataItem, "Name" ) %></th>
						<td>
							<asp:TextBox ID="txtMumbaiPrice" Text='<%# DataBinder.Eval( Container.DataItem, "MumPrice" ).ToString() %>' Columns="10" MaxLength="9" runat="server" />
							
						</td>
						<td>
							<asp:TextBox ID="txtMumbaiInsurance" Text='<%# DataBinder.Eval( Container.DataItem, "MumInsurance" ).ToString() %>' Columns="10" MaxLength="9" runat="server" />
							
						</td>
						<td>
							<asp:TextBox ID="txtMumbaiRTO" Text='<%# DataBinder.Eval( Container.DataItem, "MumRTO" ).ToString() %>' Columns="10" MaxLength="9" runat="server" />
							
						</td>
						<td>
							<asp:TextBox ID="txtMumbaiCorporateRTO" Text='<%# DataBinder.Eval( Container.DataItem, "MumCorporateRTO" ).ToString() %>' Columns="10" MaxLength="9" runat="server" />
							
						</td>
						
						<td class="met">
							<asp:TextBox ID="txtMumbaiMetPrice" Text='<%# DataBinder.Eval( Container.DataItem, "MumMetPrice" ).ToString() %>' Columns="10" MaxLength="9" runat="server" />
							
						</td>
						<td class="met">
							<asp:TextBox ID="txtMumbaiMetInsurance" Text='<%# DataBinder.Eval( Container.DataItem, "MumMetInsurance" ).ToString() %>' Columns="10" MaxLength="9" runat="server" />
							
						</td>
						<td class="met">
							<asp:TextBox ID="txtMumbaiMetRTO" Text='<%# DataBinder.Eval( Container.DataItem, "MumMetRTO" ).ToString() %>' Columns="10" MaxLength="9" runat="server" />
							
						</td>
						<td class="met">
							<asp:TextBox ID="txtMumbaiMetCorporateRTO" Text='<%# DataBinder.Eval( Container.DataItem, "MumMetCorporateRTO" ).ToString() %>' Columns="10" MaxLength="9" runat="server" />
							
						</td>
					</tr>
			</itemtemplate>
			<footertemplate>
				</table>
			</footertemplate>
		</asp:Repeater>
		<br />
		<div align="Left">
			Select <a href="javascript:selectAll('all','chkUpdate')"><strong>All</strong></a> 
			<a href="javascript:selectAll('none','chkUpdate')"><strong>None</strong></a>
			<asp:Button ID="btnSave" Text="Save All Prices" runat="server" />
		</div>
        <asp:HiddenField Id="hdnSelectedCityId" runat="server" />
	
</div>
  <script language="javascript">
	document.getElementById('btnShow').onclick = checkFind;
	document.getElementById('btnRemove').onclick = checkRemove;

	$("#ddlStates").click(function () {
        if($(this).val() > 0)
	        $("#drpCity").prop("disabled", false); 
	});

	$("#ddlStates").change(function () {
	    
	    var requestType = "7";
	    var stateId = $(this).val();
	    //alert(stateId);
	    //$("#" + hdnSelectedModel_Id).val("");
	    //$("#" + hdnSelectedVersion_Id).val("");
	    //makeId = makeId.split('_')[0];

	    $("#hdnSelectedCityId").val("0");

	    if (stateId > 0) {
	       $.ajax({
	            type: "POST",
	            url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
	            data: '{"requestType":"' + requestType + '", "stateId":"' + stateId + '"}',
	            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetCities"); },
	            success: function (response) {
	                var responseJSON = eval('(' + response + ')');
	                var resObj = eval('(' + responseJSON.value + ')');
	                var dependentCmbs = new Array();
	                bindDropDownList(resObj, $("#drpCity"), "hdn_ddlCities", dependentCmbs, "--Select City--");
	            }
	        });
	    } else {
	        $("#drpCity").val("0").attr("disabled", true);
	    }
	});

	$("#drpCity").change(function () {
	    if ($("#ddlStates").val() > 0) {
	        $("#hdnSelectedCityId").val($(this).val());
	    }
	    else {
	        $("#hdnSelectedCityId").val("0");
	    }
	   //alert($("#hdnSelectedCityId").val());
	});
	
	qryStrMake = '<%=qryStrMake%>' ;
	qryStrModel = '<%=qryStrModel%>' ;
	qryStrCity = '<%=qryStrCity%>' ;
	
	make = document.getElementById('cmbMake');
	model = document.getElementById('cmbModel');
	city = document.getElementById('drpCity');
	
	<% if ( IsPostBack ) 
	{ %>
      qryStrMake = '<%=cmbMake.SelectedValue%>';
		qryStrModel = '<%=Request.Form["cmbModel"]%>';
		qryStrCity = '<%=drpCity.SelectedValue%>'
	<%}%>
	
	for ( var i = 0; i < make.options.length; i++ )
	{
		if ( make.options[ i ].value == qryStrMake ) make.options[ i ].selected = true;
	}
	for ( var i = 0; i < model.options.length; i++ )
	{
	    //alert(qryStrModel + " " + model.options.length);
		if ( model.options[ i ].value == qryStrModel ) model.options[ i ].selected = true;
	}
	
	for ( var i = 0; i < city.options.length; i++ )
	{
		if ( city.options[ i ].value == qryStrCity ) city.options[ i ].selected = true;
	}
	
	function checkFind( e )
	{
		if ( document.getElementById('cmbModel').options[0].selected ) 
		{
			document.getElementById('selectModel').innerHTML = "Select Model First"; 
			document.getElementById('cmbModel').focus();
			return false;
		}
		else document.getElementById('selectModel').innerHTML = "";

		if ($("#drpCity").val() <= 0)
		{
		    $("#spnCity").text("Select City");
		    return false;
		}
		else
		    $("#spnCity").text("");

		if ($("#ddlStates").val() <= 0) {
		    $("#spnCity").text("Select state");
            return false;
       }
       else
		    $("#spnCity").text("");

		return true;
	}
	
	function checkRemove( e )
	{
		if ( document.getElementById('cmbModel').options[0].selected ) 
		{
			document.getElementById('selectModel').innerHTML = "Select Model First"; 
			document.getElementById('cmbModel').focus();
			return false;
		}
		else document.getElementById('selectModel').innerHTML = ""; 
		
		return confirm("Do you really want to remove prices of all selected verisons for selected city?");
			
	}
	
	var txts = document.getElementsByTagName("input");
	var ids = "";
	
	for ( var i = 0; i<txts.length; i++ )
	{
		if ( txts[i].id.indexOf("txtMumbaiPrice") > 0 )
		{
			txts[i].onblur = calculatePrices;
			txts[i].onchange = enableCheckboxtxtMumbaiPrice;
		}
		
		if ( txts[i].id.indexOf("txtMumbaiInsurance") > 0 )
		{
			txts[i].onchange = enableCheckboxtxtMumbaiInsurance;
		}
		
		if ( txts[i].id.indexOf("txtMumbaiRTO") > 0 )
		{
			txts[i].onchange = enableCheckboxtxtMumbaiRTO;
		}
		
		if ( txts[i].id.indexOf("txtMumbaiCorporateRTO") > 0 )
		{
			txts[i].onchange = enableCheckboxtxtMumbaiCorporateRTO;
		}
		
		//---------------------------------------------------------------
		
		if ( txts[i].id.indexOf("txtMumbaiMetPrice") > 0 )
		{
			txts[i].onblur = calculateMetPrices;
			txts[i].onchange = enableCheckboxtxtMumbaiMetPrice;
		}
		
		if ( txts[i].id.indexOf("txtMumbaiMetInsurance") > 0 )
		{
			txts[i].onchange = enableCheckboxtxtMumbaiMetInsurance;
		}
		
		if ( txts[i].id.indexOf("txtMumbaiMetRTO") > 0 )
		{
			txts[i].onchange = enableCheckboxtxtMumbaiMetRTO;
		}
		
		if ( txts[i].id.indexOf("txtMumbaiMetCorporateRTO") > 0 )
		{
			txts[i].onchange = enableCheckboxtxtMumbaiMetCorporateRTO;
		}
	}
	
	function enableCheckboxtxtMumbaiPrice(e)
	{
		var eSrc = e ? e.target : event.eventSrc;

		var idPart = eSrc.id.substring(0, eSrc.id.indexOf("txtMumbaiPrice"));
		var idPartIndex = eSrc.id.split('_')[2];
		
		document.getElementById(idPart + "chkUpdate" + "_" + idPartIndex).checked = true;
	}
	
	function enableCheckboxtxtMumbaiInsurance(e)
	{
		var eSrc = e ? e.target : event.eventSrc;

		var idPart = eSrc.id.substring(0, eSrc.id.indexOf("txtMumbaiInsurance"));
		var idPartIndex = eSrc.id.split('_')[2];
		
		document.getElementById(idPart + "chkUpdate" + "_" + idPartIndex).checked = true;
	}
	
	function enableCheckboxtxtMumbaiRTO(e)
	{
		var eSrc = e ? e.target : event.eventSrc;

		var idPart = eSrc.id.substring(0, eSrc.id.indexOf("txtMumbaiRTO"));
		var idPartIndex = eSrc.id.split('_')[2];

		document.getElementById(idPart + "chkUpdate" + "_" + idPartIndex).checked = true;
	}
	
	function enableCheckboxtxtMumbaiCorporateRTO(e)
	{
		var eSrc = e ? e.target : event.eventSrc;

		var idPart = eSrc.id.substring(0, eSrc.id.indexOf("txtMumbaiCorporateRTO"));
		var idPartIndex = eSrc.id.split('_')[2];

		document.getElementById(idPart + "chkUpdate" + "_" + idPartIndex).checked = true;
	}
	
	//----------------------------------------------
	function enableCheckboxtxtMumbaiMetPrice(e)
	{
		var eSrc = e ? e.target : event.eventSrc;

		var idPart = eSrc.id.substring(0, eSrc.id.indexOf("txtMumbaiMetPrice"));
		var idPartIndex = eSrc.id.split('_')[2];

		document.getElementById(idPart + "chkUpdate" + "_" + idPartIndex).checked = true;
	}
	
	function enableCheckboxtxtMumbaiMetInsurance(e)
	{
		var eSrc = e ? e.target : event.eventSrc;

		var idPart = eSrc.id.substring(0, eSrc.id.indexOf("txtMumbaiMetInsurance"));
		var idPartIndex = eSrc.id.split('_')[2];

		document.getElementById(idPart + "chkUpdate" + "_" + idPartIndex).checked = true;
	}
	
	function enableCheckboxtxtMumbaiMetRTO(e)
	{
		var eSrc = e ? e.target : event.eventSrc;

		var idPart = eSrc.id.substring(0, eSrc.id.indexOf("txtMumbaiMetRTO"));
		var idPartIndex = eSrc.id.split('_')[2];

		document.getElementById(idPart + "chkUpdate" + "_" + idPartIndex).checked = true;
	}
	
	function enableCheckboxtxtMumbaiMetCorporateRTO(e)
	{
		var eSrc = e ? e.target : event.eventSrc;

		var idPart = eSrc.id.substring(0, eSrc.id.indexOf("txtMumbaiMetCorporateRTO"));
		var idPartIndex = eSrc.id.split('_')[2];

		document.getElementById(idPart + "chkUpdate" + "_" + idPartIndex).checked = true;
	}
	
	function calculatePrices( e )
	{	    
	    var eSrc = e ? e.target : event.eventSrc;

		var idPart = eSrc.id.substring(0, eSrc.id.indexOf("txtMumbaiPrice")); 
		var idPartIndex = eSrc.id.split('_')[2];

		var insId = idPart + "txtMumbaiInsurance" + "_" + idPartIndex;
		var rtoId = idPart + "txtMumbaiRTO" + "_" + idPartIndex;
		
		var verId = document.getElementById(idPart + "lblVersionId" + "_" + idPartIndex).innerHTML;
		
		var cityId = document.getElementById( "drpCity" ).value;
		
		//alert( verId + " " + cityId );
		
		//document.getElementById(insId).value = AjaxFunctions.GetInsurancePremium( verId, cityId, eSrc.value ); 
		
		document.getElementById(insId).value = Math.round(AjaxFunctions.CalculateInsurancePremium( verId, cityId, eSrc.value ).value,0);
		document.getElementById(rtoId).value = Math.round(AjaxFunctions.CalculateRegistrationCharges( verId, cityId, eSrc.value ).value,0);
	} 
	
	function calculateMetPrices( e )
	{
		var eSrc = e ? e.target : event.eventSrc;

		var idPart = eSrc.id.substring(0, eSrc.id.indexOf("txtMumbaiMetPrice")); 
		var idPartIndex = eSrc.id.split('_')[2];

		var insId = idPart + "txtMumbaiMetInsurance";
		var rtoId = idPart + "txtMumbaiMetRTO";
		
		var verId = document.getElementById(idPart + "lblVersionId" + "_" + idPartIndex).innerHTML;
		var cityId = document.getElementById( "drpCity" ).value;
	
		//alert( verId + " " + cityId );
		
		//document.getElementById(insId).value = AjaxFunctions.GetInsurancePremium( verId, cityId, eSrc.value ); 
		
		document.getElementById(insId).value = Math.round(AjaxFunctions.CalculateInsurancePremium( verId, cityId, eSrc.value ).value,0);
		document.getElementById(rtoId).value = Math.round(AjaxFunctions.CalculateRegistrationCharges( verId, cityId, eSrc.value ).value,0);
	} 
	
	function selectAll(type,chkId)
	{
		var obj = document.getElementsByTagName("input");
		
		if(type == "all"){
			bolVal = true;
		}
		else{
			bolVal = false;
		}
		for ( var i = 0 ; i < obj.length ; i++ )
		{
			if ( obj[i].type == "checkbox" && obj[i].id.indexOf(chkId) != -1 )
			{
				obj[i].checked = bolVal;
			}
		}
	}
</script>
<!-- #Include file="/includes/footerNew.aspx" -->