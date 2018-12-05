function onYouTubeIframeAPIReady() {
    YoutubeAPI.updateApiStatus(true);
}

var YoutubeAPI = (function () {
    var _playerContainer;
    var _imgSource = "https://img.youtube.com/vi/";
    var _imgQuality = "/maxresdefault.jpg";
    var _ytApiReady = false;
    var _youTubePlayer;
    var _isVideoPlaying = false;

    function _noop() { };

    function _setSelector() {
        _playerContainer = document.getElementById("bw-youtube-player");
    };

    function _setListeners() {
        _playerContainer.addEventListener("click", function (event) {
            var clickedvideo = event.currentTarget;
            var ytVideoId = clickedvideo.dataset.embed;

            generateIFrame(ytVideoId);
        });
    };

    function _initializeYoutubeAPI() {
        var tag = document.createElement('script');
        var parentElement = document.getElementById("galleryRoot");

        tag.src = "https://www.youtube.com/iframe_api";
        var firstScriptTag = document.getElementsByTagName('script')[0];
        firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
    };

    function registerEvents() {
        _initializeYoutubeAPI();
        _setSelector();
        _generatePreview();
        _setListeners();
    };

    function _generatePreview() {
        var videoId = _playerContainer.dataset.embed;

        var image = new Image();
        image.src = _imgSource + videoId + _imgQuality;;

        image.addEventListener("load", function () {
            _playerContainer.appendChild(image);
        });
    };

    function generateIFrame(ytVideoId) {
        if (_ytApiReady) {

            _playerContainer.innerHTML = "";
            var divId = "player-" + ytVideoId;
            var iframeContainer = document.createElement("div");
            iframeContainer.setAttribute("id", divId);
            _playerContainer.appendChild(iframeContainer);

            _generateYTPlayerObj(divId, ytVideoId);
        }
    };

    function _generateYTPlayerObj(id, ytVideoId) {

        _youTubePlayer = new YT.Player(id, {
            width: '100%',
            height: '100%',
            playerVars: { rel: 0, showinfo: 0, fs: 0 },
            videoId: ytVideoId,
            events: {
                'onReady': _onPlayerReady,
                'onStateChange': _onPlayerStateChange
            }
        });
    };

    function _onPlayerReady(event) {
        _youTubePlayer.playVideo();
    };

    function _onPlayerStateChange(event) {
        if (event.data == YT.PlayerState.PLAYING) {
            _isVideoPlaying = true;
            YoutubeAPI.onVideoPlaying();
        }
        else if (event.data == YT.PlayerState.ENDED
            || event.data == YT.PlayerState.PAUSED
            || event.data == YT.PlayerState.UNSTARTED) {
            _isVideoPlaying = false;
            YoutubeAPI.onVideoPlaybackSuspended();
        }
    };

    function pauseYoutubeVideo() {
        if (_isVideoPlaying) {
            _youTubePlayer.pauseVideo();
        }
    };

    function stopYoutubeVideo() {
        if (_isVideoPlaying) {
            _youTubePlayer.stopVideo();
        }
    };

    function updateApiStatus(isReady) {
        _ytApiReady = isReady;
    };

    return {
        registerEvents: registerEvents,
        stopYoutubeVideo: stopYoutubeVideo,
        pauseYoutubeVideo: pauseYoutubeVideo,
        updateApiStatus: updateApiStatus,
        generateIFrame: generateIFrame,
        onVideoPlaying: _noop,
        onVideoPlaybackSuspended: _noop
    }

})();

docReady(YoutubeAPI.registerEvents);