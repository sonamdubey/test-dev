
import {isServer} from './commonUtils'
import { unlockScroll, lockScroll } from './scrollLock';
var topCount = "5";
function closeGlobalSearchPopUp() {
    hideElement(document.getElementById('global-search-popup'));
	unlockPopup();
}


function closeGlobalCityPopUp() {
    hideElement(document.getElementById('globalcity-popup'));
    unlockPopup();
}
function closeOnRoadPricePopUp() {
    resetOnRoadPricePopup();
    hideElement(document.getElementById('popupWrapper'));

    closeCityAreaSelectionPopup();
    unlockPopup();
}

function closeCityAreaSelectionPopup() {
	var popupContent = document.getElementById('popupContent');

	popupContent.style.left = '100%';
	popupContent.classList.remove('fix-input-field');
}

function openCityAreaSelectionPopup() {
	var popupContent = document.getElementById('popupContent');

	popupContent.style.left = '0';
	setTimeout(function () {
		popupContent.classList.add('fix-input-field');
	}, 500);
}

function closeGlobalCityPopUpByButtonClick() {
    hideElement(document.getElementById('globalcity-popup'));
    unlockPopup();
	window.history.back();	
}

function lockPopup() {
    document.body.classList.add('lock-browser-scroll');
    showElement(document.getElementsByClassName('blackOut-window')[0]);

}

function unlockPopup() {
    document.body.classList.remove('lock-browser-scroll');
    hideElement(document.getElementsByClassName('blackOut-window')[0]);
}

function hideElement(element) {
	try {
		element.style.display = 'none';
	}
	catch(e){}
}

function showElement(element) {
	try{
		element.style.display = 'block'	
	}
	catch(e) {}
	
}

var appendHash = function (state) {
    window.location.hash = state;
}


function getGlobalCity() {
    if(isServer()) {
        return null;
    }
	var cookieName = "location";
    var locationCookie = getCookie(cookieName);
    if (locationCookie) {
        locationCookie = (locationCookie.replace('-', ' ')).split("_");
        var cityName = locationCookie[1];
        var globalCityId = parseInt(locationCookie[0]); 
        return {
        	name : cityName,
        	id : globalCityId
        }

    }
    return null;
}

function setGlobalCity(cityId, cityName, globalCityId) {
    if (cityId != globalCityId) {
        SetCookieInDays("location", cityId + "_" + cityName, 365);
        bwcache.set("userchangedlocation", window.location.href, true);
    }
}

function IsGlobalCityPresent(cityList, globalCityId) {
    if (cityList != null && cityList.some(item => item.cityId === globalCityId)) {
        return true;
    }
    return false;
}

function getCookie(key) {
    var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');
    return keyValue ? keyValue[2] : null;
}

function isCookieExists(cookiename) {
    var coockieVal = document.cookie(cookiename);
    if (coockieVal == undefined || coockieVal == null || coockieVal == "-1" || coockieVal == "")
        return false;
    return true;
}

function getHost() {
    var host = document.domain;
    if (host.match("bikewale.com$"))
        host = ".bikewale.com";
    return host;
}

