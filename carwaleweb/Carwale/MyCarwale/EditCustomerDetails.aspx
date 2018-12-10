<%@ Page trace="false" Inherits="Carwale.UI.MyCarwale.EditCustomerDetails" AutoEventWireUp="false" Language="C#" EnableEventValidation="false" %>
<%@ Register TagPrefix="Vspl" TagName="Calender" src="/Controls/DateControl.ascx" %>
<%@ Import NameSpace="Carwale.UI.Common" %>

<!doctype html>
<html>
<head>

<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 71;
	Title 			= "Edit Profile Details";
	Description 	= "";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Dynamic";
    AdId            = "1337162297840";
    AdPath          = "/7590/CarWale_MyCarWale/CarWale_MyCarWale_Misc/CarWale_MyCarWale_Misc_";
%>
<!-- #include file="/includes/global/head-script.aspx" -->
<style type="text/css">
	.dashborder{border-bottom:dotted thin #DCDCDC;}
	.subTitle{color:#767676;}
	.insetborder{border-right:inset thin;}
	.mandatory{color:#ff0000;}

    /* Css for content tabs */
.ui-tabs .ui-tabs-hide { display: none !important; }/* Caution! Ensure accessibility in print and other media types... */	
.ui-tabs-nav { list-style: none;  margin: 0; padding: 0 0 0 3px;}/* Skin */	
/* clearing without presentational markup, IE gets extra treatment */
.ui-tabs-nav:after {display: block; clear: both; content: " "; }
.ui-tabs-nav li {margin:0 0 0 .5em; float: left;  font-weight: bold; list-style:none;}
/* fixes dir=ltr problem and other quirks IE */
.ui-tabs-nav a, .ui-tabs-nav a span {float: left; padding: 0 7px 0 10px; background: url(https://img.carwale.com/cw-common/tab2.png) no-repeat;}
/* position: relative makes opacity fail for disabled tab in IE */
.ui-tabs-nav a { margin: 5px 0 0;  padding-left: 0; background-position: 100% 0; text-decoration: none; white-space: nowrap; /* @ IE 6 */outline: 0; /* @ Firefox, prevent dotted border after click */ }
.ui-tabs-nav a:link, .ui-tabs-nav a:visited {color: #FFF; font-weight:bold;} 
.ui-tabs-nav .ui-tabs-selected a { position: relative; top: 5px; z-index: 2; margin-top: 0; background-position: 100% -25px;}
.ui-tabs-nav a span { padding-top: 0px; padding-right: 0;height: 25px; background-position: 0 0; line-height:25px;} 
.ui-tabs-nav .ui-tabs-selected a span { padding-top: 1px; height: 25px; background-position: 0 -25px; line-height:25px; color:#000;}
.ui-tabs-nav .ui-tabs-selected a:link, .ui-tabs-nav .ui-tabs-selected a:visited,
.ui-tabs-nav .ui-tabs-disabled a:link, .ui-tabs-nav .ui-tabs-disabled a:visited { cursor: text;/* @ Opera, use pseudo classes otherwise it confuses cursor... */}
.ui-tabs-nav a:hover, .ui-tabs-nav a:focus, .ui-tabs-nav a:active,
.ui-tabs-nav .ui-tabs-unselect a:hover, .ui-tabs-nav .ui-tabs-unselect a:focus, .ui-tabs-nav .ui-tabs-unselect a:active { cursor: pointer;}/* @ Opera, we need to be explicit again here now... */
.ui-tabs-disabled {opacity: .4;filter: alpha(opacity=40);}
.ui-tabs-nav .ui-tabs-disabled a:link, .ui-tabs-nav .ui-tabs-disabled a:visited { color: #000;}
.ui-tabs-panel { border: 1px solid #6598CC; padding: 10px; background: #fff; min-height:50px; display:block;margin-top:-5px; } /* declare background color for container to avoid distorted fonts in IE while fading */  
.ui-tabs-nav {display: inline-block;}/* auto clear @ IE 6 & IE 7 Quirks Mode */
</style>
<script  language="javascript"  src="/static/src/ajaxfunctions.js" ></script>
<%--<script language="javascript">
	$(document).ready(function(){
		$("#uiTabs").tabs();
	});
</script>--%>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <input type="hidden" id="hdnIsPageFromCache" runat="server" />
        <section class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
        </section>
        <div class="clear"></div>
        <section>
            <div class="container">
             <div class="grid-12">
	       <div id="content" style="width:100%;" class="margin-bottom20">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul class="special-skin-text">
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/mycarwale/default.aspx">My CarWale</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="MyContactDetails.aspx">My Profile</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Update Contact Details </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
			<div class="note" id="divNote" runat="server">
				<div>Attention : Your Contact Information Missing</div>
				Please update your contact information. It is required to process your inquiry.
			</div>
			<div>
				<ul class="ui-tabs-nav">
					<li><a href="/mycarwale/MyContactDetails.aspx"><span>My Profile</span></a></li>
					<li class="ui-tabs-selected"><a href="#tb1"><span>Update Contact Details</span></a></li>				
					<li><a href="/users/EditUserProfile.aspx"><span>Update Community Profile</span></a></li>				
					<li><a href="/users/changePassword.aspx"><span>Change Password</span></a></li>								
				</ul>
				<!--My Profile Content-->
				<div  id="tb1" class="ui-tabs-panel" >
					<table class="mainTitle" border="0" width="100%" cellspacing="0" cellpadding="5" id="tblAsk">
						<tr>
							<td colspan="4" class="dashborder" >
								<!--<p>CarWale Community requires you to complete this one-time registration process. Not all fields are mandatory.</p>--><br />
								Marked fields (<span class="mandatory">*</span>) are  Required 
								<span id="spnError" class="error" runat="server" />				
							</td>
						</tr>
						<tr>
							<td style="background:#E7F8FF;" class="insetborder" align="right"><strong>Update Your Name</strong></td>
							<td colspan="3" ></td>
						</tr>
						<tr>
							<td align="right" class="insetborder">
								<span class="mandatory">*</span>Your Name:</td>
							<td colspan="3">
                                <div class="classform-control-box inline-block" >
								    <asp:TextBox ID="txtName" MaxLength="50" Columns="20" runat="server" CssClass="text form-control ui-autocomplete-input blur" Width="150px" />
                                </div>
								<span class="subTitle">Maximum 15 characters</span>          
								<span id="errName" style="color:red;font-weight:bold;"></span> 
							</td>
						</tr>
						<tr>
							<td align="right" class="insetborder"><span class="mandatory">*</span>Your Email:</td>
							<td colspan="3"><asp:Label ID="lblEmail"  runat="server" CssClass="text" /> </td>
						</tr>
						<tr>
							<td class="dashborder" style="border-right:inset thin;">&nbsp;</td>
							<td colspan="3" class="dashborder">&nbsp;</td>
						</tr>
						<tr>
							<td style="background:#E7F8FF;font:bold;" class="insetborder" align="right"><strong>Personal Details</strong></td>
							<td colspan="3"></td>
						</tr>
						<tr>
							<td align="right" class="insetborder"><span class="mandatory">*</span>Phone Number :</td>
							<td colspan="3">
                                <div class="classform-control-box inline-block" >
								    <asp:TextBox ID="txtStdCode1" Width="35" MaxLength="4" Columns="3" runat="server" CssClass="text form-control ui-autocomplete-input" />
                                </div>
                                 - 
                                <div class="classform-control-box inline-block" ><asp:TextBox ID="txtPhone1" Width="70" MaxLength="10" Columns="8" runat="server"  CssClass="text form-control ui-autocomplete-input"/></div>	            
								<span class="subTitle">Country Code, Area Code, Phone Number OR</span>
							</td>
						</tr>
						<tr>
							<td align="right" class="insetborder"><span class="mandatory">*</span>Mobile :</td>
							<td colspan="3">+91 -<div class="classform-control-box inline-block" > <asp:TextBox ID="txtMobile" MaxLength="10"  runat="server" CssClass="text form-control ui-autocomplete-input" /></div>				
							</td>
						</tr>
						<tr>
							<td align="right" class="insetborder" valign="top">Address :</td>
							<td colspan="3">
								<asp:TextBox ID="txtAddress" Columns="30"  MaxLength="100" runat="server" TextMode="MultiLine" CssClass="text" />
								<span style="vertical-align:top;" class="subTitle">Please write Home No, Building, Street, Colony etc.</span>	
							</td>
						</tr>
						<tr>
							<td align="right" class="insetborder"><span class="mandatory">*</span>State :</td>
							<td colspan="3">
                                <div class="form-control-box inline-block" >
                                    <span class="select-box fa fa-angle-down"></span>
								    <asp:DropDownList ID="drpState" runat="server" CssClass="form-control"/>
                                </div>
								<span id="spnState" class="error" runat="server" />				
							</td>
						</tr>
						<tr>
							<td align="right" class="insetborder"><span class="mandatory">*</span>City :</td>
							<td colspan="3">
                                <div class="form-control-box inline-block" >
                                    <span class="select-box fa fa-angle-down"></span>
								    <asp:DropDownList ID="drpCity" runat="server"  CssClass="form-control">
									    <asp:ListItem Text="--Select--" Value="0" />	
								    </asp:DropDownList>
                                </div>
								<input type="hidden" id="hdn_drpCity" runat="server" />
								<span id="spnCity" class="error" runat="server" /> 							
							</td>
						</tr>
						<tr>
							<td class="dashborder" style="border-right:inset thin;">&nbsp;</td>						
							<td colspan="3" class="dashborder">&nbsp;</td>
						</tr>
						<tr>
							<td style="background:#E7F8FF;font:bold;" class="insetborder" align="right"><strong>Email Subscription :<strong></td>
							<td><strong>Emails you would like to receive</strong></td>
							<td colspan="2"></td>
						</tr>						
						<tr>
							<td class="insetborder">&nbsp;</td>
							<td><asp:CheckBox ID="chkNewsLetter" Checked="true" Text=" Do you want us to send you Newsletters?" runat="server" /></td>
							<td colspan="2"></td>
						</tr>					
						<tr>
							<td class="insetborder">&nbsp;</td>
                            <asp:HiddenField id="setToken" runat="server"/>
							<td align="left"><asp:Button ID="btnSave"  CssClass="btn btn-orange btn-xs"  Text="Update Details"  runat="server" /></td>
							<td colspan="3"></td>
						</tr>
					</table>
				</div>				
			</div>
	       </div>
       </div>
   </div>
</section>
        
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
    </form>

    <script language="javascript">

        document.getElementById("drpState").onchange = drpState_Change;

        function drpState_Change(e) {
            var stateId = document.getElementById("drpState").value;
            var response = AjaxFunctions.GetCities(stateId);

            var dependentCmbs = new Array();
            //add the name of the dependent combos on this combo
            dependentCmbs[0] = "drpArea";
            //call the function to consume this data
            BindCities(response, document.getElementById("drpCity"), "hdn_drpCity", dependentCmbs, "--Select City--");
        }

        function BindCities(response, cmbToFill, hdnId, dependentCmbs, selectString) {
            var _delimeter = "|";
            if (response.error != null) {
                alert("ERROR : " + response.error);
                return;
            }
            var objHdn = document.forms[0][hdnId];
            //now fill the values to the drop down
            if (cmbToFill) {
                var j = 1;
                var ds = response.value;
                if (ds != null && typeof (ds) == "object" && ds.Tables != null) {
                    var content = "";
                    for (i = 0; i < ds.Tables[0].Rows.length; i++) {
                        cmbToFill.options[j] = new Option(ds.Tables[0].Rows[i].CityName, ds.Tables[0].Rows[i].CityId);
                        if (content == "") {
                            content = ds.Tables[0].Rows[i].CityName + _delimeter + ds.Tables[0].Rows[i].CityId;
                        } else {
                            content += _delimeter + ds.Tables[0].Rows[i].CityName + _delimeter + ds.Tables[0].Rows[i].CityId;
                        }
                        j++;
                    }
                    //add the content to the hidden value
                    if (objHdn) {
                        objHdn.value = content;
                    }
                    if (j > 1) {
                        cmbToFill.disabled = false;
                    } else {
                        cmbToFill.disabled = true;
                        if (dependentCmbs) {
                            for (var i = 0; i < dependentCmbs.length; i++) {
                                var depCmb = document.forms[0][dependentCmbs[i]];
                                if (depCmb) {                                    
                                    depCmb.disabled = true;
                                }
                            }
                        }
                    }
                }
            }
        }

</script>
<script language="javascript">

    function validateForm() {
        var spn = document.getElementById('spnCustom');
        var isError = false;

        var re = /^[0-9]*$/

        //at least one phone no. is compulsory
        if (getId('txtPhone1').value == "" && getId('txtMobile').value == "") {
            spn.innerHTML = "Please provide atleast one phone number";
            isError = true;
        }
        else {
            spn.innerHTML = '';
        }

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
        if (isError) {
            window.scroll(0, 0);
            return false;
        }
    }

    function getId(controlId) {
        return document.getElementById(controlId);
    }

    document.forms[0].onsubmit = validateForm;

</script>
</body>
</html>

