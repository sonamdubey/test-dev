//function ddlMake_Change(e) {    
//    showLoading('ddlModel');
//    var makeId = document.getElementById("ddlMake").value;
//    var dependentCmbs = new Array;

//    if (Number(makeId) > 0) {
//        $.ajax({
//            type: "POST", url: "/ajaxpro/CarwaleAjax.AjaxPQ,CarwaleAjax.ashx",
//            data: '{"makeId":"' + makeId + '"}',
//            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetNewModelsPrices"); },
//            success: function (response) {
//                var jsonString = eval('(' + response + ')');
//                var resObj = eval('(' + jsonString.value + ')');


//                dependentCmbs[0] = "ddlVersion";
//                dependentCmbs[1] = "drpCity";

//                bindDropDownList(resObj, document.getElementById("ddlModel"), 'hdn_ddlModel', dependentCmbs, '--Select Model--');
//            }
//        });
//    } else {
//        dependentCmbs[0] = "ddlModel";
//        dependentCmbs[1] = "ddlVersion";
//        dependentCmbs[2] = "drpCity";
//        resetDependentFields(dependentCmbs);
//        $("#sponsored-car").show();
//        $("#model_img_container").hide();
//    }
//    $("#divPQTD").hide();
//}

//function ddlModel_Change(e) {
//    showLoading('ddlVersion');
//    var modelId = document.getElementById("ddlModel").value;
//    var cmb = document.getElementById("ddlVersion");
//    var dependentCmbs = new Array;
//    $("#divPQTD").hide();
//    if (Number(modelId) > 0) {
//        $.ajax({
//            type: "POST", url: "/ajaxpro/CarwaleAjax.AjaxPQ,CarwaleAjax.ashx",
//            data: '{"modelId":"' + modelId + '"}',
//            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetNewVersions"); },
//            success: function (response) {
//                var jsonString = eval('(' + response + ')');
//                var resObj = eval('(' + jsonString.value + ')');
                
//                dependentCmbs[0] = "drpCity";
//                bindDropDownList(resObj, document.getElementById("ddlVersion"), 'hdn_ddlVersion', dependentCmbs, '--Select Version--');

//                if (toString(verId) != "") {
//                    for (var i = 0; i < cmb.options.length; i++) {
//                        if (cmb.options[i].value == verId) {
//                            cmb.options[i].selected = true;
//                            break;
//                        }
//                    }
//                }
//                FillCity();

//                if (modelId != "") {
//                    showModelImage(modelId);
//                }
//            }
//        });
//    } else {
//        dependentCmbs[0] = "ddlVersion";
//        dependentCmbs[1] = "drpCity";
//        resetDependentFields(dependentCmbs);
//    }  
//}

function showModelImage(modelId) {    
    var img_obj = $("#model-img");
    $("#sponsored-car").hide();
    $("#model_img_container").show();
    img_obj.attr("src", "https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/loader.gif"); //.removeClass("img-border").addClass("img-border");
    $.ajax({
        type: "POST", url: "/ajaxpro/CarwaleAjax.AjaxPQ,CarwaleAjax.ashx",
        data: '{"modelId":"' + modelId + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModelImage"); },
        success: function (response) {
            var objResponse = eval('(' + response + ')');
            if (objResponse.value != "") {
                $("#sponsored-car").hide();
                img_obj.attr("src", objResponse.value); //.removeClass("img-border").addClass("img-border");
            } else {
                $("#model_img_container").hide();
                $("#sponsored-car").show();         
            }
        }
    });
}

function drpCity_Change() {
    var cityName = document.getElementById("drpCity").options[document.getElementById("drpCity").selectedIndex].text;
    document.getElementById("hdn_drpCityName").value = cityName;
    CheckTestDriveAvailability();
}

function drpBuyTime_Change() {
    document.getElementById("rdoBuyTime").checked = true;
}

function rdoResearh_Change() {
    document.getElementById("drpBuyTime").options[0].selected = true;
}

function FillCity() {
    var modelId = document.getElementById("ddlModel").value;
    var cmb = document.getElementById("drpCity");
    if (modelId != "" && modelId != "0") {
        var ldType = "1";
        showLoading('drpCity');
        $.ajax({
            type: "POST", url: "/ajaxpro/CarwaleAjax.AjaxPQ,CarwaleAjax.ashx",
            data: '{"modelId":"' + modelId + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetNewCarPriceCities"); },
            success: function (response) {
                var jsonString = eval('(' + response + ')');
                var resObj = eval('(' + jsonString.value + ')');
                var dependentCmbs = new Array;
                dependentCmbs[0] = "drpDealer";
                bindDropDownList(resObj, document.getElementById("drpCity"), 'hdn_drpCity', '', '--Select City--');

                if (citId != "") {
                    for (var i = 0; i < cmb.options.length; i++) {
                        if (cmb.options[i].value == citId) {
                            cmb.options[i].selected = true;
                            var cityName = document.getElementById("drpCity").options[document.getElementById("drpCity").selectedIndex].text;
                            document.getElementById("hdn_drpCityName").value = cityName;
                            CheckTestDriveAvailability();
                            break;
                        }
                    }
                }
            }
        });
    }
}

function resetDependentFields(arrDependencies) {
    if (arrDependencies && arrDependencies.length > 0) {
        for (var i = 0; i < arrDependencies.length; i++) {
            $("#" + arrDependencies[i]).empty().attr("disabled", true);
        }
    }
}