function SetCookieInDays(cookieName, cookieValue, nDays) {
    var today = new Date();
    var expire = new Date();
    expire.setTime(today.getTime() + 3600000 * 24 * nDays);
    cookieValue = cookieValue.replace(/\s+/g, '-');
    if (/MSIE (\d+\.\d+);/.test(navigator.userAgent) || /Trident\//.test(navigator.userAgent))
        document.cookie = cookieName + "=" + cookieValue + ";expires=" + expire.toGMTString() + '; path =/';
    else
        document.cookie = cookieName + "=" + cookieValue + ";expires=" + expire.toGMTString() + ';domain=' + getHost() + '; path =/';

    bwcache.remove("userchangedlocation", true);
}


function showHideMatchError(element, error) {
    if (error) {
    	document.getElementById('error-icon').classList.remove('hide');
    	document.getElementById('bw-blackbg-tooltip').classList.remove('hide');
    	element.classList.add('border-red');
    }
    else {
    	document.getElementById('error-icon').classList.add('hide');
    	document.getElementById('bw-blackbg-tooltip').classList.add('hide');
    	element.classList.remove('border-red');

    }
}


if(!isServer()) {
    try {
        docReady(function() {
            var ele = document.getElementsByClassName('blackOut-window');
            if(ele.length>0)
            {
                ele[0].addEventListener('mouseup',function(e) {  
                    var globalSearchPopup = document.getElementById('global-search-popup');
                
                    if(e.target.id !== globalSearchPopup.getAttribute('id')) // TODO && !globalSearchPopup.has(e.target).length)
                    {
                        hideElement(globalSearchPopup);
                        unlockPopup();
                    }

                }) 
            }

            window.addEventListener('hashchange' ,function(e) {
                var oldUrl, oldHash;
                oldUrl = e.oldURL;
                if (oldUrl && (oldUrl.indexOf('#') > 0)) {
                    oldHash = oldUrl.split('#')[1];
                    closePopUp(oldHash);
                };
            });

            
        })    
    }
    catch(err) {}
    
}

var recentSearches =
{
    searchKey: "recentsearches",
    trendingKey: "trendingbikes",
    options: {
        bikeSearchEle: isServer() ? null :  document.getElementById('globalSearch'),
        recentSearchesEle: isServer() ? null : ((document.getElementById("new-global-recent-searches") && document.getElementById("new-global-recent-searches").length) ? document.getElementById("new-global-recent-searches") : document.getElementById("global-recent-searches")),
        recentSearchesLoaded: false
    },
    saveRecentSearches: function (opt) {
        if (opt && opt.payload && opt.payload.makeId > 0) {
            var objSearches = bwcache.get(this.searchKey) || {};
            opt.payload["name"] = opt.label;
            objSearches.searches = objSearches.searches || [];
            var eleIndex = this.objectIndexOf(objSearches.searches, opt.payload);
            if (objSearches.searches != null && eleIndex > -1) objSearches.searches.splice(eleIndex, 1);
            objSearches.searches.unshift(opt.payload);

            objSearches["lastModified"] = new Date().getTime();
            if (objSearches.searches.length > 3)
                objSearches.searches.pop();
            objSearches["noOfSearches"] = objSearches.searches.length;
            bwcache.set(this.searchKey, objSearches);
        }
    },
    showRecentSearches: function (showRecentSearchList) {
            var objSearches = bwcache.get(this.searchKey);
            if (objSearches && objSearches.searches) {
                showRecentSearchList(objSearches.searches);
            }
    },
    getRecentSearches: function (showRecentSearchList) {
            var objSearches = bwcache.get(this.searchKey);
            if (objSearches && objSearches.searches) {
                return objSearches.searches;
            }
            else return null;
    },
    showTrendingSearches: function(showTrendingSearchList) {
        var objSearches = bwcache.get(this.trendingKey);
        if (objSearches) {
            showTrendingSearchList(objSearches);
        }
    },
    getTrendingSearches: function (showTrendingSearchList) {
        var trendingSearches = bwcache.get(this.trendingKey);
        if (!trendingSearches) {
            var xhr = new XMLHttpRequest();
            xhr.open('GET', "/api/popularbikes/?topCount=" + topCount);
            xhr.setRequestHeader("Content-Type","application/json; charset=utf-8");
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4) {
                    if (xhr.status == 200) {
                        var response = JSON.parse(xhr.responseText);
                        if (response != null) {
                            trendingSearches = response;
                            localStorage.setItem("bwc_trendingbikes", JSON.stringify(trendingSearches));
                            showTrendingSearchList()
                        }
                    }
                }
            }
            xhr.send();
        }
        return trendingSearches;
    },
    objectIndexOf: function (arr, opt) {
        var makeId = opt.makeId, modelId = opt.modelId;
        for (var i = 0, len = arr.length; i < len; i++)
            if (arr[i]["makeId"] === opt.makeId && arr[i]["modelId"] === opt.modelId) return i;
        return -1;
    }
};  


