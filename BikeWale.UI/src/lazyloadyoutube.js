var lazyloadYoutube = {

    _videosLoaded: false,
    html: document.documentElement,
    imgSource: "https://img.youtube.com/vi/",
    imgQuality: "/maxresdefault.jpg",
    videoSource: "https://www.youtube.com/embed/",


    getAllVideoSlots: function () {
        return document.querySelectorAll(".bw-youtube");
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


    generateIFrame: function (videoId) {
        var iframe = document.createElement("iframe");
        iframe.setAttribute("frameborder", "0");
        iframe.setAttribute("allowfullscreen", "");
        iframe.setAttribute("src", lazyloadYoutube.videoSource + videoId + "?rel=0&showinfo=0&autoplay=1");
        return iframe;
    },

    loadYoutubeVideos: function () {

        var youtubeVideos = lazyloadYoutube.getAllVideoSlots();

        for (var i = 0; i < youtubeVideos.length; i++) {
            var ytVideo = youtubeVideos[i];
            var ytVideoId = ytVideo.dataset.embed;
            ytVideo.addEventListener("click", function (event) {
                var iframe = lazyloadYoutube.generateIFrame(ytVideoId);
                ytVideo.innerHTML = "";
                ytVideo.appendChild(iframe);
            });
        }
    }
}

docReady(lazyloadYoutube.setVideoPreviews);
docReady(lazyloadYoutube.loadYoutubeVideos);