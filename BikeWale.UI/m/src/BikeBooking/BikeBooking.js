$(document).ready(function () {
    $("#getDealerDetails,#dealerPriceQuote").click(function () {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
            data: '{"pqId":"' + pqId + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "IsNewBikePQExists"); },
            success: function (response) {
                var responseJSON = eval('(' + response + ')');
                var resObj = eval('(' + responseJSON.value + ')');
                if (resObj == false) {
                    $(".contact-details").show();
                    $("#txtName").focus();
                    $("html, body").animate({ scrollTop: $(".contact-details").offset().top }, 0);
                }
                else
                    location.href = "/m/pricequote/DetailedDealerQuotation.aspx";
            }
        });        
    });

    $(".close-btn").click(function () {
        $(".contact-details").hide();
    });

    $("#editNum").click(function () {
        $(".edit-mob").hide();
        $(".edit-done-mob").show();
        $("#editedMobNo").focus();

    });

    $("#txtName").val(Customername);
    $("#verify-mobile").val(mobileNo);
    $("#txtEmail").val(email);
});

function validateDetails() {

    Customername = $("#txtName").val();
    email = $("#txtEmail").val().toLowerCase();
    mobileNo = $("#verify-mobile").val();

    var retVal = true;
    var errorMsg = "";

    var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
    var reMobile = /^[0-9]*$/;
    var name = /^[a-zA-Z& ]+$/;

    // $("#hdnMobile").val($("#numMobile").val());

    if (Customername == "") {
        retVal = false;
        errorMsg += "Please enter your Name<br>";
    }
    else if (!name.test(Customername)) {
        retVal = false;
        errorMsg += "Please enter valid Name<br>";
    }

    if (email == "") {
        retVal = false;
        errorMsg += "Please enter your Email<br>";
    }
    else if (!reEmail.test(email)) {
        retVal = false;
        errorMsg += "Invalid Email<br>";
    }

    if (mobileNo == "") {
        retVal = false;
        errorMsg += "Please enter your Mobile Number<br>";
    }
    else if (!reMobile.test(mobileNo)) {
        retVal = false;
        errorMsg += "Mobile Number should be numeric<br>";
    }
    else if (mobileNo.length != 10) {
        retVal = false;
        errorMsg += "Mobile Number should be of 10 digits<br>";
    }
    if (retVal == false) {
        $("#spnError").html(errorMsg);
        $("#popupDialog").popup("open");
    }
    else {
        SaveCustomerDeatails();
    }

    return retVal;
}

function SaveCustomerDeatails() {
    $.ajax({
        type: "POST",
        url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
        data: '{"dealerId":"' + dealerId + '", "pqId":"' + pqId + '", "customerName":"' + Customername + '", "customerMobile":"' + mobileNo + '", "customerEmail":"' + email + '", "versionId":"' + versionId + '", "cityId":"' + cityId + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SaveCustomerDetail"); },
        success: function (response) {
            var responseJSON = eval('(' + response + ')');
            var resObj = eval('(' + responseJSON.value + ')');
            if (resObj == false) {
                $(".contact-details").hide();
                $(".mobile-verification").show();
                $(".mobile-verification").find(".close-btn").click(function () {
                    $(".mobile-verification").hide();
                    $(".contact-details").show();
                });
                $("#mobNo").text(mobileNo);
                $("#txtCwi").focus();
            }
            else {
                CallABApi();
                //location.href = "/m/pricequote/DetailedDealerQuotation.aspx?dealerId=" + dealerId;
            }
        }
    });
}