function closePopUp(state) {
    switch (state) { 
        case "globalCity":
            closeGlobalCityPopUp();
            break;
        case "onRoadPrice":
            closeOnRoadPricePopUp();
            break;
        default:
            if (window.popupCallback != undefined && window.popupCallback[state] != undefined) {
                window.popupCallback[state]();
                window.popupCallback[state] = undefined;
                unlockScroll();
            }
            return true;
    }
};


function getStrippedTerm(term) {
	try {
		return term.replace(/^\s\s*/, '').replace(/\s\s*$/, '').replace(/-/g, ' ').replace(/[^A-Za-z0-9 ]/g, '').toLowerCase().trim();	
	}
	catch(e) {
		return '';
	}
	
}

function autocomplete(options,term) {
	var orgTerm = term;
    var reqTerm = getStrippedTerm(orgTerm);

    var year = options.year;
    if (year != null && year != undefined && year != '')
        year = year.val(); 
    else
        year = '';

    var cacheProp = reqTerm + '_' + year;
    if (!(cacheProp in options.cache) && reqTerm.length > 0) {
        showElement(document.getElementById("loaderGlobalCity"));
        var indexToHit = options.source;
        var count = options.recordCount;
        var path = "/api/AutoSuggest/?source=" + indexToHit + "&inputText=" + encodeURIComponent(reqTerm) + "&noofrecords=" + count;

        options.cache[cacheProp] = new Array();

        var xhr = new XMLHttpRequest();
        xhr.open('GET',path);
        xhr.setRequestHeader("Content-Type","application/json; charset=utf-8");
    	if (options.loaderStatus != null && typeof (options.loaderStatus) == "function") 
			options.loaderStatus(false);
        	
		xhr.onreadystatechange = function() {
			
        	if(xhr.readyState == 4) { 
        		if(xhr.status == 200) {	
					hideElement(document.getElementById("loaderGlobalCity"));
					var response = JSON.parse(xhr.responseText);  // TODO remve comment
				   var jsonData = response.suggestionList; 
					options.cache[reqTerm + '_' + year] = jsonData.map(function (item) {
	                    return { label: item.text, payload: item.payload }
	                });
	                var result = options.cache[cacheProp];
	               
	                if (options.afterfetch != null && typeof (options.afterfetch) == "function") 
	                	options.afterfetch(result, reqTerm);
        		} 
        		else {
        			var result = undefined;
                    options.afterfetch(result, reqTerm);
	               

        		}	
        		if (options.loaderStatus != null && typeof (options.loaderStatus) == "function") 
        			options.loaderStatus(true);
        	}
        }
        xhr.send();


    }
    else {
        var result = options.cache[cacheProp];
        if (options.afterfetch != null && typeof (options.afterfetch) == "function") 
        	options.afterfetch(result, reqTerm);
    }

}

function highlightText(text,matchingText) {
	var matcher = new RegExp("(" + matchingText.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, "\\$&") + ")", "ig");
    return text.replace(matcher, "<strong>$1</strong>");
}

function setPriceQuoteFlag() {
    hideElement(document.getElementById('global-search-popup'));
    unlockPopup();
    IsPriceQuoteLinkClicked = true; 
    
}

function checkCookies() {
    var c = document.cookie.split('; ');
    for (var i = c.length - 1; i >= 0; i--) {
        var C = c[i].split('=');
        if (C[0] == "location") {
            var cData = (String(C[1])).split('_');
            onCookieObj.PQCitySelectedId = parseInt(cData[0]) || 0;
            onCookieObj.PQCitySelectedName = cData[1] ? cData[1].replace(/-/g, ' ') : "";
            onCookieObj.PQAreaSelectedId = parseInt(cData[2]) || 0;
            onCookieObj.PQAreaSelectedName = cData[3] ? cData[3].replace(/-/g, ' ') : "";
        }
    }
}

