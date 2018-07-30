/*  Written By : Ashish G. Kamble on 21/8/2012  Summary : common js functions for sell bikes    */
var re = /^[0-9]*$/;
var reDecimal = /^[0-9]*?[\.]?[0-9]*?$/;
//var regAlpha = /^[a-zA-Z]*$/;
function valBikeDetails() {
    var isError = false;
    var alertObj = $("#alertObj");
    var drpVersion = $("#drpVersion");
    var drpModel = $("#drpModel");
    var drpMake = $("#drpMake");

    $("#msgYourBike").html(" ");
    $("#msgOwner").html(" ");
    $("#msgBikeColor").html(" ");
    $("#msgKms").html(" ");
    $("#msgRegNo").html(" ");
    $("#msgRegAt").html(" ");
    $("#msgLifeTax").html(" ");
    $("#msgBikeIns").html(" ");
    $("#msgValidTill").html(" ");
    $("#msgPrice").html(" ");

    if (drpVersion.prop("selectedIndex").toString() == "0") {
        $("#msgYourBike").html("Required!");

        if (drpMake.prop("selectedIndex").toString() == "0") {
            drpMake.focus();
        }
        else if (drpMake.prop("selectedIndex").toString() != "0"
						&& drpModel.prop("selectedIndex").toString() == "0") {
            drpModel.focus();
        }
        else if (drpMake.prop("selectedIndex").toString() != "0"
						&& drpModel.prop("selectedIndex").toString() != "0"
									&& drpVersion.prop("selectedIndex").toString() == "0") {
            drpVersion.focus();
        }

        isError = true;
    }

    if ($("#drpOwner").val() == "0") {
        $("#msgOwner").html("Required!");
        $("#drpOwner").focus();
        isError = true;
    }

    if ($("#txtColor").val() == "") {
        $("#msgBikeColor").html("Required!");
        $("#txtColor").focus();
        isError = true;
    }

    //if (regAlpha.test($("#txtColor").val()) == false) {
    //    $("#msgBikeColor").html("Color cannot be in Numeric Characters.");
    //    $("#txtColor").focus();
    //    isError = true;
    //}
      

    var txtKms = $("#txtKms");    
    if (txtKms.val() == "") {
        $("#msgKms").html("Required!");
        txtKms.focus();
        isError = true;
    } else if (isNaN(txtKms.val())) {
        $("#msgKms").html("Invalid kilometers. It should be numeric only");
        txtKms.focus();
        isError = true;
    } else if (txtKms.length > 7) {
        $("#msgKms").html("Invalid kilometers. It should be less than 7 digits.");
        txtKms.focus();
        isError = true;
    }

    if ($("#txtRegNo").val() == "") {
        $("#msgRegNo").html("Required!");
        $("#txtRegNo").focus();
        isError = true;
    }

    if ($("#txtRegAt").val() == "") {
        $("#msgRegAt").html("Required!");
        $("#txtRegAt").focus();
        isError = true;
    }

    if ($("#btnTaxI").is(":checked") == false && $("#btnTaxC").is(":checked") == false) {
        $("#msgLifeTax").html("Required!");
        $("#btnTaxI").focus();
        isError = true;
    }

    if ($("#rdoThirdParty").is(":checked") == false && $("#rdoComprehensive").is(":checked") == false && $("#rdoNoInsurance").is(":checked") == false) {
        $("#msgBikeIns").html("Required!");
        $("#rdoThirdParty").focus();
        isError = true;
    }

    var makeYear = parseInt($("#calMakeYear_txtYear").val());
    var insuranceYear = parseInt($("#calValidTill_txtYear").val());
    var makeMonth =parseInt( $("#calMakeYear_cmbMonth").val());
    var insuranceMonth = parseInt($("#calValidTill_cmbMonth").val());
    
    if (makeYear == insuranceYear)
    {
        if (makeMonth > insuranceMonth)
        {
            $("#msgValidTill").text("Invalid insurance expiry date.");
            isError = true;
        }
    }
    else if (makeYear > insuranceYear) {
        $("#msgValidTill").text("Invalid insurance expiry date.");
        isError = true;
    }
    else {
        $("#msgValidTill").text("");
    }


    if ($("#drpCities").prop("selectedIndex") == 0) {
        $("#msgCity").text("Required!");
        isError = true;
    } else {
        $("#msgCity").text("");
    }

    var txtPrice = $("#txtPrice");
    if (txtPrice.val() == "") {
        $("#msgPrice").html("Required!");
        txtPrice.focus();
        isError = true;
    } else if (isNaN(txtPrice.val())) {
        $("#msgPrice").html("Invalid price. It should be numeric only");
        txtPrice.focus();
        isError = true;
    } else if (txtPrice.val().length > 7) {
        $("#msgPrice").html("Price entered is more than max limit.");
        txtPrice.focus();
        isError = true;
    }

    $("#errComments").text("");
    comments = $("#txtComments").val();
    var arrLineBreak = comments.match(/\n/g);
    var commentsLength = comments.length;
    
    if (comments != "")
    {
        if (arrLineBreak != "" && arrLineBreak != null)
            commentsLength += arrLineBreak.length;
        if (commentsLength > 250) {
            $("#errComments").text("Exceeded maximum 250 character limit. Please remove some characters.");
            $("#txtComments").focus();
            isError = true;
        }        
    }

    $("#errModification").text("");
    modifications = $("#txtModifications").val();
    var modLength = modifications.length;

    if (modifications != "")
    {
        if (arrLineBreak != "" && arrLineBreak != null)
            modLength += arrLineBreak.length;
        if (modLength > 250)
        {
            $("#errModification").text("Exceeded maximum 250 character limit. Please remove some characters.");
            $("#txtModifications").focus();
            isError = true;
        }
    }

    $("#errWarranties").text("");
    warranties = $("#txtWaranties").val();
    var warLength = warranties.length;

    if (warranties != "")
    {
        if (arrLineBreak != "" && arrLineBreak != null)
            warLength += arrLineBreak.length;
        if (warLength > 250) {
            $("#errWarranties").text("Exceeded maximum 250 character limit. Please remove some characters.");
            $("#txtWaranties").focus();
            isError = true;
        }
    }

    return !isError
}

