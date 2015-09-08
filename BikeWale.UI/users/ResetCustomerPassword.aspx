<%@ Page Inherits="Bikewale.Users.ResetCustomerPassword" AutoEventWireUp="false" Language="C#" Trace="false" Debug="false" %>
<!-- #include file="/includes/headMyBikeWale.aspx" -->
    <div class="container_12">
        <div class="grid_12">
            <ul class="breadcrumb">
                <li>You are here: </li>
                <li><a href="/">Home</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li><a href="/mybikewale/">My BikeWale</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong>Reset Password</strong></li>
            </ul><div class="clear"></div>
        </div> 
	    <div id="content" class="grid_8">				
			<div id="uiTabs" class="min-height">
			<!--Change Password Content-->
                <h2 class="margin-top10">Reset Your Password</h2>
				<span id="spnError" class="error margin-top15" runat="server"></span>
				<table id="tblPassword" border="0" cellpadding="5" width="100%"  cellspacing="0" class="margin-top15" runat="server">
					<tr>
						<td colspan="2">
							<p><span class="required">* </span>All fields are mandatory.</p>
						</td>
					</tr>					
					<tr>
						<td >New Password :</td>
						<td>
							<asp:TextBox ID="txtNewPassword" MaxLength="20" TextMode="Password" runat="server"  CssClass="text"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td>Confirm New Password :</td>
						<td>
							<asp:TextBox ID="txtConfirmNewPassword" MaxLength="20" TextMode="Password" runat="server"  CssClass="text"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td>				
							<asp:Button ID="butChange" Text="Reset Password" CssClass="buttons" runat="server"></asp:Button>
							&nbsp;&nbsp;
							<asp:Button ID="butCancel" CausesValidation="false" Text="Cancel" CssClass="buttons" runat="server"></asp:Button>				
						</td>
					</tr>
				</table>
                <div id="divErrMsg" runat="server" class="grey-bg border-light margin-top20 content-block"></div>
			</div>			
	    </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $("#butChange").click(function () {
            if (validateInputs())
            {
                return false;
            }
        });
    });

    function validateInputs()
    {
        var isError = false;
        
        var objNewPass = $("#txtNewPassword");
        var objConfPass = $("#txtConfirmNewPassword");
        var objError = $("#spnError");
        var regexPassword = /^[a-zA-Z]+$/;

        objError.val("");

        if (objNewPass.val() == "" || objConfPass.val() == "")
        {
            objError.text("Please Fill all the fields.");
            isError = true;
        } else if (objNewPass.val().length < 6) {
            objError.text("Password should be atleast 6 characters long.");
            isError = true;
        } else if (regexPassword.test(objNewPass.val())) {
            objError.text("Password should contain atleast one number or special character.");
            isError = true;
        } else if (objNewPass.val() != objConfPass.val()) {
            objError.text("New password and confirm password do not match.");
            isError = true;
        }

        return isError;
    }
</script>
<!-- #include file="/includes/footerInner.aspx" -->
<!-- Footer ends here -->
