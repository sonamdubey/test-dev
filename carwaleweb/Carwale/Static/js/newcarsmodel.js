var vwGroupMakeIdArray = typeof (vwGroupMakeIds) == "undefined" ? [] : vwGroupMakeIds;

var UpComing_Cars = {

    initLocationPluginForORP: function () {
        var div = $('.btnShowOnRoadPrice');
        var location = new LocationSearch((div), {
            showCityPopup: true,
            callback: function (locationObj) {
                var modelMaskingName = location.selector().data('modelmaskingname');
                var makeMaskingName = location.selector().data('makemaskingname');
                NewCar_Common.redirectToModelPage(makeMaskingName, modelMaskingName);
            },
            isDirectCallback: true,
            isAreaOptional: true,
            validationFunction: function () {
                return PriceBreakUp.Quotation.getGlobalLocation();
            }
        });
    },

    initLocationPluginForVPB: function () {
        var div = $('.btnViewPriceBreakup');
        var location = new LocationSearch((div), {
            showCityPopup: true,
            callback: function (locationObj) {
                var carModelId = location.selector().data('model');
                var pageId = location.selector().data('pageid');
                PriceBreakUp.Quotation.RedirectToPQ({ 'modelId': carModelId, 'location': locationObj, 'pageId': pageId });
            },
            isDirectCallback: true,
            validationFunction: function () {
                return PriceBreakUp.Quotation.getGlobalLocation();
            }
        });
    }
}

