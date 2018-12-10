var masterCityId;
var masterCityName;
var currentSuggestResult = [];
var currentObjFromSuggest;
var suggestedFromList=false;
var ISAUTOCOMPLETE;
var fibonacci = [2, 3, 5, 8, 13, 21]
var fibonacciVal;
var day;

//below code will display the city popup box
function popUphtml() {
    ISAUTOCOMPLETE = "";
    $("#message").hide();
    $("#errMessage").hide();
    setTimeout(function () {
		$("#findCityContentNew").show();
		$("#findCitypopup").show();
        $("#cwCitypopup").attr("placeholder", "-- Type to Select Your City --");
        $("#findCityContentNew").prev().show();
        $("#cwCitypopup").val("");
        var label = ($.cookie("_CustCityMaster") == "" || $.cookie("_CustCityMaster") == null) ? "auto-display-" + CityNameSelectedbyIp.toLowerCase() : "manual-display-" + $.cookie("_CustCityMaster").toLowerCase();
        dataLayer.push({
            event: "master-city-popup",
            cat: "mastercitypopup",
            act: "popup-open",
            lab: label
        });
        //below code will set cookie value to city selected by geo-ip location as no city is selected by user when user visits first time.
        if (CitySelectedbyIP != "" && ($.cookie("_CustCityMaster") == "" || $.cookie("_CustCityMaster") == null) && CitySelectedbyIP > 0) {
            $("#autoCity").text(CityNameSelectedbyIp);
            $("#autoText").text(" (Confirm city or click to change)");
            setCookies();
            if ($.cookie("_CustCityMaster") != "" || $.cookie("_CustCityMaster") != null || $.cookie("_CustCityMaster") != "Select City") {
                $("#cwCitypopup").attr("placeholder", $.cookie("_CustCityMaster"));
            }
            dataLayer.push({
                event: "master-city-popup",
                cat: "mastercitypopup",
                act: "geo-located-shown",
                lab: CityNameSelectedbyIp.toLowerCase()
            });
            SetCityByPriority();
        }
        else {
            if ($.cookie("_CustCityMaster") != null) {
                if ($.cookie("_CustCityMaster") != "Select City") {
                    $("#cwCitypopup").attr("placeholder", $.cookie("_CustCityMaster"));
                    $("#cwCitypopup").val($.cookie("_CustCityMaster")).change();
                } else {
                    $("#cwCitypopup").attr("placeholder", "-- Type to Select Your City --");
                }
            }
            $("#autoDetectedCity").hide();
            $("#cwCitypopup").show();
            $("#cwCitypopup").focus();
        }
        //showAnimation();
    }, 0);
}

//below function will set usercity cookies for seven month
function setCookies() {
    var now = new Date();
    var Time = now.getTime();
    Time += 1000 * 60 * 60 * 5040;
    now.setTime(Time);
    if (typeof cityDrpId != 'undefined') {
        document.cookie = '_CustCityPopUp=' + $.cookie('_CustCityMaster') + "|" + $("#" + cityDrpId + ' option:selected').text() + "|" + CityNameSelectedbyIp + '; expires = ' + now.toGMTString() + '; path =/';
        document.cookie = '_CustCityIdPopUp=' + $.cookie('_CustCityIdMaster') + "|" + $("#" + cityDrpId).val() + "|" + CitySelectedbyIP + '; expires = ' + now.toGMTString() + '; path =/';
    }
    else {
        document.cookie = '_CustCityPopUp=' + $.cookie('_CustCityMaster') + "|" + "" + "|" + CityNameSelectedbyIp + '; expires = ' + now.toGMTString() + '; path =/';
        document.cookie = '_CustCityIdPopUp=' + $.cookie('_CustCityIdMaster') + "|" + "-1" + "|" + CitySelectedbyIP + '; expires = ' + now.toGMTString() + '; path =/';
    }
}

