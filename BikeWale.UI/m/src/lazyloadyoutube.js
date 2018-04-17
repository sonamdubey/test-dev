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
        var bodyEle = lazyloadYoutube.body;
        var htmlEle = lazyloadYoutube.html;
        return Math.max(bodyEle.scrollHeight, bodyEle.offsetHeight, htmlEle.clientHeight, htmlEle.scrollHeight, htmlEle.offsetHeight);
    },

    appendImage: function (ytVideo, img) {
        ytVideo.appendChild(img);
    },

    setVideoPreviews: function () {
        var youtubeVideos = lazyloadYoutube.getAllVideoSlots();
        var noOfVideoSlots = youtubeVideos.length;

        for (var i = 0; i < noOfVideoSlots; i++) {

            var ytVideoId = youtubeVideos[i].dataset.embed;

            var source = lazyloadYoutube.imgSource + ytVideoId + lazyloadYoutube.imgQuality;

            var image = new Image();
            image.src = source;
            image.addEventListener("load", lazyloadYoutube.appendImage(youtubeVideos[i], image));
        }
    },

    getScrollPercent: function () {
        var scrollTop = lazyloadYoutube.html.scrollTop;
        return (scrollTop / lazyloadYoutube.getPageHeight());
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
            ytVideo.innerHTML = "";
            ytVideo.appendChild(iframe);
        }

        lazyloadYoutube._videosLoaded = true;
    }
}

docReady(lazyloadYoutube.setVideoPreviews);

window.addEventListener("scroll", function (event) {
    if (!lazyloadYoutube._videosLoaded) {
        if (lazyloadYoutube.getScrollPercent() > lazyloadYoutube.loadVideosAt) {
            lazyloadYoutube.loadYoutubeVideos();
        }
    }
});