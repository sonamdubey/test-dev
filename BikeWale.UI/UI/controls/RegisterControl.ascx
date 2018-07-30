<%@ Control Language="C#" AutoEventWireup="true" Inherits="Bikewale.Controls.RegisterControl" %>
<%@ Import NameSpace="Bikewale.Common" %>
<h1>Please register here &nbsp; &nbsp;<span> Already registered? <a href="/users/login.aspx">Login</a></span></h1>
<p class="desc-para"><font color="red">*</font> marked fields are compulsory</p>
<div class="margin-top5">	
	<span id="spnError" class="required" runat="server" />
	<table class="tbl-default" border="0" width="100%" cellpadding="3" cellspacing="3" runat="server">		
		<tr>
			<td width="30%">Your Name <span class="required">*</span> </td>
			<td><asp:TextBox ID="txtName" CssClass="txtGrad" MaxLength="50" Columns="20" runat="server" />            
				<span id="errName" style="color:red;font-weight:bold;"></span> 
			</td>
		</tr>
		<tr>
			<td>Your Email <span class="required">*</span> </td>
			<td><asp:TextBox ID="txtEmail" CssClass="txtGrad" MaxLength="50" Columns="25" runat="server" />
				<span id="errEmail" runat="server" style="color:red;font-weight:bold;"></span><br />				
			</td>
		</tr>
		<tr>
			<td>Confirm Email <span class="required">*</span> </td>
			<td><asp:TextBox ID="txtEmailConf" CssClass="txtGrad" MaxLength="50" Columns="25" runat="server" />
				<span id="errEmailConf" runat="server" style="color:red;font-weight:bold;"></span>
			</td>
		</tr>
		<tr>
			<td>Set Password <span class="required">*</span></td>
			<td>
				<asp:TextBox ID="txtPassword" TextMode="Password" CssClass="txtGrad" MaxLength="20" Columns="20" runat="server" />            
				<span id="errPassword" style="color:red;font-weight:bold;"></span>				
			</td>
		</tr>
		<tr>
			<td>Confirm Password <span class="required">*</span></td>
			<td>
				<asp:TextBox ID="txtConfirmPassword" TextMode="Password" CssClass="txtGrad" MaxLength="20" Columns="20" runat="server" />
				<span id="errPasswordMatch" style="color:red;font-weight:bold;"></span>
			</td>
		</tr>
        <tr>
			<td><span class="right-float">+91-</span>Mobile No. </td>
			<td>
                <asp:TextBox ID="txtMobile" MaxLength="10"  runat="server" />
                <span id="errMobile" style="color:red;font-weight:bold;"></span>
			</td>            			
		</tr>		
		<tr class="hide">
			<td>Where did you hear about BikeWale ? <span class="style1">*</span></td>
			<td>
				<asp:DropDownList ID="cmbAboutCarwale" runat="server">
					<asp:ListItem Text="Select" Value="" Selected="true" />            
					<asp:ListItem Text="Friend" Value="Friend" />            
					<asp:ListItem Text="Newspaper" Value="Newspaper" />            
					<asp:ListItem Text="SMS" Value="SMS" />   
					<asp:ListItem Text="Email" Value="Email" />            
					<asp:ListItem Text="Google Search" Value="Google" />
					<asp:ListItem Text="Rediff" Value="Rediff" />
					<asp:ListItem Text="Yahoo Search" Value="Yahoo" />
					<asp:ListItem Text="MSN Search" Value="MSN" />
					<asp:ListItem Text="Other Search" Value="Other Search" />
					<asp:ListItem Text="Pamphlet" Value="Pamphlet" />
					<asp:ListItem Text="Magazine" Value="Magazine" />
					<asp:ListItem Text="Bus" Value="Bus" />
					<asp:ListItem Text="Other" Value="Other" />				
				</asp:DropDownList> 
				<span id="errCarwaleContact" style="color:red;font-weight:bold;"></span>			
			</td>
		</tr>
		<tr>
            <td>&nbsp;</td>
			<td>
				<input type="checkbox" onClick="checkStatus(this)" name="chkPrivacy" checked value="checkbox">
				I have read and agree with the <a target="_blank" href="<%=CommonOpn.AppPath%>PrivacyPolicy.aspx">User Agreement and Privacy Policy</a> 
			</td>
		</tr>
		<tr>
            <td>&nbsp;</td>
			<td><asp:Button ID="btnRegister" CssClass="action-btn text_white" Text="Register Me" runat="server" /></td>			
		</tr>
	</table>
