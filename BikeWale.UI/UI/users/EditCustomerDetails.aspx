<%@ Page Inherits="Bikewale.MyBikeWale.EditCustomerDetails" AutoEventWireUp="false" Language="C#" EnableEventValidation="false" Trace="false" Debug="false" %>
<%@ Import NameSpace="Bikewale.Common" %>
<% //Modified By :Sajal Gupta on 03 August 2016
    isAd300x250Shown = false;
    isAd300x250BtfShown = false; %>
<!-- #include file="/Includes/headMyBikeWale.aspx" -->
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/" itemprop="url">
                    <span itemprop="title">Home</span>
                </a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/mybikewale/" itemprop="url">
                    <span  itemprop="title">My BikeWale</span>
                </a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>Edit Contact Details</strong></li>
        </ul><div class="clear"></div>
    </div>
	<div id="content" class="grid_8">
        <h2 class="margin-top10">Update Contact Details</h2>
		<div class="note" id="divNote" runat="server">
			<div>Attention : Your Contact Information Missing</div>
			Please update your contact information. It is required to process your inquiry.
		</div>		
		<!--My Profile Content-->
		<div  id="tb1">
			<table border="0" width="100%" cellspacing="0" cellpadding="5" id="tblAsk" runat="server">
				<tr>
					<td colspan="4" >
						<!--<p>CarWale Community requires you to complete this one-time registration process. Not all fields are required.</p><br />-->
						Marked fields (<span class="required">* </span>) are  Required 
						<span id="spnError" class="error" runat="server" />				
					</td>
				</tr>
				<tr>
					<td><h3>Update Your Name</h3></td>
					<td colspan="3" ></td>
				</tr>
				<tr>
					<td>
						<span class="required">* </span>Your Name:</td>
					<td colspan="3">
						<asp:TextBox ID="txtName" MaxLength="50" Columns="20" runat="server" CssClass="text" />  
						<span class="subTitle">Maximum 15 characters</span>          
						<span id="errName" class="error"></span>
					</td>
				</tr>
				<tr>
					<td><span class="required">* </span>Your Email:</td>
					<td colspan="3"><asp:Label ID="lblEmail"  runat="server" CssClass="text" /> </td>
				</tr>
				<tr>
					<td >&nbsp;</td>
					<td colspan="3">&nbsp;</td>
				</tr>
				<tr>
					<td><h3>Personal Details</h3></td>
					<td colspan="3"></td>
				</tr>
			<%--	<tr>
					<td><span class="required">* </span>Phone Number :</td>
					<td colspan="3">
						<asp:TextBox ID="txtStdCode1" Width="35" MaxLength="4" Columns="3" runat="server" CssClass="text" /> - <asp:TextBox ID="txtPhone1" Width="70" MaxLength="10" Columns="8" runat="server"  CssClass="text"/>	            
						<span class="subTitle">Country Code, Area Code, Phone Number OR</span><span id="spnErrPhone" class="error"></span>
					</td>
				</tr>--%>
				<tr>
					<td class="margin-left10"><span class="left-float">Mobile : </span><span class="right-float">+91 - </span><div class="clear"></div></td>
					<td colspan="3"><asp:TextBox ID="txtMobile" MaxLength="10"  runat="server" CssClass="text" /><span id="spnCustom" class="error"></span>				
					</td>
				</tr>
				<%--<tr>
					<td valign="top">Address :</td>
					<td colspan="3">
						<asp:TextBox ID="txtAddress" Columns="30"  MaxLength="100" runat="server" TextMode="MultiLine" CssClass="text" />
						<span style="vertical-align:top;" class="subTitle">Please write Home No, Building, Street, Colony etc.</span>	
					</td>
				</tr>--%>
				<tr>
					<td class="margin-left10">State :</td>
					<td colspan="3">
						<asp:DropDownList ID="drpState" EnableViewState="true" runat="server" />
						<span id="spnState" class="error" runat="server" />				
					</td>
				</tr>
				<tr>
					<td class="margin-left10">City :</td>
					<td colspan="3">
						<asp:DropDownList ID="drpCity" runat="server" >
							<asp:ListItem Text="--Select--" Value="0" />	
						</asp:DropDownList>
						<input type="hidden" id="hdn_drpCity" runat="server" />
						<span id="spnCity" class="error" runat="server" /> 							
					</td>
				</tr>
				<tr>
					<td >&nbsp;</td>						
					<td colspan="3">&nbsp;</td>
				</tr>
				<tr>
					<td><h3>Email Subscription :<h3></td>
					<td class="text-highlight">Emails you would like to receive</td>
					<td colspan="2"></td>
				</tr>						
				<tr>
					<td>&nbsp;</td>
					<td><asp:CheckBox ID="chkNewsLetter" Checked="true" Text=" Do you want us to send you Newsletters?" runat="server" /></td>
					<td colspan="2"></td>
				</tr>					
				<tr>
					<td>&nbsp;</td>
					<td align="left"><asp:Button ID="btnSave"  CssClass="buttons text_white"  Text="Update Details"  runat="server" /></td>
					<td colspan="3"></td>
				</tr>
			</table>
		</div>				
	</div>
