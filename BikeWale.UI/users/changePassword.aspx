<%@ Page Inherits="Bikewale.Users.ChangePassword" AutoEventWireUp="false" Language="C#" Trace="false" Debug="false" %>
<% //Modified By :Sajal Gupta on 03 August 2016
    isAd300x250Shown = false;
    isAd300x250BtfShown = false; %>
<!-- #include file="/includes/headMyBikeWale.aspx" -->
    <div class="container_12 container-min-height">
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
                <li class="current"><strong>Change Password</strong></li>
            </ul><div class="clear"></div>
        </div> 
	    <div id="content" class="grid_8">				
			<div id="uiTabs">
			<!--Change Password Content-->
                <h2 class="margin-top10">Change Password</h2>
				<span id="spnError" class="error margin-top15" runat="server"></span>
				<table border="0" cellpadding="5" width="100%"  cellspacing="0" class="margin-top15">
					<tr>
						<td colspan="2">
							<p><span class="required">* </span>All fields are mandatory.</p>
						</td>
					</tr>
					<tr>
						<td>Current Password :</td>
						<td>
							<asp:TextBox ID="txtCurPassword" MaxLength="20" TextMode="Password" runat="server"  CssClass="text"></asp:TextBox>
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
							<asp:Button ID="butChange" Text="Change Password" CssClass="buttons text_white" runat="server"></asp:Button>
							&nbsp;&nbsp;
							<asp:Button ID="butCancel" CausesValidation="false" Text="Cancel" CssClass="buttons text_white" runat="server"></asp:Button>				
						</td>
					</tr>
				</table>
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

        var objCurPass = $("#txtCurPassword");
        var objNewPass = $("#txtNewPassword");
        var objConfPass = $("#txtConfirmNewPassword");
        var objError = $("#spnError");
        var regexPassword = /^[a-zA-Z]+$/;

        objError.val("");

        if (objCurPass.val() == "" || objNewPass.val() == "" || objConfPass.val() == "")
        {
            objError.text("Please Fill all the fields.");
            isError = true;
        } else if (objNewPass.val().length < 6) {
            objError.text("Password should be atleast 6 characters long.");
            isError = true;
        } else if (regexPassword.test(objNewPass.val())) {
            objError.text("Password should contain atleast one number or special character.");
            isError = true;
        } else if (objNewPass.val() == objCurPass.val()) {
            objError.text("New Password should not match current password.");
            isError = true;
        }
        else if (objNewPass.val() != objConfPass.val()) {
            objError.text("New password and confirm password do not match.");
            isError = true;
        }

        return isError;
    }
</script>
<!-- #include file="/includes/footerInner.aspx" -->
<!-- Footer ends here -->
