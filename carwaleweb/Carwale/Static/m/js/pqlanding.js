selectedModelId = null;
var focusedCity;
var prefillEnum = new Object();
prefillEnum.nofill = 0;
prefillEnum.bymodel = 1;
prefillEnum.byversion = 2;
var prefill = prefillEnum.nofill;
showPrefilLoading();

objNewPrice = new Object();

var pqCarSelectInputField = $('#pqCarSelect');

$(pqCarSelectInputField).cw_easyAutocomplete({
    inputField: pqCarSelectInputField,
    isPriceExists: 1,
    resultCount: 5,
    textType: ac_textTypeEnum.model,//+ ',' + ac_textTypeEnum.version,
    source: ac_Source.generic,
    onClear: function () {
        objNewPrice = {};
    },

    click: function (event) {
        suggestions = {
            position: pqCarSelectInputField.getSelectedItemIndex() + 1,
            count: objNewPrice.result.length
        };
        var selectionValue = pqCarSelectInputField.getSelectedItemData().value,
            selectionLabel = pqCarSelectInputField.getSelectedItemData().label;

        var carData = selectionValue.split(':');
        simulatedClick(selectionLabel, selectionValue.split(':')[2]);

        if (selectionValue.indexOf('sponsor') > 0) {
            var sponsorLabel = globalSearchAdTracking.featuredModelName + '_' + globalSearchAdTracking.targetModelName + '_' + suggestions.count + "_" + suggestions.position;            
            trackGlobalSearchAd('New_m_Click', sponsorLabel, 'SearchResult_Ad_PQ_m');
            cwTracking.trackCustomData('SearchResult_Ad_PQ_m', 'New_m_Click', sponsorLabel, false);
        }
        if (isEligibleForORP) {
            setRedirectUrl(carData);
        }
    },

    afterFetch: function (result, searchText) {
        objNewPrice.result = result;

        if ((result.filter(function (suggestion) { return suggestion.value.toString().indexOf('sponsor') > 0 })).length > 0) {
            globalSearchAdTracking.trackData(result, 'SearchResult_Ad_PQ_m');
        }
        else {
            globalSearchAdTracking.featuredModelIdPrev = 0;
        }
    },

    keyup: function () {
        objNewPrice.Name = label;
        objNewPrice.Id = id;
        showHideDrpError($(pqCarSelectInputField).closest('.easy-autocomplete').nextAll(), false);
        showHideDrpError($("#drpCity_").nextAll(), false);
    }
});

var CityId = "-1";
var ZoneId = "";
var drpCity;

