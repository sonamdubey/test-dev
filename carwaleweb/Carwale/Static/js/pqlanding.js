var NewPricemodelId = null;
var objNewPrice = new Object();
var focusedCity, carDetails;

$(document).ready(function () {
    $('#pqCarSelect').val("");
    if (isCookieExists('_userModelHistory')) {
        prefilUserModel();
    }

    $('#btnGetPQ').on('click', function () {
        $('#pqCarSelect').attr('placeholder') == $('#pqCarSelect').val() ? NewPricemodelId = null : "";
        var selectedCity = $('#drpPQCity Option:selected');
        var selectedCityId = selectedCity.val();
        if (Number(selectedCityId) > 0 && Number(NewPricemodelId) > 0) {
            var selectedCityName = selectedCity.text();
            var makeModelMaskingName = $("#pqCarSelect").attr("make-model");

            if (!isCookieExists("_CustCityIdMaster") || ($.cookie("_CustCityIdMaster") != selectedCityId)) {
                Location.globalSearch.setLocationCookies(selectedCityId, selectedCityName, "", "Select Area");
            }

            Common.autoComplete.redirectToModelPage(makeModelMaskingName);
        } else {
            ShakeFormView($(".pqlanding-form-container .form-control-box"));
            displayErrors(selectedCityId);
        }
    });

    $('#pqCarSelect').focus();
});

$('#pqCarSelect').cw_autocomplete({
    isPriceExists: 1,
    resultCount: 5,
    textType: ac_textTypeEnum.model,//+ ',' + ac_textTypeEnum.version,
    source: ac_Source.generic,
    onClear: function () {
        objNewPrice = new Object();
    },
    click: function (event, ui, orgTxt) {
        $('#pqCarSelect').attr("make-model", ui.item.id);
        simulatedClick(ui.item.label, ui.item.id.split(':')[2]);
    },
    open: function (result) {
        objNewPrice.result = result;
    },
    keyup: function () {
        objNewPrice.Name = label;
        objNewPrice.Id = id;
        showHideDrpError($('#pqCarSelect').nextAll(), false);
        showHideDrpError($("#drpPQCity").nextAll(), false);
    },
    focusout: function () {
        if ($('li.ui-state-focus a:visible').text() != "") {
            focusedCity = objNewPrice.result[$('li.ui-state-focus').index()];
            NewPricemodelId = focusedCity.id.split(':')[2];
            bindModelCity(NewPricemodelId, 'drpPQCity', bindModelCityCallBack);
            showHideDrpError($('#pqCarSelect').nextAll(), false);
            showHideDrpError($("#drpPQCity").nextAll(), false);
            $("#drpPQCity").prop('disabled', false);
            $('#drpPQCity').removeClass('border-red');
        }
    }
});

function autoOpenCityDrp() {
    if ($('#drpPQCity').val() <= 0)
        $('#drpPQCity').openSelect();
}

function showPrefilLoading() {
    $('#blackOut-window-pq').show();
    $('#loadingCarImg').show();
}

function hidePrefilLoading() {
    $('#blackOut-window-pq').hide();
    $('#loadingCarImg').hide();
}

function bindModelCity(selectedModelId, drpId, callback) {
    $('#' + drpId).empty();
    $.ajax({
        type: 'GET',
        url: '/webapi/GeoCity/GetPQCitiesByModelId/?modelid=' + selectedModelId,
        dataType: 'Json',
        success: function (json) {
            var viewModel = {
                pqCities: ko.observableArray(json)
            };
            ko.cleanNode(document.getElementById(drpId));
            ko.applyBindings(viewModel, document.getElementById(drpId));
            ModelCar.PQ.bindZones('', '#' + drpId);
            if (json.length > 0) {
                $('#' + drpId).removeAttr('disabled');
                $('#' + drpId).removeClass('btn-disable');
            }
            // make select header disabled
            $("#" + drpId).prepend('<option value=-1>Select City</option>');
            $("#" + drpId + " option[value=" + -1 + "]").attr('disabled', 'disabled');
            $("#" + drpId + " option[value=" + -2 + "]").attr('disabled', 'disabled');
            $('#' + drpId).val("-1");

            if (typeof (callback) == "function") callback();
        }
    });
}

