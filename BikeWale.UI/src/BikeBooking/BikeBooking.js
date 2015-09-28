$(document).ready(function (e) {
    //$('.bw-popup, #blackOut-window, .bw-contact-popup').hide();
    $('#btnGetDealerDetails, #dealerPriceQuote').click(function () {

        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
            data: '{"pqId":"' + pqId + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "IsNewBikePQExists"); },
            success: function (response) {
                var responseJSON = eval('(' + response + ')');
                var resObj = eval('(' + responseJSON.value + ')');
                if (resObj == false) {
                    $(".blackOut-window").show();
                    $('.bw-contact-popup').show();
                    $("#txtName").focus();
                }
                else
                    location.href = "/pricequote/DetailedDealerQuotation.aspx";
            }
        });
    });

    $("#txtName").val(Customername);
    $("#txtMobile").val(mobileNo);
    $("#txtEmail").val(email);
    $('#btnNext').click(function () {
        if (validateDetails()) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
                data: '{"dealerId":"' + dealerId + '", "pqId":"' + pqId + '", "customerName":"' + Customername + '", "customerMobile":"' + mobileNo + '", "customerEmail":"' + email + '", "versionId":"' + versionId + '", "cityId":"' + cityId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SaveCustomerDetail"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    if (resObj == false) {
                        $('.verify-popup').show();
                        $('.bw-contact-popup').hide();
                        $("#mobNo").text(mobileNo);
                        $("#txtCwi").focus();
                    }
                    else {
                        CallABApi();
                        //location.href = "/pricequote/DetailedDealerQuotation.aspx?dealerid=" + dealerId;
                    }
                }
            });

        }
        return false;
    });
    $('.close-btn').click(function () {
        closeContactPopup();
    });

    $(document).keydown(function (e) {
        // ESCAPE key pressed
        if (e.keyCode == 27)
            closeContactPopup();
    });

    $(".blackOut-window").click(function () {
        closeContactPopup();
    });

    function closeContactPopup() {
        $(".blackOut-window").hide();
        $('.bw-contact-popup').hide();
        $('.bw-popup').hide();
    };

    $(".edit-done-mob").hide();
    $("#editNum").click(function () {
        $(".edit-mob").hide();
        $(".edit-done-mob").show();
        $("#editedMobNo").focus();
    });
    $("#done-btn").click(function () {
        if (UpdateMobile()) {
            $(".edit-mob").show();
            $(".edit-done-mob").hide();
            $("#mobNo").text($("#editedMobNo").val());
            mobileNo = $("#editedMobNo").val();

            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
                data: '{"pqId":"' + pqId + '", "customerName":"' + Customername + '", "customerMobile":"' + mobileNo + '", "customerEmail":"' + email + '", "versionId":"' + versionId  + '", "cityId":"' + cityId + '", "branchId":"' + dealerId + '"}',
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
                        //location.href = "/pricequote/DetailedDealerQuotation.aspx?dealerId=" + dealerId;
                    }
                    
                }
            });
        }
    });


    $("#resendCwiCode").click(function () {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
            data: '{ "customerMobile":"' + mobileNo + '", "customerEmail":"' + email + '", "customerName":"' + Customername + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ResendVerificationCode"); },
            success: function (response) {
                if (response) {
                    alert("You will shortly receive a verification code on your mobile number.");
                    $("#txtCwi").focus();
                }
            }
        });
    });

    $("#btnSavePriceQuote").click(function () {
        var cwiCode = $("#txtCwi").val();
        //alert(pqId + ":" + mobileNo + ":" + email + ":" + cwiCode);
        if (VerifyCustomer(cwiCode)) {
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
                        //location.href = "/pricequote/DetailedDealerQuotation.aspx?dealerid=" + dealerId;
                    }
                    else {
                        $("#spnCwi").text("Invalid verification code.");
                    }
                }
            });
        }
    });
});

function validateDetails() {

    Customername = $("#txtName").val();
    email = $("#txtEmail").val().toLowerCase();
    mobileNo = $("#txtMobile").val();
    $("#spnName").text("");
    $("#spnMobile").text("");
    $("#spnEmail").text("");

    //alert(name + " " + email + " " + mobileNo);

    var retVal = true;

    var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
    var reMobile = /^[0-9]*$/;
    var name = /^[a-zA-Z& ]+$/;

    // $("#hdnMobile").val($("#numMobile").val());

    if (Customername == "") {
        retVal = false;
        $("#spnName").text("Please enter your Name");
    }
    else if (!name.test(Customername)) {
        retVal = false;
        $("#spnName").text("Please enter valid Name");
    }

    if (email == "") {
        retVal = false;
        $("#spnEmail").text("Please enter your Email");
    }
    else if (!reEmail.test(email)) {
        retVal = false;
        $("#spnEmail").text("Invalid Email");
    }

    if (mobileNo == "") {
        retVal = false;
        $("#spnMobile").text("Please enter your Mobile Number");
    }
    else if (!reMobile.test(mobileNo)) {
        retVal = false;
        $("#spnMobile").text("Mobile Number should be numeric");
    }
    else if (mobileNo.length != 10) {
        retVal = false;
        $("#spnMobile").text("Mobile Number should be of 10 digits");
    }
    return retVal;
}

function UpdateMobile() {
    var retVal = true;
    var reMobile = /^[0-9]*$/;

    var CustomerMobile = $("#editedMobNo").val();
    $("#spnEditNo").text("");

    var errorMsg = "";
    if (CustomerMobile == "") {
        retVal = false;
        $("#spnEditNo").text("Please enter your Mobile Number");
    }
    else if (!reMobile.test(CustomerMobile)) {
        retVal = false;
        $("#spnEditNo").text("Mobile Number should be numeric");
    }
    else if (CustomerMobile.length != 10) {
        retVal = false;
        $("#spnEditNo").text("Mobile Number should be of 10 digits");
    }
    return retVal;
}

function VerifyCustomer(cwiCode) {
    var retVal = true;
    var isNumber = /^[0-9]*$/;
    $("#spnCwi").text("");

    if (cwiCode == "") {
        retVal = false;
        $("#spnCwi").text("Please enter your Verification Code");
    }
    else if (!isNumber.test(cwiCode)) {
        retVal = false;
        $("#spnCwi").text("Verification Code should be numeric");
    }
    else if (cwiCode.length != 5) {
        retVal = false;
        $("#spnCwi").text("Verification Code should be of 5 digits");
    }
    return retVal;
}

function RedirectToDealerPQ() {
    location.href = "/pricequote/DetailedDealerQuotation.aspx";
}

function CallABApi()
{
    var json = '{\\"CustomerName\\":\\"' + Customername + '\\", \\"CustomerMobile\\":\\"' + mobileNo + '\\", \\"CustomerEmail\\":\\"' + email + '\\", \\"VersionId\\":\\"' + versionId + '\\", \\"CityId\\":\\"' + cityId + '\\", \\"InquirySourceId\\":\\"39\\", \\"Eagerness\\":\\"1\\",\\"ApplicationId\\":\\"2\\"}';
    $.ajax({
        type: "POST",
        url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
        data: '{"branchId":"'+ dealerId +'", "jsonInquiryDetails":"' + json + '","pqId":"'+ pqId +'"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "PushInquiryInAB"); },
        success: function (response) {
            var responseJSON = eval('(' + response + ')');
            var resObj = eval('(' + responseJSON.value + ')');
            //if (response == true) {
                //redirect to dealer price quote page
                RedirectToDealerPQ();
            //}
        },
        error: function (error) {
            RedirectToDealerPQ();
        }
    });
}