$(document).ready(function () {
    preSelectCheckedCars();
    AdBlockTracking();
    if (newCar)
        userHistory.trackUserHistory([modelId]);

    $(document).on("mastercitychange", function (event, cityName, cityId, item) {
        masterCityChange(cityName, cityId, item);
    });

    $("#esLeadFormSubmit").attr('data-label', 'submit').addClass("click_track");
    //$("#es-leadform").append('<div class="margin-top15">Or Call us on <span class="text-green font16 text-bold toll-free-number" title="Toll Free">1800-2090-230</span></div>');
    $($("#es-leadform").children()[2]).hide();
    $("#personEmail").parent().hide();
    $($("#es-leadform").children()[0]).text("Please share your contact details");
    $("#es-thankyou h2").html("Thank You for sharing your details. We will share your details with the manufacturer.");

    $(document).on('click', '.specs-accordion__list-item', function () {
        var featureList = $(this).children('.model-feature-list');
        var toggleIcon = $(this).find('.accordion-open-icon');
        if (featureList.is(':visible')) {
            featureList.fadeOut(200);
            toggleIcon.removeClass('accordion--close');
        }
        else {
            featureList.fadeIn(300);
            toggleIcon.addClass('accordion--close');
        }
    });

    $("input.chkCompare").bt({
        contentSelector: "$('#btCompare').html()", fill: '#F7F7F7', strokeWidth: 1, strokeStyle: '#ccc', trigger: ['change', 'none'], width: '450px', padding: '15px', shadow: true, positions: ['right'],
        preShow: function (box) {
            $("div.bt-wrapper").hide();
        },
        showTip: function (box) {
            boxObj = $(box);
            boxObj.show();

            boxObj.find("#closeBox").click(function () {
                boxObj.hide();
            });

            versionId = $(this).attr("value");

            if ($(this).is(":checked") == true && $("input.chkCompare:checked").length <= 4) {
                var liAppend = "<li class='" + versionId + "'>" + $(this).attr("data-version__name") + " <a class='removeCompHref' style='float:right;cursor:pointer;' onclick='removeClicked(this);'>remove</a></li>";
                $("#btCompare .selectedCars").append(liAppend);
                boxObj.find(".selectedCars").append(liAppend);

            }
            else if ($(this).is(":checked") == true && $("input.chkCompare:checked").length > 4) {
                $(this).removeAttr("checked");

                boxObj.find("h4").attr("class", "alert");
            } else {
                removeComp(versionId);
                boxObj.find("h4").attr("class", "hd4");
            }

        }
    });



    initModelOverviewPrice();
    initVersionListPrice();
    initCityPopUp();

    if (IsOverviewPage) {
        $('#overview').removeAttr("href");
    }
    selectSubNaviTab(window.location.hash.toLowerCase());

    $("#btnSubscribe").click(function (e) {
        e.preventDefault();
        if (validateEmailAddress()) {
            Subscribe();
        }
        return false;
    });

    $('#txtUpcomingAlertEmail').on('keyup', function (e) {
        e.preventDefault();
        if (e.keyCode == 13) {
            if (validateEmailAddress()) {
                return Subscribe();
            }
        }
        return false;
    });

    $("img.lazy-uc").lazyload();

    $("#luxuryCarImage,#luxuryCarTitle,#luxuryCarPriceKm").click(function (e) {
        dataLayer.push({ event: 'BBT_CarClick', cat: 'UsedCarOtherCWPages', act: 'BBT_CarClick' });
    });
    $("#showroomBtn").click(function (e) {
        dataLayer.push({ event: 'BBT_ShowroomClick', cat: 'UsedCarOtherCWPages', act: 'BBT_ShowroomClick' });
    });

    var prevNextHandleVar = Math.ceil(parseFloat($('#authorCarousel li').length));
    var prevNextHandleConst = prevNextHandleVar;
    var videoSlider = 1;
    prevNextDisplay(prevNextHandleConst);

    if (isCityPage) {
        if ($("#divAdvantageLink").is(":visible")) {
            Common.utils.trackAction('CWNonInteractive', 'deals_desktop', 'dealsimpression_desktop', "priceincitydesk");
        }
    } else {
        if ($("#divAdvantageLink").is(":visible")) {
            Common.utils.trackAction('CWNonInteractive', 'deals_desktop', 'dealsimpression_desktop', "modelpagedesk");
        }
    }

    if ($('.advantageCardLink').is(":visible")) {
        if (isCityPage)
            Common.utils.trackAction('CWNonInteractive', 'deals_desktop', 'dealsimpression_desktop', 'priceincitypage_carsuggestions_desk');
        else
            Common.utils.trackAction('CWNonInteractive', 'deals_desktop', 'dealsimpression_desktop', 'modelpage_carsuggestions_desk');
    }

    if (isCityPage) {
        $('.trackVersion').each(function () {
            Common.utils.trackAction('CWNonInteractive', 'deals_desktop', 'dealsimpression_desktop', 'priceincitypage_variantlistdesk');
        });
    }
    else {
        $('.trackVersion').each(function () {
            Common.utils.trackAction('CWNonInteractive', 'deals_desktop', 'dealsimpression_desktop', 'modelpage_variantlistdesk');
        });

    }

    if ($('#advantageAd').is(':visible')) {
        Common.utils.trackAction('CWNonInteractive', 'deals_desktop', ' dealsimpression_desktop', 'modelpage_sideslugdesk');
    }

    $(' .trackVersion').click(function () {
        if (isCityPage) {
            Common.utils.trackAction('CWInteractive', 'deals_desktop', 'dealsaccess_desktop', 'priceincitypage_variantlistdesk');
        } else {
            Common.utils.trackAction('CWInteractive', 'deals_desktop', 'dealsaccess_desktop', 'modelpage_variantlistdesk');
        }
    });

    if ($('.getOffersLink').length > 0) {
        if (isCityPage) {
            Common.utils.trackAction('CWNonInteractive', 'Desktop-PriceInCityPage-VersionList_CampaignLink', ' Link_Shown', modelName + ',' + sponsoredDealerName + ',' + sponsoredDealerId + ',' + CityName);
        } else {
            Common.utils.trackAction('CWNonInteractive', 'Desktop-ModelPage-VersionList_CampaignLink', ' Link_Shown', modelName + ',' + sponsoredDealerName + ',' + sponsoredDealerId + ',' + ($.cookie("_CustCityMaster") == zoneNameForModelVersion ? $.cookie("_CustCityMaster") : ($.cookie("_CustCityMaster") != "Select City" ? $.cookie("_CustCityMaster") + ',' + zoneNameForModelVersion : "No City")));
        }
    }

    tracking.trackBhriguImpression();

    $('#model-version-dropdown li').on('click', function (e) {
        var version = $(this);
        var oldVersionId = getVersionState()[modelId];
        var versionsElement = $('#model-version-dropdown');
        var versionId = version.find(".pop-version-name").data("versionid");
        var versionName = version.find(".pop-version-name").val();
        $("#selectcustom-input-box-holder .selectcustom-input").text(version.find('.pop-version-name').text());
        if (leadVersionId != undefined) {
            leadVersionId = versionId;
        }
        $.get("/pricebreakup/",
        { versionId: versionId, versionName: versionName, cityId: versionsElement.data("cityid"), modelId: versionsElement.data("modelid"), makeId: versionsElement.data("makeid"), isCampaignAvailble: (versionsElement.data("iscampaignavailable") == "True"), makeName: versionsElement.data("makename"), modelName: versionsElement.data("modelname"), cityName: versionsElement.data("cityname"), showCampaignLink: versionsElement.data("showcampaignlink"), campaignDealerId: versionsElement.attr("campaignid_dealerid"), campaignLeadCTA: versionsElement.data("campaignleadcta") }).done(
        function (response) {
            if (response) {
                $("#price-breakup-section").html(response);
                btObj.registerEventsClass();
                EmiCalculator.EmiCalculatorDocReady();
                if (Number(version.attr('data-price-status')) === PriceStatusEnum.PriceAvailable) {
                    EmiCalculatorExtended.showEmiCalcSlug(version, version.data("verid"), version.data("versionname"));
                    if (vwGroupMakeIdArray.includes(Number(makeId))) {
                        EmiCalculatorExtended.getThirdPartyEmiDetails(Number(version.data("verid")), false, Number(version.attr('data-downpaymentmaxvalue')), oldVersionId);
                    }
                    else {
                        Common.utils.callTracking($('#' + versionId), '_shown');
                    }
                    $('.emi-link-wrapper').removeClass('hide');
                }
                else {
                    $('.emi-link-wrapper').addClass('hide');
                }
                if (window.registerCampaignEvent) {
                    var priceBreakupSection = document.getElementById("price-breakup-section");
                    window.registerCampaignEvent(priceBreakupSection);
                }
            }
        });
        updateSelectedVersionCookie(versionId);
    });

    $("#expertreviews img.lazy").lazyload();
    UpComing_Cars.initLocationPluginForORP();
    UpComing_Cars.initLocationPluginForVPB();
    $('#spnSmallDescSynopsis > p').contents().unwrap();
});

