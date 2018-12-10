<%@ Page Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Users.ResetPassword" %>
<!doctype html>
<html>
<head>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
    PageId          = 2;
    Title           = "Reset Password";
    AdId            = "1337162297840";
    AdPath          = "/7590/CarWale_MyCarWale/CarWale_MyCarWale_Misc/CarWale_MyCarWale_Misc_";
%>
<!-- #include file="/includes/global/head-script.aspx" -->
<style>
    td {padding:5px;vertical-align:middle}
</style>
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
        <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
            <div class="container">
                <div class="grid-12">                  
                    <h1 class="font30 text-black special-skin-text">Reset Password</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-10">
						<div class="mid-box margin-top10">
                        <div class="" id="tb1">                            
                            <div class="clear"></div>
                            <div class="white-shadow content-inner-block">
                                <form method="post">
                                    <h3>
                                        Hi <%= cust.Name %>, please provide a new password
                                    </h3><br />
                                    <table>
                                        <tr>
                                            <td style="width:115px;">
                                                New Password
                                            </td>
                                            <td>
                                                <input class="text rightfloat form-control" type="password" name="txtNewPassword" id="txtNewPassword"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Confirm Password
                                            </td>
                                            <td>
                                                <input class="text rightfloat form-control" type="password" name="txtConfirmPassword" id="txtConfirmPassword"/>
                                            </td>
                                        </tr>
                                        <tr style="display:none;" id="error">
                                            <td colspan="2">
                                                <span class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <input class="rightfloat btn btn-orange" style="width:165px; padding:5px;" id="btnSubmit" type="submit" value="Change Password" />
                                            </td>
                                        </tr>
                                    </table>
                                    <input type="hidden" id="hdnAT" runat="server"/>
                                </form>
                            </div>
                        </div>
                    </div>
					</div>
				</div>




				<div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
        <script>
            $('#btnSubmit').click(function () {
                var resp = true;
                var pass = $.trim($('#txtNewPassword').val());
                if (pass == $.trim($('#txtConfirmPassword').val())) {
                    if (pass.length < 6) {
                        resp = false;
                        $('#error').show();
                        $('#error span').text('Password must contain atleast 6 characters.');
                    }
                }
                else {
                    resp = false;
                    $('#error').show();
                    $('#error span').text('Passwords do not match');
                }
                if (resp)
                    $('#error').hide();
                return resp;
            });
</script>
</form>
</body>
</html>
