
/* JS related to FeaturesDetails.apsx page*/
function NextClicked() {
    currentIndex++;
    fullUrl = $("#divPhotos .thumbDiv").eq(currentIndex).find("img").attr("src").toString().replace("/t/", "/l/");
    $("#divPhotosOverlay").attr("style", "position:absolute;top:0;left:0;width:100%;height:" + (parseInt($("#divLargeImgContainer").height()) + 20) + "px;z-index;1000;background-color:black;filter:alpha(opacity=80);opacity:0.80;background-image: url('/m/image/loader.gif');background-position:center center;background-repeat:no-repeat;");
    LoadLargePhoto();
}

function PrevClicked() {
    currentIndex--;
    fullUrl = $("#divPhotos .thumbDiv").eq(currentIndex).find("img").attr("src").toString().replace("/t/", "/l/");
    $("#divPhotosOverlay").attr("style", "position:absolute;top:0;left:0;width:100%;height:" + (parseInt($("#divLargeImgContainer").height()) + 20) + "px;z-index;1000;background-color:black;filter:alpha(opacity=80);opacity:0.80;background-image: url('/m/image/loader.gif');background-position:center center;background-repeat:no-repeat;");
    LoadLargePhoto();
}

function ShowLargePhotos(_thumbDiv) {
    fullUrl = $(_thumbDiv).find("img").attr("src").toString().replace("/t/", "/l/");

    var thumbDivs = $("#divPhotos .thumbDiv");
    totalThumbDivs = thumbDivs.length;

    currentIndex = 0;
    var i = 0;

    thumbDivs.each(function () {
        if ($(this).find("img").attr("src").toString().replace("/t/", "/l/") == fullUrl)
            currentIndex = i;
        else
            i++;
    });

    $("#divPhotosOverlay").attr("style", "position:absolute;top:0;left:0;width:100%;height:" + (parseInt($("#divPhotos").height()) + 10) + "px;z-index;1000;background-color:black;filter:alpha(opacity=80);opacity:0.80;background-image: url('/m/image/loader.gif');background-position:center center;background-repeat:no-repeat;");
    LoadLargePhoto();
}