var collectImpTrackData = {

    getCategory: function () {
        return isCityPage ? 'CityPage' : 'ModelPage';
    },
    getAction: function () {
        return isCityPage ? 'CityImpression' : 'ModelImpression';
    },
    getLabel: function () {
        label = "modelid=" + modelId + "|cityid=" + (isCityPage ? CityId : $.cookie("_CustCityIdMaster")) + "|source=1";
        label = label + (isPriceShown == 'True' ? "|isorpshown=1" : '');
        label = label + (window.campaignId > 0 ? "|campaignid=" + campaignId + "|iscampaignshown=1|campaignpanel=" + sponsorDlrLeadPanel + "|campaigntype=1" : '');
        return label;
    }
}

function isCityDuplicate(item) {
    return item != undefined && item != null && item.payload.isDuplicate && (item.payload.cityId != 599 && item.payload.cityId != 1358);
}

function masterCityChange(cityName, cityId, item) {
    var url = window.location.href;
    if (url.split("price-in").length == 2) {
        url = url.split("price-in")[0] + ((item != undefined) ? "price-in-" + item.payload.cityMaskingName + "/" : "");
        window.location.href = url;
    }
    else if (url.indexOf("-cars/") > -1) {
        window.location.href = url;
    }
}

function ShowAllDisVer() {
    $("#divDisc").hide();
    $("#divDiscontinued").show();
}

