var masterCityId;
var masterCityName;
var currentSuggestResult = [];
var currentObjFromSuggest;
var ISAUTOCOMPLETE;
var counter = 0;
var iphone = 0;
var cookieVal;
var changeCityIconShow = true;
function popupReady() {
    popUphtml();

    var objCity = new Object();
    $('#select-city-auto').cw_autocomplete({
        prependTo: '.ui-front',
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
            cookieSet();
            $(".m-city-selection-pop").hide();

            $("html,body").removeClass("lock-browser-scroll");

            // tracking code
            if (CityNameSelectedbyIP == null || CityNameSelectedbyIP == "") {
                dataLayer.push({
                    event: "master-city-popup-mobile",
                    cat: "masterCityPopupMobile",
                    act: "city-selected-user-typed",
                    lab: masterCityName
                });
             
            }
            if ((CityNameSelectedbyIP != null && CityNameSelectedbyIP != (ui.item.label)) || (CityNameSelectedbyIP != "" && CityNameSelectedbyIP != (ui.item.label))) {
                dataLayer.push({
                    event: "master-city-popup-mobile",
                    cat: "masterCityPopupMobile",
                    act: "geoLocated-value-changed-new-value-entered",
                    lab: CityNameSelectedbyIP + ","+masterCityName

                });

            }
            $('#citychange').addClass('city-position-active');
            $('#citychange').focus();
            
            return true;
        },
        open: function (result, a, b) {
            currentSuggestResult = result;
        },
        afterfetch: function (result,reqTerm) {
            currentSuggestResult = result;
            if ($("#select-city-auto").val() == '') {
                $("#clear-icon").hide();
                $('#message').hide();
            }
            if (!validate() && $('#select-city-auto').val() != "") {
                if(reqTerm.length>1)$('#message').show();
            }
        },
        keyup: autoCompleteKeyUp
    });
   
    $("#select-city-auto").bind('input keyup', autoCompleteKeyUp);

    $('#select-city-auto').click(function () {
        if ($('#select-city-auto').val() == CityNameSelectedbyIP) {
            $('#select-city-auto').val("");
            $("#clear-icon").hide();
            $("#submitButton").hide();

        }

    });

    //below code is  for close icon  and check the cookie status 
    $('#close-popup').on('click', function () {
        $("html,body").removeClass("lock-browser-scroll");

        dataLayer.push({
            event: "master-city-popup-mobile",
            cat: "masterCityPopupMobile",
            act: "x-button-close"
        });
        if (getCookie("_CustCityIdMaster") == "") {
            document.cookie = '_CustCityMaster= Select City; path =/; domain=' + defaultCookieDomain;
            document.cookie = '_CustCityIdMaster= -1; path =/; domain=' + defaultCookieDomain; 
        }
        else {
            $(".m-city-selection-pop").hide();
            $("#m-blackOut-window").hide();
            return false;
        }
        $(".m-city-selection-pop").hide();
        $("#m-blackOut-window").hide();
        return false;

    });

    //Done Button click
    $("#submitButton").click(function () {
        $('#citychange').addClass('city-position-active');
        if ($('#select-city-auto').val() == CityNameSelectedbyIP && CityNameSelectedbyIP!="") {
            masterCityName = CityNameSelectedbyIP;
            masterCityId = CitySelectedbyIP;
            cookieSet();
            $(".m-city-selection-pop").hide();
            $("#m-blackOut-window").hide();

            $("html,body").removeClass("lock-browser-scroll");
            
            dataLayer.push({
                event: "master-city-popup-mobile",
                cat: "masterCityPopupMobile",
                act: "geoLocated-selected-done-button",
                lab: masterCityName
            });
           
            return true;
        }

        else if (getCookie('_CustCityIdMaster')) {
            $("html,body").removeClass("lock-browser-scroll");
             $(".m-city-selection-pop").hide();
            $("#m-blackOut-window").hide();
            return true;
        }

    });

    //this function is to select the  popular city onclick and save the cookie 
    $('#outerDiv').find('#userPopularCities').find('li').click(function () {
        $('#citychange').addClass('city-position-active');
        masterCityName = $.trim($(this).text());
        masterCityId = $(this).attr('masterCityId');
        $('#select-city-auto').val("");
        var input = $('#select-city-auto');
        input.val(input.val() + masterCityName);
        cookieSet();
        $("html,body").removeClass("lock-browser-scroll");
        dataLayer.push({
            event: "master-city-popup-mobile",
            cat: "masterCityPopupMobile",
            act: "popular-city-selected",
            lab: masterCityName.toLowerCase()
        });
      
        return true;


    });
   
        $("#clear-icon").click(function (e) {
            $('#select-city-auto').val('').keyup();
            $("#clear-icon").hide();
            $("#submitButton").hide();
            $('#select-city-auto').focus();
            $('#message').hide();

        });
    }
   