</div>
<script type="text/javascript">	
	$(document).ready(function () {
	    $("#drpState").change(function () {	drpState_Change($(this)); });	
	    //$("#drpCity").change(function () { drpCity_Change($(this)); });
	    $("#btnSave").click(function () {
	        if (validateForm())
	        {
	            return false;
	        }
	    });
	});
    
	function drpState_Change(e) {
	    var stateId = e.val();	    	    
	    if (stateId != 0) {
	        $.ajax({
	            type: "POST",
	            url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
	            data: '{"requestType":"ALL", "stateId":"' + stateId + '"}',
	            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetCities"); },
	            success: function (response) {
	                var responseJSON = eval('(' + response + ')');
	                var resObj = eval('(' + responseJSON.value + ')');

	                var dependentCmbs = new Array();

	                bindDropDownList(resObj, $("#drpCity"), "hdn_drpCity", dependentCmbs, "--Select City--");
	            }
	        });
	    } else {
	        $("#drpCity").val("0").attr("disabled", true);
	    }	
	}

	//function drpState_Change(e){
	//	var stateId = document.getElementById("drpState").value;
	//	var response = AjaxFunctions.GetCities(stateId);
		
	//	var dependentCmbs = new Array();
	//	//add the name of the dependent combos on this combo
	//	dependentCmbs[0] = "drpArea";
	//	//call the function to consume this data
	//	FillCombo_Callback(response, document.getElementById("drpCity"), "hdn_drpCity" , dependentCmbs);
	//}
	
	//function drpCity_Change(e){
	//	var cityId = document.getElementById("drpCity").value;
	//	var response = AjaxFunctions.GetAreas(cityId);
		
	//	//call the function to consume this data
	//	FillCombo_Callback(response, document.getElementById("drpArea"), "hdn_drpArea");
	//}
	function validateForm() {
	    var spn = document.getElementById('spnCustom');
	    var isError = false;

	    var re = /^[0-9]*$/
	    
	    //at least one phone no. is compulsory
	    //if (getId('txtPhone1').value == "" && getId('txtMobile').value == "") {
	    //    spn.innerHTML = "Please provide atleast one phone number";
	    //    isError = true;	        
	    //}else {
	    //    spn.innerHTML = '';
	    //}
	    
	    //if (getId('txtStdCode1').value != "" && getId('txtPhone1').value == "") {
	    //    spn.innerHTML = "Please provide phone number";
	    //    isError = true;
	    //} else {
	    //    spn.innerHTML = '';
	    //}

	    //if (isNaN(getId('txtStdCode1').value) || isNaN(getId('txtPhone1').value) || isNaN(getId('txtMobile').value)) {
	    //    spn.innerHTML = "Please enter valid phone number";
	    //    isError = true;
	    //} else {
	    //    spn.innerHTML = '';
	    //}

	    if (getId('txtName').value == "") {
	        document.getElementById('errName').innerHTML = "Required";
	        isError = true;
	    }
	    else {
	        document.getElementById('errName').innerHTML = "";
	    }

	    if (getId('drpState').value == "") {
	        document.getElementById('spnState').innerHTML = "Required";
	        isError = true;
	    }
	    else {
	        document.getElementById('spnState').innerHTML = "";
	    }

	    if (getId('drpCity').options[0].selected) {
	        document.getElementById('spnCity').innerHTML = "Required";
	        isError = true;
	    }
	    else {
	        document.getElementById('spnCity').innerHTML = "";
	    }	    
	    return isError;
	}

	function getId(controlId) {
	    return document.getElementById(controlId);
	}
</script>
<!-- #include file="../Includes/footerInner.aspx" -->
<!-- Footer ends here -->