function preSelectCheckedCars() {
    $("input.chkCompare:checked").each(function () {
        versionId = $(this).attr("value");
        if ($("input.chkCompare:checked").length <= 4) {
            var liAppend = "<li class='" + versionId + "'>" + $(this).attr("data-version__name") + " <a class='removeCompHref' style='float:right;cursor:pointer;' onclick='removeClicked(this);'>remove</a></li>";
            $("#btCompare .selectedCars").append(liAppend);
        }
    });
}

function removeClicked(removelink) {
    var vId = $(removelink).parent().attr("class");
    $("input.chkCompare[value='" + vId + "']").removeAttr("checked");
    removeComp(vId);
    boxObj.find("h4").attr("class", "hd4");
}

function removeComp(versionId) {
    $("#btCompare .selectedCars li." + versionId).remove();
    boxObj.find(".selectedCars li." + versionId).hide();
    if ($("input.chkCompare:checked").length == 0)
        boxObj.hide();
}

function CompareCars(pageSource) {
    var chkSelected = $("input.chkCompare:checkbox:checked");

    if (chkSelected.length < 2) {
        alert("Please select at least two cars to compare");
        return;
    }
    var url = "";
    var tmname = formatURL(makename);
    var tmoname = maskingName;
    for (var i = 0 ; i < chkSelected.length - 1; i++) {
        url += tmname + "-" + tmoname + "-vs-"
    }
    if (chkSelected.length == 2)
        compareUrl = "/comparecars/" + url + tmname + "-" + tmoname + "/?c1=" + $(chkSelected[0]).val() + "&c2=" + $(chkSelected[1]).val();
    else if (chkSelected.length == 3)
        compareUrl = "/comparecars/" + url + tmname + "-" + tmoname + "/?c1=" + $(chkSelected[0]).val() + "&c2=" + $(chkSelected[1]).val() + "&c3=" + $(chkSelected[2]).val();
    else if (chkSelected.length == 4)
        compareUrl = "/comparecars/" + url + tmname + "-" + tmoname + "/?c1=" + $(chkSelected[0]).val() + "&c2=" + $(chkSelected[1]).val() + "&c3=" + $(chkSelected[2]).val() + "&c4=" + $(chkSelected[3]).val();

    location.href = compareUrl + (Number(pageSource) > 0 ? "&source=" + pageSource : "");
}
filterTable();
$(".filter__option").change(filterTable);
function filterTable() {
    var filters = $(".filter__option:checked");
    var variants = $(".variant");
    if (filters.length > 0) {
        variants.hide();
        $(".filter__no-result").hide();
        var fuelTypeFilters = filters.filter("[id^=ft__]");
        var transmissionTypeFilters = filters.filter("[id^=tr__]");
        if (fuelTypeFilters.length > 0 && transmissionTypeFilters.length > 0) {
            var filteredVariants = [];
            fuelTypeFilters.each(function () { filteredVariants.push.apply(filteredVariants, variants.filter("[fuel-type='" + this.value + "']")); });
            transmissionTypeFilters.each(function () { $(filteredVariants).filter("[transmission-type='" + this.value + "']").show(); });
        }
        else {
            fuelTypeFilters.each(function () { variants.filter("[fuel-type='" + this.value + "']").show(); });
            transmissionTypeFilters.each(function () { variants.filter("[transmission-type='" + this.value + "']").show(); });
        }

        if (variants.filter(":visible").length == 0) { $(".filter__no-result").show(); }
    }
    else {
        variants.show();
        $(".filter__types").text("Fuel type & Transmission");
    }
}
var sessionId;
$(".filter__option").change(trackFilters);
function trackFilters() {
    sessionId = sessionId || $.cookie("_cwv").split(".")[1];
    var timeStamp = new Date().getTime();
    var label = sessionId + '-' + timeStamp + '-' + modelName + '-' + ($(this).is(':checked') ? "S" : "DS") + ':' + this.value;
    Common.utils.trackAction("CWInteractive", "ModelPage", 'VersionTableFilterUsage', label);
}
toggleFilterTags.call($(".filter__toggle"));
$(".filter__toggle").change(toggleFilterTags);
function toggleFilterTags() {
    if (!$(this).is(":checked")) {
        var filters = $(".filter__option:checked");
        if (filters.length > 0) {
            var tags = "";
            filters.each(function () { tags = tags + "<span class='filter__tag close'>" + this.value + "</span>" })
            $(".filter__types").html(tags);
            $(".filter__tag").click(function () { $(this).hide(); $(".filter__option[value='" + this.innerHTML + "']").trigger('click'); });
        }
    }
    else {
        $(".filter__types").text("Fuel type & Transmission");
    }
}