function showHideDrpError(element, error) {
    if (error) {
        $(element[0]).removeClass('hide');
        $(element[1]).removeClass('hide');
    }
    else {
        $(element[0]).addClass('hide');
        $(element[1]).addClass('hide');
    }
}

function checkForCity(cityId, drpCity) {
    if ($("#" + drpCity + " option[value='" + cityId + "']").length > 0)
        return true;
    else
        return false;
}

function CookieCityValidForModel() {
    if ($("#drpPQCity option[value=" + $.cookie("_CustCityId") + "]").length > 0) return true;
    return false;
}

function bindModelCityCallBack() {
    ModelCar.PQ.preselectPQDropDown('drpPQCity');
    autoOpenCityDrp();
}

function simulatedClick(label, id) {
    objNewPrice.Name = formatSpecial(label);
    objNewPrice.Id = id;
    NewPricemodelId = id;
    $('#pqCarSelect').removeClass('border-red');
    $('#drpPQCity').removeClass('border-red');
    $('#pqCarSelect').val(label);
    bindModelCity(NewPricemodelId, 'drpPQCity', bindModelCityCallBack);
    $("#drpPQCity").prop('disabled', false);
    showHideDrpError($('#pqCarSelect').nextAll(), false);
    showHideDrpError($("#drpPQCity").nextAll(), false);
}

$(document).keydown(function (e) {
    // ESCAPE key pressed
    if (e.keyCode == 27) {
        var faq = $("#faqPopUpContainer");
        faq.removeClass("show").addClass("hide");
        Common.utils.unlockPopup();
    }
});

function getDetails() {
    var url = '/webapi/CarModelData/GetCarDetailsByModelId/?modelid=' + window.modelId
    $.ajax({
        type: 'GET',
        url: url,
        async: false,
        dataType: 'json',
        success: function (json) { carDetails = json; window.modelId = carDetails.ModelId; }
    });
}

function prefilUserModel() {
    showPrefilLoading();
    var recentModelCookie = $.cookie('_userModelHistory').split('~');
    NewPricemodelId = window.modelId = recentModelCookie[recentModelCookie.length - 1];
    getDetails();
    if (carDetails.New) {
        prefillMakeModelName();
        bindModelCity(NewPricemodelId, 'drpPQCity', bindModelCityCallBack, false);
    }
        hidePrefilLoading();
}

function prefillMakeModelName() {
    var makeModelMaskingName = Common.utils.formatSpecial(carDetails.MakeName).toLowerCase() + ":" + carDetails.MakeId + "|"
                                + carDetails.MaskingName
                                + ":" + carDetails.ModelId;
    var makeModelInput = $("#pqCarSelect");

    makeModelInput.val(carDetails.MakeName + " " + carDetails.ModelName);
    makeModelInput.attr("make-model", makeModelMaskingName);
}

function redirectToPQ() {
    $.cookie('_PQModelId', NewPricemodelId, { path: '/' });
    $.cookie('_PQVersionId', (typeof (window.versionId) != "undefined" ? window.versionId : "0"), { path: '/' });
    $.cookie('_PQPageId', "1", { path: '/' });
    setCityCookie('drpPQCity');
    window.location.href = (window.location.pathname.indexOf('/m/') == 0 ? "/m/research/quotation.aspx" : "/new/quotation.aspx");

}

function displayErrors(selectedCityId) {
    if (NewPricemodelId == null || $('#pqCarSelect').val() == "") {
        $('.brand-error-icon').removeClass('hide');
        $('.brand-err-msg').removeClass('hide');
        $('#pqCarSelect').addClass('border-red');
    }
    else {
        $('.brand-error-icon').addClass('hide');
        $('.brand-err-msg').addClass('hide');
        $('#pqCarSelect').removeClass('border-red');
    }
    if (selectedCityId === undefined || selectedCityId == "-1") {
        $('.city-error-icon').removeClass('hide');
        $('.city-err-msg').removeClass('hide');
        $('#drpPQCity').addClass('border-red');
    }
    else {
        $('.city-error-icon').addClass('hide');
        $('.city-err-msg').addClass('hide');
        $('#drpPQCity').removeClass('border-red');
    }
}