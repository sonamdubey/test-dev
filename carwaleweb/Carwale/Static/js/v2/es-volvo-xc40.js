
$(document).ready(function () { 
    // code for Hotspot 
    $(".feature-pointer").click(function () {
        $(".feature-div").addClass("hide");
        $(this).next(".feature-div").removeClass("hide");
        $(".feature-close").removeClass("hide")
    });

    $(".feature-pointer-tooltip").click(function () {
        $(".feature-div").addClass("hide");
        $(this).prev(".feature-div").removeClass("hide");
        $(".feature-close").removeClass("hide")
    });

    $(".feature-close").click(function () {
        $(".feature-div").addClass("hide")
        $(this).addClass("hide");
    });
    // Ends



    $(".cw-tabs li").on('click', function () {
        $('.buying-reasons-contentbox').hide();
        $(".reason-card").hide();
    });
    
    $('.d-survey-popup-close').on('click', function () {
        $('.buying-reasons-contentbox').hide();
        $(".reason-card").show();
        $(".cw-tabs li").removeClass('active');
    });

    // on mobile open pop-up code start here
    $(".mob-result-container li").click(function () {
        $("img.force-lazy").trigger('appear');
        $('.blackOut-window').show();
        Common.utils.lockPopup();
    });
    $('.blackOut-window, .survey-popup-close').on('click', function () {
        $('.survey-result-popup').hide();
        $('.blackOut-window').hide();
        Common.utils.unlockPopup();
    });
    $(document).keydown(function (e) {
        // ESCAPE key pressed
        if (e.keyCode == 27) {
            $('.survey-result-popup').hide();
            $('.blackOut-window').hide();
            Common.utils.unlockPopup();
        }
    });
    
});
var video = 'QNTipOz8GVw', player;
var tag = document.createElement('script');
tag.src = 'https://www.youtube.com/player_api';
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

var volumeMuteState = false;

function onYouTubePlayerAPIReady() {
	player = new YT.Player('YouTubeVideoPlayer', {
		autoplay: true,
		videoId: video,
		playerVars: {
			autoplay: 0,
			controls: 1,
			showinfo: 0,
			modestbranding: 0,
			loop: 1,
			fs: 0,
			rel: 0,
			cc_load_policy: 0,
			iv_load_policy: 3,
			autohide: 0
		},
		events: {
			'onReady': onReadyMute,
			'onStateChange': onPlayerStateChange,
			'onVolumeChange': onPlayerVolumeChange,
		}
	});
}
function onReadyMute(e) {
	volumeMuteState = true;

}
function onPlayerVolumeChange() {
	label = 'Volvo-XC40-micro-site';
	if (player.isMuted() == false && volumeMuteState == false) {

		dataLayer.push({
			event: 'CWInteractive',
			cat: VolvoBookingEngineCategory,
			act: 'Video_unmuted ' + label + ' | t: ' + cleanTime(),
			lab: label + ' | t: ' + cleanTime()
		});

		volumeMuteState = true;
		bhriguClickEvent(VolvoBookingEngineCategory + '|video-state=' + 'Video_unmuted|video-time=' + cleanTime());
	}
	if (player.isMuted() == true && volumeMuteState == true) {
		dataLayer.push({
			event: 'CWInteractive',
			cat: VolvoBookingEngineCategory,
			act: 'Video_Muted ' + label + ' | t: ' + cleanTime(),
			lab: label + ' | t: ' + cleanTime()
		});
		volumeMuteState = false;
		bhriguClickEvent(VolvoBookingEngineCategory + '|video-state=' + 'Video_Muted|video-time=' + cleanTime());
	}
};