$("#txtUpcomingAlertEmail").click(function () {
    if ($("#txtUpcomingAlertEmail").val() == "Enter your email address") {
        $("#txtUpcomingAlertEmail").val("");
    }
}).focus(function () {
    if ($("#txtUpcomingAlertEmail").val() == "Enter your email address") {
        $("#txtUpcomingAlertEmail").val("");
    }
}).blur(function () {
    $("#txtUpcomingAlertEmail").val($("#txtUpcomingAlertEmail").val().trim());
    if ($("#txtUpcomingAlertEmail").val() == "") {
        $("#txtUpcomingAlertEmail").val("Enter your email address");
    }
});

$("#btnSubscribe").ajaxStart(function () {
    $('#ajaxBusy').show();
});

function validateEmailAddress() {
    var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    var email = $("#txtUpcomingAlertEmail").val();
    var error = false;
    if (email != null && email != "") {
        if (email != $("#txtUpcomingAlertEmail").attr("placeholder")) {
            if (!emailPattern.test(email)) {
                alert("Please enter a valid email");
                error = true;
            }
        } else {
            alert("Please enter your email");
            error = true;
        }
    } else {
        alert("Please enter your email");
        error = true;
    }
    if (error == false)
        return true
    else
        return false
}

function Subscribe() {
    $('#divSubscription').append('<label id="ajaxBusy" style="display:none;margin:0px;font-size:10px">processing...</label>');
    $.ajax({
        type: "POST", url: "/ajaxpro/CarwaleAjax.AjaxResearch,Carwale.ashx",
        data: '{"emailAddress":"' + $("#txtUpcomingAlertEmail").val() + '", "subscriptionCategory":"' + subscriptionCategory + '", "subscriptionType":"' + subscriptionType + '"}',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("X-AjaxPro-Method", "Subscribe");
        },
        success: function (response) {
            $('#ajaxBusy').hide();
            var responseJSON = eval('(' + response + ')');
            if (responseJSON.value == false) {
                alert("You are already subscribed");
            }
            else {
                alert("You are successfully subscribed");
            }
        }
    });

    return false;
}

function AdBlockTracking() {
    if (window.adblockDetecter === undefined)
        dataLayer.push({ event: 'CWNonInteractive', cat: 'AdBlocker', act: 'Modelpage', lab: modelName });
}

function initVersionListPrice() {
    var div = $('.version-Price-list');
    var location = new LocationSearch((div), {
        showCityPopup: true,
        callback: function (locationObj) {
            var carModelId = modelId;
            var versionId = location.selector().attr('data-versionid');
            var pageId = location.selector().attr('data-pageId');
            PriceBreakUp.Quotation.RedirectToPQ({ 'modelId': carModelId, 'versionId': versionId, 'location': locationObj, 'pageId': pageId });
        },
        isDirectCallback: true,
        validationFunction: function () { return PriceBreakUp.Quotation.getGlobalLocation(); }
    });
}

