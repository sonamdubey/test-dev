var lazyloadYoutube = {

    _videosLoaded: false,
    loadVideosAt: 0.4,
    body: document.body,
    html: document.documentElement,
    imgSource: "https://img.youtube.com/vi/",
    imgQuality: "/sddefault.jpg",
    videoSource: "https://www.youtube.com/embed/",


    getAllVideoSlots: function() {
        return document.querySelectorAll(".bw-youtube");
    },

    getPageHeight: function() {
        return Math.max(lazyloadYoutube.body.scrollHeight, lazyloadYoutube.body.offsetHeight, lazyloadYoutube.html.clientHeight, lazyloadYoutube.html.scrollHeight, lazyloadYoutube.html.offsetHeight);
    },

    setVideoPreviews: function () {
        var youtubeVideos = lazyloadYoutube.getAllVideoSlots();
        var noOfVideoSlots = youtubeVideos.length;

        for (var i = 0; i < noOfVideoSlots; i++) {

            var ytVideoId = youtubeVideos[i].dataset.embed;

            var source = lazyloadYoutube.imgSource + ytVideoId + lazyloadYoutube.imgQuality;

            var image = new Image();
            image.src = source;
            image.addEventListener("load", function () {
                youtubeVideos[i].appendChild(image);
            }(i));
        }
    },

    getScrollPercent: function () {
        var scrollTop = lazyloadYoutube.html.scrollTop;
        var scrollPercentage = (scrollTop / lazyloadYoutube.getPageHeight());
        return scrollPercentage;
    },

    generateIFrame: function (videoId) {
        var iframe = document.createElement("iframe");
        iframe.setAttribute("frameborder", "0");
        iframe.setAttribute("allowfullscreen", "");
        iframe.setAttribute("src", lazyloadYoutube.videoSource + videoId + "?rel=0&showinfo=0&autoplay=0");
        return iframe;
    },

    loadYoutubeVideos: function () {

        var youtubeVideos = lazyloadYoutube.getAllVideoSlots();

        for (var i = 0; i < youtubeVideos.length; i++) {
            var ytVideo = youtubeVideos[i];
            var ytVideoId = ytVideo.dataset.embed;
            var iframe = lazyloadYoutube.generateIFrame(ytVideoId);
            ytVideo.appendChild(iframe);
        }

        lazyloadYoutube._videosLoaded = true;
    }
}

docReady(lazyloadYoutube.setVideoPreviews());

window.addEventListener("scroll", function (event) {
    if (!lazyloadYoutube._videosLoaded) {
        if (lazyloadYoutube.getScrollPercent() > lazyloadYoutube.loadVideosAt) {
            lazyloadYoutube.loadYoutubeVideos();
        }
    }
});