function onPlayerStateChange(event) {
	label = 'Volvo-XC40-micro-site';
	switch (event.data) {
		case YT.PlayerState.PLAYING:
			if (cleanTime() == 0) {
				dataLayer.push({
					event: 'CWInteractive',
					cat: VolvoBookingEngineCategory,
					act: 'Video_started ' + label + ' | t: ' + cleanTime(),
					lab: label + ' | t: ' + cleanTime()

				}); bhriguClickEvent(VolvoBookingEngineCategory + '|video-state=' + 'Video_started|video-time=' + cleanTime());

			} else {
				dataLayer.push({
					event: 'CWInteractive',
					cat: VolvoBookingEngineCategory,
					act: 'Video_played ' + label + ' | t: ' + cleanTime(),
					lab: label + ' | t: ' + cleanTime()
				});
				bhriguClickEvent(VolvoBookingEngineCategory + '|video-state=' + 'Video_played|video-time=' + cleanTime());
			};
			break;
		case YT.PlayerState.PAUSED:
			if (player.getDuration() - player.getCurrentTime() != 0) {
				dataLayer.push({
					event: 'CWInteractive',
					cat: VolvoBookingEngineCategory,
					act: 'Video_paused ' + label + ' | t: ' + cleanTime(),
					lab: label + ' | t: ' + cleanTime()
				}); bhriguClickEvent(VolvoBookingEngineCategory + '|video-state=' + 'Video_paused|video-time=' + cleanTime());
			};
			break;
		case YT.PlayerState.ENDED:
			dataLayer.push({
				event: 'CWInteractive',
				cat: VolvoBookingEngineCategory,
				act: 'Video_ended ' + label + ' | t: ' + cleanTime(),
				lab: label + ' | t: ' + cleanTime()
			}); bhriguClickEvent(VolvoBookingEngineCategory + '|video-state=' +'Video_ended|video-time=' + cleanTime());

			break;
	};
};
function cleanTime() {
	return Math.round(player.getCurrentTime())
};

//Bhrigu Tracking on page load and click event

var bhriguUrl = "https://" + cwTrackingPath + "/bhrigu/pixel.gif?",//bhrigu url

lbl = "makeid=37|makename=Volvo|modelname=XC40|modelid=1156|page=",//label
bhriguTracking = document.createElement("img"),// creating <img> for Impression Tracking
t = "%%PATTERN:url%%";//storing page url
bhriguTracking.setAttribute("height", "1");// setting Height of <img>
bhriguTracking.setAttribute("width", "1");// setting Width of <img>

// function impressionTracking for Impression Tracking on page load
function impressionTracking(VolvoBookingEngineCategory) {

	var bhriguImpressionUrl = bhriguUrl + 't=' + (new Date()).getTime() + '&cat=ESProperties&act=Impression&lbl=' + lbl + VolvoBookingEngineCategory + '&' + getPageUrlQS(t); // setting standard Bhrigu URL
	bhriguTracking.setAttribute("src", bhriguImpressionUrl);// setting <img> src attribute
	document.getElementById("impressiontracking").append(bhriguTracking);// append <img> on page load in div with id="impressiontracking" 
}

// function impressionTracking for Impression Tracking on Button Click Event
function bhriguClickEvent(act) {
	var bhriguClickUrl = bhriguUrl + 't=' + (new Date()).getTime() + '&cat=ESProperties&act=Click&lbl=' + lbl + act + '&' + getPageUrlQS(t);//setting standard Bhrigu URL
	bhriguTracking.setAttribute("src", bhriguClickUrl);	//setting <img> src attribute
	document.getElementById("impressiontracking").append(bhriguTracking);//append <img> on page load in div with id="btnContainer" on Click

}
function getPageUrlQS(t) {
	var t = document.location.href;
	if (parent !== window) {
		t = document.referrer;

	}
	var e = "";
	t.indexOf("?") > 0 ? e = t.split("?")[1] : t.indexOf("#") > 0 && (e = t.split("#")[1]);
	var qs = e.length > 0 ? "&qs=" + (e = e.replace(/&+/g, "|")) : "",
	pi = t.indexOf("?") > 0 ? "pi=" + t.split("?")[0] : t.indexOf("#") > 0 ? "pi=" + t.split("#")[0] : "pi=" + t;
	return pi + qs;
}



