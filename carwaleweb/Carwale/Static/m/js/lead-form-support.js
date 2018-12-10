PageInfo = {
    isModelCity : window.isModelCityPage && window.isModelCityPage.toString().toLowerCase() == "true",
    isVersion : window.isVersionPage && window.isVersionPage.toString().toLowerCase() == "true",
    isCompare: window.isComparePage && window.isComparePage.toString().toLowerCase() == "true",
    isQuotation: window.isQuotationPage && window.isQuotationPage.toString().toLowerCase() == "true",
    isModelPage: window.isModelPage && window.isModelPage.toString().toLowerCase() == "true"
}

CampaignAdditionalInfo = {
    isDealerCampaignExist: function (clicksource) {
        return $(clicksource).attr("dealercampaignexist") ? $(clicksource).attr("dealercampaignexist").toString().toLowerCase() : "false";
    },

    isDealerExistInCity: function (clicksource) {
        return $(clicksource).attr("isdealerexistincity") ? $(clicksource).attr("isdealerexistincity").toString().toLowerCase() : "false";
    },
    isDealerLocatorAvailable: function (clicksource) {
        return $(clicksource).attr("dealerlocatoradavailable") ? $(clicksource).attr("dealerlocatoradavailable").toString().toLowerCase() : "false";
    },
    setDealerCampaignAttr: function (clicksource) {
        $(clicksource).attr("dealercampaignexist", true);
    }
}

GATracking = {
    adType: function (clicksource) {
        if($(clicksource).attr('isgetoffers-versionlink')) {
            if(PageInfo.isModelCity) {
                return "MSite-PriceInCityPage-CampaignLink";
            } else {
                return "MSite-ModelPage-CampaignLink";
            }
        } else if (PageInfo.isCompare) {
            return "emi-compare-page";
        } else if (PageInfo.isVersion) {
            return "emi-version-page";
        } else if (PageInfo.isModelCity) {
            return "emi-modelCity-page"; 
        } else
            return "emi-model-page";
    },
    gaCat: function (clicksource) {
        if($(clicksource).attr('isgetoffers-versionlink')) { 
            if (PageInfo.isModelCity) {
                return "MSite-PriceInCityPage-CampaignLink";
            }else { 
                return "MSite-ModelPage-CampaignLink";
            }
        } else if (PageInfo.isCompare) {
            return "emi-compare-page"; 
        } else if (PageInfo.isVersion) {
            return "EmiAssistVersionPage";
        } else if (PageInfo.isModelCity) {
            return "EmiAssistModelCityPage";
        } else {
            return "EmiAssistModelPage";
        }
    }
}

LeadSource = {
    inquirySourceId: function (clicksource) {
        if(typeof $(clicksource).attr('leadinquirysource') != "undefined"){
            return $(clicksource).attr('leadinquirysource');
        } else if (PageInfo.isVersion) {
            return 111;
        } else if (PageInfo.isModelCity) {
            return 125;
        } else if (PageInfo.isCompare) {
            return 206;
        } else if (PageInfo.isQuotation) {
            return 36;
        } else {
            return 112;
        }
    },

    leadclicksourceId: function (clicksource) {
        return $(clicksource).attr('sourceclick') ? $(clicksource).attr('sourceclick') : 0;
    },

    recoLeadClickSource: function (clicksource) {
        if (typeof $(clicksource).attr('recoleadsource') != "undefined") {
            return $(clicksource).attr('recoleadsource');
        } else if (PageInfo.isVersion) {
            return 142;
        } else if (PageInfo.isModelCity) {
            return 385;
        } else if (PageInfo.isCompare) {
            return 343;
        } else if (PageInfo.isQuotation) {
            return 394;
        } else {
            return 141;
        }
    },

    recoInquirySource: function (clicksource) {
        if (typeof $(clicksource).attr('recoinquirysource') != "undefined") {
            return $(clicksource).attr('recoinquirysource');
        } else if (PageInfo.isVersion) {
            return 132;
        } else if (PageInfo.isCompare) {
            return 206;
        } else if (PageInfo.isQuotation) {
            return 36;
        } else {
            return 131;
        }
    },

    mlaLeadclickSource: 362
}

