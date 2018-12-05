function toggleFullScreen(goFullScreen) {
    var doc = window.document;
    var docElement = doc.documentElement;

    var requestFullScreen = checkFullScreenSupport();
    var cancelFullScreen = doc.exitFullscreen || doc.mozCancelFullScreen || doc.webkitExitFullscreen || doc.msExitFullscreen || doc.webkitCancelFullScreen;

    if (goFullScreen && requestFullScreen != undefined) {
        requestFullScreen.call(docElement);
    }
    else if (cancelFullScreen != undefined) {
        cancelFullScreen.call(doc);
    }
}

function checkFullScreenSupport() {
    var docElement = window.document.documentElement;

    var requestFullScreen = docElement.requestFullscreen || docElement.mozRequestFullScreen || docElement.webkitRequestFullScreen || docElement.msRequestFullscreen;

    return requestFullScreen;
}