//below function will set the cookie for seven month
function cookieSet() {
    var now = new Date();
    var Time = now.getTime();
    Time += 1000 * 60 * 60 * 5040;
    now.setTime(Time);
    document.cookie = '_CustCityMaster=' + masterCityName + '; expires = ' + now.toGMTString() + "; domain=" + defaultCookieDomain + '; path =/';
    document.cookie = '_CustCityIdMaster=' + masterCityId + '; expires = ' + now.toGMTString() + "; domain=" + defaultCookieDomain + '; path =/';

    $("#findCityContentNew").prev().hide();
    $("#findCityContentNew").hide();
    $("#cwTopCityBox").text($.cookie("_CustCityMaster"));
    
    //below code will set usercity cookies each time cookieset function is called
      SetCityByPriority();
    //location.reload();
}

var objCity = new Object();
$(document).ready(function () {
    var label = null;
    var id = null;
    //below code will check either cookie is set or not if not then popup will display
    if (($.cookie("_CustCityMaster") == "" || $.cookie("_CustCityMaster") == null) && checkpath()) {
		// for IE7 city popup hide
		if($.browser.msie && parseFloat($.browser.version) < 8 && ($.cookie("_ie7_recommendation") == "" || $.cookie("_ie7_recommendation") == null)){
			$("#findCityContentNew").hide();
			$("#findCitypopup").hide();
		}else{
			popUphtml();
		}
    }
    else {
        $("#findCityContentNew").prev().hide();
    }
    function checkpath() {
        if (window.location.pathname.indexOf("/used/") == 0) return false;
        else if (window.location.pathname.indexOf("AllOffers.aspx") != -1) return false;
        else if (window.location.pathname.indexOf("alloffers.aspx") != -1) return false;
        else if (window.location.pathname.indexOf("prices.aspx") != -1) return false;
        else if (window.location.pathname.indexOf("quotation.aspx") != -1) return false;
        else if (window.location.pathname.indexOf("/dealer/") == 0) return false;
        else if (window.location.pathname.indexOf("/users/") == 0) return false;
        else if (window.location.pathname.indexOf("/Users/") == 0) return false;
        else if (window.location.pathname.indexOf("sitemap.aspx") != -1) return false;
        else if (window.location.pathname.indexOf("aboutus.aspx") != -1) return false;
        else if (window.location.pathname.indexOf("/media/") == 0) return false;
        else if (window.location.pathname.indexOf("/mycarwale/") == 0) return false;
        else if (window.location.pathname.indexOf("/MyCarwale/") == 0) return false;
        else if (window.location.pathname.indexOf("/mygarage/") == 0) return false;
        else if (window.location.pathname.indexOf("/MyGarage/") == 0) return false;
        else if (window.location.pathname.indexOf("/community/") == 0) return false;
        else if (window.location.pathname.indexOf("/Community/") == 0) return false;
        else if (window.location.pathname.indexOf("contactus.aspx") != -1) return false;
        else if (window.location.pathname.indexOf("visitoragreement.aspx") != -1) return false;
        else if (window.location.pathname.indexOf("privacypolicy.aspx") != -1) return false;
        else if (window.location.pathname.indexOf("PrivacyPolicy.aspx") != -1) return false;
        else if (window.location.pathname.indexOf("carwalestory.aspx") != -1) return false;
        else if (window.location.pathname.indexOf("/award/") == 0) return false;
        else if (window.location.pathname.indexOf("/PressReleases/") == 0) return false;
        else if (window.location.pathname.indexOf("/pressreleases/") == 0) return false;
        else if (window.location.pathname.indexOf("visitoragreement.aspx") != -1) return false;
        else if (window.location.pathname.indexOf("career.aspx") != -1) return false;
        else if (window.location.pathname.indexOf("advertiseWithUs.aspx") != -1) return false;
        else if (window.location.pathname.indexOf("TermsConditions.aspx") != -1) return false;
        else if (window.location.pathname.indexOf("refundpolicy.aspx") != -1) return false;
        else if (window.location.pathname.indexOf("refundfaq.aspx") != -1) return false;
        else if (window.location.pathname.indexOf("offerTermsAndConditions.aspx") != -1) return false;
        else if (window.location.pathname.match(/(.*)offers(.*)/) != null) return false;
        return true;
    }

    //below code will display the cityName in span of heading
    if ($.cookie("_CustCityMaster") != "" || $.cookie("_CustCityMaster") != null) {
        var a = $.cookie("_CustCityMaster");
        $("#cwTopCityBox").text(a);

        //below code will set usercity cookies each time on ready 
        if ($.cookie("_CustCityIdPopUp") != "" && $.cookie("_CustCityIdPopUp") != null) {
            SetCityByPriority();
        }
    }

    //below code will display the select city hard coded in header when user first open the site
    if ($.cookie("_CustCityMaster") == "" || $.cookie("_CustCityMaster") == null || $.cookie("_CustCityMaster") == "-1") {
        $("#cwTopCityBox").html("Select City");
    }

    //below code is for the funtionality of autocomplete
    $('#cwCitypopup').cw_autocomplete({
        width: 195,
        source: ac_Source.allCarCities,
        onClear: function () {
            objCity = new Object();

        },
        click: function (event, ui, orgTxt) {            
            objCity.Name = (ui.item.label);
            objCity.Id = ui.item.id;
            label = (ui.item.label);
            id = ui.item.id;
            masterCityId = id;
            masterCityName = label;
            dataLayer.push({
                event: "master-city-popup",
                cat: "mastercitypopup",
                act: "autocomplete-click",
                lab: masterCityName.toLowerCase()
            });
            var now = new Date();
            var Time = now.getTime();
            Time += 1000 * 60 * 60 * 5040;
            now.setTime(Time);
            if (typeof cityDrpId != 'undefined') {
                document.cookie = '_CustCityPopUp=' + masterCityName + "|" + $("#" + cityDrpId + ' option:selected').text() + "|" + CityNameSelectedbyIp + '; expires = ' + now.toGMTString() + '; path =/';
                document.cookie = '_CustCityIdPopUp=' + masterCityId + "|" + $("#" + cityDrpId).val() + "|" + CitySelectedbyIP + '; expires = ' + now.toGMTString() + '; path =/';
            }
            else {
                document.cookie = '_CustCityPopUp=' + masterCityName + "|" + "" + "|" + CityNameSelectedbyIp + '; expires = ' + now.toGMTString() + '; path =/';
                document.cookie = '_CustCityIdPopUp=' + masterCityId + "|" + "-1" + "|" + CitySelectedbyIP + '; expires = ' + now.toGMTString() + '; path =/';
            }
            cookieSet();
            return true;
        },
        open: function (result, a, b) {
            currentSuggestResult = result;
            if (currentSuggestResult.length == 1 || $.trim($("#cwCitypopup").val().toLowerCase()) == $.trim(currentSuggestResult[0].label.toLowerCase())) { suggestedFromList = true; currentObjFromSuggest = currentSuggestResult[0]; }
            else { suggestedFromList = false;}
            ISAUTOCOMPLETE = result;
            validate();
            //ISAUTOCOMPLETE = "";

        },
        keyup: function (event, ui, orgTxt) {
            var selectednode = $(".ui-autocomplete li .ui-state-focus");
            if (selectednode.length > 0) {
                currentObjFromSuggest = findFromSuggestion(selectednode.text());
                suggestedFromList = true;
            }
            else { suggestedFromList = false; }
            if (!validate()) { $('#message').show(); }
            if ($("#errMessage").is(":visible")) $("#errMessage").hide();

        },
        focusout: function (a,b,c) {
            if ((objCity.Name == undefined || objCity.Name == null || objCity.Name == '') && objCity.result != undefined && objCity.result != null && objCity.result.length > 0) {
                
            } else {
                
            }
        }

    });

    //.close-icon-md-cp
    //below code is  for close icon  and check the cookie status 
    $('.city-content-close').click(function () {
        closeMasterPopup("x-button");
        $("#cwTopCityBox").text($.cookie("_CustCityMaster"));
        return false;
    });
    
    //Confirm City Button click
    $("#btnConfirmCity").click(function () {

        if (suggestedFromList && currentObjFromSuggest != null && $('#message').is(":visible")==false) {

            var now = new Date();
            var Time = now.getTime();
            Time += 1000 * 60 * 60 * 5040;
            now.setTime(Time);
            document.cookie = '_CustCityMaster=' + currentObjFromSuggest.label + '; expires = ' + now.toGMTString() + "; domain=" + defaultCookieDomain + '; path =/';
            document.cookie = '_CustCityIdMaster=' + currentObjFromSuggest.id + '; expires = ' + now.toGMTString() + "; domain=" + defaultCookieDomain + '; path =/';

            setCookies();
            SetCityByPriority();
            $("#findCityContentNew").prev().hide();
            $("#findCityContentNew").hide();
            $("#errMessage").hide();
            suggestedFromList = false;
            
            dataLayer.push({
                event: "master-city-popup",
                cat: "mastercitypopup",
                act: "scrolled-n-confirmclick",
                lab: currentObjFromSuggest.label
            });
            return true;
        }
        
        if ($('#message').is(":visible")) {
            dataLayer.push({
                event: "master-city-popup",
                cat: "mastercitypopup",
                act: "confirm-validation-error",
                lab: $.trim($("#cwCitypopup").val().toLowerCase())
            });
            return false;
        }
        if ($.trim($("#cwCitypopup").val().toLowerCase()) == "-- type to select your city --") {
            $("#message").hide();
            $("#errMessage").show();
            dataLayer.push({
                event: "master-city-popup",
                cat: "mastercitypopup",
                act: "confirm-validation-error",
                lab: $.trim$(("#cwCitypopup").val().toLowerCase())
            });
            return false;
        }
        else if ($.cookie("_CustCityMaster")!=null&& $.trim($.cookie("_CustCityMaster").toLowerCase()) == $.trim($("#cwCitypopup").val().toLowerCase())) {
            $("#findCityContentNew").prev().hide();
            $("#findCityContentNew").hide();
            $("#errMessage").hide();
            dataLayer.push({
                event: "master-city-popup",
                cat: "mastercitypopup",
                act: "confirm-click-success",
                lab: "reconfirm-already-set-mastercity"
            });
            return true;
        }
        else if (CityNameSelectedbyIp!=""&&$.trim($("#autoCity").text().toLowerCase())!=""&&$.trim($("#autoCity").text().toLowerCase()) == $.trim(CityNameSelectedbyIp.toLowerCase())) {
            var now = new Date();
            var Time = now.getTime();
            Time += 1000 * 60 * 60 * 5040;
            now.setTime(Time);
            document.cookie = '_CustCityMaster=' + CityNameSelectedbyIp + '; expires = ' + now.toGMTString() + "; domain=" + defaultCookieDomain + '; path =/';
            document.cookie = '_CustCityIdMaster=' + CitySelectedbyIP + '; expires = ' + now.toGMTString() + "; domain=" + defaultCookieDomain + '; path =/';

            setCookies();
            SetCityByPriority();
            $("#findCityContentNew").prev().hide();
            $("#findCityContentNew").hide();
            $("#errMessage").hide();
            dataLayer.push({
                event: "master-city-popup",
                cat: "mastercitypopup",
                act: "geolocated-confirmed",
                lab: $.trim(CityNameSelectedbyIp.toLowerCase())
            });
            return true;
        }
        else { $("#message").show(); return false; }
    });
    //below code will hide autodetected city and show text box for autocomplete city selection
    $("#autoDetectedCity").click(function () {
        $("#autoDetectedCity").hide();
        $("#cwCitypopup").show();
        $("#cwCitypopup").focus();
    });
});
function validate() {
    var cityval = $("#cwCitypopup").val();
    if (cityval != null || cityval != undefined) {
        if (cityval.length == 0) {
            //$("#errMessage").show();
            $("#message").show();
            return false;
        } else if (currentSuggestResult!=null) {
            if (currentSuggestResult.length == 1 && currentSuggestResult[0].label == cityval) {
                $("#message").hide();
                return true;
            }
            else if (currentSuggestResult.length < 1 || findFromSuggestion($("#cwCitypopup").val()) == undefined) {
                $("#message").show();
                return false;
            }
        }
    }
    if (currentSuggestResult != null && currentSuggestResult.length < 1) $("#message").show();
    if (ISAUTOCOMPLETE == '' || ISAUTOCOMPLETE == null || ISAUTOCOMPLETE == undefined) {
        $("#message").show();
        $("#errMessage").hide();
        return false;
    }
    $("#message").hide();
    return true;
}

