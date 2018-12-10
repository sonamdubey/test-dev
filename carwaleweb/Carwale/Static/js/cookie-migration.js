/*
    @description : Cookie Migration script
    @author : Meet Shah
    @createdOn : 12 Dec, 2016
*/

(function ($) {
    'use strict';

    var cookiesTobeMigrated = [["_CustAreaId",180], ["_CustAreaName",180], ["_CustCity",180], ["_CustCityId",180], ["_CustCityIdMaster",180], ["_CustCityMaster",180], ["_CustEmail",180], ["_CustMobile",180], ["_CustZoneIdMaster",180], ["_CustZoneMaster",180], ["_CustomerName",180], ["_PQModelId",180], ["_PQPageId",180], ["_PQVersionId",180], ["_PQZoneId",180], ["_abtest",90], ["_dealStockId",0], ["_dealerCityModel",180], ["_userModelHistory",180]];

    var cookieValue;

    function getCookieTime(days) {
        var now = new Date();
        var time = now.getTime();
        time += 1000 * 60 * 60 * 24 * days;
        now.setTime(time);
        return (now.toGMTString());
    }

    function setAbTestCookie() {
        cookieValue = Math.floor(Math.random() * (Number( abTestKeyMaxValue ) - Number(abTestKeyMinValue) + 1)  + Number(abTestKeyMinValue)) ;
        document.cookie = "_abtest=" + cookieValue + "; expires=" + getCookieTime(90) + "; domain=" + defaultCookieDomain + "; path=/";
    }
    
    function migrateAbTestCookies() {
        if (window.localStorage && window.localStorage.getItem("abtestversion1") != null && window.localStorage.getItem("abtestversion1") != abTestKey) {
            setAbTestCookie();
        }
        window.localStorage && window.localStorage.setItem("abtestversion1", abTestKey);
    }

    function setAreaZoneCookie() {
        try {
            $.when(Common.utils.ajaxCall({
                url: '/api/location/area/?id=' + Number($.cookie('_CustAreaId')),
                type: "GET", contentType: "application/json; charset=utf-8", dataType: "json"
            })).done(function (data) {
                if (data != null) {
                    Common.utils.setEachCookie('_CustZoneIdMaster', data.zoneId);
                    Common.utils.setEachCookie('_CustZoneMaster', data.zoneName);
                }
            });
        }
        catch (e) {
            console.log(e)
        }
    }

    function migrateAreaZoneCookies()  {        
        if (window.localStorage.getItem("areamappingversion") != "1" && Number($.cookie('_CustCityIdMaster')) == 2 && Number($.cookie('_CustAreaId')) > 0) {
            setAreaZoneCookie();
            window.localStorage.setItem("areamappingversion", "1");
        }
    }

    function migrateAllCookies() {
        if (window.localStorage && window.localStorage.getItem("cookie_migration") != "true")  {
            for (var i = 0; i < cookiesTobeMigrated.length; i++) {
                cookieValue     = $.cookie(cookiesTobeMigrated[i][0]);
                document.cookie = cookiesTobeMigrated[i][0] + "=; expires=" + (new Date()).toGMTString() + "; domain=carwale.com;";
                document.cookie = cookiesTobeMigrated[i][0] + "=; expires=" + (new Date()).toGMTString() + "; domain=www.carwale.com; path=/";
                document.cookie = cookiesTobeMigrated[i][0] + "=; expires=Thu, 01 Jan 1970 00:00:00 GMT;";
                document.cookie = cookiesTobeMigrated[i][0] + "=; expires=Thu, 01 Jan 1970 00:00:00 GMT; domain=carwale.com;";
                document.cookie = cookiesTobeMigrated[i][0] + "=; expires=Thu, 01 Jan 1970 00:00:00 GMT; domain=www.carwale.com;";
                document.cookie = cookiesTobeMigrated[i][0] + "=; expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/";
                document.cookie = cookiesTobeMigrated[i][0] + "=; expires=Thu, 01 Jan 1970 00:00:00 GMT; domain=carwale.com; path=/";
                document.cookie = cookiesTobeMigrated[i][0] + "=; expires=Thu, 01 Jan 1970 00:00:00 GMT; domain=www.carwale.com; path=/";
                if (cookieValue != null && cookiesTobeMigrated[i][1] != 0)  {
                    document.cookie = cookiesTobeMigrated[i][0] + "=" + cookieValue + "; expires=" + getCookieTime(cookiesTobeMigrated[i][1]) + "; domain=" + defaultCookieDomain + "; path=/";
                }
                else if(cookieValue != null && cookiesTobeMigrated[i][1] == 0)
                    document.cookie = cookiesTobeMigrated[i][0] + "=" + cookieValue + "; domain=" + defaultCookieDomain + "; path=/";
            }
            window.localStorage.setItem("cookie_migration", "true");
        }
    }

    migrateAbTestCookies();
    migrateAllCookies();
    migrateAreaZoneCookies();
    
})($);