function setDataForPriceQuotePopup(event,bikeObj) {
    checkCookies();
    var item = bikeObj;
    if(bikeObj != null && (bikeObj.payload != null || bikeObj.payload != undefined)) {
        item = bikeObj.payload;
    }
    onRoadPricePopupDataObject.SelectedModelId = (item.modelId != null && item.modelId != undefined) ? item.modelId : 0;
    onRoadPricePopupDataObject.SelectedCity = (onCookieObj.PQCitySelectedId > 0)?{ 'id': onCookieObj.PQCitySelectedId, 'name': onCookieObj.PQCitySelectedName }:null;
    onRoadPricePopupDataObject.SelectedArea = (onCookieObj.PQAreaSelectedId > 0)?{ 'id': onCookieObj.PQAreaSelectedId, 'name': onCookieObj.PQAreaSelectedName }:null;
    onRoadPricePopupDataObject.SelectedCityId = (item.preselcity !=null && item.preselcity != undefined ? item.preselcity : 0) || onCookieObj.PQCitySelectedId || 0;
    onRoadPricePopupDataObject.SelectedAreaId = onCookieObj.PQAreaSelectedId || 0;
    onRoadPricePopupDataObject.BookingCities = [],
    onRoadPricePopupDataObject.BookingAreas = [],
    onRoadPricePopupDataObject.ModelName = item.modelName != null && item.modelName!=null ? item.modelName : "";
    onRoadPricePopupDataObject.MakeName = item.makeName != null && item.makeName!=null ? item.makeName : "";
    onRoadPricePopupDataObject.PageCatId = item.pagecatId != null && item.pagecatId!=null ? item.makeName : "";
    onRoadPricePopupDataObject.IsPersistence = item.persistent != undefined && item.persistent != null ? item.persistent : false;
    onRoadPricePopupDataObject.IsReload = item.reload != undefined && item.reload != null ? item.reload : false;
    if(onRoadPricePopupDataObject.SelectedCityId == 0 )
        onRoadPricePopupDataObject.LoadingText = 'Fetching Cities...';
    if(onRoadPricePopupDataObject.IsPersistence )
        onRoadPricePopupDataObject.LoadingText = 'Loading locations...';
   
    showElement(document.getElementById('popupWrapper'));
    showElement(document.getElementById('popupContent'));
    document.getElementById('popupWrapper').classList.add('loader-active');
    
    if(window.location.hash != '') {
        window.location.hash ='';
    }
    appendHash("onRoadPrice");
   
}

var popupState = {
    popupClosed : "popup-closed",
    cityPopupOpen : "city-popup-open",
    areaPopupOpen : "area-popup-open"
}
function resetOnRoadPricePopup() {

    onRoadPricePopupDataObject.SelectedModelId = 0,
    onRoadPricePopupDataObject.SelectedCity = null , 
    onRoadPricePopupDataObject.SelectedArea = null , 
    onRoadPricePopupDataObject.HasAreas = false,
    onRoadPricePopupDataObject.SelectedCityId = 0 , 
    onRoadPricePopupDataObject.SelectedAreaId = 0,
    onRoadPricePopupDataObject.BookingCities = [],
    onRoadPricePopupDataObject.BookingAreas = [],
    onRoadPricePopupDataObject.MakeName = "",
    onRoadPricePopupDataObject.ModelName = "",
    onRoadPricePopupDataObject.PageCatId = "",
    onRoadPricePopupDataObject.DealerId = "",
    onRoadPricePopupDataObject.VersionId = "",
    onRoadPricePopupDataObject.IsPersistence = false,
    onRoadPricePopupDataObject.IsReload = false,
    onRoadPricePopupDataObject.LoadingText = "",
    onRoadPricePopupDataObject.state = popupState.cityPopupOpen

    
}

var onRoadPricePopupDataObject = {
    SelectedModelId : 0,
    SelectedCity : null , 
    SelectedArea : null , 
    HasAreas : false,
    SelectedCityId : 0 , 
    SelectedAreaId : 0,
    BookingCities : [],
    BookingAreas : [],
    MakeName : "",
    ModelName : "",
    PageCatId : "",
    DealerId : "",
    VersionId : "",
    IsPersistence : false,
    IsReload : false,
    LoadingText : "",
    state : popupState.cityPopupOpen
}