function versionDropDownClickHandler(version) {
    var versionState = getVersionState();
    var oldVersionId = versionState[CarDetails.carModelId];
    if(oldVersionId == version.data("verid")){
        return;
    }
    $(".selected-version__price").text(version.data("verprice"));
    if (version.data("veremi") === "")
        $("#emiText").hide();
    else {
        $(".selected-version__emi").text(version.data("veremi"));
        $("#emiText").show();
    }
    $(".selected-version__price-label")
        .text(version.data("pricelabel"))
        .removeClass("oliveText redText")
        .addClass(version.data("labelcolor"));

    $(".selected-version__name").text(version.find(".custom-dropdown-title").text())

    $(".versiondrp__list")
        .children(".selected")
        .removeClass("selected");

    $(".versiondrp__list")
        .children("[data-verid=" + version.data("verid") + "]")
        .addClass("selected");

    if (version.data("price-status") > 1) {
        $(".selected-version__price-label").append("&nbsp;<div class='inline-block'><span class='average-info-tooltip class-ad-tooltip info-icon desk-deals-car-sprite inline-block'></span><p class='average-info-content hide'>" + version.data("reasontext") + " </p></div>");
        $("#view-breakup__btn").hide();
        btObj.registerEventsClass();
    }
    else if (version.data("price-status") <= 0) {
        $("#view-breakup__btn").hide();
    }
    else {
        $("#view-breakup__btn").show();
    }

    if (Number(version.attr('data-price-status')) === PriceStatusEnum.PriceAvailable) {
        EmiCalculatorExtended.showEmiCalcSlug(version, version.data("verid"), version.data("versionname"));
        var pageName = PageInfo.isModelPage ? "ModelDesktop" : "";
        var eventCatagory = pageName + "_EMICalculatorLink";
        var eventLabel = CarDetails.carMakeName + "_" + CarDetails.carModelName;
        Common.utils.trackAction("CWNonInteractive", eventCatagory, "EMICalculatorLink_shown", eventLabel);

        if (vwGroupMakeIdArray.includes(CarDetails.carMakeId)) {
            EmiCalculatorExtended.getThirdPartyEmiDetails(Number(version.data("verid")), false, Number(version.attr('data-downpaymentmaxvalue')), getVersionState()[modelId]);
        }
        $('.emi-link-wrapper').removeClass('hide');
    }
    else {
        $('.emi-link-wrapper').addClass('hide');
    }

    VersionId = "" + version.data("verid");
    updateSelectedVersionCookie(version.data("verid"));
}

var expertReviewActionHandler = (function () {
    var shortReviewPosition;
    function showFullDescription() {
        $('#spnSmallDescSynopsis').hide();
        $('#spnLargeDescSynopsis').show().find('img.lazy').lazyload();
        var shortReviewScrollTop = $(window).scrollTop();
        shortReviewPosition = shortReviewScrollTop;
        Common.utils.trackAction("CWInteractive", "CarWale_Opinion_expert_review_d", "full_review_clicked", modelName);
        cwTracking.trackCustomData("CarWaleOpinionExpertReview", "FullReviewClicked", ("modelid=" + modelId + "|source=1"), false);
    }

    function showShortDescription() {
        $('#spnLargeDescSynopsis').hide();
        $('#spnSmallDescSynopsis').show();
        if (shortReviewPosition != null) {
            $(window).scrollTop(shortReviewPosition);
        }
        Common.utils.trackAction("CWInteractive", "CarWale_Opinion_expert_review_d", "hide_review_clicked", modelName);
        cwTracking.trackCustomData("CarWaleOpinionExpertReview", "HideReviewClicked", ("modelid=" + modelId + "|source=1"), false);
    }
    return {
        showShortDescription: showShortDescription,
        showFullDescription: showFullDescription,
    }
})();

//for back handling
$(document).ready(function () {
    var versionState = getVersionState();
    var versionId = versionState[modelId];
    if (versionId) {
        $(".versiondrp__list")
        .children("[data-verid=" + versionId + "]").click();
    }

    if ($('.selectcustom-container').length) {
        var dropdown = new Dropdown('.selectcustom-container');
    }
});
