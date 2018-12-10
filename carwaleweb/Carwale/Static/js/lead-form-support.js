PageInfo = {
    isModelCity: window.isCityPage && getJavascriptBoolean(window.isCityPage),
    isVersion: window.ISVERSIONPAGE && getJavascriptBoolean(window.ISVERSIONPAGE),
    isCompare: window.isComparePage && getJavascriptBoolean(window.isComparePage),
    isModelPage: window.isModelPage && getJavascriptBoolean(window.isModelPage),
}

CampaignAdditionalInfo = {
    isDealerCampaignExist: function (clicksource) {
        return $(clicksource).attr("emiadavailable") ? getJavascriptBoolean($(clicksource).attr("emiadavailable")).toString() : "false";
    },

    isDealerExistInCity: function (clicksource) {
        return $(clicksource).attr("dealerexistance") ? getJavascriptBoolean($(clicksource).attr("dealerexistance")).toString() : "false";
    },
    isDealerLocatorAvailable: function (clicksource) {
        return $(clicksource).attr("dealerlocatoradavailable") ? getJavascriptBoolean($(clicksource).attr("dealerlocatoradavailable")).toString() : "false";
    },
    setDealerCampaignAttr: function (clicksource) {
        $(clicksource).attr("emiadavailable", true);
    }
}

GATracking = {
    adType: function () {
        if (PageInfo.isVersion) {
            return "emi-version-page-desktop";
        } else if (PageInfo.isModelCity) {
            return "emi-model-city-page-desktop";
        } else if (PageInfo.isCompare) {
            return "emi-cc-page-desktop";
        } else {
            return "emi-model-page-desktop";
        }
    },
    gaCat: function () {
        if (PageInfo.isVersion) {
            return "EMIPopupVersionPage";
        } else if (PageInfo.isModelCity) {
            return "EMIPopupModelCityPage";
        } else if (PageInfo.isCompare) {
            return "EMIPopupCCPage";
        } else {
            return "EMIPopupModelPage";
        }
    },
    gaActionDifferential: function () {
        if (PageInfo.isVersion) {
            return "-desktop-versionpage-";
        } else if (PageInfo.isModelCity) {
            return "-desktop-modelcitypage-";
        } else if (PageInfo.isCompare) {
            return "-desktop-ccpage-";
        } else {
            return "-desktop-modelpage-";
        }
    }
}

LeadSource = {
    inquirySourceId: function (clicksource) {
        if (typeof $(clicksource).attr('inquirysourceid') != "undefined") {
            return $(clicksource).attr('inquirysourceid');
        } else if (PageInfo.isModelPage) {
            return 114;
        } else if (PageInfo.isVersion) {
            return 113;
        } else if (PageInfo.isCompare) {
            return 206;
        }
        else {
            return 0;
        }
    },

    leadclicksourceId: function (clicksource) {
        return $(clicksource).attr('leadclicksource') ? $(clicksource).attr('leadclicksource') : 0;
    },

    recoLeadClickSource: function (clicksource) {
        if (typeof $(clicksource).attr('recoleadsource') != "undefined") {
            return $(clicksource).attr('recoleadsource');
        } else {
            return 135;
        }
    },

    recoInquirySource: function (clicksource) {
        if (typeof $(clicksource).attr('recoinquirysource') != "undefined") {
            return $(clicksource).attr('recoinquirysource');
        } else {
            return 128;
        }
    },

    mlaLeadclickSource: 397
}

CarInfo = {
    versionId: function (clicksource) {
        if (typeof $(clicksource).data("versionid") != "undefined")
            return $(clicksource).data("versionid");
        else
            return window.VersionId || 0;
    }
}

LocationInfo = {
    cityId: function (clicksource) {
        if (clicksource && $(clicksource).data("citydetails")) {
            var cityDetail = $(clicksource).data("citydetails");
            if (typeof cityDetail != "undefined") return cityDetail.cityId;
        } else {
            return PageInfo.isModelCity ? CityId : $.cookie("_CustCityIdMaster");
        }
    },
    cityName: function (clicksource) {
        if (clicksource && $(clicksource).data("citydetails")) {
            var cityDetail = $(clicksource).data("citydetails");
            if (typeof cityDetail != "undefined") return cityDetail.cityName;
        } else {
            return PageInfo.isModelCity ? CityName : $.cookie("_CustCityMaster");
        }
    },
    zoneId: function (clicksource) {
        if (clicksource && $(clicksource).data("citydetails")) {
            var cityDetail = $(clicksource).data("citydetails");
            if (typeof cityDetail != "undefined") return cityDetail.zoneId;
        } else {
            return PageInfo.isModelCity ? ((CityId == $.cookie("_CustCityIdMaster")) && Number($.cookie("_CustZoneIdMaster")) > 0
                ? $.cookie("_CustZoneIdMaster") : "") : ((Number($.cookie("_CustZoneIdMaster")) > 0 ? $.cookie("_CustZoneIdMaster") : ""));
        }
    },
    areaId: function (clicksource) {
        if (clicksource && $(clicksource).data("citydetails")) {
            var citydetail = $(clicksource).data("citydetails");
            if(typeof citydetail != "undefined") return citydetail.areaId;
        } else {
            var cityId = $.cookie("_CustCityIdMaster");
            var areaId = $.cookie("_CustAreaId");
            if (PageInfo.isModelCity) {
                if (CityId == cityId) {
                    return areaId;
                } else {
                    return 0;
                }
            } else {
                return areaId;
            }
        }
    }
}

var arrLeadClickSource = [100, 118, 119, 120, 101, 104, 109, 110, 131, 137, 138, 130, 133, 128, 129, 155,
    156, 157, 158, 159, 160, 180, 214, 215, 216, 220, 302, 304, 307, 341, 356, 358, 360, 365, 400]; // Created for showing recommendation specific to these lead click source

var platform = 1;
var platformTrackingText = "Desktop";

function getJavascriptBoolean(value) {
    return value.toString().toLowerCase() == "true";
}

function setCTALeadFormAttribute(ctaSource, leadClickSource, recoLeadSource) {
    if (ctaSource != null && typeof ctaSource != "undefined") {
        ctaSource.removeAttribute('onclick');
        ctaSource.setAttribute('emiadavailable', 'true');
        ctaSource.setAttribute('leadclicksource', leadClickSource); 
        ctaSource.setAttribute('data-versionid', VersionId);

        if (recoLeadSource) {
            ctaSource.setAttribute('recoleadsource', recoLeadSource);
        }
    }
}