function validateUserDetails() {
    var isError = false;
    var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
    var mobile = $.trim($("#txtMobile").val());
    var email = $.trim($("#txtEmail").val());

    $("#msgName").text("");
    $("#msgEmail").text("");
    $("#msgMobile").text("");

    if ($("#txtName").val() == "") {
        $("#msgName").text("Required!");
        isError = true;
    }

    if (email == "") {
        $("#msgEmail").text("Required!");
        isError = true;
    } else if (!reEmail.test(email.toLowerCase())) {        
        $("#msgEmail").text("Invalid Email!");
        isError = true;
    }


    if (mobile == "") {
        $("#msgMobile").text("Required!");
        isError = true;
    } else if (mobile != "" && re.test(mobile) == false) {
        $("#msgMobile").text("Invalid Mobile");
        isError = true;
    } else if (mobile != "" && (!re.test(mobile) || mobile.length < 10 || mobile.length > 10)) {
        $("#msgMobile").text("Mobile number should be of 10 digits only!");
        isError = true;
    }

    if (isError == true) {
        return false;
    } else {
        if (document.getElementById('chkTerms').checked == false) {
            alert("You must agree to the terms & conditions before proceeding.");
            return false;
        } else {
            return true;
        }
    }
}

function maintainFormState() {
    var objMake = $("#drpMake");
    var objState = $("#drpStates");

    if (objMake.val().split('_')[0] != 0) {
        drpMake_Change(objMake);
    }

    if (objState.val() != 0) {
        drpState_Change(objState);
    }
}

function drpMake_Change(objMake) {
    var bikeMakeId = objMake.val().split('_')[0];
    $("#hdn_drpSelectedVersion").val("");
    $("#drpVersion").val("0").attr("disabled", true);
    if (bikeMakeId != 0) {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
            data: '{"requestType":"USED", "makeId":"' + bikeMakeId + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
            success: function (response) {
                //alert(response);
                var responseJSON = eval('(' + response + ')');
                var resObj = eval('(' + responseJSON.value + ')');

                var dependentCmbs = new Array();

                bindDropDownList(resObj, drpModel, "", dependentCmbs, "--Select Model--");
            }
        });
    } else {
        $("#drpModel").val("0").attr("disabled", true);        
    }
}

