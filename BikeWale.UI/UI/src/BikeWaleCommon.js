/* Created By : Ashish G. Kamble on 3/8/2012    Summary : Commmon javascript functions for bikewale should be written in this file    */

/* Argument Desc: 1. js object 2. id if the dropdown you want to fill 
3. associated hidden field to manage post back(view state) 4. array of dependent dropdowns. 5. string at index 0 like --select-- or any  */
function bindDropDownList(response, cmbToFill, viewStateId, dependentCmbs, selectString) {
    if (response.Table != null) {
        if (!selectString || selectString == '') selectString = "--Select--";
        $(cmbToFill).empty().append("<option value=\"0\">" + selectString + "</option>").attr("disabled", false);

        var hdnValues = "";

        for (var i = 0; i < response.Table.length; i++) {
            $(cmbToFill).append("<option value=" + response.Table[i].Value + ">" + response.Table[i].Text + "</option>");

            if (hdnValues == "")
                hdnValues += response.Table[i].Text + "|" + response.Table[i].Value;
            else
                hdnValues += "|" + response.Table[i].Text + "|" + response.Table[i].Value;
        }
        if (viewStateId) $("#" + viewStateId).val(hdnValues);
    }

    if (dependentCmbs && dependentCmbs.length > 0) {
        for (var i = 0; i < dependentCmbs.length; i++) {
            $("#" + dependentCmbs[i]).empty().attr("disabled", true);
        }
    }
}

// Function to show loading status in the model drop down box   Created by Ashish G. Kamble
function showLoading(objId) {
    $("#" + objId).find("option:first").text("--Loading--");
}

// Function to format url parameters for url rewriting. Written By : Ashish G. Kamble
function FormatSpecial(url) {
    reg = /[^/\-0-9a-zA-Z\s]*/g; // everything except a-z, 0-9, / and - 
    url = url.replace(reg, '');
    var formattedUrl = url.toLowerCase().replace(/ /g, "").replace(/-/g, "").replace("/", "");
    return formattedUrl;
}

// Function to scroll window up with scrollSpeed
//input parameter : id of element, scroll up speed 
function ScrollToTop(id, scrollSpeed) {

    $("#" + id).hide();

    $(window).scroll(function () {
        if ($(window).scrollTop() > 100) {
            $("#" +id ).fadeIn(1500);
        }
        else {
            $("#" + id).fadeOut(1500);
        }
    });
    //back to top
    $("#" + id).click(function () {
        $('body,html').animate({ scrollTop: 0 }, scrollSpeed);
        return false;
    });
}

function GetGlobalCityArea() {
    var cookieName = "location";
    var cityArea = '';
    if (isCookieExists(cookieName)) {
        var arrays = getCookie(cookieName).split(",")[0].split("_");
        if (arrays.length > 2) {
            cityArea = arrays[1] + '_' + arrays[3];
        }
        return cityArea;
    }
}

function isCookieExists(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0)
            return true;
    }
    return false;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}
