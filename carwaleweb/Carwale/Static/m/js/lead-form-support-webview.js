PageInfo = {
    isModelCity: "false",
    isVersion: window.webviewScreenId == 3,
    isCompare: window.webviewScreenId == 9,
    isQuotation: window.webviewScreenId == 16,
    isModelPage: window.webviewScreenId == 1,
    isGalleryPage : window.webviewScreenId == 27,
    pageName : "unknown"
}


CampaignAdditionalInfo = {
    isDealerCampaignExist: function (clicksource) {
        return window.webviewDealerCampaignExist ? "true" : "false";
    },
    isDealerExistInCity: function (clicksource) {
        return "false";
    },
   
    isDealerLocatorAvailable: function (clicksource) {
        return window.webviewDealerLocatorAvailable ? "true" : "false";
    },
}

GATracking = {
    adType: function (clicksource) {
            
        if (PageInfo.isVersion) {
            return platformTrackingText.toLowerCase() + "-emi-version-page";
        } else if (PageInfo.isQuotation) {
            return platformTrackingText.toLowerCase() + "-emi-quotation-page";
        } else if (PageInfo.isGalleryPage) {
            return platformTrackingText.toLowerCase() + "-emi-gallery-page";
        }
        else
            return platformTrackingText.toLowerCase() + "-emi-model-page";
    },
    gaCat: function (clicksource) {
        if (PageInfo.isCompare) {
            return platformTrackingText.toLowerCase() + "-emi-compare-page";
        } else if (PageInfo.isVersion) {
            return platformTrackingText + "EmiAssistVersionPage";
        } else if (PageInfo.isGalleryPage) {
            return platformTrackingText + "EmiAssistGalleryPage";
        } else {
            return platformTrackingText + "EmiAssistModelPage";
        }
    }
}

LeadSource = {
    inquirySourceId: function (clicksource) {
        if (window.webviewInquiryclickSource > 0) {
            return webviewInquiryclickSource;
        } else if (PageInfo.isVersion) {
            return platform == 74 ? 118 : 168;
        } else if (PageInfo.isModelPage) {
            return platform == 74 ? 117 : 167;
        } else if (PageInfo.isGalleryPage) {
            return platform == 74 ? 116 : 143;
        } else {
            return platform == 74 ? 159 : 160;
        }
    },

    leadclicksourceId: function (clicksource) {
        if (webviewLeadClickSource > 0)
            return webviewLeadClickSource;
        else if(platform == 74)
            return 399;
        else
            return 401
    },

    recoLeadClickSource: function (clicksource) {
        if (PageInfo.isVersion) {
            return platform == 74 ? 143 : 198;
        } else if (PageInfo.isModelPage) {
            return platform == 74 ? 144 : 197;
        } else {
            return platform == 74 ? 147 : 197;
        }
    },

    recoInquirySource: function (clicksource) {
        if (PageInfo.isVersion) {
            return platform == 74 ? 137 : 164;
        } else if (PageInfo.isModelPage) {
            return platform == 74 ? 138 : 163;
        } else {
            return platform == 74 ? 139 : 143;
        }
    },

    mlaLeadclickSource: 398
}

CarInfo = {
    versionId: function (clicksource) {
        return window.webviewVersionId;
    }
}

LocationInfo = {
    cityId: function (clicksource) {
        return window.webviewCityId;
    },
    cityName: function (clicksource) {
        return window.webviewCityName;
    },
    zoneId: function (clicksource) {
        return window.webviewZoneId;
    },
    areaId: function (clicksource) {
        return window.webviewAreaId;
    },
    areaName: function (clicksource) {
    return window.webviewAreaName;
    }
}
// TBD : Remove this
var arrLeadClickSource = [116, 123, 124, 146, 151, 153, 154, 195, 196, 199, 200, 201, 202, 222, 223, 224, 399, 401];//array contains lead click sources for which recommendations will come.

var platform = window.webviewPlatformId > 0 ? window.webviewPlatformId : 74;
var platformTrackingText = platform == 74 ? "Android" : "IOS";

function webviewPageName(webviewScreenId)
{
    switch (webviewScreenId) {
        case 3:
            return "VersionPage"; break;
        case 1:
            return "ModelPage";break;
        case 27:
            return "GalleryMain"; break;
        default:
            return "others";
    }
}

function setWebviewLocation(locationObj) {
    window.webviewCityId = locationObj.cityId;
    window.webviewCityName = locationObj.cityName;
    window.webviewAreaId = typeof (locationObj.areaId) != "undefined" ? locationObj.areaId : 0;
    window.webviewZoneId = typeof (locationObj.zoneId) != "undefined" ? locationObj.zoneId : 0;
    window.webviewAreaName = locationObj.areaName ? location.areaName : "Select Area";
}
function SendDataToApp()
{
    var feedback = { CityId: LocationInfo.cityId(), CityName: LocationInfo.cityName(), ZoneId: LocationInfo.zoneId(), AreaId: LocationInfo.areaId(), AreaName: LocationInfo.areaName() };
    if (platform == 74)
        Android.leadFormCityCallBack(JSON.stringify(feedback));
    else
        webkit.messageHandlers.leadFormCityCallBack.postMessage(JSON.stringify(feedback));
}
function WebviewClosed()
{
    if (platform == 74)
        Android.leadFormClosed();
    else
        webkit.messageHandlers.leadFormClosed.postMessage(JSON.stringify(""));
}
function setLocationCookie() {
    var now = new Date();
    var Time = now.getTime();
    Time += 1000 * 60 * 60 * 24 * 30;
    now.setTime(Time);

    document.cookie = '_CustCityIdMaster=' + LocationInfo.cityId() + '; expires = ' + now.toGMTString() + '; domain=' + defaultCookieDomain + '; path =/';
    document.cookie = '_CustCityMaster=' + LocationInfo.cityName() + '; expires = ' + now.toGMTString() + '; domain=' + defaultCookieDomain + '; path =/';
    document.cookie = '_CustZoneIdMaster=' + (LocationInfo.zoneId() || -1) + '; expires = ' + now.toGMTString() + '; domain=' + defaultCookieDomain + '; path =/';
    document.cookie = '_CustAreaId=' + (LocationInfo.areaId() || -1) + '; expires = ' + now.toGMTString() + '; domain=' + defaultCookieDomain + '; path =/';

}

$(document).ready(function () {
    _campaignViewModel = new campaignViewModel();
    _sellerViewModel = new sellerViewModel();
    emiassist.intialize();
    TestDrive.registerEvents();
    setLocationCookie();
    window.isWebview = true;
    if (typeof (CarDetails) == "undefined" || typeof(campaignId) == "undefined" && LocationInfo.cityId() > 0 && (LocationInfo.areaId() > 0 || window.askingAreaCityId.indexOf(LocationInfo.cityId()) == -1)) {
        emiassist.BindCampaign(null, LocationInfo.cityId(), null);
        emiassist.bindControlEvents();
    }
    else {
        PageInfo.pageName = webviewPageName(window.webviewScreenId);
        emiassist.openEmiPopup("#emiPopup");
    }
})