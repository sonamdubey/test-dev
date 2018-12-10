var player;
// 3. This function creates an <iframe> (and YouTube player)
//    after the API code downloads.
var trackingStatus = {};

var trackList = [10, 25, 50, 75, 100];

function onYouTubeIframeReady(videoId, createUniqueId) {
    try {
        var postFix = createUniqueId ? '-' + videoId : '';
        player = new YT.Player('videoIframe' + postFix, {
            playerVars: {
                'autoplay': 0,
            },
            events: {
                'onReady': onPlayerReady,
                'onStateChange': onPlayerStateChange,
            }
        });
    }
    catch (e) {
        setTimeout(function () { onYouTubeIframeReady(videoId, createUniqueId); }, 300);
    }
}

// 4. The API will call this function when the video player is ready.
function onPlayerReady(event) {
}

// 5. The API calls this function when the player's state changes.
//    The function indicates that when playing a video (state=1),
//    the player should play for six seconds and then stop.

function onPlayerStateChange(event) {
    if (event.data == YT.PlayerState.PLAYING) {
        if (typeof setPlayerPlayed == "function")
            setPlayerPlayed();
        var videoId = event.target.a.getAttribute("data-id");
        var playerTotalTime = player.getDuration();
        createTrackObj(videoId);
        window.videoTrackingTimer = setInterval(function () {
            var playerCurrentTime = player.getCurrentTime();

            var playerTimeDifference = (playerCurrentTime / playerTotalTime) * 100;
            
            videosFlow(playerTimeDifference, videoId);
        }, 4000);
    }
    else if (event.data == YT.PlayerState.ENDED) {
        if (typeof autoPlayNextVideo == "function") {
            autoPlayNextVideo();
        }
    }
    if (event.data !== YT.PlayerState.PLAYING && window.videoTrackingTimer != undefined)
    {
        clearTimeout(window.videoTrackingTimer);
    }
    if(event.data == YT.PlayerState.PAUSED) {
         Common.utils.trackAction("CWInteractive", "contentcons", "Videos-Details-Paused", !player? "" :player.getVideoData().video_id);
    }
}

function preProcess(basicId, trackcount) {
    for (var count = trackcount; count >= 0 ; count--)
    {
        trackingStatus[basicId][trackList[count]] = true;
    }
}

function createTrackObj(videoId) {
    if (trackingStatus[videoId] == undefined) {
        trackingStatus[videoId] = {
            10: false, 25: false, 50: false, 75: false, 100: false
        };
    }
}

function videosFlow(percentage, videoId) {
    Common.utils.trackAction("CWInteractive", "contentcons", "Videos-Details-Playing", videoId);
    var date = new Date();
    if (percentage >= 99) {
        if (!trackingStatus[videoId][100]) {
            trackingStatus[videoId][100] = true;
            preProcess(videoId,3);
            Common.utils.trackAction("CWInteractive", "contentcons", "Videos-Details-100", date.toLocaleString());
        }
    }
    else if (percentage >= 75) {
        if (!trackingStatus[videoId][75]) {
            trackingStatus[videoId][75] = true;
            preProcess(videoId, 2);
            Common.utils.trackAction("CWInteractive", "contentcons", "Videos-Details-75", date.toLocaleString());
        }
    }
    else if (percentage >= 50) {
        if (!trackingStatus[videoId][50]) {
            trackingStatus[videoId][50] = true;
            preProcess(videoId, 1);
            Common.utils.trackAction("CWInteractive", "contentcons", "Videos-Details-50", date.toLocaleString());
        }
    }
    else if (percentage >= 25) {
        if (!trackingStatus[videoId][25]) {
            trackingStatus[videoId][25] = true;
            preProcess(videoId, 1);
            Common.utils.trackAction("CWInteractive", "contentcons", "Videos-Details-25", date.toLocaleString());
        }
    }
    else if (percentage >= 10) {
        if (!trackingStatus[videoId][10]) {
            trackingStatus[videoId][10] = true;
            Common.utils.trackAction("CWInteractive", "contentcons", "Videos-Details-10", date.toLocaleString());
        }
    }
}
