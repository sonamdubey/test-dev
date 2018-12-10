<%@ Page Language="C#" Inherits="Carwale.UI.Used.ReportListing" AutoEventWireup="false" Trace="false" %>
<style type="text/css">
.bharti-content { height:290px; overflow:auto; border-bottom:1px solid #CCCCCC; }
.ul-bullets { margin:0px; padding:0px; }
.ul-bullets ul { margin-left:20px;  }
.ul-bullets li { list-style-type:disc; margin-bottom:5px; }
.bharti-axa-logo {
    background:url(https://img.aeplcdn.com/insurance/bharti-axa/bharti-axa-logo.png?v1.0) no-repeat;
    display:inline-block;
    height:45px;
    width:63px;
}
.powered-bt-text {
    font-size: 11px;
    position: relative;
    right: 5px;
    vertical-align:top;
}
#insFormContainer input[type=text] { margin-bottom:10px; }
</style>
<div class="seller-details-on-sms">
    <div class="content-inner-block-10">
        <!-- form starts here -->
        <div id="insFormContainer">
            <div>
                <h4 class="leftfloat">Request a call</h4>
                <div class="rightfloat">
                    <div>
                        <span class="powered-bt-text">Powered by</span>
                        <span class="bharti-axa-logo"></span>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
            <div class="margin-top10 seller-details-form" id="buyer_form">
                <div>
                    <input type="text" placeholder="Name" class="form-control" id="txtInsName" name="txtInsName">
                    <span class="cw-used-sprite uc-uname"></span>
                    <span id="txtInsNameError" class="cw-blackbg-tooltip hide" style="display: none;">Please </span>
                    <span id="errorInsNameCircle" class="cwsprite error-icon hide" style="display: none;"></span>
                </div>
                <div class="mobile-box">
                    <span class="mobile-text">+91</span>
                    <input type="text" maxlength="10" placeholder="Mobile Number" class="form-control" id="txtInsMobile" name="txtInsMobile">
                    <span class="cw-used-sprite uc-mobile"></span>
                    <span id="txtInsMobileError" class="cw-blackbg-tooltip hide" style="display: none;"></span>
                    <span id="errorInsMobileCircle" class="cwsprite error-icon hide" style="display: none;"></span>
                </div>
                <div>
                    <input type="text" placeholder="Email" class="form-control" id="txtInsEmail" name="txtInsEmail">
                    <span class="cw-used-sprite uc-email"></span>
                    <span id="txtInsEmailError" class="cw-blackbg-tooltip hide" style="display: none;"></span>
                    <span id="errorInsEmailCircle" class="cwsprite error-icon hide" style="display: none;"></span>
                </div>
                <div class="margin-top10">
                    <a class="btn btn-orange text-uppercase" id="btnSubmitAxaLead">Submit</a>
                </div>
                <div class="margin-top10">
                    <p class="font11">By submitting your information, you accept that we or our partner Bharti AXA General Insurance may contact you regarding your inquiry.</p>
                </div>
                <div class="clear"></div>
            </div>
        </div>
        <!-- form ends here -->
        <!-- thank you message after form submission-->
        <div class="margin-top20" id="insThankYou" style="display:none;">
            <h4>Thank You.</h4>
            <p class="margin-top15">An executive from our partner Bharti AXA will contact you.</p>
        </div>
        <!-- thank you messge ends here-->
        <!-- or text ends here -->
    </div>
</div>
<div class="margin-top20">	
    <div class="clear"></div>
</div>
<script type="text/javascript">
    var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
    var re = /^[0-9]*$/;

    $(document).ready(function () {
        if ($.cookie("TempCurrentUser") != null) {
            var objCust = $.cookie("TempCurrentUser").split(':');
            $("#txtInsName").val(objCust[0]);
            $("#txtInsMobile").val(objCust[1]);
            $("#txtInsEmail").val(objCust[2]);
        }
        $("#btnSubmitAxaLead").click(function () {
            var custName = $("#txtInsName").val();
            var custMobile = $("#txtInsMobile").val();
            var custEmail = $("#txtInsEmail").val();

            $("#txtInsNameError").hide();
            $("#errorInsNameCircle").hide();
            $("#txtInsMobileError").hide();
            $("#errorInsMobileCircle").hide();
            $("#txtInsEmailError").hide();
            $("#errorInsEmailCircle").hide();

            if ($.trim(custName) == "") {
                $("#txtInsNameError").text("Please enter your name").show();
                $("#errorInsNameCircle").show();
            }
            else {
                
                if ($.trim(custMobile) == "") {
                    $("#txtInsMobileError").text("Please enter your mobile number").show();
                    $("#errorInsMobileCircle").show();
                }
                else if (!re.test(custMobile)) {
                    $("#txtInsMobileError").text("Invalid mobile number").show();
                    $("#errorInsMobileCircle").show();
                }
                else {

                    if ($.trim(custEmail) == "") {
                        $("#txtInsEmailError").text("Please enter your email address").show();
                        $("#errorInsEmailCircle").show();
                    }
                    else if (!reEmail.test(custEmail)) {
                        $("#txtInsEmailError").text("Invalid email address").show();
                        $("#errorInsEmailCircle").show();
                    }
                    else {
                        $(this).text("Submitting..").attr("disabled", true);
                        SaveAndSendBhartiAxaLead($.trim(custName), custMobile, custEmail);
                    }
                }
            }
        });
        $("#insFormContainer input[type=text]").focus(function () {
            $(this).next().next().hide();
            $(this).next().next().next().hide();
        })
    });

    function SaveAndSendBhartiAxaLead(custName, custMobile, custEmail) {
        var jsonData = new Object({
            BhartiAxaLeadId: -1, Mobile: custMobile, Name: custName,
            Email: custEmail, CityName: cityName, MakeName: MakeName,
            ModelName: ModelName, VersionName: VersionName,
            InsuranceCompany: "Used Car Details - ProfileId:" + profileId + " Bharti Axa general insurance",
            Agency: "carwale-used", InsuranceType: true
        });
        //$.post('/webapi/bhartiaxa/saveinsurancelead/', { '': JSON.stringify(jsonData) })
        //.done(function (response) {
        //    console.log(response);
        //});
        $.ajax({
            url: '/webapi/bhartiaxa/saveinsurancelead/',
            type: "POST",
            data: JSON.stringify(jsonData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $("#insFormContainer").hide();
                $("#insThankYou").show();
            }
        });
    }
</script>