$(document).ready(function () {
    Common.showCityPopup = false;
    drpCity = "#drpCity_";
    // $("#drpCity_").disable();
    $("#drpCity_").text("Select City");
    $('#pqCarSelect').val("");

    if (getQueryStringParam("versionId") !== "" && $.isNumeric(getQueryStringParam("versionId"))) {
        window.versionId = getQueryStringParam("versionId");
        prefill = prefillEnum.byversion;
        $("#drpCity_").removeClass('disable-div');
    }
    else if (getQueryStringParam("modelId") !== "" && $.isNumeric(getQueryStringParam("modelId"))) {
        window.modelId = getQueryStringParam("modelId");
        $("#drpCity_").removeClass('disable-div');
        prefill = prefillEnum.bymodel;
    }
    if (getQueryStringParam("cityid") !== "" && $.isNumeric(getQueryStringParam("cityid")))
        window.cityId = getQueryStringParam("cityid");

    if (prefill > 0) {
        showPrefilLoading();
        getDetails();
        $("#pqCarSelect").val(carDetails.MakeName + " " + carDetails.ModelName);
        simulatedClick(carDetails.MakeName + " " + carDetails.ModelName, window.modelId);
    }
    else {
        hidePrefilLoading();
    }

    $("a.FAQsLink").click(function (e) {
        e.preventDefault();
        faqPopupShow();
    });

    $(".blackOut-window").mouseup(function (e) {
        var faq = $("#faqPopUpContainer");
        if (e.target.id !== faq.attr('id') && !faq.has(e.target).length) {
            faq.removeClass("show").addClass("hide");
            unlockPopup();
        }
    });

    $('#btnGetPQ').on('click', function (e) {
        $('#pqCarSelect').attr('placeholder') == $('#pqCarSelect').val() ? selectedModelId = null : "";
        var selectedCityId = CityId;
        var globalCityId = Number($.cookie("_CustCityIdMaster"));
        var globalZoneId = Number($.cookie("_CustZoneIdMaster"));
        var isCityZoneCombinationValid = ((globalCityId != 1 && globalCityId != 10) || globalZoneId > 0 || isEligibleForORP);
        selectedModelId = (selectedModelId.indexOf('|') > -1) ? selectedModelId.substring(0, selectedModelId.indexOf('|')) : selectedModelId;
        if (!isNaN(selectedCityId) && !isNaN(selectedModelId) && selectedModelId != null && selectedCityId != "-1" && isCityZoneCombinationValid) {
            $.cookie('_PQModelId', selectedModelId, { path: '/' });
            $.cookie('_PQVersionId', (typeof (window.versionId) != "undefined" ? window.versionId : "0"), { path: '/' });
            $.cookie('_PQPageId', "2", { path: '/' });
            setCityCookie($('#drpCity_'));
            if (!isEligibleForORP) {
                window.location.href = (window.location.pathname.indexOf('/m/') == 0 ? "/m/research/quotation.aspx" : "/new/quotation.aspx");
            }
            else {
                Common.utils.trackAction('CWInteractive', 'ShowORPTest', "CORPPage_BtnClick", $('#pqCarSelect').val() + "-" + $.cookie("_CustCityMaster"));
                location.href = $(this).attr("data-redirect-url");
              }
            } else {
            if (selectedModelId == null || $('#pqCarSelect').val() == "") {
                $('.brand-error-icon').removeClass('hide');
                $('.brand-err-msg').removeClass('hide');
                $('#pqCarSelect').addClass('border-red');
            }
            else {
                $('.brand-error-icon').addClass('hide');
                $('.brand-err-msg').addClass('hide');
                $('#pqCarSelect').removeClass('border-red');
            }
            if (selectedCityId === undefined || (selectedCityId == "-1" && $("#drpCity_").val() != "Select City") || !isCityZoneCombinationValid) {
                $('.city-error-icon').removeClass('hide');
                $('.city-err-msg').removeClass('hide');
                $('#drpCity_').addClass('border-red');
            }
            else {
                $('.city-error-icon').addClass('hide');
                $('.city-err-msg').addClass('hide');
                $('#drpCity_').removeClass('border-red');
            }
        }
    });
    if (!$('#globalCityPopUp').is(":visible")) $('#pqCarSelect').focus();
});


function getDetails() {
    var url = prefill == prefillEnum.byversion ? ('/webapi/CarVersionsData/GetCarDetailsByVersionId/?versionid=' + window.versionId) : ('/webapi/CarModelData/GetCarDetailsByModelId/?modelid=' + window.modelId)
    $.ajax({
        type: 'GET',
        url: url,
        async: false,
        dataType: 'json',
        success: function (json) { carDetails = json; window.modelId = carDetails.ModelId; }
    });
}

function simulatedClick(label, id) {
    $("#drpCity_").text("Select City");
    objNewPrice.Name = formatSpecial(label);
    objNewPrice.Id = id;
    selectedModelId = id;
    $('#pqCarSelect').removeClass('border-red');
    $('#drpCity_').removeClass('border-red');
    $('#pqCarSelect').val(label);
    bindModelCity(selectedModelId, 'drpCity_');
    $("#drpCity_").removeClass('disable-div');

    showHideDrpError($(pqCarSelectInputField).closest('.easy-autocomplete').nextAll(), false);
    showHideDrpError($("#drpCity_").nextAll(), false);
}

function showHideDrpError(element, error) {
    if (error) {
        $(element[0]).removeClass('hide');
        $(element[1]).removeClass('hide');
    }
    else {
        $(element[0]).addClass('hide');
        $(element[1]).addClass('hide');
        $(element[2]).addClass('hide')
    }
}


function cookieCallBackFunction() {
    if (!isNaN(selectedModelId) && selectedModelId != null) {
        $.cookie('_PQModelId', selectedModelId, { path: '/' });
        $.cookie('_PQVersionId', (typeof (window.versionId) != "undefined" ? window.versionId : "0"), { path: '/' });
        $.cookie('_PQPageId', "2", { path: '/' });
    }
}

function setRedirectUrl(carData)
{
    var modelUrl = "/" + carData[0].toLowerCase() + "-cars/" + carData[1].split('|')[1].toLowerCase() + "/";
    $('#btnGetPQ,#drpCity_').attr("data-redirect-url", modelUrl);
}