function isValidated() {
    var re = /^[0-9]*$/;
    var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;

    var isError = false;

    var version = $("#ddlVersion");
    var name = $("#txtName").val();
    var email = $.trim($("#txtEmail").val());
    var mobile = $.trim($("#txtMobile").val());
    var stdCode = $.trim($("#txtStdCode").val());
    var phone = $.trim($("#txtLandline").val());
    var city = $("#ddlCity");
    var area = $("#ddlArea");
    var rdoResearch = $("#rdoResearching");

    var emailMsg = $("#errEmail");
    var nameMsg = $("#errName");
    var contMsg = $("#errMobile");
    var verMsg = $("#spnVersion");
    var cityMsg = $("#spnCity");
    var areaMsg = $("#spnArea");
    var buyTimeMsg = $("#spnBuyTime");
    var spnPhone = $("#landline");

    if (email == "") {
        emailMsg.text("Required");
        isError = true;
    } else if (!reEmail.test(email.toLowerCase())) {
        emailMsg.text("Invalid Email");
        isError = true;
    } else {
        emailMsg.text("");
    }
    
    if (version.val() <= 0 || version.val() == "" || version.prop("disabled")) {
        verMsg.text("Please select bike version.");
        isError = true;
    } else {
        verMsg.text("");
    }

    if (city.val() <= 0 || city.val() == "" || city.prop("disabled")) {
        cityMsg.text("Required");
        isError = true;
    } else {
        cityMsg.text("");
    }
    if (city.val() == 1 || city.val() == 13 || city.val() == 40) {
        if (area.val() <= 0 || area.val() == "" || area.prop("disabled")) {
            areaMsg.text("Required");
            isError = true;
        } else {
            areaMsg.text("");
        }
    }

    if (name == "") {
        nameMsg.text("Required");
        isError = true;
    } else if (name.length == 1) {
        nameMsg.text("Please enter your complete name");
        isError = true;
    } else {
        nameMsg.text("");
    }

    if (mobile == "") {
        contMsg.text("Required");
        isError = true;
    } else if (mobile != "" && re.test(mobile) == false) {
        contMsg.text("Please provide numeric data only in your mobile number.");
        isError = true;
    } else if (mobile.length != 10) {
        contMsg.text("Your mobile number should be of 10 digits.");
        isError = true;
    } else {
        contMsg.text("");
    }

    if (!isBuyPrefChecked()) {
        buyTimeMsg.text("Please tell us your buying preferences.");
        isError = true;
    } else {
        buyTimeMsg.text("");
    }

    if (!isError) {
        if (!$("#userAgreement").prop("checked")) {
            alert("You must be agree with BikeWale visitor agreement and privacy policy.");
            isError = true;
        }
    }


    if (isError == true) {
        return false;
    } else {
        return true;
    }
}

function CheckName() {
    var name = $("#txtName").val();
    var nameMsg = $("#errName");
       
    if (name == "") {
        nameMsg.text("Required");        
    } else if (name.length == 1) {
        nameMsg.text("Please enter your complete name");        
    } else {
        nameMsg.text("");
    }
}

function CheckEmail() {
    var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
    var email = $.trim($("#txtEmail").val());
    var emailMsg = $("#errEmail");    

    if (email == "") {
        emailMsg.text("Required");
    } else if (!reEmail.test(email.toLowerCase())) {
        emailMsg.text("Invalid Email");
    } else {
        emailMsg.text("");
    }
}

function CheckMobile() {
    var re = /^[0-9]*$/;
    var mobile = $.trim($("#txtMobile").val());
    var contMsg = $("#errMobile");

    if (mobile == "") {
        contMsg.text("Required");
    } else if (mobile != "" && re.test(mobile) == false) {
        contMsg.text("Please provide numeric data only in your mobile number.");
    } else if (mobile.length != 10) {
        contMsg.text("Your mobile number should be of 10 digits.");
    } else {
        contMsg.text("");
    }
}

function CheckLandline() {
    var re = /^[0-9]*$/;
    var stdCode = $.trim($("#txtStdCode").val());
    var phone = $.trim($("#txtLandline").val());
    var spnPhone = $("#landline");

    if (stdCode != "" || phone != "") {
        if (stdCode != "" && phone == "") {
            if (!re.test(stdCode)) {
                spnPhone.text("STD code can only be numeric.");
            } else if (stdCode.length > 4 || stdCode.length < 2) {
                spnPhone.text("STD code has minimum 2 digits and maximum 4 digits.");
            } else {
                spnPhone.text("Enter Landline number.");
            }
        } else if (stdCode == "" && phone != "") {
            if (!re.test(phone)) {
                spnPhone.text("Phone number can only be numeric.");
            } else {
                spnPhone.text("Enter STD code.");
            }
        } else if ((stdCode != "" && phone != "")) {
            if (!re.test(stdCode) || !re.test(phone)) {
                spnPhone.text("The STD code and number have to be numeric.");
            } else if ((stdCode.length + phone.length) > 11 || (stdCode.length + phone.length) < 10) {
                spnPhone.text("The STD code and number entered by you has less digits than required.");
            }
            else {
                spnPhone.text("");
            }
        } else {        
            spnPhone.text("");
        }
    } else {
        spnPhone.text("");
    }
}

function isBuyPrefChecked() {
    var isChecked = false;
    //$('#buy_pref input:radio').each(function () {
    //    if ($(this).attr("checked") && !isChecked) {
    //        isChecked = true;
    //    }
    //});

    if ($("#hdnBuyTimeSelected").val() != "")
    {
        isChecked = true;
    }
    return isChecked;
}

function buyTimeSelected(e) {
    if (!document.getElementById("drpBuyTime").options[0].selected) {
        document.getElementById("rdbBuyTime").checked = true;
    } else {
        document.getElementById("rdbBuyTime").checked = false;
    }
}
