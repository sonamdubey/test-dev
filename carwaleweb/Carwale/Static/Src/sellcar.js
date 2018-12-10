function deleteImg(photoId) {
    if (confirm("Are you sure want to delete this photo?")) {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/CarwaleAjax.AjaxSellCar,Carwale.ashx",
            data: '{"inquiryId":"' + inquiryId + '", "photoId":"' + photoId + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "RemoveCarPhotos"); },
            success: function (response) {
                responseObj = eval('(' + response + ')');
                if (responseObj.value) {
                    $("#" + photoId).fadeOut(500, function () { $(this).remove() });
                    var currImgCnt = $("#divImageCount").text();
                    var newImgCnt = currImgCnt != "0" ? (parseInt(currImgCnt) - 1) : "0";
                    $("#divImageCount").text(newImgCnt);
                } else { // unsuccessfull
                    alert("Unable to delete this file");
                }
            }
        });
    }
}

function makeMainImg(photoId) {
    $.ajax({
        type: "POST",
        url: "/ajaxpro/CarwaleAjax.AjaxSellCar,Carwale.ashx",
        data: '{"inquiryId":"' + $.trim(inquiryId) + '", "photoId":"' + $.trim(photoId) + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "MakeMainImage"); },
        success: function (response) {
            responseObj = eval('(' + response + ')');
            if (responseObj.value) {

            }
        }
    });
}

function clearTextArea(objTextArea) {
    $(objTextArea).html("");
}

function msgTextArea(objTextArea) {
    objJQ = $(objTextArea);
    if (objJQ.html() == "" || objJQ.html() == "Describe this image here")
        objJQ.html("Describe this image here");
}

function mDone() {
    processingWait(false);
    if (requestCount == 0)
        setTimeout('nextStep()', 1000);
}

function nextStep() {
    if (nextStepUrl != "") {
        window.location.href = nextStepUrl;
    } else {
        alert("You have added description to the images successfully.");
    }
}

function formatNumeric(num) {
    var formatted = "";
    var breakPoint = 3;
    var numStr = num.toString();

    for (var i = numStr.toString().length - 1; i >= 0; i--) {
        formatted = numStr.charAt(i) + formatted;

        if ((numStr.length - i) == breakPoint && numStr.length > breakPoint) {
            formatted = "," + formatted;
            breakPoint += 2;
        }
    }
    return formatted;
}