function drpModel_Change(objModel) {
    var bikeModelId = objModel.val();
    $("#hdn_drpSelectedVersion").val("");
    if (bikeModelId != 0) {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
            data: '{"requestType":"USED", "modelId":"' + bikeModelId + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetVersions"); },
            success: function (response) {
                var responseJSON = eval('(' + response + ')');
                var resObj = eval('(' + responseJSON.value + ')');

                var dependentCmbs = new Array();

                bindDropDownList(resObj, drpVersion, "", dependentCmbs, "--Select Version--");
            }
        });
    } else {
        $("#drpVersion").val("0").attr("disabled", true);
    }
}

function drpState_Change(objState) {
    var bikeStateId = objState.val();
    $("#hdn_drpSelectedCity").val("");

    if (bikeStateId != 0) {
        $.ajax({            
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
            data: '{"requestType":"7", "stateId":"' + bikeStateId + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetCities"); },
            success: function (response) {
                var responseJSON = eval('(' + response + ')');
                var resObj = eval('(' + responseJSON.value + ')');
                var dependentCmbs = new Array();
                bindDropDownList(resObj, drpCities, "", dependentCmbs, "--Select City--");
            }
        });
    } else {
        $("#drpCities").val("0").attr("disabled", true);
    }
}

function deleteImg(photoId) {
    if (confirm("Are you sure want to delete this photo?")) {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxSellBike,Bikewale.ashx",
            data: '{"inquiryId":"' + inquiryId + '", "photoId":"' + photoId + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "RemoveBikePhotos"); },
            success: function (response) {
                responseObj = eval('(' + response + ')');
                if (responseObj.value) {
                    $("#" + photoId).fadeOut(500, function () { $(this).remove(); decreasePhotoCount(); });                    
                } else { // unsuccessfull
                    alert("Unable to delete this file");
                }
            }
        });
    }
}

function decreasePhotoCount()
{
    var initPhotoCount = $("#spnPhotoCount").text();
    
    if (initPhotoCount >= 0) {
        $("#spnPhotoCount").text(--initPhotoCount);
    }
}

function makeMainImg(photoId) {
    $.ajax({
        type: "POST",
        url: "/ajaxpro/Bikewale.Ajax.AjaxSellBike,Bikewale.ashx",
        data: '{"inquiryId":"' + inquiryId + '", "photoId":"' + photoId + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "MakeMainImage"); },
        success: function (response) {            
            responseObj = eval('(' + response + ')');
            if (responseObj.value) {

            }
        }
    });
}

function mDone() {    
    $("div.img-preview").each(function (index) {
        var photoId = $(this).attr("id");
        var desc = $("#desc" + photoId).val();
        if (desc != "Describe this image here" && desc != "") {
            requestCount++;
            addDescription(photoId, desc);
        }
    });

    if (requestCount == 0)
        setTimeout('nextStep()', 1000);
}

function addDescription(photoId, imgDesc) {
    $.ajax({
        type: "POST",
        url: "/ajaxpro/Bikewale.Ajax.AjaxSellBike,Bikewale.ashx",
        data: '{"photoId":"' + photoId + '", "imgDesc":"' + imgDesc + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "AddImageDescription"); },
        success: function (response) {
            responseObj = eval('(' + response + ')');

            if (responseObj.value) {
                responseCount++;
                if (requestCount == responseCount) {
                    setTimeout('nextStep()', 1000);
                }
            } else { // unsuccessfull
                alert("Unable to add description");
            }
        }
    });
}

function nextStep() {
    if (nextStepUrl != "") {
        window.location.href = nextStepUrl;
    } else {
        alert("You have added description to the images successfully.");
    }
}

function clearTextArea(objTextArea) {
    if ($(objTextArea).html() == "Describe this image here")
    {
        $(objTextArea).html("");
    }    
}

function msgTextArea(objTextArea) {
    objJQ = $(objTextArea);
    if (objJQ.html() == "" || objJQ.html() == "Describe this image here")
        objJQ.html("Describe this image here");
}