if ($('#select-city-auto').val() == "Search by city name") {

    $('#select-city-auto').val("");
    $('#select-city-auto').focus();
    $("#clear-icon").hide();
}
$('#select-city-auto').on('focus', function () {

    if ($('#select-city-auto').val() == CityNameSelectedbyIP && CityNameSelectedbyIP != '') {

        $('#select-city-auto').val("");
        $("#clear-icon").hide();
        $("#submitButton").hide();

    }

});

function validate() {
    var cityval = $("#select-city-auto").val();
    if (currentSuggestResult != null) {
        if (currentSuggestResult.length == 1 && currentSuggestResult[0].label == cityval) {
            $("#message").hide();
            return true;
        }
        else if (currentSuggestResult.length < 1 ) {
            return false;
        }
        $("#message").hide();
        return true;
    }
}

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
function autoCompleteKeyUp() {
    $("#submitButton").hide();

    $("#clear-icon").show();
}
function popUphtml() {
    $("#submitButton").hide();
    $("#m-blackOut-window").show()
    $(".m-city-selection-pop").show();
   
   $("html,body").addClass("lock-browser-scroll");
    if (CitySelectedbyIP != "" && CitySelectedbyIP != null && CitySelectedbyIP != "-1" && CitySelectedbyIP != '0') {

        $("#select-city-auto").val(CityNameSelectedbyIP);
        $("#submitButton").show();
        $("#clear-icon").show();
        dataLayer.push({
            event: "master-city-popup-mobile",
            cat: "masterCityPopupMobile",
            act: "display-with-GeoLocatedValue",
            lab: CityNameSelectedbyIP
        });

    }
    else {
        $("#submitButton").hide();
    }
    dataLayer.push({
        event: "master-city-popup-mobile",
        cat: "masterCityPopupMobile",
        act: "display-popup-without-GeoLocated"
    });
}

function cookieSet() {
    var now = new Date();
    var Time = now.getTime();
    Time += 1000 * 60 * 60 * 5040;
    now.setTime(Time);
    document.cookie = '_CustCityMaster=' + masterCityName + '; expires = ' + now.toGMTString() + "; domain=" + defaultCookieDomain + '; path =/';
    document.cookie = '_CustCityIdMaster=' + masterCityId + '; expires = ' + now.toGMTString() + "; domain=" + defaultCookieDomain + '; path =/';

    $(".m-city-selection-pop").hide();
    $("#m-blackOut-window").hide();
}

/* document ready code starts */
var citypopped = jQuery.Event("citypopped");
$(document).ready(function () {
  
    if (changeCityIconShow == false) {
        $('#citychange').hide();

    }
    var cookieValue = getCookie("_CustCityIdMaster");
    if (cookieValue == "" && checkpath()) {
            $("#cityPopUp").load("/m/research/PopUpCityMobile.aspx", function () {
           
                popupReady()
                $(document).trigger(citypopped);

                dataLayer.push({
                    event: "master-city-popup-mobile",
                    cat: "masterCityPopupMobile",
                    act: "popup-open",
                    lab: "auto"
                });
            });
    } else if (cookieValue != "-1") {
        $('#citychange').addClass('city-position-active');
    }

    pageLoadCounter(cookieValue);

    $("#citychange").click(function () {
            $("#cityPopUp").load("/m/research/PopUpCityMobile.aspx", function () {
                popupReady();
                cookieVal = getCookie("_CustCityIdMaster");
                if (cookieVal != '-1' && cookieVal != '' && cookieVal != CitySelectedbyIP) {
                    $('#select-city-auto').val(getCookie("_CustCityMaster"));
                    $("#clear-icon").show();
                    $("#submitButton").show();
                }

                else {
                    dataLayer.push({
                        event: "master-city-popup-mobile",
                        cat: "masterCityPopupMobile",
                        act: "popup-open",
                        lab: "icon-empty"
                    });

                }
            });
    });
    
});

function checkpath() {
    if (window.location.pathname.indexOf("/m/research/locatedealerpopup.aspx") == -0) return false;
    if (window.location.pathname.indexOf("/m/research/quotation.aspx") == 0) return false;
    return true;
}

function showToolTip() {

$("#tooltip").show();
    setTimeout(function () {
        $("#tooltip").hide();
    }, 3000);
}

function getCookie(cookieString) {
    var x = document.cookie;
    var r = x.split(";");
    var l = r.length;
    for (var i = 0; i < l; i++) {
        var s = r[i].split("=");
        if ($.trim(s[0]) == cookieString) {
            return s[1]
        }
    }
    return "";
}

function pageLoadCounter(cookieValue) {

    var pageLoads = getCookie('_PageLoadCounter');
    if (pageLoads != '') {
        var count = parseInt(pageLoads) + 1;
        document.cookie = '_PageLoadCounter=' + count.toString() + '; path =/';

        if (parseInt(getCookie('_PageLoadCounter')) % 4 == 0 && (cookieValue == "-1" || cookieValue == "") && changeCityIconShow ==true) {
            showToolTip();
        }
    } else {
        document.cookie = '_PageLoadCounter=1; path =/';
    }
}

