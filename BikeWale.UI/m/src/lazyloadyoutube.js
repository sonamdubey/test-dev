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

    getVideoSlotsInScope: function(scope) {
        selectedScope = document.getElementById(scope);
        return selectedScope.querySelectorAll(".bw-youtube");
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

    generateIFrame: function (videoId, ytParams) {
        var iframe = document.createElement("iframe");
        iframe.setAttribute("frameborder", "0");
        iframe.setAttribute("allowfullscreen", "");
        iframe.setAttribute("src", lazyloadYoutube.videoSource + videoId + ytParams);
        return iframe;
    },

    loadYoutubeVideos: function (input) {

        if (!input.scope) {
            var youtubeVideos = lazyloadYoutube.getAllVideoSlots();
        }
        else {
            var youtubeVideos = lazyloadYoutube.getVideoSlotsInScope(input.scope);
        }

        var ytParams = "?rel=0&amp;showinfo=0";

        if (input.enableYTAPI) {
            ytParams = ytParams + "&amp;enablejsapi=1";
        }

        var noOfVideos = youtubeVideos.length;

        for (var i = 0; i < noOfVideos; i++) {
            var ytVideo = youtubeVideos[i];
            var videoLoaded = Boolean(ytVideo.dataset.videoloaded);

            if (!videoLoaded) {
                var ytVideoId = ytVideo.dataset.embed;
                var iframe = lazyloadYoutube.generateIFrame(ytVideoId, ytParams);
                ytVideo.innerHTML = "";
                ytVideo.appendChild(iframe);
                ytVideo.setAttribute("data-videoloaded", "true");
            }
        }

        lazyloadYoutube._videosLoaded = true;
    },

    setEventListener: function(eventname, callback) {
        window.addEventListener(eventname, callback);
    }
}

