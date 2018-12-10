<%@ Control Inherits="Carwale.UI.Controls.RegisterControl" Language="C#" %>
<%@ Import NameSpace="Carwale.UI.Common" %>

<style type="text/css">
<!-- 
	.infoTop {width:520px; margin-top:5px;}
    .padding5 { padding:5px; }
-->
</style>
<h1 class="content-inner-block" style="font-size:19px;">Create Your CarWale Account</h1>
<div class="white-shadow content-inner-block">
	<span style="font-weight:normal;color:#333333;text-align:right">
		<font color="red">*</font> Marked fields are compulsory
	</span>
	<span id="spnError" class="error" runat="server" />
	<table border="0" width="100%" cellpadding="3" cellspacing="3" runat="server" class="margin-top10">
		<%--<tr>
			<td colspan="2" class="midHead">
				Contact Information
			</td>
		</tr>--%>
		<tr>
			<td width="30%">Name <span class="style1">*</span> </td>
			<td><asp:TextBox ID="txtName" CssClass="txtGrad" MaxLength="50" Columns="25" runat="server" />            
				<span id="errName" style="color:red;font-weight:bold;"></span> 
			</td>
		</tr>
		<tr>
			<td>Email <span class="style1">*</span> </td>
			<td><asp:TextBox ID="txtEmail" CssClass="txtGrad" MaxLength="50" Columns="25" runat="server" />
				<span id="errEmail" runat="server" style="color:red;font-weight:bold;"></span><br />				
			</td>
		</tr>
		<tr>
			<td>Confirm Email <span class="style1">*</span> </td>
			<td><asp:TextBox ID="txtEmailConf" CssClass="txtGrad" MaxLength="50" Columns="25" runat="server" />
				<span id="errEmailConf" runat="server" style="color:red;font-weight:bold;"></span>
			</td>
		</tr>
		<tr>
			<td>Create a Password <span class="style1">*</span></td>
			<td>
				<asp:TextBox ID="txtPassword" TextMode="Password" CssClass="txtGrad padding5" MaxLength="15" runat="server" style="border: 1px solid #C9C9C9;"/>            
				<span id="errPassword" style="color:red;font-weight:bold;"></span>				
			</td>
		</tr>
		<tr>
			<td>Confirm your Password <span class="style1">*</span></td>
			<td>
				<asp:TextBox ID="txtConfirmPassword" TextMode="Password" CssClass="txtGrad padding5" MaxLength="15" runat="server" style="border: 1px solid #C9C9C9;"/><br>
				<span id="errPasswordMatch" style="color:red;font-weight:bold;"></span>
			</td>
		</tr>
		<tr id="trContactDetails" runat="server">
			<td>Mobile No. <span class="rightfloat">+91-</span><span class="clear"></span></td>
            <td><asp:TextBox ID="txtMobile" MaxLength="10"  runat="server" />
                <span id="spnCustom" style="color:red;font-weight:bold;"></span>
            </td>
		</tr>
		<tr>
			<td colspan="2" class="midHead" style="font-weight:normal">
				<input type="checkbox" onClick="checkStatus(this)" name="chkPrivacy" checked value="checkbox">
				I have read and agree with the <a target="_blank" href="<%=CommonOpn.AppPath%>PrivacyPolicy.aspx">User Agreement and Privacy Policy</a> 
			</td>
		</tr>
		<tr align="center">
			<td colspan="2">
				<div class="buttons">
					<asp:Button ID="btnRegister" CssClass="buttons" Text="Register Me"  runat="server" />    
				</div>
			</td>
		</tr>
	</table>
</div>
<div style="clear:both;"></div>
<script language="javascript">
	
	function checkStatus( chk ){
		getCtrlId('btnRegister').disabled = chk.checked ? false : true; 
	}
	/* Privacy Check-Uncheck ends here */
	var controlPrefix = '';
	var tables = document.getElementsByTagName('table');

	for( var i=0; i<tables.length; i++ ){
		if ( tables[i].id && tables[i].id.indexOf("tblAsk") > 0 ){
			controlPrefix = tables[i].id.substring( 0, tables[i].id.lastIndexOf("_") );
		}
	}
	
	getCtrlId('btnRegister').onclick = verifyInputsRegistration;
				
	function verifyInputsRegistration(e){
		var spn = document.getElementById('spnCustom');
		var isError = false;
		
		var showContactDetails = <%= showContactDetails.ToString().ToLower()%>;
		
		var re = /^[0-9]*$/
		var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
		
		if ( getCtrlId ( 'txtName' ).value == "" ){
			document.getElementById('errName').innerHTML = "Required";
			isError = true;
		}else{
			document.getElementById('errName').innerHTML = "";
		}
		
		var eMailT = getCtrlId ( 'txtEmail' ).value.toLowerCase();
		var eMailC = getCtrlId ( 'txtEmailConf' ).value.toLowerCase();
						
		if ( eMailT == "" ){
			getCtrlId('errEmail').innerHTML = "Required";
			isError = true;
		}else if ( !reEmail.test ( eMailT ) ){ // verify valid email!
			getCtrlId('errEmail').innerHTML = "Invalid EmailId";
			isError = true;
		}else{
			getCtrlId('errEmail').innerHTML = "";
		}
		
		if ( eMailC == "" ){
			getCtrlId('errEmailConf').innerHTML = "Required";
			isError = true;
		}else if ( !reEmail.test ( eMailC ) ){
			getCtrlId('errEmailConf').innerHTML = "Invalid Email";
			isError = true;
		}else if(eMailT != eMailC){
			getCtrlId('errEmailConf').innerHTML = "Emails do not match. Please retype carefully.";
			isError = true;
		}else{
			getCtrlId('errEmailConf').innerHTML = "";
		}
				
		if ( getCtrlId ( 'txtPassword' ).value == "" ){
			document.getElementById('errPassword').innerHTML = "Required";
			isError = true;
		}else{
			document.getElementById('errPassword').innerHTML = "";
		}
		
		if ( getCtrlId ( 'txtConfirmPassword' ).value != getCtrlId ( 'txtPassword').value ){
			document.getElementById('errPasswordMatch').innerHTML = "Password didn't match. Please retype carefully."
			isError = true;
		}else{
			document.getElementById('errPasswordMatch').innerHTML = '';
		}
		
		if(showContactDetails){		
		    if ( getCtrlId ( 'txtMobile' ).value == "")
		    {
		        if(!re.test(getCtrlId ( 'txtMobile' ).value)){
		            spn.innerHTML = "<br>Mobile No. should be numeric only";
		            isError = true;			
		        }
		    }else{
		        spn.innerHTML = '';
		    }
		}
		
		if ( isError ){
			return false;
		}
	}
	
	function getCtrlId( controlId ){
		return document.getElementById( '<%=this.ID%>_' + controlId );
	}
	
</script>