function UpdateMobile() {
    var retVal = true;
    var reMobile = /^[0-9]*$/;

    var CustomerMobile = $("#editedMobNo").val();

    var errorMsg = "";
    if (CustomerMobile == "") {
        retVal = false;
        errorMsg += "Please enter your Mobile Number<br>";
    }
    else if (!reMobile.test(CustomerMobile)) {
        retVal = false;
        errorMsg += "Mobile Number should be numeric<br>";
    }
    else if (CustomerMobile.length != 10) {
        retVal = false;
        errorMsg += "Mobile Number should be of 10 digits<br>";
    }
    if (retVal == false) {
        $("#spnError").html(errorMsg);
        $("#popupDialog").popup("open");

        $(".edit-mob").hide();
        $(".edit-done-mob").show();
    }
    else {
        $(".edit-mob").show();
        $(".edit-done-mob").hide();
        $("#mobNo").text(CustomerMobile);
        mobileNo = CustomerMobile;

        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
            data: '{"pqId":"' + pqId + '", "customerName":"' + Customername + '", "customerMobile":"' + mobileNo + '", "customerEmail":"' + email + '", "versionId":"' + versionId + '", "cityId":"' + cityId + '", "branchId":"' + dealerId + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdateMobileNumber"); },
            success: function (response) {
                var responseJSON = eval('(' + response + ')');
                var resObj = eval('(' + responseJSON.value + ')');
                if (resObj == false) {
                    alert("The verification code will be sent to this number..");
                    $("#txtCwi").focus();
                }
                else {
                    CallABApi();
                    //location.href = "/m/pricequote/DetailedDealerQuotation.aspx?dealerId=" + dealerId;
                }
            }
        });
    }
}

function VerifyCustomer() {
    var retVal = true;
    var isNumber = /^[0-9]*$/;

    var cwiCode = $("#txtCwi").val();

    var errorMsg = "";

    if (cwiCode == "") {
        retVal = false;
        errorMsg += "Please enter your Verification Code<br>";
    }
    else if (!isNumber.test(cwiCode)) {
        retVal = false;
        errorMsg += "Verification Code should be numeric<br>";
    }
    else if (cwiCode.length != 5) {
        retVal = false;
        errorMsg += "Verification Code should be of 5 digits<br>";
    }
    if (retVal == false) {
        $("#spnError").html(errorMsg);
        $("#popupDialog").popup("open");

        $(".edit-mob").hide();
        $(".edit-done-mob").show();
    }
    else {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
            data: '{"pqId":"' + pqId + '", "customerMobile":"' + mobileNo + '", "customerEmail":"' + email + '", "cwiCode":"' + cwiCode + '", "versionId":"' + versionId + '", "cityId":"' + cityId + '", "branchId":"' + dealerId + '", "customerName":"' + Customername + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdateIsMobileVerified"); },
            success: function (response) {
                var responseJSON = eval('(' + response + ')');
                var resObj = eval('(' + responseJSON.value + ')');
                if (resObj == true) {
                    CallABApi();
                    //location.href = "/m/pricequote/DetailedDealerQuotation.aspx?dealerId=" + dealerId;
                }
                else {
                    alert("Invalid verification code.");
                }
            }
        });
    }
}


function ResendCode() {
    $.ajax({
        type: "POST",
        url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
        data: '{ "customerMobile":"' + mobileNo + '", "customerEmail":"' + email + '", "customerName":"' + Customername + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ResendVerificationCode"); },
        success: function (response) {
            var responseJSON = eval('(' + response + ')');
            var resObj = eval('(' + responseJSON.value + ')');
            if (resObj == true) {
                alert("You will shortly receive a verification code on your mobile number.");
                $("#txtCwi").focus();
            }
        }
    });
}


function RedirectToDealerPQ() {
    location.href = "/m/pricequote/DetailedDealerQuotation.aspx";
}

function CallABApi() {
    var json = '{\\"CustomerName\\":\\"' + Customername + '\\", \\"CustomerMobile\\":\\"' + mobileNo + '\\", \\"CustomerEmail\\":\\"' + email + '\\", \\"VersionId\\":\\"' + versionId + '\\", \\"CityId\\":\\"' + cityId + '\\", \\"InquirySourceId\\":\\"39\\", \\"Eagerness\\":\\"1\\",\\"ApplicationId\\":\\"2\\"}';
    $.ajax({
        type: "POST",
        url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
        data: '{"branchId":"'+ dealerId +'", "jsonInquiryDetails":"' + json + '","pqId":"' + pqId + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "PushInquiryInAB"); },
        success: function (response) {
            var responseJSON = eval('(' + response + ')');
            var resObj = eval('(' + responseJSON.value + ')');
           // if (response == true) {
                //redirect to dealer price quote page
                RedirectToDealerPQ();
          //  }
        },
        error: function (error) {
            RedirectToDealerPQ();
        }
    });
}