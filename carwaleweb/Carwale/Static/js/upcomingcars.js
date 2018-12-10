$(document).ready(function () {
    if (Number(upcomingPage.sort) > 0) $("#UpcomingCarSearch_drpUCSortList option[value=" + upcomingPage.sort + "]").attr("selected", "selected");
    if (Number(upcomingPage.makeId) > 0) $("#UpcomingCarSearch_drpMake option[value=" + upcomingPage.makeId + "]").attr("selected", "selected");
    $("#UpcomingCarSearch_drpUCSortList").change(function () {
        var sortVal = $("#UpcomingCarSearch_drpUCSortList").val();
        if (Number(sortVal) > 0) {
            var indexOfPage = url.indexOf("/page");
            var indexOfSort = url.indexOf("/sort");
            if (indexOfPage > -1 || indexOfSort > -1) {
                if (indexOfPage > -1) {
                    url = url.substring(0, indexOfPage) + "/sort/" + sortVal + "/";
                }
                if (indexOfSort > -1) {
                    url = url.substring(0, indexOfSort + 5) + "/" + sortVal + "/";
                }
            } else {
                url += "sort/" + sortVal + "/";
            }
            window.location.href = url;
        } else {
            window.location.href = "/" + ((upcomingPage.makeId) > 0 ? upcomingPage.makemask : "upcoming") + "-cars/" + (Number(upcomingPage.makeId) > 0 ? "upcoming/" : "");
        }
    });
    var spaces = /\s/g;
    $("div.cwcHigh").bt({ contentSelector: "$('#cwcContentHigh').html()", fill: '#fff', strokeWidth: 1, strokeStyle: '#666666', width: '300px', cssClass: 'f-small', spikeLength: 7, trigger: ['hover', 'none'] });
    $("div.cwcMedium").bt({ contentSelector: "$('#cwcContentMedium').html()", fill: '#fff', strokeWidth: 1, strokeStyle: '#666666', width: '300px', cssClass: 'f-small', spikeLength: 7, trigger: ['hover', 'none'] });
    $("div.cwcLow").bt({ contentSelector: "$('#cwcContentLow').html()", fill: '#fff', strokeWidth: 1, strokeStyle: '#666666', width: '300px', cssClass: 'f-small', spikeLength: 7, trigger: ['hover', 'none'] });
});
function MakeModelSearch_click() {
    var makeId = $("#UpcomingCarSearch_drpMake").val();
    if (Number(makeId) > 0) {
        var makeName = $("#UpcomingCarSearch_drpMake option:selected").text();
        var loc = '/' + makeName.toLowerCase().replace(" ", "").replace("-", "") + "-cars/upcoming/";
        window.location.href = loc;
    } else {
        window.location.href = "/upcoming-cars/";
    }
}
$(document).ready(function () {
    $("#btnSubscribe").click(function (e) {
        e.preventDefault();
        if (validateEmailAddress()) {
            Subscribe();
        }
        return false;
    });
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
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
    var subscriptionCategory = 1;
    var subscriptionType = -1;
    $.ajax({
        type: "POST", url: "/ajaxpro/CarwaleAjax.AjaxResearch,Carwale.ashx",
        data: '{"emailAddress":"' + $("#txtUpcomingAlertEmail").val() + '", "subscriptionCategory":"' + subscriptionCategory + '", "subscriptionType":"' + subscriptionType + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "Subscribe"); },
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