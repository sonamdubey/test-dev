var youtubeVideos = document.querySelectorAll(".bw-youtube");
var _videosLoaded = false;
var loadVideosAt = 0.4;
var body = document.body, html = document.documentElement;
var pageHeight = Math.max(body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight);

for (var i = 0; i < youtubeVideos.length; i++) {
    var ytVideoId = youtubeVideos[i].dataset.embed;
    var source = "https://img.youtube.com/vi/" + ytVideoId + "/sddefault.jpg";

    var image = new Image();
    image.src = source;
    image.addEventListener("load", function () {
        youtubeVideos[i].appendChild(image);
    }(i));
}


function getScrollPercent() {
    var scrollTop = html.scrollTop;
    var scrollPercentage = (scrollTop / pageHeight);
    return scrollPercentage;
}


function loadYoutubeVideos() {
    for (var i = 0; i < youtubeVideos.length; i++) {
        var ytVideo = youtubeVideos[i];
        var ytVideoId = ytVideo.dataset.embed;

        var iframe = document.createElement("iframe");
        iframe.setAttribute("frameborder", "0");
        iframe.setAttribute("allowfullscreen", "");
        iframe.setAttribute("src", "https://www.youtube.com/embed/" + ytVideoId + "?rel=0&showinfo=0&autoplay=0");
        ytVideo.innerHTML = "";
        ytVideo.appendChild(iframe);
    }
    _videosLoaded = true;
}

window.addEventListener("scroll", function (event) {
    if (!_videosLoaded) {
        if (getScrollPercent() > loadVideosAt) {
            loadYoutubeVideos();
        }
    }
});