CarInfo = {
    versionId : function(clicksource) {
        if(typeof $(clicksource).data("versionid") != "undefined")
            return $(clicksource).data("versionid");
        else
            return window.defaultVerId || 0;
    }
}

LocationInfo = {
    cityId: function (clicksource) {
        if (clicksource && $(clicksource).data("citydetails")) {
            var cityDetail = $(clicksource).data("citydetails");
            if (typeof cityDetail != "undefined") return cityDetail.cityId;
        } else {
            return PageInfo.isModelCity || PageInfo.isQuotation ? userCityId : $.cookie("_CustCityIdMaster");
        }
    },
    cityName: function (clicksource) {
        if (clicksource && $(clicksource).data("citydetails")) {
            var cityDetail = $(clicksource).data("citydetails");
            if (typeof cityDetail != "undefined") return cityDetail.cityName;
        } else {
            return PageInfo.isModelCity || PageInfo.isQuotation ? CityName : $.cookie("_CustCityMaster");
        }
    },
    zoneId: function (clicksource) {
        if (clicksource && $(clicksource).data("citydetails")) {
            var cityDetail = $(clicksource).data("citydetails");
            if (typeof cityDetail != "undefined") return cityDetail.zoneId;
        } else {
            return PageInfo.isModelCity ? ((userCityId == $.cookie("_CustCityIdMaster")) && Number($.cookie("_CustZoneIdMaster")) > 0
                ? $.cookie("_CustZoneIdMaster") : "") : ((Number($.cookie("_CustZoneIdMaster")) > 0 ? $.cookie("_CustZoneIdMaster") : ""));
        }
    },
    areaId: function (clicksource) {
        if (clicksource && $(clicksource).data("citydetails")) {
            var cityDetail = $(clicksource).data("citydetails");
            if (typeof cityDetail != "undefined") {
                return cityDetail.areaId;
            }
        } else {
            var cityId = $.cookie("_CustCityIdMaster");
            var areaId = $.cookie("_CustAreaId");
            if (PageInfo.isModelCity || PageInfo.isQuotation) {
                return userCityId == cityId ? areaId : window.userAreaId ? userAreaId : 0;
            } else {
                return areaId;
            }
        }
    }
}
    // TBD : Remove this
var arrLeadClickSource = [1, 2, 111, 112, 132, 139, 140, 161, 162, 163, 164, 143, 176, 177, 178, 179, 181, 182, 183, 184, 185, 186, 204, 205,
    206, 145, 222, 227, 229, 231, 233, 235, 237, 239, 241, 243, 245, 253, 303, 305, 308, 309, 310, 313, 314, 317, 318, 321, 322, 325, 326, 329, 330,
    335, 336, 337, 342, 344, 345, 346, 366, 368, 370, 372, 373, 375, 377, 379, 381, 383, 386, 393, 395, 396];//array contains lead click sources for which recommendations will come.


var platform = window.platform || 43;
var platformTrackingText = platform == 43 ? "Msite" : platform == 74 ? "Android" : "IOS";


function getJavascriptBoolean(value) {
    return value.toString().toLowerCase() == "true";
}

function setCTALeadFormAttribute(ctaSource, leadClickSource, recoLeadSource) {
    if (ctaSource != null && typeof ctaSource != "undefined") {
        ctaSource.removeAttribute('onclick');
        ctaSource.setAttribute('dealercampaignexist', 'true');
        ctaSource.setAttribute('sourceclick', leadClickSource);
        ctaSource.setAttribute('data-versionid', defaultVerId);

        if (recoLeadSource) {
            ctaSource.setAttribute('recoleadsource', recoLeadSource);
        }
        if (PageInfo.isQuotation) {
            ctaSource.setAttribute('data-predictionScore', predictionScore);
            ctaSource.setAttribute('data-predictionLabel', predictionLabel);
            ctaSource.setAttribute('data-citydetails', JSON.stringify(CityDetail));
        }
    }
}