function GetGlobalCityArea() {
    var cookieName = "location";
    var cityArea = getCookie(cookieName);
    if(cityArea != null) {
        cityArea = cityArea.replace(/[0-9](_)*/g, '').replace(/-+/g, ' ');
    }
    else {
        cityArea = '';
    }   
    return cityArea;
}

function gtmCodeAppender(pageId, action, label) {
    var category = '';
    if (pageId != null) {
        switch (pageId) {
            case "1":
                category = 'Make_Page';
                break;
            case "2":
                category = "CheckPQ_Series";
                action = "CheckPQ_Series_" + action;
                break;
            case "3":
                category = "Model_Page";
                action = "CheckPQ_Model_" + action;
                break;
            case '4':
                category = 'New_Bikes_Page';
                break;
            case '5':
                category = 'HP';
                break;
            case '6':
                category = 'Search_Page';
                break;
        }
        if (label) {
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': action, 'lab': label });
        }
        else {
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': action });
        }
    }

}

function MakeModelRedirection(item ) {
    if (!IsPriceQuoteLinkClicked) {
        if(item.payload != null) {
            var make = new Object();
            make.maskingName = item.payload.makeMaskingName;
            make.id = item.payload.makeId;
            var model = null;
            if (item.payload.modelId > 0) {
                model = new Object();
                model.maskingName = item.payload.modelMaskingName;
            }
            recentSearches.saveRecentSearches(item);
            closeGlobalSearchPopUp();
            if (model != null && model != undefined) {
                window.location.href = "/m/" + make.maskingName + "-bikes/" + model.maskingName + "/";
                return true;
            } else if (make != null && make != undefined) {
                window.location.href = "/m/" + make.maskingName + "-bikes/";
                return true;
            }
        }
	}
    if(IsPriceQuoteLinkClicked) {
        IsPriceQuoteLinkClicked = false;
    }
    
}

function openPopupWithHash(openingFunction, closingFunction, hash) {
    try {
        if (typeof openingFunction === "function" && typeof closingFunction === "function") {
            openingFunction();
            appendHash(hash);
            if (window.popupCallback == undefined) {
                window.popupCallback = {};
                window.popupCallback[hash] = closingFunction;
            }
            else {
                window.popupCallback[hash] = closingFunction;
            }
            lockScroll();
        }
    }
    catch (e) {
        console.log(e);
    }
}

function closePopupWithHash(closingFunction) {
    try {
        if (typeof closingFunction === "function") {
            unlockScroll();
            closingFunction();
            if (window.popupCallback != undefined) {
                let hash = (window.location.hash || "#").slice(1);
                if (hash.length > 0) {
                    window.popupCallback[hash] = undefined;
                    appendHash('');
                }
            }
        }
    }
    catch (e) {
        console.log(e);
    }
}


var globalCityCache = new Object(); // variable for global city autocomplete
var globalSearchCache = new Object(); // variable for global search autocomplete
var pqSourceId = "38";
var globalSearchStatus = {
	RESET : 0,
	ERROR : 1,
	RECENTSEARCH : 2 ,
	AUTOCOMPLETE : 3
};

var  IsPriceQuoteLinkClicked = false;
var onCookieObj = new Object();
module.exports = {
	closeGlobalSearchPopUp,
	closeGlobalCityPopUp,
	closeGlobalCityPopUpByButtonClick,
	lockPopup,
	unlockPopup,
	hideElement,
	showElement,
	recentSearches,
	SetCookieInDays,
	autocomplete,
	showHideMatchError,
	globalCityCache,
	globalSearchCache,
	pqSourceId,
	getGlobalCity,
	highlightText,
	getStrippedTerm,
	setPriceQuoteFlag,
	MakeModelRedirection,
	globalSearchStatus,
    onCookieObj,
    setDataForPriceQuotePopup,
    onRoadPricePopupDataObject,
    getCookie,
    GetGlobalCityArea,
    gtmCodeAppender,
    popupState,
    resetOnRoadPricePopup,
    closeCityAreaSelectionPopup,
    openCityAreaSelectionPopup,
    setGlobalCity,
    IsGlobalCityPresent,
    openPopupWithHash,
    closePopupWithHash
}