//this function is to select the  popular city onclick and save the cookie 
$(function () {
    $('#userPopularCities li').click(function () {
        suggestedFromList = false;
        masterCityName = $.trim($(this).text());
        masterCityId = $(this).attr('masterCityId');
        dataLayer.push({
            event: "master-city-popup",
            cat: "mastercitypopup",
            act: "popularcities-click",
            lab: masterCityName.toLowerCase()
        });
        var action = $('#autoCity').text().toLowerCase() != masterCityName.toLowerCase() ? "geolocated-but-diff-popularcity" : "geolocated-same-popularcity-click";
        var label=(action=="geolocated-but-diff-popularcity")?$('#autoCity').text().toLowerCase()+"-to-"+masterCityName.toLowerCase():masterCityName.toLowerCase();
        if ($('#autoText').is(':visible')) {
            dataLayer.push({
                event: "master-city-popup",
                cat: "mastercitypopup",
                act: action,
                lab: label
            });
        }
        $('#cwCitypopup').val("");
        var input = $('#cwCitypopup');
        input.val(input.val() + masterCityName);
        var now = new Date();
        var Time = now.getTime();
        Time += 1000 * 60 * 60 * 5040;
        now.setTime(Time);
        if (typeof cityDrpId != 'undefined') {
            document.cookie = '_CustCityPopUp=' + masterCityName + "|" + $("#" + cityDrpId + ' option:selected').text() + "|" + CityNameSelectedbyIp + '; expires = ' + now.toGMTString() + '; path =/';
            document.cookie = '_CustCityIdPopUp=' + masterCityId + "|" + $("#" + cityDrpId).val() + "|" + CitySelectedbyIP + '; expires = ' + now.toGMTString() + '; path =/';
        }
        else {
            document.cookie = '_CustCityPopUp=' + masterCityName + "|" + "" + "|" + CityNameSelectedbyIp + '; expires = ' + now.toGMTString() + '; path =/';
            document.cookie = '_CustCityIdPopUp=' + masterCityId + "|" + "-1" + "|" + CitySelectedbyIP + '; expires = ' + now.toGMTString() + '; path =/';
        }
        cookieSet();
        return true;

    });
});
//below Code  will close the popup when clicked outside anywhere except popup box and set cookie value -2 if cookie is Null or Empty
$(document).bind('click touch', function (event) {
    if (!$(event.target).parents().addBack().is('#findCityContentNew') && $(event.target).parents().addBack().is('#findCitypopup')) {
        closeMasterPopup('outside-click');
    }
});

