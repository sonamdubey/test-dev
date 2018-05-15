﻿var SwiperYT = {
	isVideoPlaying: false,
	Initialize: function () {
		$('.swiper-youtube:not(".noSwiper")').each(function (index, element) {
			var currentSwiper = $(this);
			var spaceBetween = currentSwiper.attr('data-spacebetween');
			var spaceBetweenValue = 10;

			if (spaceBetween) {
				spaceBetweenValue = isNaN(spaceBetween) ? spaceBetweenValue : Number(spaceBetween);
			}

			currentSwiper.addClass('sw-yt-' + index).swiper({
				effect: 'slide',
				speed: 300,
				nextButton: currentSwiper.find('.swiper-button-next'),
				prevButton: currentSwiper.find('.swiper-button-prev'),
				pagination: currentSwiper.find('.swiper-pagination'),
				slidesPerView: 'auto',
				paginationClickable: true,
				spaceBetween: spaceBetweenValue,
				preloadImages: false,
				lazyLoading: true,
				lazyLoadingInPrevNext: true,
				watchSlidesVisibility: true,
				onInit: SwiperYT.initSwiper,
				onSlideChangeStart: SwiperYT.slideChangeStart
			});
		});
	},

	initSwiper: function (swiper) {
		$(window).resize(function () {
			swiper.update(true);
		})
	},

	slideChangeStart: function () {
		if (SwiperYT.YouTubeApi.playerState == 'playing' || SwiperYT.YouTubeApi.playerState == 'buffering') {
			try {
				SwiperYT.handleOrientationType();
			} catch (e) {
				console.warn(e);
			}
		}
	},

	handleOrientationType: function() {
		if ("orientation" in screen && screen.orientation.type === "landscape-primary") {
			if (typeof screenfull !== "undefined" && !screenfull.isFullscreen) {
				SwiperYT.YouTubeApi.videoPause();
			}
		}
		else {
			SwiperYT.YouTubeApi.handleVideoPause();
		}

		if (typeof handleFullscreenAnchestor !== "undefined") {
      handleFullscreenAnchestor();
    }
	},

	populateVideoIframes: function () {
	    $('.swiper-youtube iframe:not([data-video])').each(function () {
	        var ytApi = SwiperYT.YouTubeApi;
	        var ytIndex = ytApi.index;
	        var selectedElement = $(this);
	        selectedElement.attr('data-video', ytIndex);
	        var videoId = "video_" + ytIndex;

	        selectedElement.attr('id', videoId);
	        ytApi.videoPos = selectedElement.position();
	        ytApi.player[ytApi.index] = new YT.Player(videoId, {
	            events: {
	                'onStateChange': ytApi.onPlayerStateChange,
	                "onReady": ytApi.onPlayerReady,
	                "onError": ytApi.onPlayerError
	            }
	        });
	        ytApi.index += 1;
	    });

	    $('.bw-youtube').append('<span class="iframe-overlay"></span>');
	},

	YouTubeApi: {
		player: [],
		id: '',
		index: 0,
		count: 0,
		countArray: [],
		playerState: '',
		targetClick: '',
		targetOverlay: '',
		videoPos: '',
		addApiScript: function () {
			var tag = document.createElement('script');
			tag.src = "https://www.youtube.com/iframe_api";
			var firstScriptTag = document.getElementsByTagName('script')[0];
			firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
			SwiperYT.YouTubeApi.apiCommon();
		},

		apiCommon: function () {
			window.onYouTubeIframeAPIReady = function () {
			}

			//play video on click
			$(document).on('click', '.youtube-iframe-preview .iframe-overlay', function (event) {
				SwiperYT.YouTubeApi.targetClick = $(event.target).attr('class');
				if (SwiperYT.YouTubeApi.targetClick == 'iframe-overlay') {
					SwiperYT.YouTubeApi.targetOverlay = $(this);
					SwiperYT.YouTubeApi.videoPlay();
				}
			});

			var handleScroll = function () {
				try {
					if ("orientation" in screen && screen.orientation.type === "portrait-primary") {
						SwiperYT.YouTubeApi.handleVideoPause();
					}
					else {
						SwiperYT.YouTubeApi.handleOrientationType();
					}
				} catch (e) {
					console.log(e);
				}
			}

			$(window).on('scroll', handleScroll);
		},

		handleVideoPause: function() {
			if ($('.youtube-iframe-preview').find('iframe.current').length) {
				var videoFrame = $('.youtube-iframe-preview').find('iframe.current');
				var inViewPortTopBottom = ViewPort.isElementInViewportTopBottom(videoFrame);

				if (!inViewPortTopBottom) {
					SwiperYT.YouTubeApi.videoPause();
				}
			}
		},

		//youtube player state change event
		onPlayerStateChange: function (event) {
			switch (event.data) {
				case YT.PlayerState.UNSTARTED:
					SwiperYT.YouTubeApi.playerState = 'unstarted';
					break;
				case YT.PlayerState.ENDED:
					SwiperYT.YouTubeApi.playerState = 'ended';
					$('.youtube-iframe-preview .iframe-overlay').show();
					break;
				case YT.PlayerState.PLAYING:
					SwiperYT.YouTubeApi.playerState = 'playing';
					break;
				case YT.PlayerState.PAUSED:
					SwiperYT.YouTubeApi.playerState = 'paused';
					break;
				case YT.PlayerState.BUFFERING:
					SwiperYT.YouTubeApi.playerState = 'buffering';
					break;
				case YT.PlayerState.CUED:
					SwiperYT.YouTubeApi.playerState = 'cued';
					break;
			}
		},

		onPlayerReady: function (event) { },

		onPlayerError: function (event) { console.warn('onPlayerError:error!'); },

		videoPlay: function () {
			SwiperYT.YouTubeApi.videoPause();

			$('.swiper-youtube iframe').removeClass('current');
			var targetIframe = SwiperYT.YouTubeApi.targetOverlay.siblings('iframe');
			targetIframe.addClass('current');
			SwiperYT.YouTubeApi.count = targetIframe.attr('data-video');
			SwiperYT.YouTubeApi.player[SwiperYT.YouTubeApi.count].playVideo();
			targetIframe.siblings('.iframe-overlay').hide();
			SwiperYT.YouTubeApi.countArray.push(SwiperYT.YouTubeApi.count);
			SwiperYT.isVideoPlaying = true;
		},

		videoPause: function () {
			if (SwiperYT.YouTubeApi.playerState === 'playing' || SwiperYT.YouTubeApi.playerState === 'buffering') {
				for (var j = 0; j < SwiperYT.YouTubeApi.countArray.length; j++) {
					SwiperYT.YouTubeApi.player[SwiperYT.YouTubeApi.countArray[j]].pauseVideo();
				}
				$('.youtube-iframe-preview .iframe-overlay:not(":visible")').show();
				SwiperYT.YouTubeApi.countArray = [];
				SwiperYT.isVideoPlaying = false;
			}
		}
	}
}