</div>
<div class="clear"></div>
<script type="text/javascript">
    $("#<%=btnRegister.ClientID.ToString() %>").click(function () {       
        if (validateControl()) {
            return false;
        }
        else {
            return true;
        }
    });

    function validateControl() {

        var reName = /^[a-zA-Z0-9'\- ]+$/;
        var re = /^[0-9]*$/
        var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
        var isError = false;
        var regPass = /^[a-zA-Z]+$/;

        var name = $("#<%=txtName.ClientID.ToString() %>").val();
        var email = $("#<%=txtEmail.ClientID.ToString() %>").val();
        var con_email = $("#<%=txtEmailConf.ClientID.ToString() %>").val();
        var pass = $("#<%=txtPassword.ClientID.ToString() %>").val();
        var con_pass = $("#<%=txtConfirmPassword.ClientID.ToString() %>").val();
        var mobile = $("#<%=txtMobile.ClientID.ToString() %>").val();
        var name_msg = $("#errName");
        var email_msg = $("#<%=errEmail.ClientID.ToString() %>");
        var con_email_msg = $("#<%=errEmailConf.ClientID.ToString() %>");
        var pass_msg = $("#errPassword");
        var con_pass_msg = $("#errPasswordMatch");
        var mobile_msg = $("#errMobile");       
        
        if ($.trim(name) == "") {          
            name_msg.html("Required");
            isError = true;
        } else if (!reName.test($.trim(name))) {
            name_msg.html("Name should be Alphanumeric.");
            isError = true;
        }
        else {
            name_msg.html("");
        }

        if ($.trim(email) == "") {
            email_msg.html("Required");
            isError = true;
        } else if (!reEmail.test($.trim(email).toLowerCase())) {
            email_msg.html("Invalid EmailId");
            isError = true;
        } else {
            email_msg.html("");
        }        
       
        if ($.trim(con_email) == "") {
            con_email_msg.html("Required");
            isError = true;
        } else if (!reEmail.test($.trim(con_email).toLowerCase())) {
            con_email_msg.html("Invalid EmailId");
            isError = true;
        } else if ($.trim(email) != $.trim(con_email)) {
            con_email_msg.html("Emails do not match. Please retype carefully.");
            isError = true;
        } else {
            con_email_msg.html("");
        }

        if ($.trim(pass) == "") {
            pass_msg.html("Required");
            isError = true;
        } else if ($.trim(pass).length < 6) {
            pass_msg.html("Password should be atleast 6 characters long");
            isError = true;
        } else if (regPass.test($.trim(pass))) {
            pass_msg.html("Password should contain atleast one number or special character.");
            isError = true;
        }
        else {
            pass_msg.html("");
        }

        if ($.trim(con_pass) == "") {
            con_pass_msg.html("Required");
            isError = true;
        } else if ($.trim(pass) != $.trim(con_pass)) {
            con_pass_msg.html("Password didn't match. Please retype carefully.");
            isError = true;
        }else {
            con_pass_msg.html("");
        }

        if ($.trim(mobile) != "") {
            if (!re.test($.trim(mobile).toLowerCase())) {
                mobile_msg.html("Mobile No. should be numeric only");
                isError = true;                
            } else if (mobile.length < 10) {
                mobile_msg.html("Mobile no should be greater than 10 digits");
                isError = true;
            } else {
                mobile_msg.html("");
            }
        }
        return isError;
    }
	
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
	
    function getCtrlId( controlId ){
        return document.getElementById( '<%=this.ID%>_' + controlId );
    }
	
</script>