function findFromSuggestion(selectedCity) {
    var ret;
    if (currentSuggestResult.length > 0) {
        for (i = 0; i < currentSuggestResult.length; i++) {
            if ($.trim(currentSuggestResult[i].label.toLowerCase()) == $.trim(selectedCity.toLowerCase())) {
                ret = currentSuggestResult[i];
                break;
            }
        }
    }
    else {
        ret = new Object();
        ret.Name = $.cookie("_CustCityMaster");
        ret.Id = $.cookie("_CustCityIdMaster");
    }
    return ret;
}
function SetCityByPopUp() {
    setCookies();
    SetCityByPriority();
}
//below code will set city according to usercity cookie selected.
//Priority for city selection in desc order of Master City,User Action City, Geo-Ip detected City
function SetCityByPriority() {
    if ($.cookie("_CustCityIdPopUp") != "-1") {
        var arrCity = $.cookie("_CustCityIdPopUp").split("|");
        var arrCityName = $.cookie("_CustCityPopUp").split("|");
        if (arrCity.length > 0) {
            if (arrCity[0].toString() != "" && arrCity[0] != "null" && arrCity[0] != "-1" && arrCity[0] != "-2") {
                
                if (typeof cityDrpId != 'undefined') {
                    $("#" + cityDrpId).val(arrCity[0].toString());
                }
                $("#cwTopCityBox").text(arrCityName[0].toString());        //set cityname confirmed by user in TopCityBox
            }
            else if (arrCity[1].toString() != "" && arrCity[1] != "null" && arrCity[1] > 0) {
                if (typeof cityDrpId != 'undefined') {
                    $("#" + cityDrpId).val(arrCity[1].toString());
                }
            }
            else if (arrCity[2].toString() != "" && arrCity[2] != "null" && arrCity[2] > 0) {
                if (typeof cityDrpId != 'undefined') {
                    $("#" + cityDrpId).val(arrCity[2].toString());         //set cityname detected by GeoIP in dropdown list
                }
                $("#autoCity").text(arrCityName[2].toString());           //set cityname detected by GeoIP in autocomplete textbox
                $("#autoText").text(" (Confirm city or click to change)");

            }
        }
        if (typeof cityDrpId != 'undefined') {
            if (cityDrpId == "ucLocateDealer_cmbCity" || cityDrpId == "cmbCity") {
                if ($("#" + cityDrpId).val() > 0) {
                    BindDealerMakeByCity(cityDrpId);
                }
            }
        }
    }
}
function fibonacciTracker() {
    var ExpiryDate = new Date();
    var TimeNow = ExpiryDate.getTime() + 1000 * 60 * 60 * 5040;
    ExpiryDate.setTime(TimeNow);
    var days = 0;

    if ($.cookie("_FibonacciCounter") == "" || $.cookie("_FibonacciCounter") == null) {
        document.cookie = '_FibonacciCounter=' + "0" + '; expires = ' + ExpiryDate.toGMTString() + '; path =/';
        fibonacciVal = 0;
        days = fibonacci[fibonacciVal];
    }
    else if (parseInt($.cookie("_FibonacciCounter")) <= 4) {
        fibonacciVal = parseInt($.cookie("_FibonacciCounter")) + 1;
        document.cookie = '_FibonacciCounter=' + fibonacciVal.toString() + '; expires = ' + ExpiryDate.toGMTString() + '; path =/';
        days = fibonacci[fibonacciVal];
    }
    else {
        days = 30;
    }
    return days;
}
function setCookie(CustCityMaster, CustCityIdMaster) {
    day = fibonacciTracker();
    var now = new Date();
    var Time = now.getTime();
    Time += 1000 * 60 * 60 * 24 * day;
    now.setTime(Time);
    document.cookie = '_CustCityMaster=' + CustCityMaster + '; expires = ' + now.toGMTString() + "; domain=" + defaultCookieDomain + '; path =/';
    document.cookie = '_CustCityIdMaster=' + CustCityIdMaster + '; expires = ' + now.toGMTString() + "; domain=" + defaultCookieDomain + '; path =/';

}

function closeMasterPopup(closeSource){	   
	if ($.cookie("_CustCityMaster") == "" || $.cookie("_CustCityMaster") == null) {
            setCookie("Select City", "-1");
    }
	$("#findCityContentNew").prev().hide();
	$("#findCityContentNew").hide();
		dataLayer.push({
			event: "master-city-popup",
			cat: "mastercitypopup",
			act: "popup-close",
			act: closeSource
	});
	suggestedFromList = false;	
}
$(document).keydown(function(e){
	// ESCAPE key pressed
    if (e.keyCode == 27) {
		closeMasterPopup('escape button');
	}
});