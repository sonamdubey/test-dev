<%@ Page Language="C#" Trace="false" %>
<%@ Register TagPrefix="BikeWale" TagName="Login" src="/Controls/LoginControl.ascx" %>
<%
    title = "Forgot Password? - BikeWale";
    description = "Retrieve Your Password On Your Email";
    keywords = "password, retrieve your password, get password";
    //Modified By :Sajal Gupta on 03 August 2016
    isAd300x250Shown = false;
    isAd300x250BtfShown = false;
%>
<!-- #include file="/includes/headMyBikeWale.aspx" -->    
<div class="container_12 margin-top10">
    <div class="grid_8 min-height content-inner-block-10">
        <h1>Forgot Password?</h1>  
        <table class="tbl-default margin-top15" border="0" cellspacing="0" cellpadding="3">
	        <tr>
		        <td width="130"><strong>Enter your email id</strong></td>
		        <td><input type="text" size="25" id="txtEmail" />&nbsp;&nbsp;</td>
	        </tr>
            <tr>
                <td>&nbsp;</td>
                <td><b><span id="processing_pwd_fp" class="hide"> Please wait...</span></b>	</td>
            </tr> 
            <tr>
                <td>&nbsp;</td>
                <td><input type="button" id="btnGetPassword" value="Get Password" class="action-btn text_white" /></td>
            </tr>          
        </table>	   				
    </div>
</div>  
<script type="text/javascript">
    $("#btnGetPassword").click(function () {
        sendPwd();
    });

    function sendPwd() {
        var email = $("#txtEmail").val();
        var reEmail = /^[a-z]+(([a-z_0-9]*)|([a-z_0-9]*\.[a-z_0-9]+))*@([a-z_0-9\-]+)((\.[a-z]{3})|((\.[a-z]{2})+)|(\.[a-z]{3}(\.[a-z]{2})+))$/;

        if (reEmail.test(email)) {
            $("#processing_pwd_fp").removeClass("hide");
            setTimeout('requestForgotPwd()', 1000); // prepare loading	
        } else {
            alert("Please enter valid email to retrieve password")
            return false;
        }
    }

    function requestForgotPwd() {
        var response = "";
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
            data: '{"email":"' + $("#txtEmail").val() + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SendCustomerPwd"); },
            success: function (response) {
                var responseJSON = eval('(' + response + ')');
                if (responseJSON.value == true) {                  
                    $("#processing_pwd_fp").html("Your password has been sent to your email address.");
                }
                else if(responseJSON.value == false) {
                    $("#processing_pwd_fp").html("<span class=readmore>This email id is not registered with us. <a href='login.aspx' style='font-weight:normal;'>Register Now</a><span>");
                }
            }
        });
    }

    //$(document).ready(function () {
    //    $("#txtEmail").click(function () {
    //        if ($("#processing_pwd").text() == "Your password has been sent to your email address.")
    //        {
    //            $("#processing_pwd").text("");
    //        }
    //    });
    //});

</script>
<!-- #include file="/includes/footerinner.aspx" -->
